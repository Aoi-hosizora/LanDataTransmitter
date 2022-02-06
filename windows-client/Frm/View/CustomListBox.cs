using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LanDataTransmitter.Frm.View {

    public sealed class CustomListBox : ListBox {

        public CustomListBox() {
            if (!DesignMode) {
                DrawMode = DrawMode.OwnerDrawFixed;
            }
        }

        private int _hoverIndex = -1;

        protected override void OnMouseMove(MouseEventArgs e) {
            if (PointOutOfRange(e.Location)) {
                if (_hoverIndex != -1) {
                    Invalidate(GetItemRectangle(_hoverIndex)); // redraw the last line
                    _hoverIndex = -1; // empty
                }
            } else {
                var index = IndexFromPoint(e.Location);
                if (index != _hoverIndex) {
                    if (_hoverIndex != -1) {
                        Invalidate(GetItemRectangle(_hoverIndex)); // redraw the last line
                    }
                    _hoverIndex = index;
                    if (_hoverIndex != -1) {
                        Invalidate(GetItemRectangle(index)); // redraw the current line
                    }
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            if (_hoverIndex != -1) {
                Invalidate(GetItemRectangle(_hoverIndex)); // redraw the last line
                _hoverIndex = -1; // empty
            }
            base.OnMouseLeave(e);
        }

        private bool PointOutOfRange(Point p) {
            if (Items.Count == 0) {
                return true;
            }
            var rect = GetItemRectangle(Items.Count - 1);
            return p.Y > rect.Top + rect.Height;
        }

        private readonly Color _hoverBackColor = Color.FromArgb(229, 243, 251);
        private readonly Color _selectBackColor = Color.FromArgb(209, 232, 255);
        private readonly Color _selectBorderColor = Color.FromArgb(102, 167, 232);

        protected override void OnDrawItem(DrawItemEventArgs e) {
            if (e.Index < 0 || e.Index >= Items.Count) {
                return;
            }
            if ((e.State & DrawItemState.Disabled) == DrawItemState.Disabled) {
                return;
            }
            var g = e.Graphics;
            var b = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height);
            g.SmoothingMode = SmoothingMode.HighQuality;

            e.DrawBackground();
            if (e.Index == _hoverIndex) { // hovering
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) { // hovering + selected
                    g.FillRectangle(new SolidBrush(_selectBackColor), b);
                    g.DrawRectangle(new Pen(_selectBorderColor), b);
                } else { // hovering
                    g.FillRectangle(new SolidBrush(_hoverBackColor), b);
                    g.DrawRectangle(new Pen(_hoverBackColor), b);
                }
            } else if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) { // selected
                g.FillRectangle(new SolidBrush(_selectBackColor), b);
                g.DrawRectangle(new Pen(_selectBackColor), b);
            } else { // normal
                g.FillRectangle(new SolidBrush(BackColor), b);
                g.DrawRectangle(new Pen(BackColor), b);
            }
            var text = Items[e.Index].ToString();
            g.DrawString(text, e.Font, new SolidBrush(ForeColor), b, StringFormat.GenericDefault);
        }

    }
}
