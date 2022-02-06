using System;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            lsbRecord.Items.Add("0000000000000");
            lsbRecord.Items.Add("1111111111111");
            lsbRecord.Items.Add("2222222222222");
            lsbRecord.Items.Add("3333333333333");
            // PrepareUi
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                lblBehavior.Text = $"当前作为服务器，正在监听 {Global.Server.Service.Address}:{Global.Server.Service.Port}";
                lblClientInfo.Text = "未连接客户端";
                btnForceDisconnect.Enabled = false;
                btnStop.Text = "结束监听";
            } else {
                lblBehavior.Text = $"当前作为客户端，已连接到 {Global.Client.Service.Address}:{Global.Client.Service.Port}";
                lblClientInfo.Text = "客户端标识：" + Global.Client.Id;
                btnForceDisconnect.Visible = false;
                btnStop.Text = "断开连接";
                lblSendTo.Visible = false;
                cboSendTo.Visible = false;
                splContent.Height = cboSendTo.Top + cboSendTo.Height - splContent.Top;
            }
            UpdateButtonsState();

            // PrepareServer
            await Task.Run(() => {
                if (Global.Behavior == ApplicationBehavior.AsServer) {
                    PrepareGrpcServer();
                } else {
                    PrepareGrpcClient();
                }
            });
        }

        private void PrepareGrpcServer() {
            Global.Server.Service.SetupTransmitter(
                onConnected: id => {
                    this.InvokeAction(() => {
                        lblClientInfo.Text = $"已连接 {Global.Server.ConnectedClients.Count} 个客户端";
                        cboSendTo.Items.Add(id);
                        UpdateButtonsState();
                    });
                },
                onDisconnected: id => {
                    this.InvokeAction(() => {
                        lblClientInfo.Text = Global.Server.ConnectedClients.Count == 0
                            ? "未连接客户端"
                            : $"已连接 {Global.Server.ConnectedClients.Count} 个客户端";
                        cboSendTo.Items.Remove(id);
                        UpdateButtonsState();
                    });
                },
                onReceived: req => {
                    var text = req.Text;
                    var time = DateTimeOffset.FromUnixTimeSeconds(req.Timestamp).LocalDateTime;
                    this.InvokeAction(() => lsbRecord.AddToLast($"from {req.Id}: {text} - {time:yyyy-MM-dd HH:mm:ss}"));
                }
            );
        }

        private void PrepareGrpcClient() {
            try {
                Global.Client.Service.StartReceivingText(
                    onReceived: reply => {
                        var text = reply.Text;
                        var time = DateTimeOffset.FromUnixTimeSeconds(reply.Timestamp).LocalDateTime;
                        this.InvokeAction(() => lsbRecord.AddToLast($"from server: {text} - {time}"));
                    }
                ).ContinueWith(
                    continuationFunction: async t => {
                        // 服务器要求客户端断开连接 / 客户端主动断开连接
                        var byClient = await t;
                        // try {
                        //     await Global.Client.Service.Disconnect();
                        // } catch (Exception) { }
                        this.InvokeAction(() => {
                            lblBehavior.Text = "当前作为客户端，与服务器的连接已断开";
                            btnSendFile.Enabled = false;
                            btnSendText.Enabled = false;
                            btnStop.Enabled = false;
                            if (!byClient) {
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

        private void UpdateButtonsState() {
            var emptyText = edtText.Text.Trim().Length == 0;
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                var noClient = Global.Server.ConnectedClients.Count == 0;
                Console.WriteLine(Global.Server.ConnectedClients.Count);
                btnForceDisconnect.Enabled = !noClient;
                btnSendText.Enabled = !noClient && !emptyText;
                btnSendFile.Enabled = !noClient;
                if (!noClient && cboSendTo.SelectedItem is null) {
                    cboSendTo.SelectedIndex = 0;
                }
            } else {
                btnSendText.Enabled = !emptyText;
                btnSendFile.Enabled = true;
            }
        }

        private void cboSendTo_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateButtonsState();
            var selected = (string) cboSendTo.SelectedItem;
            tipMain.SetToolTip(cboSendTo, selected);
        }

        private void edtText_TextChanged(object sender, EventArgs e) {
            UpdateButtonsState();
        }

        private async void btnSendText_Click(object sender, EventArgs e) {
            var content = edtText.Text.Trim();
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                if (!(cboSendTo.SelectedItem is string id) || id.Length == 0) {
                    return;
                }
                await Task.Run(async () => {
                    try {
                        var now = DateTime.Now;
                        await Global.Server.Service.SendText(id, content, now);
                        this.InvokeAction(() => {
                            edtText.Text = "";
                            lsbRecord.AddToLast($"to {id}: {content} - {now}");
                        });
                    } catch (Exception ex) {
                        this.InvokeAction(() => this.ShowError("发送失败", $"发送文本至客户端失败。\n原因：{ex.Message}"));
                    }
                });
            } else {
                await Task.Run(async () => {
                    try {
                        var now = DateTime.Now;
                        await Global.Client.Service.SendText(content, now);
                        this.InvokeAction(() => {
                            edtText.Text = "";
                            lsbRecord.AddToLast($"to server: {content} - {now}");
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

        private async void btnStop_Click(object sender, EventArgs e) {
            btnStop.Enabled = false;
            var ok = this.ShowQuestion("结束确认", "是否断开连接并退出应用？");
            if (ok) {
                await Task.Run(async () => {
                    try {
                        if (Global.Behavior == ApplicationBehavior.AsServer) {
                            await Global.Server.Service.Shutdown();
                        } else {
                            await Global.Client.Service.Disconnect();
                        }
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
                    try {
                        await Global.Server.Service.ForceDisconnectAll();
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
    }
}
