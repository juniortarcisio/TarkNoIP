using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace TarkNoIP.Client
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Hide();
            SetStartup();
            timer1.Start();
            KeepAlive();
            notifyIcon1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private async void KeepAlive()
        {
            HttpClient client = new HttpClient();
            var url = "http://tarksapi.azurewebsites.net/Server/KeepAlive/" + numericUpDown1.Value.ToString();

            var response = await client.PostAsync(url, null);

            if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                lblLastUpdate.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                lblNextUpdate.Text = DateTime.Now.AddMilliseconds(timer1.Interval).ToString("yyyy/MM/dd HH:mm:ss");
                lblErro.Text = $"";
                txtIP.Text = content.Replace('"',' ');
            }
            else
            {
                var message = await response.RequestMessage.Content.ReadAsStringAsync();
                lblErro.Text = $"Error {response.StatusCode.ToString()} - {message}";

                notifyIcon1.BalloonTipText = lblErro.Text;
                notifyIcon1.ShowBalloonTip(2000);
                txtIP.Text = "-";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            KeepAlive();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Visible = false;
        }

        //Startup registry key and value
        private static readonly string StartupKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private static readonly string StartupValue = "MyApplicationName";

        private static void SetStartup()
        {
            //Set the application to run at startup
            RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true);
            key.SetValue(StartupValue, Application.ExecutablePath.ToString());
        }

        private void btnUpdateNow_Click(object sender, EventArgs e)
        {
            KeepAlive();
        }

        private void txtIP_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtIP.Text);
        }

        private void txtIP_MouseDown(object sender, MouseEventArgs e)
        {
            txtIP.BackColor = Color.Blue;
        }

        private void txtIP_MouseUp(object sender, MouseEventArgs e)
        {
            txtIP.BackColor = Color.DeepSkyBlue;
        }
    }
}
