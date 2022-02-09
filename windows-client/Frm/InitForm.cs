using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanDataTransmitter.Service;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Frm {

    public partial class InitForm : Form {

        private InitForm() {
            InitializeComponent();
        }

        private static InitForm _instance;

        public static InitForm Instance {
            get {
                if (_instance == null || _instance.IsDisposed) {
                    _instance = new InitForm();
                }
                return _instance;
            }
        }

        private void InitForm_Load(object sender, EventArgs e) {
            // ui
            Height -= grpClient.Top - grpServer.Top;
            grpClient.Top = grpServer.Top;
            rdoServer.Checked = true;
            cboServeInterface.EnableComboBoxTooltip(tipMain);

            // interface
            var interfaces = Utils.GetNetworkInterfaces();
            cboServeInterface.Items.AddRange(interfaces.ToArray<object>());
            cboServeInterface.SelectedIndex = 0;
        }

        private void btnExit_Click(object sender, EventArgs e) {
            Close();
        }

        private bool IsServerBehavior() => rdoServer.Checked;

        private void rdoBehavior_CheckedChanged(object sender, EventArgs e) {
            var isServer = IsServerBehavior();
            grpServer.Visible = isServer;
            grpClient.Visible = !isServer;
        }

        private void cboInterface_SelectedIndexChanged(object sender, EventArgs e) {
            edtServeAddress.Text = Utils.GetNetworkInterfaceIPv4((string) cboServeInterface.SelectedItem);
        }

        private async void btnStart_Click(object sender, EventArgs e) {
            ProcessTrying();
            // !!!!!!
            await Task.Run(async () => {
                try {
                    if (IsServerBehavior()) {
                        var (addr, port) = (edtServeAddress.Text.Trim(), (int) numServePort.Value);
                        var service = new GrpcServerService(addr, port);
                        await service.Serve();
                        this.InvokeAction(() => Global.InitializeServer(service)); // => ApplicationState.Running
                    } else {
                        var (addr, port, name) = (edtTargetAddress.Text.Trim(), (int) numTargetPort.Value, edtClientName.Text.Trim());
                        var service = new GrpcClientService(addr, port);
                        var id = await service.Connect(name);
                        this.InvokeAction(() => Global.InitializeClient(id, name, service)); // => ApplicationState.Running
                    }
                    this.InvokeAction(() => {
                        MainForm.Instance.Show();
                        Close();
                    });
                } catch (Exception ex) {
                    this.InvokeAction(() => ProcessFailed(ex.Message));
                }
            });
        }

        private void ProcessTrying() {
            lblState.Text = IsServerBehavior() ? "尝试监听..." : "尝试连接...";
            lblState.Visible = true;
            btnStart.Enabled = false;
            grpBehavior.Enabled = false;
            grpServer.Enabled = false;
            grpClient.Enabled = false;
        }

        private void ProcessFailed(string reason) {
            if (IsServerBehavior()) {
                this.ShowError("监听失败", $"无法监听指定的地址和端口。\n原因：{reason}");
            } else {
                this.ShowError("连接失败", $"无法连接到指定的地址和端口。\n原因：{reason}");
            }
            lblState.Visible = false;
            btnStart.Enabled = true;
            grpBehavior.Enabled = true;
            grpServer.Enabled = true;
            grpClient.Enabled = true;
        }
    }
}
