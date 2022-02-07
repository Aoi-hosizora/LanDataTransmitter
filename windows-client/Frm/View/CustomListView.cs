using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Frm.View {
    public class CustomListView : ListView {

        private readonly ColumnHeader _dummyHeader;
        private readonly ImageList _dummyImgList;

        public CustomListView() {
            base.HeaderStyle = ColumnHeaderStyle.None;
            base.OwnerDraw = true;

            _dummyHeader = new ColumnHeader { Text = "", Name = "dummyHeader" };
            base.Columns.Add(_dummyHeader);
            _dummyHeader.Width = ClientSize.Width;

            _dummyImgList = new ImageList { ImageSize = new Size(1, 42) };
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
            _oldWidth = ClientSize.Width;
        }

        private int TwiceFontHeight => Font.Height * 2 + 8;

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

        protected override void OnDrawItem(DrawListViewItemEventArgs e) {
            if (e.Item.Tag is CustomListViewObject == false) {
                e.DrawDefault = true;
                return;
            }
            var obj = (CustomListViewObject) e.Item.Tag;
            var f = TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis | TextFormatFlags.NoPrefix;
            if (!obj.AlignRight) {
                f |= TextFormatFlags.Left;
            } else {
                f |= TextFormatFlags.Right;
            }
            e.DrawText(f);
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e) {
            e.DrawDefault = false;
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e) {
            e.DrawDefault = false;
        }

        private ContextMenuStrip _oldContextMenu;

        protected override void OnSelectedIndexChanged(EventArgs e) {
            base.OnSelectedIndexChanged(e);
            var empty = SelectedIndices.Count == 0;
            if (empty && ContextMenuStrip != null) {
                _oldContextMenu = ContextMenuStrip;
                _oldContextMenu.Enabled = false;
                ContextMenuStrip = null;
            } else if (!empty && ContextMenuStrip == null && _oldContextMenu != null) {
                _oldContextMenu.Enabled = true;
                ContextMenuStrip = _oldContextMenu;
            }
        }

        public void SetSingleSelected(int index) {
            SelectedIndices.Clear();
            if (index < Items.Count) {
                SelectedIndices.Add(index);
            }
        }

        public void AppendItem(string line1, string line2, bool right = false) {
            var obj = new CustomListViewObject { Line1 = line1, Line2 = line2, AlignRight = right };
            if (!right) {
                line1 = "< " + line1;
            } else {
                line1 += " >";
            }
            line2 = line2.Replace("\n", "↴");
            var lvi = new ListViewItem { Text = line1 + Environment.NewLine + line2, Tag = obj };
            Items.Add(lvi);
            SetSingleSelected(Items.Count - 1);
        }

        public class CustomListViewObject {
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public bool AlignRight { get; set; }
        }

    }
}
