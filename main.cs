using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static steam.Form1;
using static steam.main;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Microsoft.Win32;
using System.Management;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.ServiceProcess;
using Application = System.Windows.Forms.Application;
using static Memory.Mem;
using Memory;
using static Memory.Imps;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Net.NetworkInformation;

namespace steam
{
    public partial class main : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetAsyncKeyState(int vKey);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);

        [DllImport("user32.dll")]
        public static extern bool SetWindowDisplayAffinity(IntPtr hwnd, uint affinity);

        [DllImport("KERNEL32.DLL")]
        public static extern IntPtr CreateToolhelp32Snapshot(uint flags, uint processid);
        [DllImport("KERNEL32.DLL")]
        public static extern int Process32First(IntPtr handle, ref ProcessEntry32 pe);
        [DllImport("KERNEL32.DLL")]
        public static extern int Process32Next(IntPtr handle, ref ProcessEntry32 pe);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        [DllImport("ntdll.dll", PreserveSig = false)]
        public static extern void NtSuspendProcess(IntPtr processHandle);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

        public struct ProcessEntry32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;
        }

        public main()
        {
            InitializeComponent(); 
            InitializeParticles();
            timer2.Interval = 30;
            //timer1.Tick += Timer1_Tick;
            timer2.Start();

            Paint += guna2Panel4_Paint;
            Paint += guna2Panel3_Paint;
            Paint += guna2Panel2_Paint;
            Paint += guna2Panel1_Paint;
        }
        private List<Particle> particles = new List<Particle>();
        private Random random = new Random();

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
        public static bool sounds = false;
        public class Particle
        {
            public PointF Position { get; set; }
            public PointF Velocity { get; set; }
            //public Color Color { get; set; }
            public int Lifespan { get; set; }
        }
        private void main_Load(object sender, EventArgs e)
        {
            guna2Panel4.Hide();
            label7.Text = Form1.KeyAuthApp.expirydaysleft();
            timer1.Start();
            guna2Panel1.Hide();
            guna2Panel2.Hide();
            guna2Panel3.Hide();
            if (ShowInTaskbar == true)
            {
                guna2CustomCheckBox2.Checked = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void guna2CustomCheckBox1_Click(object sender, EventArgs e)
        {
            IntPtr hwnd = this.Handle;

            if (guna2CustomCheckBox1.Checked == true)
            {
                SetWindowDisplayAffinity(hwnd, 0x11);
            }
            else
            {
                SetWindowDisplayAffinity(hwnd, 0);
            }
        }

        private void guna2CustomCheckBox2_Click(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox2.Checked == true)
            {
                this.ShowInTaskbar = true;
            }
            else
            {
                this.ShowInTaskbar = false;
            }
        }

        private void guna2CustomCheckBox3_Click(object sender, EventArgs e)
        {
            if (sounds == true)
            {
                sounds = true;
            }
            else
            {
                sounds = false;
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            
        }

        public static bool userpanel2 = false;
        public static bool settingspanel2 = false;
        public static bool homepanel2 = false;

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            var userpanel = guna2Panel2;
            var settingspanel = guna2Panel1;
            var homepanel = guna2Panel3;

            if (guna2Button4.Checked == true)
            {
                userpanel.Hide();
                homepanel.Hide();
                userpanel2 = false;
                homepanel2 = false;
                settingspanel2 = true;
                settingspanel.Show();
            }
            if (guna2Button3.Checked ==  true)
            {
                homepanel.Hide();
                settingspanel.Hide();
                settingspanel2 = false;
                homepanel2 = false;
                userpanel2 = true;
                userpanel.Show();
            }
            if (guna2Button5.Checked == true)
            {
                userpanel.Hide();
                settingspanel.Hide();
                settingspanel2 = false;
                userpanel2 = false;
                homepanel2 = true;
                homepanel.Show();
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            UpdateParticles();
            Refresh();
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {
            if (homepanel2)
            {
                foreach (var particle in particles)
                {
                    //e.Graphics.FillEllipse(new SolidBrush(particle.Color), particle.Position.X, particle.Position.Y, 5, 5);
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), particle.Position.X, particle.Position.Y, 5, 5);
                }
            }
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {
            if (userpanel2)
            {
                foreach (var particle in particles)
                {
                    //e.Graphics.FillEllipse(new SolidBrush(particle.Color), particle.Position.X, particle.Position.Y, 5, 5);
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), particle.Position.X, particle.Position.Y, 5, 5);
                }
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            if (settingspanel2)
            {
                foreach (var particle in particles)
                {
                    //e.Graphics.FillEllipse(new SolidBrush(particle.Color), particle.Position.X, particle.Position.Y, 5, 5);
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), particle.Position.X, particle.Position.Y, 5, 5);
                }
            }
        }
        public void AllertMsg(string msg, NotifyAnimation.enmType type)
        {
            new NotifyAnimation().showAllert(msg, type);
        }

       
        public static void cmd(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.StandardInput.WriteLine(command);
            process.Close();
        }
        public string GetProcID(int index)
        {
            string result = "";
            checked
            {
                if (index == 1 || index == 0)
                {
                    IntPtr intPtr = IntPtr.Zero;
                    uint num = 0u;
                    IntPtr intPtr2 = CreateToolhelp32Snapshot(2u, 0u);
                    if ((int)intPtr2 > 0)
                    {
                        ProcessEntry32 pe = default(ProcessEntry32);
                        pe.dwSize = (uint)Marshal.SizeOf(pe);
                        for (int num2 = Process32First(intPtr2, ref pe); num2 == 1; num2 = Process32Next(intPtr2, ref pe))
                        {
                            IntPtr intPtr3 = Marshal.AllocHGlobal((int)pe.dwSize);
                            Marshal.StructureToPtr(pe, intPtr3, fDeleteOld: true);
                            object obj = Marshal.PtrToStructure(intPtr3, typeof(ProcessEntry32));
                            ProcessEntry32 processEntry = ((obj != null) ? ((ProcessEntry32)obj) : default(ProcessEntry32));
                            Marshal.FreeHGlobal(intPtr3);
                            if (processEntry.szExeFile.Contains("lsass.exe") && processEntry.cntThreads > num)
                            {
                                num = processEntry.cntThreads;
                                intPtr = (IntPtr)processEntry.th32ProcessID;
                            }
                            if (processEntry.szExeFile.Contains("lsass") && processEntry.cntThreads > num)
                            {
                                num = processEntry.cntThreads;
                                intPtr = (IntPtr)processEntry.th32ProcessID;
                            }
                            if (processEntry.szExeFile.Contains("lsass.exe") && processEntry.cntThreads > num)
                            {
                                num = processEntry.cntThreads;
                                intPtr = (IntPtr)processEntry.th32ProcessID;
                            }
                            if (processEntry.szExeFile.Contains("lsass.exe") && processEntry.cntThreads > num)
                            {
                                num = processEntry.cntThreads;
                                intPtr = (IntPtr)processEntry.th32ProcessID;
                            }
                            if (processEntry.szExeFile.Contains("lsass.exe") && processEntry.cntThreads > num)
                            {
                                num = processEntry.cntThreads;
                                intPtr = (IntPtr)processEntry.th32ProcessID;
                            }
                            if (processEntry.szExeFile.Contains("lsass.exe") && processEntry.cntThreads > num)
                            {
                                num = processEntry.cntThreads;
                                intPtr = (IntPtr)processEntry.th32ProcessID;
                            }
                            if (processEntry.szExeFile.Contains("lsass") && processEntry.cntThreads > num)
                            {
                                num = processEntry.cntThreads;
                                intPtr = (IntPtr)processEntry.th32ProcessID;
                            }
                        }
                    }
                    result = Convert.ToString(intPtr);
                    PID.Text = Convert.ToString(intPtr);
                }
                return result;
            }
        }
        private void lsassl()
        {
            GetProcID(1);
            Rep("73 6b 72 69 70 74 2e 67 67", "63 68 72 6f 6d 65 2e 65 78 65");
            Thread.Sleep(1000);
            Rep("00 2e 00 73 00 6b 00 72 00 69 00 70 00 74 00 2e 00 67 00 67", "63 68 72 6f 6d 65 2e 65 78 65");
            Thread.Sleep(1000);
            Rep("73 00 6b 00 72 00 69 00 70 00 74 00 2e 00 67 00 67 00", "63 68 72 6f 6d 65 2e 65 78 65");
            Thread.Sleep(1000);
            Rep("70 00 74 00 2e 67 67", "63 68 72 6f 6d 65 2e 65 78 65");
            Thread.Sleep(1000);
            Rep("70 00 74 00 2e 00 67 00 67 00", "63 68 72 6f 6d 65 2e 65 78 65");
            Thread.Sleep(1000);
            Rep("6b 00 65 00 79 00 61 00 75 00 74 00 68 00 2e 00 77 00 69 00 6e 00", "63 68 72 6f 6d 65 2e 65 78 65");
            Thread.Sleep(1000);
            Rep("6b 00 65 00 79 00 61 00 75 00 74 00 68 00 2e 00 77 00 69 00 6e 00", "63 68 72 6f 6d 65 2e 65 78 65");
            Thread.Sleep(1000);
            Rep("6b 65 79 61 75 74 68 2e 77 69 6e", "63 68 72 6f 6d 65 2e 65 78 65");
            Thread.Sleep(1000);
            Rep("0d 2a 2e 6b 65 79 61 75 74 68 2e 77 69 6e", "63 68 72 6f 6d 65 2e 65 78 65");
            Thread.Sleep(1000);
            Rep("6b 65 79 61 75 74 68 2e 77 69 6e", "63 68 72 6f 6d 65 2e 65 78 65");
        }
        public Mem MemLib = new Mem();

        public async void Rep(string original, string replace)
        {
            try
            {
                MemLib.OpenProcess(Convert.ToInt32(PID.Text));
                IEnumerable<long> obj = await MemLib.AoBScan(0L, 140737488355327L, original, true, true, "");
                long num = obj.FirstOrDefault();
                MemoryProtection val = default(MemoryProtection);


                foreach (long item in obj)
                {

                    MemLib.ChangeProtection(item.ToString("X"), (MemoryProtection)4, out val, "");
                    MemLib.WriteMemory(item.ToString("X"), "bytes", replace, "", (Encoding)null);
                }
                if (num != 0L)
                {
                    if (sounds == true)
                    {
                        Console.Beep(300, 200);
                    }
                }
            }
            catch
            {   }
        }
        public static void command(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Verb = "runas";
            process.Start();
            process.StandardInput.WriteLine(command);
            process.Close();
        }

        private void servicestopped()
        {
            string[] array = new string[4] { "pcasvc", "bam", "diagtrack", "dps" };
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController serviceController in services)
            {
                string[] array2 = array;
                foreach (string text in array2)
                {
                    if (serviceController.ServiceName.ToLower() == text)
                    {
                        foreach (var serviceControllerss in Process.GetProcessesByName(serviceController.ServiceName))
                        {
                            serviceControllerss.Kill();
                            serviceControllerss.WaitForExit();
                        }
                    }
                }
            }
        }

        private void regedits()
        {
            string name = "SYSTEM\\MountedDevices";
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name, writable: true);
            string[] valueNames = registryKey.GetValueNames();
            foreach (string text in valueNames)
            {
                if (text.Contains("#{"))
                {
                    registryKey.DeleteValue(text);
                }
            }
            Registry.LocalMachine.DeleteSubKeyTree("SYSTEM\\ControlSet001\\Control\\Session Manager\\AppCompatCache", throwOnMissingSubKey: false);
            Registry.LocalMachine.DeleteSubKeyTree("SYSTEM\\ControlSet001\\Control\\Session Manager\\AppCompatCache", throwOnMissingSubKey: false);
            Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\CurrentVersion\\TrayNotify", throwOnMissingSubKey: false);
            Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Microsoft\\Windows\\Shell\\BagMRU", throwOnMissingSubKey: false);
            Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\\\Windows Search\\VolumeInfoCache\\\\H\\\\", writable: true);
            command("REG DELETE HKCU\\SOFTWARE\\AMD\\HKIDs /f");
            command("REG ADD HKCU\\SOFTWARE\\AMD\\HKIDs");
            command("REG DELETE \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows Search\\VolumeInfoCache\\H:\" /F");
        }

        public static string timer()
        {
            using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher(new SelectQuery("SELECT LastBootUpTime FROM Win32_OperatingSystem WHERE Primary='true'")).Get().GetEnumerator())
            {
                if (managementObjectEnumerator.MoveNext())
                {
                    return ManagementDateTimeConverter.ToDateTime(((ManagementObject)managementObjectEnumerator.Current).Properties["LastBootUpTime"].Value.ToString()).ToLongTimeString();
                }
            }
            return null;
        }

        private void servicestart()
        {
            string[] array = new string[5] { "pcasvc", "bam", "diagtrack", "CDPUserSvc_17a41d", "dps" };
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController serviceController in services)
            {
                string[] array2 = array;
                foreach (string text in array2)
                {
                    if (!(serviceController.ServiceName.ToLower() == text))
                    {
                        continue;
                    }
                    Thread.Sleep(1000);
                    ServiceController serviceController2 = new ServiceController(text);
                    if (serviceController2.Status == ServiceControllerStatus.Running)
                    {
                        serviceController2.Stop();
                        serviceController2.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10.0));
                    }
                    serviceController2.Start();
                }
            }
        }


        private void dnscache()
        {
            string[] array = new string[1] { "dnscache" };
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController serviceController in services)
            {
                string[] array2 = array;
                foreach (string text in array2)
                {
                    if (serviceController.ServiceName.ToLower() == text)
                    {
                        foreach (var serviceControllerss in Process.GetProcessesByName(serviceController.ServiceName))
                        {
                            serviceControllerss.Kill();
                            serviceControllerss.WaitForExit();
                        }


                        //Process.Start(new ProcessStartInfo
                        //{

                        //    FileName = "TASKKILL",
                        //    Arguments = "/F /FI \"SERVICES eq " + serviceController.ServiceName + "\"",
                        //    CreateNoWindow = true,
                        //    UseShellExecute = false
                        //}).WaitForExit();
                        Thread.Sleep(1000);
                    }
                }
            }
        }
        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            
            AllertMsg("Injecting...", NotifyAnimation.enmType.Error);
            guna2Panel4.Show();
            await Task.Run(() =>
            {
                try
                {
                    string tempPath = @"C:\Users\Administrator\AppData\Local\Temp";

                    Environment.SetEnvironmentVariable("TEMP", tempPath, EnvironmentVariableTarget.User);

                }
                catch (Exception)
                {
                }
                servicestopped();
                byte[] bytes = Form1.KeyAuthApp.download("570331"); // qui per il download usa keyauth api
                File.WriteAllBytes("C:\\Windows\\USBDeview.exe", bytes);
                string fileName = "C:\\Windows\\USBDeview.exe";
                Process.Start(new ProcessStartInfo
                {
                    FileName = fileName,
                    WindowStyle = ProcessWindowStyle.Hidden
                });
            });
            guna2Panel4.Hide();
            AllertMsg("Skript Injected!", NotifyAnimation.enmType.Error);
        }

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            AllertMsg("Unloading...", NotifyAnimation.enmType.Error);
            guna2Panel4.Show();
            await Task.Run(delegate
            {
                try
                {
                    File.Delete("C:\\Windows\\prefetch\\CHXSMARTSCREEN.EXE-9DF92DA5.pf");
                    File.Delete("C:\\Windows\\prefetch\\SMARTSCREEN.EXE-3A39E32D.pf");
                }
                catch
                {   }
                try
                {
                    File.Delete("C:\\Windows\\USBDeview.exe");
                    File.Delete("C:\\Windows\\USBDeview.dll");
                }
                catch
                {   }
                string[] array = new string[1] { "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\RunMRU" };
                Process process = new Process();
                string[] array2 = array;
                string[] array3 = array2;
                foreach (string text in array3)
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = "/C reg delete " + text + " /f && reg add " + text + " /f";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.Start();
                }
                cmd("del /f /q /s C:\\Users\\%username%\\AppData\\Local\\Microsoft\\CLR_v4.0\\UsageLogs");
                cmd("del /f /q /s C:\\Users\\%username%\\AppData\\Local\\Microsoft\\CLR_v4.0_32\\UsageLogs");
                regedits();
                process.WaitForExit();
                Thread.Sleep(1200);
                process.StartInfo.FileName = "rundll32.exe";
                process.StartInfo.Arguments = "kernel32.dll,BaseFlushAppcompatCache";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
                Thread.Sleep(1200);
                process.StartInfo.FileName = "rundll32.exe";
                process.StartInfo.Arguments = "apphelp.dll,ShimFlushCache";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
                Thread.Sleep(1200);
                regedits();
                Thread.Sleep(2500);
                command("del /s /f /q C:\\Users\\%username%\\AppData\\Local\\CrashDumps\\*.*");
                try
                {
                    cmd("vssadmin delete shadows /for=c: /all ");
                    cmd("vssadmin create shadows /for=c: /all ");
                    cmd("code /Shadow={XXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}");
                }
                catch
                {
                    if (sounds == true)
                    {
                        Console.Beep(600, 300);
                    }
                    
                }
                try
                {
                    cmd("sc stop dps");
                    File.Delete("C:\\Windows\\System32\\sru\\SRUDB.dat");
                    cmd("sc start dps");
                }
                catch
                {
                    if (sounds == true)
                    {
                        Console.Beep(600, 300);
                    }
                }
                string[] array4 = new string[7] { "pcasvc", "bam", "WSearch", "dnscache", "diagtrack", "CDPUserSvc_17a41d", "dps" };
                //string text2 = timer();
                string text3 = DateTime.Now.ToString("HH:mm:ss");
                cmd("time " + text3);
                array2 = array4;
                string[] array5 = array2;
                foreach (string text4 in array5)
                {
                    cmd("sc stop " + text4);
                    Thread.Sleep(1200);
                    cmd("sc start " + text4);
                    Thread.Sleep(1200);
                }
                DateTime.Now.ToString("HH:mm:ss");
                new WebClient();
                dnscache();
                Thread.Sleep(1350);
            
                lsassl();
                regedits();
                lsassl();
                Thread.Sleep(200);
                command("del /s /f /q c:\\windows\\temp\\*.*");
                command("del rd /s /q c:\\windows\\temp");
                command("del md c:\\windows\\temp");
                Thread.Sleep(2000);
                servicestart();
                Thread.Sleep(2000);
                cmd("time " + DateTime.Now.ToString("HH:mm:ss"));
                Thread.Sleep(2000);
                foreach (var regprocess in Process.GetProcessesByName("reg"))
                {
                    regprocess.Kill();
                }
                command("net stop eventlog /y");
                command("wevtutil cl Security");
                command("net start eventlog");
                string location = Assembly.GetExecutingAssembly().Location;
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/C ping 1.1.1.1 -n 1 -w 4000 > Nul & Del \"" + location + "\"",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                });
                Environment.Exit(1);
                Application.Exit();
            });
            guna2Panel4.Hide();
            AllertMsg("Cleaning Completed!", NotifyAnimation.enmType.Error);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            string dslink = KeyAuthApp.var("discordinvite");

            Process.Start(dslink);
        }

        private void guna2Panel4_Paint(object sender, PaintEventArgs e)
        {
            if (guna2Panel4.Visible)
            {
                foreach (var particle in particles)
                {
                    //e.Graphics.FillEllipse(new SolidBrush(particle.Color), particle.Position.X, particle.Position.Y, 5, 5);
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), particle.Position.X, particle.Position.Y, 5, 5);
                }
            }
        }
    }
}
