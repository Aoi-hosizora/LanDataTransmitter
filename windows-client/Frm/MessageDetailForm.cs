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
            edtTime.Text = Utils.FromTimestamp(_record.Timestamp).ToString("yyyy-MM-dd HH:mm:ss");
            edtContent.Text = _record.Text;

            if (Global.Behavior == ApplicationBehavior.AsServer) {
                if (_record.IsCtS) { // received
                    edtSender.Text = _record.ClientFullDisplayName;
                    edtReceiver.Text = "server (me)";
                } else { // sent
                    edtSender.Text = "server (me)";
                    edtReceiver.Text = _record.ClientFullDisplayName;
                }
            } else {
                if (_record.IsStC) { // received
                    edtSender.Text = "server";
                    edtReceiver.Text = _record.ClientFullDisplayName + " (me)";
                } else { // sent
                    edtSender.Text = _record.ClientFullDisplayName + " (me)";
                    edtReceiver.Text = "server";
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
