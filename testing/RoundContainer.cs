using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundContainer : Control
{
    public RoundContainer()
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
            int radius = 50; 
            path.AddLine(rect.X, rect.Y, rect.Right, rect.Y); 
            path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom - radius);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y);
            path.CloseFigure();

            g.SetClip(path);

            
            using (Brush backBrush = new SolidBrush(Color.DarkGray))
            {
                g.FillRectangle(backBrush, rect);
            }

            
            using (Pen pen = new Pen(Color.DarkGray, 10)) 
            {
                g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y); 
                g.DrawLine(pen, rect.Right, rect.Y, rect.Right, rect.Bottom - radius); 
                g.DrawLine(pen, rect.X, rect.Bottom, rect.X, rect.Bottom - radius); 

                
                g.DrawArc(pen, rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                
                g.DrawArc(pen, rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            }
        }
    }
}
