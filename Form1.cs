using KeyAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static KeyAuth.api;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar;
//using DiscordMessenger;
using Newtonsoft.Json.Linq;

namespace steam
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            InitializeParticles();

            timer1.Interval = 30;
            //timer1.Tick += Timer1_Tick;
            timer1.Start();
            Paint += guna2Panel4_Paint;
            Paint += ParticleForm_Paint;
        }

        private void InitializeParticles()
        {
            for (int i = 0; i < 100; i++)
            {
                particles.Add(new Particle
                {
                    Position = new PointF(random.Next(Width), random.Next(Height)),
                    Velocity = new PointF((float)(random.NextDouble() - 0.5) * 5, (float)(random.NextDouble() - 0.5) * 5),
                    //Color = Color.FromArgb(random.Next(255), random.Next(0), random.Next(255)),
                    Lifespan = random.Next(50, 200)
                });
            }
        }

        //particles 

        private List<Particle> particles = new List<Particle>();
        private Random random = new Random();
        //private Timer timer = new Timer();

        private void UpdateParticles()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Position = new PointF(particles[i].Position.X + particles[i].Velocity.X, particles[i].Position.Y + particles[i].Velocity.Y);
                particles[i].Lifespan--;

                if (particles[i].Lifespan <= 0)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }

            if (particles.Count < 100)
            {
                particles.Add(new Particle
                {
                    Position = new PointF(random.Next(Width), random.Next(Height)),
                    Velocity = new PointF((float)(random.NextDouble() - 0.5) * 5, (float)(random.NextDouble() - 0.5) * 5),
                    //Color = Color.FromArgb(random.Next(255), random.Next(0), random.Next(255)),
                    Lifespan = random.Next(50, 200)
                });
            }
        }
        public class Particle
        {
            public PointF Position { get; set; }
            public PointF Velocity { get; set; }
            //public Color Color { get; set; }
            public int Lifespan { get; set; }
        }

        private void ParticleForm_Paint(object sender, PaintEventArgs e)
        {
            foreach (var particle in particles)
            {
                //e.Graphics.FillEllipse(new SolidBrush(particle.Color), particle.Position.X, particle.Position.Y, 5, 5);
                e.Graphics.FillEllipse(new SolidBrush(Color.Red), particle.Position.X, particle.Position.Y, 5, 5);
            }
        }

        public static api KeyAuthApp = new api(
            name: "futwin",
            ownerid: "md8zlBXYKj",
            secret: "f2c5181ea7f7339ee41e53bf1d15f3d38c325a3ac7ef8804e0790685a4bfe504",
            version: "1.2"
        );

        public void AllertMsg(string msg, NotifyAnimation.enmType type)
        {
            new NotifyAnimation().showAllert(msg, type);
        }

        public static string webhookUrl = "https://discord.com/api/webhooks/1163155333213061190/AnhIGNRAlK_9ci1qyykpxBTF4BszlE1rG1GSHbIuekP3wiiXnNvP2nEgN_GOxaEmvoWV";

        private async void webhookmsg(string msg)
        {
            var wr = WebRequest.Create(webhookUrl);
            wr.ContentType = "application/json";
            wr.Method = "POST";

            using (var sw = new StreamWriter(wr.GetRequestStream()))
                sw.Write(msg);
            wr.GetResponse();

        }

        int discorColor = 1;

        private void Form1_Load(object sender, EventArgs e)
        {
            guna2Panel4.Hide();
            string apiUrl = "http://ipinfo.io/json";

            WebClient client = new WebClient();

            string jsonResult = client.DownloadString(apiUrl);
            var locationData = JObject.Parse(jsonResult);

            string city = locationData["city"].ToString();
            string region = locationData["region"].ToString();
            string ip = locationData["ip"].ToString();
            string country = locationData["country"].ToString();



            //try
            //{
            //    new DiscordMessage()
            //    .SetUsername("Voltage Logger")
            //    .SetAvatar("")
            //    .AddEmbed()
            //        .SetTimestamp(DateTime.Now)
            //        .SetTitle("Voltage Bypass 2.1")
            //        .SetAuthor("", "https://cdn.discordapp.com/attachments/1120779691549261897/1154535948445552690/VV3.png", "https://discord.gg/BbWcNXCMfv")
            //        .SetDescription("" +

            //        "```ansi\r\n> Loader Status : \u001b[32mRunning\u001b[0m\r\n\r\n```" +

            //        "```ansi\r\n> Username : \u001b[34m" + Environment.UserName + "\u001b[0m\r\n\r\n```" +

            //        "```ansi\r\n> IP : \u001b[34m" + ip + " \u001b[0m\r\n\r\n```" +

            //        "```ansi\r\n> Country : \u001b[34m" + country + " \u001b[0m\r\n\r\n```" +

            //        "```ansi\r\n> Region : \u001b[34m" + region + " \u001b[0m\r\n\r\n```" +

            //        "```ansi\r\n> City : \u001b[34m" + city + " \u001b[0m\r\n\r\n```")


            //        .SetColor((int)discorColor)
            //        .SetFooter("Version 2.1", "https://cdn.discordapp.com/attachments/1120779691549261897/1154535948445552690/VV3.png")
            //        .Build()
            //        .SendMessage(webhookUrl);
            //}
            //catch
            //{

            //}

            //webhookmsg();
            KeyAuthApp.init();
            timer2.Start();
            timer3.Start();
          
        }
        static string Rot13(string input)
        {
            char[] charArray = input.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    char baseChar = (char)(char.IsUpper(c) ? 'A' : 'a');
                    charArray[i] = (char)((c - baseChar + 13) % 26 + baseChar);
                }
            }
            return new string(charArray);
        }
        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2Panel4.Show();

            await Task.Run(() =>
            {
                KeyAuthApp.login(guna2TextBox1.Text, guna2TextBox2.Text);
            });

            
            if (KeyAuthApp.response.success)
            {
                main Form2 = new main();
                Form2.Show();
                this.Hide();
            }
            else
            {
                AllertMsg(KeyAuthApp.response.message, NotifyAnimation.enmType.Applied);

                guna2Panel4.Hide();
            }
        }
        
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            //Process.Start("https://discord.gg/3THE5uvkRR");
            string text3 = DateTime.Now.ToString("HH:mm:ss");
            MessageBox.Show(text3);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateParticles();
            Refresh();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            
            this.Size = new Size(330, 271);
        }
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);
        private void timer3_Tick(object sender, EventArgs e)
        {
            bool isDebuggerPresent = false;
            if (isDebuggerPresent == true)
            {
                CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);

                MessageBox.Show("Debugger Attached: " + isDebuggerPresent);
            }
            
            
        }

        private void guna2Panel4_Paint(object sender, PaintEventArgs e)
        {
            foreach (var particle in particles)
            {
                //e.Graphics.FillEllipse(new SolidBrush(particle.Color), particle.Position.X, particle.Position.Y, 5, 5);
                e.Graphics.FillEllipse(new SolidBrush(Color.Red), particle.Position.X, particle.Position.Y, 5, 5);
            }
        }
    }
}
