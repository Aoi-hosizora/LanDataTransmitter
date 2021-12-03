using System.Collections.Generic;
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

        private void InitForm_Load(object sender, System.EventArgs e) {
            var interfaces = Utils.GetNetworkInterfaces();
            interfaces.Insert(0, "All interfaces (serve on 0.0.0.0)");
            cbbInterface.Items.AddRange(interfaces.ToArray());
            cbbInterface.SelectedIndex = 0;
        }

        private void cbbInterface_SelectedIndexChanged(object sender, System.EventArgs e) {
            var selected = cbbInterface.SelectedItem as string;
            toolTip.SetToolTip(cbbInterface, selected);
            if (selected.StartsWith("All interfaces")) {
                edtAddress.Text = "0.0.0.0";
            } else {
                edtAddress.Text = Utils.GetNetworkInterfaceIPv4(selected);
            }
        }

        private void rbtnServer_CheckedChanged(object sender, System.EventArgs e) {
            if (rbtnServer.Checked) {
                cbbInterface.Enabled = true;
                edtAddress.Enabled = true;
                numPort.Enabled = true;
            }
        }

        private void rbtnClient_CheckedChanged(object sender, System.EventArgs e) {
            if (rbtnClient.Checked) {
                cbbInterface.Enabled = false;
                edtAddress.Enabled = true;
                numPort.Enabled = true;
            }
        }

        private void buttonExit_Click(object sender, System.EventArgs e) {
            Close();
        }

        private void buttonStart_Click(object sender, System.EventArgs e) {
            lblHint.Text = rbtnServer.Checked ? "尝试监听..." : "尝试连接...";
            lblHint.Visible = true;
        }
    }
}
