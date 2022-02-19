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

        private History _history;

        private void InitForm_Load(object sender, EventArgs e) {
            // ui
            Height -= grpClient.Top - grpServer.Top;
            grpClient.Top = grpServer.Top;
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                rdoServer.Checked = true;
            } else {
                rdoClient.Checked = true;
            }
            cboServeInterface.EnableComboBoxTooltip(tipMain);
            cboTargetAddress.EnableDefaultDrawItem();
            cboClientName.EnableDefaultDrawItem();

            // interface
            var interfaces = Utils.GetNetworkInterfaces();
            cboServeInterface.Items.AddRange(interfaces.ToArray<object>());
            cboServeInterface.SelectedIndex = 0;

            // history
            _history = new History();
            numServePort.Value = _history.GetServedPorts().First();
            cboTargetAddress.Items.AddRange(_history.GetTargetAddresses().Cast<object>().ToArray());
            cboTargetAddress.Text = cboTargetAddress.Items[0].ToString();
            numTargetPort.Value = _history.GetTargetPorts().First();
            cboClientName.Items.AddRange(_history.GetClientNames().Cast<object>().ToArray());
            cboClientName.Text = cboClientName.Items[0].ToString();

            // state
            Global.State = ApplicationState.Preparing;
        }

        private void btnExit_Click(object sender, EventArgs e) {
            Close();
        }

        private bool IsServerBehavior => rdoServer.Checked;

        private void rdoBehavior_CheckedChanged(object sender, EventArgs e) {
            var isServer = IsServerBehavior;
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
                    if (IsServerBehavior) {
                        var (addr, port) = (edtServeAddress.Text.Trim(), (int) numServePort.Value);
                        var service = new GrpcServerService(addr, port);
                        await service.Serve();
                        Global.InitializeServer(service); // => ApplicationState.Running
                        _history.AddServerHistory(service.Port);
                        _history.Save();
                    } else {
                        var (addr, port, name) = (cboTargetAddress.Text.Trim(), (int) numTargetPort.Value, cboClientName.Text.Trim());
                        var service = new GrpcClientService(addr, port);
                        var id = await service.Connect(name);
                        Global.InitializeClient(id, name, service); // => ApplicationState.Running
                        _history.AddClientHistory(service.Address, service.Port, name);
                        _history.Save();
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
            lblState.Text = IsServerBehavior ? "尝试监听..." : "尝试连接...";
            lblState.Visible = true;
            btnStart.Enabled = false;
            grpBehavior.Enabled = false;
            grpServer.Enabled = false;
            grpClient.Enabled = false;
        }

        private void ProcessFailed(string reason) {
            if (IsServerBehavior) {
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
