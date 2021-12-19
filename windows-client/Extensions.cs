using System;
using System.Windows.Forms;

namespace LanDataTransmitter {

    public static class Extensions {

        public static void InvokeAction(this Form form, Action action) {
            if (form.InvokeRequired) {
                form.Invoke(new MethodInvoker(() => form.InvokeAction(action)));
            } else {
                action?.Invoke();
            }
        }

        public static void AddToLast(this ListBox lsb, object item) {
            lsb.Items.Add(item);
            lsb.ClearSelected();
            lsb.SelectedIndex = lsb.Items.Count - 1;
        }

        public static void ShowError(this Form _, string title, string message) {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowInfo(this Form _, string title, string message) {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
