using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Protobuf;

namespace LanDataTransmitter.Util {

    public static class Extensions {

        public static void InvokeAction(this Form form, Action action) {
            if (form.InvokeRequired) {
                form.Invoke(new MethodInvoker(() => form.InvokeAction(action)));
            } else {
                action?.Invoke();
            }
        }

        public static void EnableComboBoxTooltip(this ComboBox cbo, ToolTip tip) {
            cbo.SelectedIndexChanged += (sender, e) => {
                var selected = (string) cbo.SelectedItem;
                tip.SetToolTip(cbo, selected);
            };
        }

        public static void EnableLabelTooltip(this Label lbl, ToolTip tip) {
            lbl.MouseEnter += (sender, e) => {
                var selected = lbl.Text;
                tip.SetToolTip(lbl, selected);
            };
        }

        public static void EnableDefaultDrawItem(this ComboBox cbo) {
            cbo.DrawItem += (sender, e) => {
                var g = e.Graphics;
                var self = (ComboBox) sender;
                var (foreColor, backColor) = (self.ForeColor, self.BackColor);
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
                    (foreColor, backColor) = (SystemColors.HighlightText, SystemColors.Highlight);
                }
                e.DrawBackground();
                g.FillRectangle(new SolidBrush(backColor), e.Bounds);
                g.DrawString(self.Items[e.Index].ToString(), self.Font, new SolidBrush(foreColor), new Point(e.Bounds.X, e.Bounds.Y));
            };
        }

        public static string UnifyToCrlf(this string s) {
            var re = new Regex(@"(?<!\r)\n");
            return re.Replace(s, "\r\n");
        }

        public static object[] ToObjectArray<T>(this IEnumerable<T> i) {
            return i.Cast<object>().ToArray();
        }

        public static void ShowError(this Form _, string title, string message) {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowInfo(this Form _, string title, string message) {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool ShowQuestion(this Form _, string title, string message, bool defaultYes = true) {
            var ok = MessageBox.Show(message, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, defaultYes ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2);
            return ok == DialogResult.Yes;
        }

        public static async Task<Task> WithTimeout(this Task task, TimeSpan duration) {
            return await Task.WhenAny(task, Task.Delay(duration)) == task
                ? task // task complete
                : Task.CompletedTask; // timeout
        }

        public static async Task<ByteString> ReadFileToByteString(this byte[] buf, FileStream file, int offset) {
            var len = await file.ReadAsync(buf, offset, buf.Length);
            if (len <= 0) {
                return ByteString.Empty;
            }
            return ByteString.CopyFrom(buf, 0, len);
        }

        public static async Task WriteByteStringToFile(this ByteString bs, FileStream fs) {
            var buf = bs.ToArray();
            if (buf.Length > 0) {
                await fs.WriteAsync(buf, 0, buf.Length);
            }
        }
    }
}
