using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsVpnMonitor
{
    public partial class Form1 : Form
    {
        public static string logPath = @"WindowsVpnMonitorLog.txt";
        public static string cfgFilePath = @"WindowsVpnMonitorConfig.txt";
        public static string processClient;
        public static string processViscosity;
        public static string processOVPN;
        public static string pathViscosity;
        public static string pathClient;
        public static string pathBrowser;
        public static string ipSupplier;
        public static string dnsUrl;

        public static bool exit = false;
        public static int windowHeight = 0;
        public static int windowWidth = 0;

        public static Thread autoThread;

        public static ContextMenu notifyIcon_menu = new ContextMenu();

        Icon iconRed = new Icon("IconRed.ico");
        Icon iconPurple = new Icon("IconPurple.ico");
        Icon iconYellow = new Icon("IconYellow.ico");
        Icon iconGreen = new Icon("IconGreen.ico");
        Icon iconOrange = new Icon("IconOrange.ico");

        public Form1()
        {
            InitializeComponent();
            Log("Program started", false);
            GetWindowSize();
            MinimizeToTray();
            ReadSettings();
            StartAutoProcess();
            CreateNotificationIconMenu();
            StartupSettings();
        }

        private void GetWindowSize()
        {
            windowHeight = this.Height;
            windowWidth = this.Width;
        }

        private void StartupSettings()
        {
            checkBoxAutoScroll.Checked = true;
            checkBoxViscosity.Checked = true;
            checkBoxClient.Checked = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        protected override void OnLoad(EventArgs e)
        {
            var screen = Screen.FromPoint(this.Location);
            //Log(screen.WorkingArea.Right + " " + screen.WorkingArea.Bottom + " " + this.Width + " " + this.Height, false);
            //this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);
            //this.Location = new Point(screen.WorkingArea.Right - 539, screen.WorkingArea.Bottom - 288);
            this.Location = new Point(screen.WorkingArea.Right - windowWidth, screen.WorkingArea.Bottom - windowHeight);
            base.OnLoad(e);
        }

        private void MinimizeToTray()
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            notifyIcon.Visible = true;
            Log("Minimized to tray", false);
        }

        private void CreateNotificationIconMenu()
        {
            notifyIcon_menu.MenuItems.Add(0, new MenuItem("Launch Browser as Admin", new System.EventHandler(StartChrome_Click)));
            notifyIcon_menu.MenuItems.Add(1, new MenuItem("VPN monitor", new System.EventHandler(Viscosity_Click)));
            notifyIcon_menu.MenuItems.Add(2, new MenuItem("Client monitor", new System.EventHandler(Client_Click)));
            notifyIcon_menu.MenuItems.Add(3, new MenuItem("Show main window", new System.EventHandler(ShowMainWindow_Click)));
            notifyIcon_menu.MenuItems.Add(4, new MenuItem("Exit", new System.EventHandler(Exit_Click)));

            notifyIcon.ContextMenu = notifyIcon_menu;
        }

        private void StartAutoProcess()
        {
            autoThread = new Thread(AutoProcess);
            try
            {
                autoThread.Start();
                Log("AutoThread started successfully", false);
            }
            catch (Exception ex)
            {
                Log("Error: Failed to start AutoThread", false);
                Log("Error message: " + ex.Message, false);
            }
        }

        private void AutoProcess()
        {
            string tmpCurrentIp = "Error";
            string tmpDnsIp = "Error";
            bool firstLap = true;
            while (true)
            {
                string currentIp = CheckIp("ip");
                string dnsIp = CheckIp("dns");
                if (checkBoxAutoScroll.Checked) this.Invoke((MethodInvoker)(() => listBoxLog.TopIndex = listBoxLog.Items.Count - 1));

                if (checkBoxViscosity.Checked)
                {

                    if (tmpCurrentIp != currentIp && !firstLap)
                    {
                        Log("New IP: " + currentIp, true);
                        ShowBalloonTip("IP was updated", "New IP: " + currentIp, 5, true);
                    }
                    if (tmpDnsIp != dnsIp && !firstLap)
                    {
                        Log("Current DNS IP changed, new DNS IP: " + dnsIp, true);
                        ShowBalloonTip("DNS IP was updated", "New DNS IP: " + dnsIp, 5, true);
                    }
                    firstLap = false;
                    tmpDnsIp = dnsIp;
                    tmpCurrentIp = currentIp;

                    if (!checkBoxViscosity.Checked) StopProcess(processClient);

                    if (currentIp == dnsIp || currentIp == "Error" || dnsIp == "Error")
                    {
                        if (currentIp == dnsIp) Log("VPN is not connected, IP: " + currentIp, true);
                        StopProcess(processClient);
                        if (!CheckProcess(processClient))
                        {
                            this.Invoke((MethodInvoker)(() => notifyIcon.Text = "VPN: ERROR\nIP:" + currentIp));
                            this.Invoke((MethodInvoker)(() => notifyIcon.Icon = iconYellow));
                        }
                        else
                        {
                            this.Invoke((MethodInvoker)(() => notifyIcon.Text = "WARNING!\nClient is running without VPN!"));
                            this.Invoke((MethodInvoker)(() => notifyIcon.Icon = iconRed));
                            Log("WARNING! Client is running without VPN!", true);
                            ShowBalloonTip("WARNING", "Client is running without VPN", 120, true);
                        }
                        Thread.Sleep(2500);
                        if (CheckProcess(processViscosity)) StopProcess(processViscosity);
                        Thread.Sleep(2500);
                        if (checkBoxViscosity.Checked)
                        {
                            StartProcess(pathViscosity);
                            int count = 0;
                            Log("Waiting for VPN to connect...", true);
                            string time = DateTime.Now.ToString();
                            do
                            {
                                StopProcess(processClient);

                                for (int i = 0; i < 5; i++)
                                {
                                    this.Invoke((MethodInvoker)(() => listBoxLog.Items.RemoveAt(listBoxLog.Items.Count - 1)));
                                    this.Invoke((MethodInvoker)(() => listBoxLog.Items.Add(time + ": Waiting for VPN to connect... " + (1 + i + count * 5) + " sec (max 60)")));
                                    Thread.Sleep(1000);
                                }

                                currentIp = CheckIp("ip");
                                dnsIp = CheckIp("dns");
                                count++;
                            } while (currentIp == dnsIp && count < 12);

                            if (currentIp == dnsIp || currentIp == "Error" || dnsIp == "Error") Log("Failed to establish VPN connection. Will try again shortly.", true);
                            else
                            {
                                this.Invoke((MethodInvoker)(() => listBoxLog.Items.Remove(listBoxLog.Items.Count - 1)));
                                Log("VPN connected successfully (it took " + (count * 5) + " sec)", true);
                                ShowBalloonTip("VPN Connected successfylly", "New VPN IP is " + currentIp, 5, true);
                            }
                        }

                    }
                    if (currentIp != dnsIp && currentIp != "Error" && dnsIp != "Error")
                    {
                        if (!CheckProcess(processClient))
                        {
                            if (checkBoxClient.Checked) StartProcess(pathClient);
                            else
                            {
                                this.Invoke((MethodInvoker)(() => notifyIcon.Icon = iconYellow));
                                this.Invoke((MethodInvoker)(() => notifyIcon.Text = "VPN: OK\nClient: OFF\nIP: " + currentIp + "\nDNS: " + dnsIp));
                            }
                        }
                        else
                        {
                            this.Invoke((MethodInvoker)(() => notifyIcon.Icon = iconGreen));
                            this.Invoke((MethodInvoker)(() => notifyIcon.Text = "VPN: OK\nClient: OK\nIP: " + currentIp + "\nDNS: " + dnsIp));
                        }
                    }
                }
                else
                {
                    if (CheckProcess(processClient)) StopProcess(processClient);
                    this.Invoke((MethodInvoker)(() => notifyIcon.Text = "All monitoring is stopped.\nIP: " + currentIp + "\nDNS: " + dnsIp));
                    this.Invoke((MethodInvoker)(() => notifyIcon.Icon = iconPurple));
                }
                for (int i = 0; i < 5; i++)
                {
                    if (this.Icon == iconPurple) this.Invoke((MethodInvoker)(() => this.Icon = iconGreen));
                    else this.Invoke((MethodInvoker)(() => this.Icon = iconPurple));
                    Thread.Sleep(1000);
                }
            }
        }

        private static bool CheckProcess(string process)
        {
            foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
            {
                if (myProc.ProcessName.ToUpper() == process.ToUpper()) return true;
            }
            return false;
        }

        public void StartProcess(string process)
        {
            try
            {
                System.Diagnostics.Process.Start(process);
                Log("Started " + process, true);
            }
            catch (Exception ex)
            {
                Log("Error starting " + process, true);
                Log("Error messsage: " + ex.Message, true);
            }
        }
        public void StopProcess(string process)
        {
            foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
            {
                if (myProc.ProcessName.ToUpper() == process.ToUpper())
                {
                    try
                    {
                        myProc.Kill();
                        Log("Killed " + process, true);
                    }
                    catch (Exception ex)
                    {
                        Log("Error killing " + process, true);
                        Log("Error message: " + ex.Message, true);
                    }
                }
            }
        }

        public string CheckIp(string type)
        {
            string ip = "Error";
            int count = 0;
            while (count < 12)
            {
                try
                {
                    if (type == "ip")
                    {
                        ip = new WebClient().DownloadString(ipSupplier).Trim();
                        this.Invoke((MethodInvoker)(() => labelIp.Text = ip));
                    }
                    if (type == "dns")
                    {
                        ip = Dns.GetHostAddresses(dnsUrl)[0].ToString();
                        this.Invoke((MethodInvoker)(() => labelDns.Text = ip));
                    }
                    count = 12;
                }
                catch (Exception ex)
                {
                    if (!exit)
                    {
                        StopProcess(processClient);
                        Log("Failed to update " + type.ToUpper(), true);
                        Log("Error message: " + ex.Message, true);
                        Thread.Sleep(5000);
                        count++;
                    }
                    else count = 12;
                }
            }
            return ip;
        }

        private void Log(string message, bool crossThread)
        {
            if (crossThread)
            {
                this.Invoke((MethodInvoker)(() => listBoxLog.Items.Add(DateTime.Now + ": " + message)));
                if (checkBoxAutoScroll.Checked) this.Invoke((MethodInvoker)(() => listBoxLog.TopIndex = listBoxLog.Items.Count - 1));
            }
            else
            {
                listBoxLog.Items.Add(DateTime.Now + ": " + message);
                if (checkBoxAutoScroll.Checked) listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
            }

            if (!File.Exists(logPath))
            {
                using (StreamWriter sw = File.CreateText(logPath))
                {
                    sw.WriteLine(DateTime.Now + ": LOGFILE CRERATED");
                }
            }
            using (StreamWriter sw = File.AppendText(logPath))
            {
                sw.WriteLine(DateTime.Now + ": " + message);
            }
        }

        private void ShowBalloonTip(string title, string message, int time, bool crossThread)
        {
            if (crossThread)
            {
                this.Invoke((MethodInvoker)(() => notifyIcon.BalloonTipTitle = title));
                this.Invoke((MethodInvoker)(() => notifyIcon.BalloonTipText = message));
            }
            else
            {
                notifyIcon.BalloonTipTitle = title;
                notifyIcon.BalloonTipText = message;
            }
            notifyIcon.ShowBalloonTip(time);
        }

        private void buttonChrome_Click(object sender, EventArgs e)
        {
            StartChrome();
        }

        private void buttonLog_Click(object sender, EventArgs e)
        {
            Log("Logfile opened", false);
            System.Diagnostics.Process.Start("notepad.exe", logPath);
        }

        private void checkBoxViscosity_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxViscosity.Checked)
            {
                Log("VPN monitoring switched off by user", false);
                if (checkBoxClient.Checked == true)
                {
                    checkBoxClient.Checked = false;
                    Log("uClient monitoring automatically shut off", false);
                    StopProcess(processClient);
                    notifyIcon.ContextMenu.MenuItems[1].Text = "Viscosity - OFF";
                }
            }
            else
            {
                notifyIcon.ContextMenu.MenuItems[1].Text = "Viscosity - ON";
                if (checkBoxClient.Checked == false) Log("VPN monitoring switched on by user", false);
            }
        }

        private void checkBoxClient_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxClient.Checked)
            {
                if (checkBoxViscosity.Checked) Log("uClient Monitoring switched on by user", false);
                else
                {
                    checkBoxViscosity.Checked = true;
                    Log("Client monitoring switched on by user", false);
                    Log("VPN monitoring automatically started", false);
                }
                notifyIcon.ContextMenu.MenuItems[2].Text = "Client - ON";
            }
            else
            {
                notifyIcon.ContextMenu.MenuItems[2].Text = "Client - OFF";
                if (checkBoxViscosity.Checked) Log("Client monitoring switched off by user", false);
            }
        }
        private void StartChrome_Click(object sender, EventArgs e)
        {
            StartChrome();
        }

        private void StartChrome()
        {
            try
            {
                System.Diagnostics.Process.Start(pathBrowser);
                Log("Chrome launched by user", false);
            }
            catch (Exception ex)
            {
                Log("Error: Failed to launch Chrome", false);
                Log("Error message: " + ex.Message, false);
            }
        }
        protected void Viscosity_Click(Object sender, System.EventArgs e)
        {
            if (checkBoxViscosity.Checked)
            {
                checkBoxViscosity.Checked = false;
            }
            else
            {
                checkBoxViscosity.Checked = true;
            }
        }
        protected void Client_Click(Object sender, System.EventArgs e)
        {
            if (checkBoxClient.Checked)
            {
                checkBoxClient.Checked = false;
            }
            else
            {
                checkBoxClient.Checked = true;
            }
        }
        protected void Exit_Click(Object sender, System.EventArgs e)
        {
            Log("Program shut down by user", false);
            exit = true;
            ExitProgram();
        }
        protected void ShowMainWindow_Click(Object sender, System.EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.Activate();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BringWindowToFront();
        }

        private void BringWindowToFront()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
            else
            {
                this.Activate();
            }
            if (checkBoxAutoScroll.Checked) listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
        }
        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if (!exit)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                Log("Program minimized", false);
            }
        }
        private void ExitProgram()
        {
            StopProcess(processClient);
            autoThread.Abort();
            Application.Exit();
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            listBoxLog.Items.Clear();
            Log("Log window cleared by user", false);
        }

        private void ReadSettings()
        {

            if (!File.Exists(cfgFilePath))
            {
                Log("No configuration file found", false);

                using (StreamWriter sw = File.CreateText(cfgFilePath))
                {

                    sw.WriteLine(@"# Process name of client to kill if VPN goes down(e.g. CHROME)");
                    sw.WriteLine(@"CHROME");
                    sw.WriteLine(@"# Process name of VPN client (e.g. VISCOSITY)");
                    sw.WriteLine(@"VISCOSITY");
                    sw.WriteLine(@"# Additional process name of VPN client (e.g. OPENVPN)");
                    sw.WriteLine(@"OPENVPN");
                    sw.WriteLine(@"# Complete file path to VPN client (e.g. c:\Program Files\Viscosity\Viscosity.exe)");
                    sw.WriteLine(@"c:\Program Files\Viscosity\Viscosity.exe");
                    sw.WriteLine(@"# Complete file path to client (e.g. c:\Program Files (x86)\Google\Chrome\Application\chrome.exe)");
                    sw.WriteLine(@"c:\Program Files (x86)\Google\Chrome\Application\chrome.exe)");
                    sw.WriteLine(@"# Complete file path to browser (e.g. c:\Program Files (x86)\Google\Chrome\Application\chrome.exe)");
                    sw.WriteLine(@"c:\Program Files (x86)\Google\Chrome\Application\chrome.exe)");
                    sw.WriteLine(@"# URL for ip look up service (e.g. http://icanhazip.com)");
                    sw.WriteLine(@"http://icanhazip.com");
                    sw.WriteLine(@"# Your IP or dyndns address (eg. yourdyndns.dyndns.org)");
                    sw.WriteLine(@"yourdyndns.dyndns.org");
                }
                Log("Configuration file created", false);
                MessageBox.Show("Please enter your configuration.\n\nThe configuration file will now be opened.\n\nDon't forget to click \"Load New Config\" when you are done.", "Configuration Needed", MessageBoxButtons.OK);
                BringWindowToFront();
                EditConfig();
            }
            var lines = File.ReadAllLines(cfgFilePath);
            int count = 0;

            foreach (var line in lines)
            {
                if (line[0] != '#')
                {
                    if (count == 0)
                    {
                        processClient = line;
                        Log("Client process set: " + line, false);
                    }
                    if (count == 1)
                    {
                        processViscosity = line;
                        Log("VPN client process set: " + line, false);
                    }
                    if (count == 2)
                    {
                        processOVPN = line;
                        Log("Secondary VPN client process set: " + line, false);
                    }
                    if (count == 3)
                    {
                        pathViscosity = line;
                        Log("VPN Client path set: " + line, false);
                    }
                    if (count == 4)
                    {
                        pathClient = line;
                        Log("Client path set: " + line, false);
                    }
                    if (count == 5)
                    {
                        pathBrowser = line;
                        Log("Browser path set: " + line, false);
                    }
                    if (count == 6)
                    {
                        ipSupplier = line;
                        Log("IP provider set: " + line, false);
                    }
                    if (count == 7)
                    {
                        Log("IP / DYNDNS set: " + line, false);
                        dnsUrl = line;
                    }

                    count++;
                }
            }
            buttonLoadConfig.Enabled = false;
        }

        private void buttonEditConfig_Click(object sender, EventArgs e)
        {
            EditConfig();
        }

        private void EditConfig()
        {
            try
            {
                Log("Configuration file opened", false);
                System.Diagnostics.Process.Start("notepad.exe", cfgFilePath);
                buttonLoadConfig.Enabled = true;
            }
            catch (Exception ex)
            {
                Log("Error: Failed to open configuration file", false);
                Log("Error message: " + ex.Message, false);
            }
        }

        private void buttonLoadConfig_Click(object sender, EventArgs e)
        {
            ReadSettings();
            buttonLoadConfig.Enabled = false;
        }



    }

}
