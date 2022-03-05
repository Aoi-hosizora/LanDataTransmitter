using System;
using System.Windows.Forms;
using LanDataTransmitter.Model;
using LanDataTransmitter.Service;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Frm {

    public partial class MessageDetailForm : Form {

        private readonly MessageRecord _record;
        private readonly int _indexInList;

        public MessageDetailForm(MessageRecord record, int index) {
            InitializeComponent();
            _record = record;
            _indexInList = index;
        }

        private void MessageDetailForm_Load(object sender, EventArgs e) {
            edtID.Text = _record.MessageId;
            edtTime.Text = Utils.FromTimestamp(_record.Text.CreatedTimestamp).ToString("yyyy-MM-dd HH:mm:ss");
            edtContent.Text = _record.Text.Text;

            var client = _record.ClientFullDisplayName;
            var server = "server";
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                server += " (me)";
                var connected = Global.Server.ConnectedClients.ContainsKey(_record.ClientId);
                if (!connected) {
                    client += " (disconnected)";
                }
                if (_record.IsCts) { // received
                    edtSender.Text = client;
                    edtReceiver.Text = server;
                } else { // sent
                    edtSender.Text = server;
                    edtReceiver.Text = client;
                }
            } else {
                client += " (me)";
                if (_record.IsStc) { // received
                    edtSender.Text = server;
                    edtReceiver.Text = client;
                } else { // sent
                    edtSender.Text = client;
                    edtReceiver.Text = server;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnNavigate_Click(object sender, EventArgs e) {
            MainForm.Instance.Activate();
            MainForm.Instance.SelectMessageRecordInList(_indexInList);
        }
    }
}
