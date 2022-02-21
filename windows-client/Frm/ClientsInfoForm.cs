using System;
using System.Linq;
using System.Windows.Forms;
using LanDataTransmitter.Model;
using LanDataTransmitter.Service;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Frm {

    public partial class ClientsInfoForm : Form {

        public ClientsInfoForm() {
            InitializeComponent();
        }

        private void ClientsInfoForm_Load(object sender, EventArgs e) {
            if (Global.Behavior == ApplicationBehavior.AsClient || Global.Server.ConnectedClients.Count == 0) {
                Close();
                this.ShowError("错误", "错误操作");
                return;
            }

            edtServerAddress.Text = $"{Global.Server.Service.Address}:{Global.Server.Service.Port}";
            cboClients.Items.AddRange(Global.Server.ConnectedClients.Values.Select(obj => obj.FullDisplayName).ToObjectArray());
            if (cboClients.Items.Count > 0) {
                cboClients.SelectedIndex = 0;
                cboClients_SelectedIndexChanged(cboClients, EventArgs.Empty);
            }
        }

        private void cboClients_SelectedIndexChanged(object sender, EventArgs e) {
            var (id, _) = ClientObject.ExtractIdAndName(cboClients.SelectedItem.ToString());
            var ok = Global.Server.ConnectedClients.TryGetValue(id, out var obj);
            if (!ok) {
                return;
            }
            edtClientId.Text = obj.Id;
            edtClientName.Text = obj.Name;
            edtConnectTime.Text = Utils.FromTimestamp(obj.ConnectedTimestamp).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void btnOK_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
