using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanDataTransmitter {

    public partial class InitForm : Form {

        private InitForm() {
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

        private void btnExit_Click(object sender, EventArgs e) {
            Close();
        }

        private bool IsServerBehavior() => rdoServer.Checked;

        private void rdoBehavior_CheckedChanged(object sender, EventArgs e) {
            var server = IsServerBehavior();
            cboInterface.Visible = server;
            lblInterface.Text = server ? "接口 :" : "请输入服务器端监听的网络地址和端口 :";
            lblInterface.TextAlign = server ? System.Drawing.ContentAlignment.MiddleLeft : System.Drawing.ContentAlignment.MiddleCenter;
            edtAddress.ReadOnly = server;
            edtAddress.Text = server ? Utils.GetNetworkInterfaceIPv4((string) cboInterface.SelectedItem) : "127.0.0.1";
        }

        private void cboInterface_SelectedIndexChanged(object sender, EventArgs e) {
            var selected = (string) cboInterface.SelectedItem;
            tipMain.SetToolTip(cboInterface, selected);
            edtAddress.Text = Utils.GetNetworkInterfaceIPv4(selected);
        }

        private async void btnStart_Click(object sender, EventArgs e) {
            ProcessTrying();
            // !!!!!!
            await Task.Run(async () => {
                var addr = edtAddress.Text;
                var port = (int) numPort.Value;
                try {
                    if (IsServerBehavior()) {
                        var service = new GrpcServerService(addr, port);
                        await service.Serve();
                        this.InvokeAction(() => ProcessSuccess(service, null));
                    } else {
                        var service = new GrpcClientService(addr, port);
                        await service.Connect("client_" + Utils.GetNowString());
                        this.InvokeAction(() => ProcessSuccess(null, service));
                    }
                } catch (Exception ex) {
                    this.InvokeAction(() => ProcessFailed(ex.Message));
                }
            });
        }

        private void ProcessTrying() {
            lblHint.Text = IsServerBehavior() ? "尝试监听..." : "尝试连接...";
            lblHint.Visible = true;
            btnStart.Enabled = false;
            grpBehavior.Enabled = false;
            grpConfig.Enabled = false;
        }

        private void ProcessSuccess(GrpcServerService server, GrpcClientService client) {
            if (IsServerBehavior()) {
                Global.InitializeServer(server);
            } else {
                Global.InitializeClient(client);
            }
            MainForm.Instance.Show();
            Close();
        }

        private void ProcessFailed(string reason) {
            if (IsServerBehavior()) {
                this.ShowError("监听失败", $"无法监听指定的地址和端口。\n原因：{reason}");
            } else {
                this.ShowError("连接失败", $"无法连接到指定的地址和端口。\n原因：{reason}");
            }
            lblHint.Visible = false;
            btnStart.Enabled = true;
            grpBehavior.Enabled = true;
            grpConfig.Enabled = true;
        }
    }
}
