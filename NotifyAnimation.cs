using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace steam
{
    public partial class NotifyAnimation : Form
    {
        public NotifyAnimation()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern bool SetWindowDisplayAffinity(IntPtr hwnd, uint affinity);

        private int x;
        private int y;

        private NotifyAnimation.enmAction action;

        private const uint WDA_EXCLUDEFROMCAPTURE = 17U;


        public enum enmAction
        {
            wait,
            start,
            close
        }

        public enum enmType
        {
            Applying,
            Applied,
            Error,
            Info
        }

        private void NotifyAnimation_Load(object sender, EventArgs e)
        {
            if (main.sounds)
            {
                Console.Beep(300, 200);

            }

            this.TopMost = true;
            this.ShowInTaskbar = false;

            //IntPtr hwnd = this.Handle;
            //SetWindowDisplayAffinity(hwnd, 0x11);



        }

        public void showAllert(string msg, NotifyAnimation.enmType type)
        {
            base.Opacity = 0.0;
            base.StartPosition = FormStartPosition.Manual;
            for (int i = 1; i < 10; i++)
            {
                string name = "alert" + i;
                if ((NotifyAnimation)Application.OpenForms[name] == null)
                {
                    base.Name = name;
                    x = Screen.PrimaryScreen.WorkingArea.Width - base.Width + 15;
                    y = Screen.PrimaryScreen.WorkingArea.Height - base.Height * i - 5 * i;
                    base.Location = new Point(x, y);
                    break;
                }
            }
            x = Screen.PrimaryScreen.WorkingArea.Width - base.Width - 5;
            switch (type)
            {
                case enmType.Applying:
                    lblMsg.Text = msg;
                    break;
                case enmType.Applied:
                    lblMsg.Text = msg;
                    break;
                case enmType.Error:
                    lblMsg.Text = msg;
                    break;
            }
            
            Show();
            action = enmAction.start;
            timer1.Interval = 5;
            timer1.Start();
        }
        

        private async void timer1_Tick(object sender, EventArgs e)
        {
            switch (action)
            {
                case enmAction.wait:
                    await Task.Delay(4000);
                    action = enmAction.close;
                    break;
                case enmAction.start:
                    timer1.Interval = 5;
                    base.Opacity += 0.1;
                    if (x < base.Location.X)
                    {
                        base.Left--;
                    }
                    else if (base.Opacity == 1.0)
                    {
                        action = enmAction.wait;
                    }
                    break;
                case enmAction.close:
                    timer1.Interval = 5;
                    base.Opacity -= 0.1;
                    base.Left -= 3;
                    if (base.Opacity == 0.0)
                    {
                        Close();
                    }
                    break;
            }
        }
    }
}
