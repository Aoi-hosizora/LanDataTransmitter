using System;
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
            get { return _instance ??= new MainForm(); }
        }

        private async void MainForm_Load(object sender, EventArgs e) {
            // ui
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                grpState.Text = "服务器状态";
                btnStop.Text = "结束监听";
            } else {
                grpState.Text = "客户端状态";
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
            await Task.Run(() => {
                if (Global.Behavior == ApplicationBehavior.AsServer) {
                    PrepareGrpcServer();
                } else {
                    PrepareGrpcClient();
                }
            });
        }

        private void UpdateButtonsState() {
            var hasStopped = Global.Stopped;
            var noClient = Global.Server.ConnectedClientsCount == 0;
            var emptyText = edtText.Text.Trim().Length == 0;

            if (Global.Behavior == ApplicationBehavior.AsServer) {
                btnForceDisconnect.Enabled = !hasStopped && !noClient;
                btnStop.Enabled = !hasStopped;
                cboSendTo.Enabled = !hasStopped && !noClient;
                cboSendTo.SelectedIndex = cboSendTo.Enabled && cboSendTo.SelectedItem is null ? 0 : cboSendTo.SelectedIndex;
                btnSendText.Enabled = !hasStopped && !noClient && !emptyText;
                btnSendFile.Enabled = !hasStopped && !noClient;
            } else {
                btnForceDisconnect.Visible = false;
                btnStop.Visible = false;
                btnSendText.Enabled = !hasStopped && !emptyText;
                btnSendFile.Enabled = !hasStopped;
            }
        }

        private void UpdateLabelsState() {
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                if (!Global.Stopped) {
                    lblBehavior.Text = $"当前作为服务器，正在监听 {Global.Server.Service.Address}:{Global.Server.Service.Port}";
                    lblClientInfo.Text = Global.Server.ConnectedClientsCount == 0 ? "未连接任何客户端" : $"已连接 {Global.Server.ConnectedClientsCount} 个客户端";
                } else {
                    lblBehavior.Text = "当前作为服务器，服务器的监听已停止";
                    lblClientInfo.Text = "";
                }
                lblRecord.Text = $"消息收发记录：(共收到 {Global.MessagesFromClient.Count} 条消息，已发送 {Global.MessagesFromServer.Count} 条消息)";
            } else {
                if (!Global.Stopped) {
                    lblBehavior.Text = $"当前作为客户端，已连接到 {Global.Client.Service.Address}:{Global.Client.Service.Port}";
                } else {
                    lblBehavior.Text = "当前作为客户端，与服务器的连接已断开";
                }
                lblClientInfo.Text = $"标识：{Global.Client.Id}" + (Global.Client.Name == "" ? "" : $"，名称：{Global.Client.Name}");
                lblRecord.Text = $"消息收发记录：(共收到 {Global.MessagesFromServer.Count} 条消息，已发送 {Global.MessagesFromClient.Count} 条消息)";
            }
        }

        #region Prepare and Stop

        private void PrepareGrpcServer() {
            Global.Server.Service.SetupTransmitter(
                onConnected: (id, name) => {
                    this.InvokeAction(() => {
                        cboSendTo.Items.Add(Utils.ConcatIdAndName(id, name));
                        UpdateButtonsState();
                        UpdateLabelsState();
                    });
                },
                onDisconnected: (id, name) => {
                    this.InvokeAction(() => {
                        cboSendTo.Items.Remove(Utils.ConcatIdAndName(id, name));
                        UpdateButtonsState();
                        UpdateLabelsState();
                    });
                },
                onReceived: r => {
                    this.InvokeAction(() => {
                        lsvRecord.AppendItem($"from {r.Id} - {Utils.FromTimestamp(r.Timestamp):yyyy-MM-dd HH:mm:ss}", r.Text);
                        UpdateLabelsState();
                    });
                }
            );
        }

        private void PrepareGrpcClient() {
            try {
                Global.Client.Service.StartReceivingText(
                    onReceived: r => {
                        this.InvokeAction(() => {
                            lsvRecord.AppendItem($"from server - {Utils.FromTimestamp(r.Timestamp):yyyy-MM-dd HH:mm:ss}", r.Text);
                            UpdateLabelsState();
                        });
                    }
                ).ContinueWith(
                    continuationFunction: async taskOfBool => {
                        var closedByClient = await taskOfBool;
                        if (!closedByClient) {
                            // TODO
                            try {
                                await Global.Client.Service.Disconnect();
                            } catch (Exception) {
                                // ignore any exceptions
                            }
                        }
                        this.InvokeAction(() => {
                            Global.Stopped = true;
                            UpdateButtonsState();
                            UpdateLabelsState();
                            if (!closedByClient) {
                                var ok = this.ShowQuestion("连接已断开", "与服务器的连接已断开，是否关闭本客户端？");
                                if (ok) {
                                    Close();
                                }
                            }
                        });
                    }
                );
            } catch (Exception ex) {
                this.ShowError("初始化失败", $"无法接受来自服务器的推送。\n原因：{ex.Message}");
            }
        }

        private async void btnStop_Click(object sender, EventArgs e) {
            btnStop.Enabled = false;
            var ok = this.ShowQuestion("结束确认", "是否断开连接并退出应用？");
            if (ok) {
                await Task.Run(async () => {
                    // TODO
                    try {
                        if (Global.Behavior == ApplicationBehavior.AsServer) {
                            await Global.Server.Service.Shutdown();
                        } else {
                            await Global.Client.Service.Disconnect();
                        }
                        Global.Stopped = true;
                        UpdateButtonsState();
                        UpdateLabelsState();
                    } catch (Exception) {
                        // ignore any exceptions
                    } finally {
                        this.InvokeAction(Close);
                    }
                });
            }
            btnStop.Enabled = true;
        }

        private async void btnForceDisconnect_Click(object sender, EventArgs e) {
            if (Global.Behavior == ApplicationBehavior.AsClient) {
                return;
            }
            btnForceDisconnect.Enabled = false;
            var ok = this.ShowQuestion("操作确认", "是否断开所有客户端的连接？");
            if (ok) {
                await Task.Run(async () => {
                    // TODO
                    try {
                        await Global.Server.Service.DisconnectAll();
                    } catch (Exception) { } finally {
                        this.InvokeAction(() => {
                            cboSendTo.Items.Clear();
                            lblClientInfo.Text = "未连接客户端";
                            UpdateButtonsState();
                        });
                    }
                });
            }
        }

        #endregion

        #region Send

        private void edtText_TextChanged(object sender, EventArgs e) {
            UpdateButtonsState();
        }

        private async void btnSendText_Click(object sender, EventArgs e) {
            var text = edtText.Text.Trim();
            var now = DateTime.Now;
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                var (id, _) = Utils.ExtractIdAndName((string) cboSendTo.SelectedItem);
                await Task.Run(async () => {
                    try {
                        var _ = await Global.Server.Service.SendText(id, text, now);
                        this.InvokeAction(() => {
                            edtText.Text = "";
                            lsvRecord.AppendItem($"to {id} - {now:yyyy-MM-dd HH:mm:ss}", text);
                            UpdateLabelsState();
                        });
                    } catch (Exception ex) {
                        this.InvokeAction(() => this.ShowError("发送失败", $"发送文本至客户端失败。\n原因：{ex.Message}"));
                    }
                });
            } else {
                await Task.Run(async () => {
                    try {
                        var _ = await Global.Client.Service.SendText(text, now);
                        this.InvokeAction(() => {
                            edtText.Text = "";
                            lsvRecord.AppendItem($"to server - {now:yyyy-MM-dd HH:mm:ss}", text);
                            UpdateLabelsState();
                        });
                    } catch (Exception ex) {
                        this.InvokeAction(() => this.ShowError("发送失败", $"发送文本至服务器失败。\n原因：{ex.Message}"));
                    }
                });
            }
        }

        private void btnSendFile_Click(object sender, EventArgs e) {
            // TODO
        }

        #endregion

        #region Menus

        private void tsmTextDetail_Click(object sender, EventArgs e) {
            if (lsvRecord.SelectedItems.Count == 0) {
                return;
            }
            this.ShowInfo("文本详情", "TODO");
        }

        private void tsmCopyInfo_Click(object sender, EventArgs e) {
            if (lsvRecord.SelectedItems.Count == 0) {
                return;
            }
            var item = lsvRecord.SelectedItems[0];
            if (item.Tag is CustomListView.CustomListViewObject obj) {
                Clipboard.SetText(obj.Line1);
            }
        }

        private void tsmCopyText_Click(object sender, EventArgs e) {
            if (lsvRecord.SelectedItems.Count == 0) {
                return;
            }
            var item = lsvRecord.SelectedItems[0];
            if (item.Tag is CustomListView.CustomListViewObject obj) {
                Clipboard.SetText(obj.Line2);
            }
        }

        #endregion

    }
}
