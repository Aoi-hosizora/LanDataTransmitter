using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanDataTransmitter {

    public partial class MainForm : Form {

        public MainForm() {
            InitializeComponent();
        }

        private static MainForm _instance;

        public static MainForm Instance {
            get { return _instance ??= new MainForm(); }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            PrepareUi();
            PrepareGrpc();
        }

        private void PrepareUi() {
            if (Global.Behavior == ApplicationBehavior.AS_SERVER) {
                lblBehavior.Text = $"当前作为服务器，正在监听 {Global.GrpcService.Address}:{Global.GrpcService.Port}";
                lblClientId.Text = "未绑定客户端";
                btnForceDisconnect.Enabled = false;
                btnForceDisconnect.Visible = true;
                btnStop.Text = "结束监听";
                btnSendText.Enabled = false;
                btnSendFile.Enabled = false;
            } else {
                lblBehavior.Text = $"当前作为客户端，已连接到 {Global.GrpcService.Address}:{Global.GrpcService.Port}";
                lblClientId.Text = "客户端标识 " + Global.SelfClientId;
                btnForceDisconnect.Visible = false;
                btnStop.Text = "断开连接";
            }
        }

        private void PrepareGrpc() {
            if (Global.Behavior == ApplicationBehavior.AS_SERVER) {
                Global.GrpcService.SetupTransmitterServer(
                    onBind: (_) => {
                        this.InvokeAction(() => {
                            lblClientId.Text = "已连接客户端 " + Global.BindClientId;
                            btnForceDisconnect.Enabled = true;
                            Global.PullChannel = new BidirectionalChannel<PullTextReply, Exception>(1);
                            btnSendText.Enabled = true;
                            btnSendFile.Enabled = true;
                        });
                    },
                    onUnbind: (_) => {
                        this.InvokeAction(() => {
                            lblClientId.Text = "未绑定客户端";
                            btnForceDisconnect.Enabled = false;
                            Global.PullChannel?.Complete();
                            Global.PullChannel = null;
                            btnSendText.Enabled = false;
                            btnSendFile.Enabled = false;
                        });
                    },
                    onReceived: (req) => {
                        this.InvokeAction(() => {
                            lsbRecord.AddToLast("Received from client: " + req.Message);
                        });
                    }
                ); // AS_SERVER
            } else {
                try {
                    Global.GrpcService.StartReceivingTextFromServer(
                        onFinish: () => {
                            this.InvokeAction(() => {
                                // TODO
                                this.ShowInfo("连接已断开", "与服务器的连接已断开。");
                                btnSendText.Enabled = false;
                                btnSendFile.Enabled = false;
                            });
                        },
                        onReceived: (reply) => {
                            this.InvokeAction(() => {
                                lsbRecord.AddToLast("Received from server: " + reply.Message);
                            });
                        }
                    );
                } catch (Exception ex) {
                    this.ShowError("初始化失败", $"无法接受来自服务器的推送。\n原因：{ex.Message}");
                } // AS_CLIENT
            }
        }

        /// <summary>Send !!!</summary>
        private async void btnSendText_Click(object sender, EventArgs e) {
            var content = edtText.Text;
            await Task.Run(async () => {
                if (Global.Behavior == ApplicationBehavior.AS_SERVER) {
                    try {
                        await Global.GrpcService.SendTextToClient(content);
                        this.InvokeAction(() => {
                            edtText.Text = "";
                            lsbRecord.AddToLast("Sent by server: " + content);
                        });
                    } catch (Exception ex) {
                        this.InvokeAction(() => this.ShowError("发送失败", $"文本发送至客户端失败。\n原因：{ex.Message}"));
                    }
                } else {
                    try {
                        await Global.GrpcService.SendTextToServer(content);
                        this.InvokeAction(() => {
                            edtText.Text = "";
                            lsbRecord.AddToLast("Sent by client: " + content);
                        });
                    } catch (Exception ex) {
                        this.InvokeAction(() => this.ShowError("发送失败", $"文本发送至服务器失败。\n原因：{ex.Message}"));
                    }
                }
            });
        }

        private void btnSendFile_Click(object sender, EventArgs e) {
            // TODO
        }

        private async void btnStop_Click(object sender, EventArgs e) {
            btnStop.Enabled = false;
            var flag = MessageBox.Show("是否断开连接并退出应用？", "结束确认", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (flag == DialogResult.Yes) {
                await Task.Run(async () => {
                    if (Global.Behavior == ApplicationBehavior.AS_SERVER) {
                        try {
                            await Global.GrpcService.Shutdown();
                            this.InvokeAction(() => Close());
                        } catch (Exception ex) {
                            this.InvokeAction(() => this.ShowError("结束监听失败", $"无法结束服务器监听。\n原因：{ex.Message}"));
                        }
                    } else {
                        try {
                            await Global.GrpcService.Disconnect();
                            this.InvokeAction(() => Close());
                        } catch (Exception ex) {
                            this.InvokeAction(() => this.ShowError("断开连接失败", $"无法断开与服务器的连接。\n原因：{ex.Message}"));
                        }
                    }
                });
            }
            btnStop.Enabled = true;
        }

        private async void btnForceDisconnect_Click(object sender, EventArgs e) {
            if (Global.Behavior == ApplicationBehavior.AS_CLIENT) {
                return;
            }
            await Task.Run(async () => {
                await Global.GrpcService.ForceDisconnect();
                // TODO
            });
        }
    }
}
