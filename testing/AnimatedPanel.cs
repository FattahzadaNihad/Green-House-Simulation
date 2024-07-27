using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testing
{
    public class AnimatedPanel
    {
        private Timer animationTimer;
        private Panel panel;
        private bool panelVisible;
        private int targetHeight;
        private int step;


        public AnimatedPanel(Panel panel, int step = 10)
        {
            this.panel = panel;
            this.step = step;
            InitializeAnimation();
        }

        private void InitializeAnimation()
        {
            panel.MaximumSize = new System.Drawing.Size(panel.Width, panel.Height); 
            panel.Height = 0; 
            panel.Visible = false;

            animationTimer = new Timer();
            animationTimer.Interval = 10; 
            animationTimer.Tick += AnimationTimer_Tick;
        }

        public void ToggleVisibility()
        {
            if (animationTimer.Enabled)
            {
                return; 
            }

            panelVisible = !panel.Visible;

            if (panelVisible)
            {
                targetHeight = panel.MaximumSize.Height; 
                panel.Height = 0;
                panel.Visible = true;
            }
            else
            {
                targetHeight = 0;
            }

            animationTimer.Start();
        }

        public void ToggleVisibility_C(bool pp)
        {
            if (animationTimer.Enabled)
            {
                return;
            }

            panelVisible = pp;

            if (panelVisible)
            {
                targetHeight = panel.MaximumSize.Height;
                panel.Height = 0;
                panel.Visible = true;
            }
            else
            {
                targetHeight = 0;
            }

            animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (panelVisible)
            {

                if (panel.Height < panel.MaximumSize.Height)
                {
                    panel.Height += step;
                    if (panel.Height >= panel.MaximumSize.Height)
                    {
                        panel.Height = panel.MaximumSize.Height;
                        animationTimer.Stop();
                    }
                }
            }
            else
            {
                if (panel.Height > 0)
                {
                    panel.Height -= step;
                    if (panel.Height <= 0)
                    {
                        panel.Height = 0;
                        animationTimer.Stop();
                        panel.Visible = false;
                    }
                }
            }
        }
    }
}
