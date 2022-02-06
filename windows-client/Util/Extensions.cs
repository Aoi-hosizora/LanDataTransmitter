using System;
using System.Windows.Forms;

namespace LanDataTransmitter.Util {

    public static class Extensions {

        public static void InvokeAction(this Form form, Action action) {
            if (form.InvokeRequired) {
                form.Invoke(new MethodInvoker(() => form.InvokeAction(action)));
            } else {
                action?.Invoke();
            }
        }

        public static void AddToLast(this ListBox list, object item) {
            list.Items.Add(item);
            list.ClearSelected();
            list.SelectedIndex = list.Items.Count - 1;
        }

        public static void ShowError(this Form _, string title, string message) {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowInfo(this Form _, string title, string message) {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool ShowQuestion(this Form _, string title, string message) {
            var ok = MessageBox.Show(message, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            return ok == DialogResult.Yes;
        }
    }
}
