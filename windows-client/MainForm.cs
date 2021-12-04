using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using Channels = System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanDataTransmitter {

    public partial class MainForm : Form {

        public MainForm() {
            InitializeComponent();
        }

        private static MainForm _instance;

        public static MainForm Instance {
            get {
                if (_instance == null) {
                    _instance = new MainForm();
                }
                return _instance;
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            if (Global.behavior == ApplicationBehavior.AS_CLIENT) {
                lblBehavior.Text = $"当前作为客户端，已连接到 {Global.grpcService.Address}:{Global.grpcService.Port}";
                lblClientId.Text = "客户端标识 " + Global.clientId;
                btnForceDisconnect.Visible = false;
                btnSend.Enabled = false;
                btnFile.Enabled = false;
                Global.grpcService.StartHandlePullText(onSuccess: () => {
                    Invoke(new Action(() => {
                        btnSend.Enabled = true;
                        btnFile.Enabled = true;
                    }));
                }, onFailed: (reason) => {
                    Invoke(new Action(() => {
                        MessageBox.Show($"无法开始接受来自服务器的推送。\n原因：{reason}", "初始化失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }, onFinish: () => {
                    Invoke(new Action(() => {
                        MessageBox.Show($"与服务器的连接已断开。", "连接断开", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        btnSend.Enabled = false;
                        btnFile.Enabled = false;
                    }));
                }, onDisplayText: (reply) => {
                    Invoke(new Action(() => {
                        lbRecord.Items.Add("Received from server: " + reply.Message);
                    }));
                }); // <<<
            } else {
                lblBehavior.Text = $"当前作为服务器，正在监听 {Global.grpcService.Address}:{Global.grpcService.Port}";
                lblClientId.Text = "未绑定客户端";
                btnForceDisconnect.Visible = true;
                Global.pullTextChannel = Channels.Channel.CreateBounded<PullTextReply>(1);
                Global.pullTextExceptionChannel = Channels.Channel.CreateBounded<Exception>(1);
                Global.grpcService.SetupTransmitter(onClientBind: (bind) => {
                    Invoke(new Action(() => {
                        if (bind) {
                            lblClientId.Text = "已连接客户端 " + Global.bindClientId;
                            btnForceDisconnect.Enabled = true;
                        } else {
                            lblClientId.Text = "未绑定客户端";
                            btnForceDisconnect.Enabled = false;
                        }
                    }));
                }, onDisplayText: (req) => {
                    Invoke(new Action(() => {
                        lbRecord.Items.Add("Received from client: " + req.Message);
                    }));
                }); // <<<
            }
        }

        private void btnStop_Click(object sender, EventArgs e) {
            var flag = MessageBox.Show("是否断开连接并退出应用？", "结束确认", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (flag == DialogResult.No || flag == DialogResult.Cancel) {
                return;
            }

            if (Global.behavior == ApplicationBehavior.AS_CLIENT) {
                var th = new Thread(async () => {
                    await Global.grpcService.Disconnect(onSuccess: () => {
                        Invoke(new Action(() => Close()));
                    }, onFailed: (reason) => {
                        // Invoke(new Action(() => MessageBox.Show($"无法结束连接。\n原因：{reason}", "结束连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                        Invoke(new Action(() => Close()));
                    });
                });
                th.Start();
            } else {
                var th = new Thread(async () => {
                    await Global.grpcService.Shutdown(onSuccess: () => {
                        Invoke(new Action(() => Close()));
                    });
                });
                th.Start();
            }
        }

        private void btnForceDisconnect_Click(object sender, EventArgs e) {
            if (Global.behavior == ApplicationBehavior.AS_CLIENT) {
                return;
            }
            Global.grpcService.ForceDisconnect(onSuccess: () => {
                //
            });
        }

        private void btnSend_Click(object sender, EventArgs e) {
            var content = edtText.Text;
            if (Global.behavior == ApplicationBehavior.AS_CLIENT) {
                var th = new Thread(async () => {
                    await Global.grpcService.SendTextToServer(content, onSuccess: () => {
                        Invoke(new Action(() => {
                            edtText.Text = "";
                            lbRecord.Items.Add("Sent by client: " + content);
                        }));
                    }, onFailed: (reason) => {
                        Invoke(new Action(() => {
                            MessageBox.Show($"文本发送至服务器失败。\n原因：{reason}", "发送失败", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                        }));
                    });
                });
                th.Start();
            } else {
                var th = new Thread(async () => {
                    await Global.grpcService.SendTextToClient(content, onSuccess: () => {
                        Invoke(new Action(() => {
                            edtText.Text = "";
                            lbRecord.Items.Add("Sent by server: " + content);
                        }));
                    }, onFailed: (reason) => {
                        Invoke(new Action(() => {
                            MessageBox.Show($"文本发送至客户端失败。\n原因：{reason}", "发送失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    });
                });
                th.Start();
            }
        }
    }
}
