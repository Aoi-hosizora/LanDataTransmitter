using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace LanDataTransmitter {

    public partial class InitForm : Form {

        public InitForm() {
            InitializeComponent();
        }

        private static InitForm _instance;

        public static InitForm Instance {
            get {
                if (_instance == null) {
                    _instance = new InitForm();
                }
                return _instance;
            }
        }

        private void InitForm_Load(object sender, EventArgs e) {
            var interfaces = Utils.GetNetworkInterfaces();
            interfaces.Insert(0, "All interfaces (serve on 0.0.0.0)");
            cbbInterface.Items.AddRange(interfaces.ToArray());
            cbbInterface.SelectedIndex = 0;
        }

        private void cbbInterface_SelectedIndexChanged(object sender, EventArgs e) {
            var selected = cbbInterface.SelectedItem as string;
            toolTip.SetToolTip(cbbInterface, selected);
            if (selected.StartsWith("All interfaces")) {
                edtAddress.Text = "0.0.0.0";
            } else {
                edtAddress.Text = Utils.GetNetworkInterfaceIPv4(selected);
            }
        }

        private bool AsServer { get => rbtnServer.Checked; }

        private void rbtn_CheckedChanged(object sender, EventArgs e) {
            cbbInterface.Enabled = AsServer;
            edtAddress.ReadOnly = AsServer;
            if (!AsServer) {
                edtAddress.Text = "127.0.0.1";
            } else {
                cbbInterface_SelectedIndexChanged(cbbInterface, new EventArgs());
            }
        }

        private void btnExit_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnStart_Click(object sender, EventArgs e) {
            edtAddress.ReadOnly = true;
            numPort.Enabled = false;
            cbbInterface.Enabled = false;
            rbtnClient.Enabled = false;
            rbtnServer.Enabled = false;
            btnStart.Enabled = false;

            lblHint.Text = AsServer ? "尝试监听..." : "尝试连接...";
            lblHint.Visible = true;
            var th = new Thread(async () => {
                var service = new GrpcService {
                    Address = edtAddress.Text,
                    Port = (int) numPort.Value,
                };
                if (AsServer) {
                    await service.Serve(onSuccess: () => handleSuccess(service), onFailed: (r) => handleFailed(r));
                } else {
                    await service.Connect(onSuccess: () => handleSuccess(service), onFailed: (r) => handleFailed(r));
                }
            });
            th.Start();
        }

        private void handleFailed(string reason) {
            Invoke(new Action(() => {
                if (AsServer) {
                    MessageBox.Show($"无法监听指定的地址和端口。\n原因：{reason}", "监听失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    MessageBox.Show($"无法连接到指定的地址和端口。\n原因：{reason}", "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                lblHint.Visible = false;
                btnStart.Enabled = true;
                edtAddress.ReadOnly = AsServer;
                numPort.Enabled = true;
                cbbInterface.Enabled = AsServer;
                rbtnClient.Enabled = true;
                rbtnServer.Enabled = true;
            }));
        }

        private void handleSuccess(GrpcService service) {
            Invoke(new Action(() => {
                Global.behavior = AsServer ? ApplicationBehavior.AS_SERVER : ApplicationBehavior.AS_CLIENT;
                Global.grpcService = service;
                MainForm.Instance.Show();
                Close();
            }));
        }
    }
}
