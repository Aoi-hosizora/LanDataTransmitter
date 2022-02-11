﻿using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanDataTransmitter.Frm.View;
using LanDataTransmitter.Service;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Frm {

    public partial class MainForm : Form {

        private MainForm() {
            InitializeComponent();
        }

        private static MainForm _instance;

        public static MainForm Instance {
            get {
                if (_instance == null || _instance.IsDisposed) {
                    _instance = new MainForm();
                }
                return _instance;
            }
        }

        #region Load, Update UI

        private void MainForm_Load(object sender, EventArgs e) {
            // ui
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                Text = "LAN Data Transmitter (服务器)";
                grpState.Text = "服务器状态";
                btnRestart.Text = "重新监听";
                btnStop.Text = "结束监听";
            } else {
                Text = "LAN Data Transmitter (客户端)";
                grpState.Text = "客户端状态";
                btnRestart.Text = "重新连接";
                btnStop.Text = "断开连接";
                lblSendTo.Visible = false;
                cboSendTo.Visible = false;
                splContent.Height = cboSendTo.Top + cboSendTo.Height - splContent.Top;
            }
            cmsListView.Renderer = new MenuNativeRenderer();
            cboSendTo.EnableComboBoxTooltip(tipMain);
            UpdateButtonsState();
            UpdateLabelsState();

            // server
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                PrepareGrpcServer();
            } else {
                PrepareGrpcClient();
            }
        }

        private void UpdateButtonsState() {
            var notRunning = Global.State != ApplicationState.Running;
            var noClient = (Global.Server.ConnectedClients?.Count ?? 0) == 0;
            var emptyText = edtText.Text.Trim().Length == 0;

            if (Global.Behavior == ApplicationBehavior.AsServer) {
                btnRestart.Visible = notRunning;
                btnForceDisconnect.Enabled = !notRunning && !noClient;
                btnStop.Enabled = !notRunning;
                cboSendTo.Enabled = !notRunning && !noClient;
                cboSendTo.SelectedIndex = cboSendTo.Enabled && cboSendTo.SelectedItem is null ? 0 : cboSendTo.SelectedIndex;
                btnSendText.Enabled = !notRunning && !noClient && !emptyText;
                btnSendFile.Enabled = !notRunning && !noClient;
            } else {
                btnRestart.Visible = notRunning;
                btnForceDisconnect.Visible = false;
                btnStop.Enabled = !notRunning;
                btnSendText.Enabled = !notRunning && !emptyText;
                btnSendFile.Enabled = !notRunning;
            }
        }

        private void UpdateLabelsState() {
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                if (Global.State == ApplicationState.Running) {
                    lblBehavior.Text = $"当前作为服务器，正在监听 {Global.Server.Service.Address}:{Global.Server.Service.Port}";
                    lblClientInfo.Text = Global.Server.ConnectedClients.Count == 0 ? "未连接任何客户端" : $"已连接 {Global.Server.ConnectedClients.Count} 个客户端";
                } else {
                    lblBehavior.Text = "当前作为服务器，服务器的监听已停止";
                    lblClientInfo.Text = "";
                }
                lblRecord.Text = $"消息收发记录：(共收到 {Global.Messages.CtSCount} 条消息，已发送 {Global.Messages.StCCount} 条消息)";
            } else {
                if (Global.State == ApplicationState.Running) {
                    lblBehavior.Text = $"当前作为客户端，已连接到 {Global.Client.Service.Address}:{Global.Client.Service.Port}";
                } else {
                    lblBehavior.Text = "当前作为客户端，与服务器的连接已断开";
                }
                lblClientInfo.Text = $"标识：{Global.Client.Id}" + (Global.Client.Name == "" ? "" : $"，名称：{Global.Client.Name}");
                lblRecord.Text = $"消息收发记录：(共收到 {Global.Messages.StCCount} 条消息，已发送 {Global.Messages.CtSCount} 条消息)";
            }
        }

        #endregion

        #region Prepare, Stop, Close

        private void PrepareGrpcServer() {
            Global.Server.Service.SetupTransmitter(
                onConnected: obj => {
                    Global.Server.ConnectedClients.Add(obj.Id, obj);
                    this.InvokeAction(() => {
                        cboSendTo.Items.Add(obj.FullDisplayName);
                        UpdateButtonsState();
                        UpdateLabelsState();
                    });
                },
                onDisconnected: obj => {
                    Global.Server.ConnectedClients.Remove(obj.Id);
                    this.InvokeAction(() => {
                        cboSendTo.Items.Remove(obj.FullDisplayName);
                        UpdateButtonsState();
                        UpdateLabelsState();
                    });
                },
                onReceived: r => {
                    Global.Messages.AddCtSMessage(r); // <<<
                    this.InvokeAction(() => {
                        AppendRecordToList(r);
                        UpdateLabelsState();
                    });
                }
            );
        }

        private void PrepareGrpcClient() {
            Task.Run(async () => {
                Exception err = null;
                try {
                    var closedByClient = await Global.Client.Service.StartPulling(
                        onReceived: r => {
                            Global.Messages.AddStCMessage(r); // <<<
                            this.InvokeAction(() => {
                                AppendRecordToList(r);
                                UpdateLabelsState();
                            });
                        }
                    );
                    // No matter closed by client itself or by server passively, it is no need to
                    // request for disconnect, except when an error occurred.
                    if (closedByClient) {
                        return;
                    }
                } catch (Exception ex) {
                    err = ex;
                    // TODO now just to disconnect the client, later will consider retrying strategy
                    try {
                        await Global.Client.Service.Disconnect();
                    } catch (Exception) {
                        // ignore all exceptions
                    }
                }
                // <- here connected has been disconnected.
                Global.State = ApplicationState.Stopped;
                this.InvokeAction(() => {
                    UpdateButtonsState();
                    UpdateLabelsState();
                    if (err != null) {
                        this.ShowError("客户端获取消息失败", $"无法接受来自服务器的推送。\n原因：{err.Message}");
                    } else {
                        this.ShowInfo("客户端连接已断开", "服务器主动断开与当前客户端的连接。");
                    }
                });
            });
        }

        private async void btnStop_Click(object sender, EventArgs e) {
            await StopManually();
        }

        private async Task<bool> StopManually() {
            btnStop.Enabled = false;
            var ok = this.ShowQuestion("结束确认", Global.Behavior == ApplicationBehavior.AsServer ? "是否结束监听？" : "是否断开连接？");
            if (!ok) {
                btnStop.Enabled = true;
                return false;
            }
            await Task.Run(async () => {
                try {
                    if (Global.Behavior == ApplicationBehavior.AsServer) {
                        await Global.Server.Service.Shutdown();
                        Global.Server.ConnectedClients.Clear();
                    } else {
                        await Global.Client.Service.Disconnect();
                    }
                } catch (Exception) {
                    // ignore all exceptions
                } finally {
                    Global.State = ApplicationState.Stopped;
                    this.InvokeAction(() => {
                        btnStop.Enabled = true;
                        cboSendTo.Items.Clear();
                        UpdateButtonsState();
                        UpdateLabelsState();
                    });
                }
            });
            return true;
        }

        private async void btnForceDisconnect_Click(object sender, EventArgs e) {
            btnForceDisconnect.Enabled = false;
            var ok = this.ShowQuestion("操作确认", "是否断开所有客户端的连接？");
            if (!ok) {
                btnForceDisconnect.Enabled = true;
                return;
            }
            await Task.Run(async () => {
                try {
                    await Global.Server.Service.DisconnectAll();
                    Global.Server.ConnectedClients.Clear();
                } catch (Exception) {
                    // ignore all exceptions
                } finally {
                    this.InvokeAction(() => {
                        btnForceDisconnect.Enabled = true;
                        cboSendTo.Items.Clear();
                        UpdateButtonsState();
                        UpdateLabelsState();
                    });
                }
            });
        }

        private bool _restarting;

        private void btnRestart_Click(object sender, EventArgs e) {
            if (Global.State != ApplicationState.Stopped) {
                return;
            }
            InitForm.Instance.Show();
            _restarting = true;
            Close();
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (_restarting) {
                return;
            }
            if (Global.State == ApplicationState.Running) {
                e.Cancel = true;
                if (await StopManually()) {
                    Close();
                }
            } else {
                e.Cancel = true;
                var ok = this.ShowQuestion("关闭确认", "是否关闭 LanDataTransmitter？");
                if (ok) {
                    e.Cancel = false;
                }
            }
        }

        #endregion

        #region Append list, Send

        private void AppendRecordToList(MessageRecord r) {
            var obj = new MessageRecordListView.MessageRecordObject { Record = r };
            var time = Utils.RenderTimeForShow(Utils.FromTimestamp(r.Timestamp));
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                if (r.IsCtS) { // received
                    obj.InfoLine = $"{r.ClientDisplayName} ({time})";
                    obj.IsReceived = true;
                } else { // sent
                    obj.IsReceived = false;
                    obj.InfoLine = $"{r.ClientDisplayName} ({time})";
                }
            } else {
                if (r.IsStC) { // received
                    obj.InfoLine = $"server ({time})";
                    obj.IsReceived = true;
                } else { // sent
                    obj.InfoLine = $"server ({time})";
                    obj.IsReceived = false;
                }
            }
            lsvRecord.AppendItem(obj);
        }

        public void SelectMessageRecordInList(int index) {
            lsvRecord.Focus();
            lsvRecord.SetSingleSelected(index);
        }

        private void edtText_TextChanged(object sender, EventArgs e) {
            UpdateButtonsState();
        }

        private async void btnSendText_Click(object sender, EventArgs e) {
            var text = edtText.Text.Trim();
            var now = DateTime.Now;
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                var (id, _) = ClientObject.ExtractIdAndName((string) cboSendTo.SelectedItem);
                await Task.Run(async () => {
                    try {
                        var r = await Global.Server.Service.SendText(id, text, now);
                        Global.Messages.AddStCMessage(r); // <<<
                        this.InvokeAction(() => {
                            edtText.Text = "";
                            AppendRecordToList(r);
                            UpdateLabelsState();
                        });
                    } catch (Exception ex) {
                        this.InvokeAction(() => this.ShowError("发送失败", $"发送文本至客户端失败。\n原因：{ex.Message}"));
                    }
                });
            } else {
                await Task.Run(async () => {
                    try {
                        var r = await Global.Client.Service.SendText(text, now);
                        Global.Messages.AddCtSMessage(r); // <<<
                        this.InvokeAction(() => {
                            edtText.Text = "";
                            AppendRecordToList(r);
                            UpdateLabelsState();
                        });
                    } catch (Exception ex) {
                        this.InvokeAction(() => this.ShowError("发送失败", $"发送文本至服务器失败。\n原因：{ex.Message}"));
                    }
                });
            }
        }

        private void btnSendFile_Click(object sender, EventArgs e) {
            this.ShowInfo("发送文件", "TODO"); // TODO
        }

        #endregion

        #region Menus

        private void tsmTextDetail_Click(object sender, EventArgs e) {
            if (lsvRecord.SelectedItems.Count == 0) {
                return;
            }
            var item = lsvRecord.SelectedItems[0];
            if (item.Tag is MessageRecordListView.MessageRecordObject obj) {
                var index = lsvRecord.SelectedIndices[0];
                var f = new MessageDetailForm(obj.Record, index);
                f.Show(this);
            }
        }

        private void lsvRecord_MouseDoubleClick(object sender, MouseEventArgs e) {
            tsmTextDetail_Click(tsmTextDetail, EventArgs.Empty);
        }

        private void tsmCopyInfo_Click(object sender, EventArgs e) {
            if (lsvRecord.SelectedItems.Count == 0) {
                return;
            }
            var item = lsvRecord.SelectedItems[0];
            if (item.Tag is MessageRecordListView.MessageRecordObject obj) {
                Clipboard.SetText(obj.InfoLine);
            }
        }

        private void tsmCopyText_Click(object sender, EventArgs e) {
            if (lsvRecord.SelectedItems.Count == 0) {
                return;
            }
            var item = lsvRecord.SelectedItems[0];
            if (item.Tag is MessageRecordListView.MessageRecordObject obj) {
                Clipboard.SetText(obj.Record.Text);
            }
        }

        #endregion
    }
}
