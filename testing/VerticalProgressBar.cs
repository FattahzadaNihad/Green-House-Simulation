using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class VerticalRoundProgressBar : ProgressBar
{
    public VerticalRoundProgressBar()
    {
        this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        this.DoubleBuffered = true;
    }



    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        Rectangle rect = this.ClientRectangle;

        
        using (GraphicsPath path = new GraphicsPath())
        {
            int radius = 50; // Yuvarlak köşe yarıçapı
            path.AddLine(rect.X, rect.Y, rect.Right, rect.Y); // Üst kısmı düz yap
            path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom - radius);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();

            g.SetClip(path);
        }


        using (Brush backBrush = new SolidBrush(Color.LightBlue))
        {
            g.FillRectangle(backBrush, rect);
        }


        int fillHeight = (int)(rect.Height * ((double)this.Value / this.Maximum));
        Rectangle fillRect = new Rectangle(rect.X, rect.Bottom - fillHeight, rect.Width, fillHeight);

        using (Brush fillBrush = new SolidBrush(Color.DarkBlue))
        {
            g.FillRectangle(fillBrush, fillRect);
        }


        using (GraphicsPath shinePath = new GraphicsPath())
        {
            int radius = 60;
            shinePath.AddLine(rect.X, rect.Y, rect.Right, rect.Y);
            shinePath.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom - radius);
            shinePath.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            shinePath.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            shinePath.CloseFigure();

            using (LinearGradientBrush shineBrush = new LinearGradientBrush(rect, Color.FromArgb(128, Color.White), Color.Transparent, LinearGradientMode.Vertical))
            {
                g.FillPath(shineBrush, shinePath);
            }
        }
    }
}
