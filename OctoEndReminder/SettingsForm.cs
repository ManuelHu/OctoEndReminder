using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OctoEndReminder
{
    public partial class SettingsForm : Form
    {
        private ReminderApplicationContext appCtx;

        public SettingsForm(ReminderApplicationContext appCtx)
        {            
            InitializeComponent();
            this.appCtx = appCtx;
            maskedTextBox1.Text = appCtx.AlertTime.ToString("hh\\:mm");
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            appCtx.AlertTime = TimeSpan.Parse(maskedTextBox1.Text);
            Close();
        }

        private void standardButton_Click(object sender, EventArgs e)
        {
            appCtx.AlertTime = TimeSpan.Parse(maskedTextBox1.Text);

            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
            config.AppSettings.Settings["AlertTime"].Value = appCtx.AlertTime.ToString("hh\\:mm");
            config.Save(ConfigurationSaveMode.Full, true);

            Close();
        }
    }
}
