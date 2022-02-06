using System;
using System.Drawing;
using System.Windows.Forms;

namespace LanDataTransmitter.Frm.View {

    public class PlaceholderTextBox : TextBox {

        private bool _placeholderTextEnabled;

        private string _placeholderText = "Input here...";

        public string PlaceholderText {
            get => _placeholderText;
            set {
                _placeholderText = value;
                Invalidate();
            }
        }

        public PlaceholderTextBox() {
            if (!DesignMode) {
                TextChanged += PlaceholderToggled;
                GotFocus += PlaceholderToggled;
                LostFocus += PlaceholderToggled;
            }
        }

        protected override void OnCreateControl() {
            base.OnCreateControl();
            PlaceholderToggled(null, null);
        }

        protected override void OnPaint(PaintEventArgs a) {
            var text = _placeholderTextEnabled ? PlaceholderText : Text;
            var font = Font;
            var brush = new SolidBrush(_placeholderTextEnabled ? SystemColors.GrayText : ForeColor);
            a.Graphics.DrawString(text, font, brush, new PointF(0.0F, 0.0F));
            base.OnPaint(a);
        }

        private void PlaceholderToggled(object sender, EventArgs args) {
            if (Text.Length == 0) {
                _placeholderTextEnabled = true;
                SetStyle(ControlStyles.UserPaint, true);
                Refresh();
            } else {
                _placeholderTextEnabled = false;
                SetStyle(ControlStyles.UserPaint, false);
            }
        }
    }
}
