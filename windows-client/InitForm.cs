using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanDataTransmitter {

    public partial class InitForm : Form {

        public InitForm() {
            InitializeComponent();
        }

        private static InitForm _instance;

        public static InitForm Instance {
            get { return _instance ??= new InitForm(); }
        }

        private void InitForm_Load(object sender, EventArgs e) {
            var interfaces = Utils.GetNetworkInterfaces();
            cboInterface.Items.AddRange(interfaces.ToArray<object>());
            cboInterface.SelectedIndex = 0;
        }

        private void cboInterface_SelectedIndexChanged(object sender, EventArgs e) {
            var selected = cboInterface.SelectedItem as string;
            tipMain.SetToolTip(cboInterface, selected);
            edtAddress.Text = Utils.GetNetworkInterfaceIPv4(selected);
        }

        private void btnExit_Click(object sender, EventArgs e) {
            Close();
        }

        private bool IsServerBehavior() => rdoServer.Checked;

        private void rdoBehavior_CheckedChanged(object sender, EventArgs e) {
            cboInterface.Enabled = IsServerBehavior();
            edtAddress.ReadOnly = IsServerBehavior();
            if (!IsServerBehavior()) {
                edtAddress.Text = "127.0.0.1";
            } else {
                cboInterface_SelectedIndexChanged(cboInterface, EventArgs.Empty);
            }
        }

        private async void btnStart_Click(object sender, EventArgs e) {
            HandleTrying();
            await Task.Run(async () => {
                var service = new GrpcService { Address = edtAddress.Text, Port = (int) numPort.Value };
                try {
                    if (IsServerBehavior()) {
                        await service.Serve();
                    } else {
                        await service.Connect();
                    }
                    this.InvokeAction(() => HandleSuccess(service));
                } catch (Exception ex) {
                    this.InvokeAction(() => HandleFailed(ex.Message));
                }
            });
        }

        private void HandleTrying() {
            lblHint.Text = IsServerBehavior() ? "尝试监听..." : "尝试连接...";
            lblHint.Visible = true;
            btnStart.Enabled = false;
            grpBehavior.Enabled = false;
            grpConnect.Enabled = false;
        }

        private void HandleSuccess(GrpcService service) {
            Global.Behavior = IsServerBehavior() ? ApplicationBehavior.AS_SERVER : ApplicationBehavior.AS_CLIENT;
            Global.GrpcService = service;
            MainForm.Instance.Show();
            Close();
        }

        private void HandleFailed(string reason) {
            if (IsServerBehavior()) {
                this.ShowError("监听失败", $"无法监听指定的地址和端口。\n原因：{reason}");
            } else {
                this.ShowError("连接失败", $"无法连接到指定的地址和端口。\n原因：{reason}");
            }
            lblHint.Visible = false;
            btnStart.Enabled = true;
            grpBehavior.Enabled = true;
            grpConnect.Enabled = true;
        }
    }
}
