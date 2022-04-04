using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanDataTransmitter.Frm.View;
using LanDataTransmitter.Model;
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
            lblBehavior.EnableLabelTooltip(tipMain);
            lblClientInfo.EnableLabelTooltip(tipMain);
            btnRestart.Left = btnStop.Left;
            btnRestart.Anchor = AnchorStyles.Right | AnchorStyles.Top;
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
                // vis
                btnClientInfo.Visible = !notRunning;
                btnRestart.Visible = notRunning;
                btnForceDisconnect.Visible = !notRunning;
                btnStop.Visible = !notRunning;
                // able
                btnClientInfo.Enabled = !notRunning && !noClient;
                btnForceDisconnect.Enabled = !notRunning && !noClient;
                btnStop.Enabled = !notRunning;
                cboSendTo.Enabled = !notRunning && !noClient;
                cboSendTo.SelectedIndex = cboSendTo.Enabled && cboSendTo.SelectedItem is null ? 0 : cboSendTo.SelectedIndex;
                btnSendText.Enabled = !notRunning && !noClient && !emptyText;
                btnSendFile.Enabled = !notRunning && !noClient;
            } else {
                // vis
                btnClientInfo.Visible = !notRunning;
                btnRestart.Visible = notRunning;
                btnForceDisconnect.Visible = false;
                btnStop.Visible = !notRunning;
                // able
                btnClientInfo.Enabled = !notRunning;
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
            } else {
                if (Global.State == ApplicationState.Running) {
                    lblBehavior.Text = $"当前作为客户端，已连接到 {Global.Client.Service.Address}:{Global.Client.Service.Port}";
                } else {
                    lblBehavior.Text = "当前作为客户端，与服务器的连接已断开";
                }
                lblClientInfo.Text = $"标识：{Global.Client.Obj.Id}" + (Global.Client.Obj.Name == "" ? "" : $"，名称：{Global.Client.Obj.Name}");
            }
            lblRecord.Text = $"消息收发记录：(共收到 {Global.Messages.ReceivedCount} 条消息，已发送 {Global.Messages.SentCount} 条消息)";
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
                    Global.Messages.AddMessage(r); // received from client
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
                            Global.Messages.AddMessage(r); // received from server
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
                        this.ShowError("获取消息失败", $"当前客户端无法接受来自服务器的推送。\n原因：{err.Message}");
                    } else {
                        var ok = this.ShowQuestion("连接已断开", "服务器主动断开与当前客户端的连接，是否也退出 LanDataTransmitter？", false);
                        if (ok) {
                            _stopPassively = true;
                            Close();
                        }
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
                    } else {
                        await Global.Client.Service.Disconnect();
                    }
                } catch (Exception) {
                    // ignore all exceptions
                } finally {
                    Global.Server.ConnectedClients?.Clear();
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
            var ok = this.ShowQuestion("断开连接确认", "是否断开所有客户端的连接？");
            if (!ok) {
                btnForceDisconnect.Enabled = true;
                return;
            }
            await Task.Run(async () => {
                try {
                    await Global.Server.Service.DisconnectAll();
                } catch (Exception) {
                    // ignore all exceptions
                } finally {
                    Global.Server.ConnectedClients.Clear();
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
        private bool _stopPassively;

        private void btnRestart_Click(object sender, EventArgs e) {
            if (Global.State != ApplicationState.Stopped) {
                return;
            }
            InitForm.Instance.Show();
            _restarting = true;
            Close();
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (_restarting || _stopPassively) {
                return;
            }
            if (Global.State == ApplicationState.Running) {
                e.Cancel = true;
                if (await StopManually()) {
                    Close();
                }
            } else {
                e.Cancel = true;
                var ok = this.ShowQuestion("退出确认", "是否退出 LanDataTransmitter？");
                if (ok) {
                    e.Cancel = false;
                }
            }
        }

        #endregion

        #region Client info, Append list, Send

        private void btnClientInfo_Click(object sender, EventArgs e) {
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                var f = new ClientsInfoForm();
                f.Show(this);
            } else {
                var f = new ClientInfoForm();
                f.Show(this);
            }
        }

        private void AppendRecordToList(MessageRecord r) {
            lsvRecord.AppendItem(r);
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
                        Global.Messages.AddMessage(r); // sent to client
                        this.InvokeAction(() => {
                            AppendRecordToList(r);
                            edtText.Text = "";
                            lsvRecord.Focus();
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
                        Global.Messages.AddMessage(r); // sent to server
                        this.InvokeAction(() => {
                            AppendRecordToList(r);
                            edtText.Text = "";
                            lsvRecord.Focus();
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

        #region Menu events

        private void tsmTextDetail_Click(object sender, EventArgs e) {
            if (lsvRecord.SelectedItems.Count == 0) {
                return;
            }
            var item = lsvRecord.SelectedItems[0];
            if (item.Tag is MessageRecordListView.MessageRecordTag tag) {
                var index = lsvRecord.SelectedIndices[0];
                var f = new MessageDetailForm(tag.Record, index);
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
            if (item.Tag is MessageRecordListView.MessageRecordTag tag) {
                Clipboard.SetText(tag.InfoLine);
            }
        }

        private void tsmCopyText_Click(object sender, EventArgs e) {
            if (lsvRecord.SelectedItems.Count == 0) {
                return;
            }
            var item = lsvRecord.SelectedItems[0];
            if (item.Tag is MessageRecordListView.MessageRecordTag tag) {
                Clipboard.SetText(tag.Record.Text.Text);
            }
        }

        #endregion

    }
}
