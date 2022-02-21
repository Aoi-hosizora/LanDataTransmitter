using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LanDataTransmitter.Model;
using LanDataTransmitter.Service;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Frm.View {

    public class MessageRecordListView : ListView {

        #region Appearance

        private readonly ColumnHeader _dummyHeader;
        private readonly ImageList _dummyImgList;

        public MessageRecordListView() {
            base.HeaderStyle = ColumnHeaderStyle.None;
            base.OwnerDraw = true;

            _dummyHeader = new ColumnHeader { Text = "", Name = "dummyHeader" };
            base.Columns.Add(_dummyHeader);
            _dummyHeader.Width = ClientSize.Width;

            _dummyImgList = new ImageList { ImageSize = new Size(1, TwiceFontHeight) };
            base.SmallImageList = _dummyImgList;
            _dummyImgList.ImageSize = new Size(1, TwiceFontHeight);
        }

        [Browsable(false)]
        public new ColumnHeaderStyle HeaderStyle {
            get => base.HeaderStyle;
            set => base.HeaderStyle = value;
        }

        [Browsable(false)]
        public new bool OwnerDraw {
            get => base.OwnerDraw;
            set => base.OwnerDraw = value;
        }

        [Browsable(false)]
        public new ColumnHeaderCollection Columns => base.Columns;

        [Browsable(false)]
        public new ImageList SmallImageList {
            get => base.SmallImageList;
            set => base.SmallImageList = value;
        }

        private int _oldWidth;

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            _dummyHeader.Width = ClientSize.Width;
            if (ClientSize.Width <= _oldWidth) { // getting narrow
                NativeMethods.ShowScrollBar(Handle, (int) NativeMethods.SB_HORZ, false);
            }
            Refresh();
            _oldWidth = ClientSize.Width;
        }

        private int TwiceFontHeight => Font.Height * 2 + 10;

        protected override void OnFontChanged(EventArgs e) {
            base.OnFontChanged(e);
            _dummyImgList.ImageSize = new Size(1, TwiceFontHeight);
        }

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (!DesignMode) {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6) {
                    NativeMethods.SetWindowTheme(Handle, "Explorer", null);
                }
            }
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e) {
            e.DrawDefault = false;
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e) {
            e.DrawDefault = false;
        }

        #endregion

        protected override void OnDrawItem(DrawListViewItemEventArgs e) {
            if (!(e.Item.Tag is MessageRecordTag tag)) {
                e.DrawDefault = true;
                return;
            }
            var flag = TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis | TextFormatFlags.NoPrefix;
            if (tag.IsReceived) {
                flag |= TextFormatFlags.Left;
            } else {
                flag |= TextFormatFlags.Right;
            }
            e.DrawText(flag);
        }

        private void UpdateMenusState() {
            if (ContextMenuStrip == null) {
                return;
            }
            var empty = SelectedIndices.Count == 0;
            foreach (ToolStripItem t in ContextMenuStrip.Items) {
                if (t is ToolStripMenuItem item && (string) item.Tag == "depend") {
                    item.Enabled = !empty;
                }
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e) {
            base.OnSelectedIndexChanged(e);
            UpdateMenusState();
        }

        protected override void OnVisibleChanged(EventArgs e) {
            base.OnVisibleChanged(e);
            if (Visible && !Disposing) {
                UpdateMenusState();
            }
        }

        public void SetSingleSelected(int index) {
            SelectedIndices.Clear();
            if (index < Items.Count) {
                SelectedIndices.Add(index);
            }
            Items[index].EnsureVisible();
        }

        private const string NewLineSymbol = "↴";

        public void AppendItem(MessageRecord r) {
            var time = Utils.FormatTimeForShow(Utils.FromTimestamp(r.Timestamp));
            var obj = new MessageRecordTag { Record = r };
            if (Global.Behavior == ApplicationBehavior.AsServer) {
                obj.InfoLine = $"{r.ClientDisplayName} ({time})";
                obj.IsReceived = r.IsCtS;
            } else {
                obj.InfoLine = $"server ({time})";
                obj.IsReceived = r.IsStC;
            }

            var (line1, line2) = (obj.InfoLine, r.Text);
            line1 = obj.IsReceived ? "→ " + line1 : line1 + " ←";
            line2 = line2.Replace("\r\n", NewLineSymbol).Replace("\n", NewLineSymbol);
            var lvi = new ListViewItem { Text = line1 + Environment.NewLine + line2, Tag = obj };
            Items.Add(lvi);
            SetSingleSelected(Items.Count - 1);
        }

        public class MessageRecordTag {
            public MessageRecord Record { get; set; }
            public string InfoLine { get; set; }
            public bool IsReceived { get; set; }
        }

    } // class MessageRecordListView
}
