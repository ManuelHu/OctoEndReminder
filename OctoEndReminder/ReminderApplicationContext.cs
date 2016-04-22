using OctoEndReminder.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OctoEndReminder
{
    public class ReminderApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayIconContextMenu;
        private ToolStripMenuItem closeMenuItem, settingsMenuItem;

        public TimeSpan AlertTime { get; set; }

        public ReminderApplicationContext()
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
            AlertTime = TimeSpan.Parse(config.AppSettings.Settings["AlertTime"].Value);

            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            closeMenuItem = new ToolStripMenuItem("Close");
            closeMenuItem.Click += CloseMenuItem_Click;

            settingsMenuItem = new ToolStripMenuItem("Settings");
            settingsMenuItem.Click += SettingsMenuItem_Click;

            trayIconContextMenu = new ContextMenuStrip();
            trayIconContextMenu.Items.Add(closeMenuItem);
            trayIconContextMenu.Items.Add(settingsMenuItem);

            trayIcon = new NotifyIcon();
            trayIcon.Icon = Resources.octoawesome;
            trayIcon.Visible = true;
            trayIcon.ContextMenuStrip = trayIconContextMenu;

            Timer t = new Timer();
            t.Interval = 1000;
            t.Tick += T_Tick;
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            if (now > AlertTime)
            {
                trayIcon.ShowBalloonTip(10, "OctoAwesome Erinnerung", $"Tom, es ist {AlertTime.ToString("hh\\:mm")}!", ToolTipIcon.Info);
                AlertTime += new TimeSpan(0, 15, 0);
            }
        }

        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm(this);
            sf.Show();
        }

        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
        }
    }
}
