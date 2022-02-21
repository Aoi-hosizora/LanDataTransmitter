using System;
using System.Windows.Forms;
using LanDataTransmitter.Service;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Frm {

    public partial class ClientInfoForm : Form {

        public ClientInfoForm() {
            InitializeComponent();
        }

        private void ClientInfoForm_Load(object sender, EventArgs e) {
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                Close();
                return;
            }
            edtClientId.Text = Global.Client.Obj.Id;
            edtClientName.Text = Global.Client.Obj.Name;
            edtConnectTime.Text = Utils.FromTimestamp(Global.Client.Obj.ConnectedTimestamp).ToString("yyyy-MM-dd HH:mm:ss");
            edtServerAddress.Text = $"{Global.Client.Service.Address}:{Global.Client.Service.Port}";
        }

        private void btnOK_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
