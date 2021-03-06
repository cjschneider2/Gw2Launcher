﻿using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Gw2Launcher.UI.Controls
{
    class FlatButton : Control
    {
        protected bool 
            redraw, 
            isHovered, 
            isSelected;

        protected BufferedGraphics buffer;

        public FlatButton()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        protected void OnRedrawRequired()
        {
            redraw = true;
            this.Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (buffer != null)
            {
                buffer.Dispose();
                buffer = null;
            }
            OnRedrawRequired();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            OnRedrawRequired();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            isHovered = false;
            OnRedrawRequired();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            OnRedrawRequired();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            OnRedrawRequired();
        }

        public bool Selected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                if (value)
                    isHovered = false;
                OnRedrawRequired();
            }
        }

        protected Color backColorHovered;
        public Color BackColorHovered
        {
            get
            {
                if (backColorHovered.IsEmpty)
                    return this.BackColor;
                return backColorHovered;
            }
            set
            {
                backColorHovered = value;
            }
        }

        protected Color foreColorHovered;
        public Color ForeColorHovered
        {
            get
            {
                if (foreColorHovered.IsEmpty)
                    return this.ForeColor;
                return foreColorHovered;
            }
            set
            {
                foreColorHovered = value;
            }
        }

        protected Color backColorSelected;
        public Color BackColorSelected
        {
            get
            {
                if (backColorSelected.IsEmpty)
                    return this.BackColor;
                return backColorSelected;
            }
            set
            {
                backColorSelected = value;
            }
        }

        protected Color foreColorSelected;
        public Color ForeColorSelected
        {
            get
            {
                if (foreColorSelected.IsEmpty)
                    return this.ForeColor;
                return foreColorSelected;
            }
            set
            {
                foreColorSelected = value;
            }
        }

        public Color BackColorCurrent
        {
            get
            {
                if (isHovered)
                    return this.BackColorHovered;
                else if (isSelected)
                    return this.BackColorSelected;
                else
                    return this.BackColor;
            }
        }

        public Color ForeColorCurrent
        {
            get
            {
                if (isHovered)
                    return this.ForeColorHovered;
                else if (isSelected)
                    return this.ForeColorSelected;
                else
                    return this.ForeColor;
            }
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            OnRedrawRequired();
        }

        protected virtual BufferedGraphics AllocateBuffer(Graphics g)
        {
            return BufferedGraphicsManager.Current.Allocate(g, this.DisplayRectangle);
        }

        protected virtual void OnPaintBuffer(Graphics g)
        {
            int w = this.Width,
                h = this.Height;

            TextRenderer.DrawText(g, this.Text, this.Font, new Rectangle(this.Padding.Left + 10, this.Padding.Top, w - 10 - this.Padding.Horizontal, h - this.Padding.Vertical), ForeColorCurrent, BackColorCurrent, TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
        }

        protected virtual void OnPaintBackgroundBuffer(Graphics g)
        {
            g.Clear(BackColorCurrent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            if (redraw)
            {
                redraw = false;

                OnPaintBuffer(buffer.Graphics);
            }

            buffer.Render(e.Graphics);

            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (redraw)
            {
                if (buffer == null)
                    buffer = AllocateBuffer(e.Graphics);

                OnPaintBackgroundBuffer(buffer.Graphics);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            isHovered = true;
            OnRedrawRequired();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            isHovered = false;
            OnRedrawRequired();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (buffer != null)
                {
                    buffer.Dispose();
                    buffer = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
