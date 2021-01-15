using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateDLL
{
    public partial class Form1 : Form
    {
        private Timer time = new Timer();
        private int timeLeft = 5;
        public Form1(string connectString , string dir)
        {
            InitializeComponent();
            IAutoUpdateDLL upd = new SQLServer_AutoUpdateDLL(connectString, dir);
            upd.GetDLLs(progressBar1, txtContent);
            btnStart.Enabled = false;
            time.Interval = 1000;
            time.Start();
            time.Tick += Time_Tick;

        }

        private void Time_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                // Display the new time left
                // by updating the Time Left label.
                timeLeft = timeLeft - 1;
                btnStart.Text = "Start(" + timeLeft +")";
            }
            else
            {
                // If the user ran out of time, stop the timer, show
                // a MessageBox, and fill in the answers.
                time.Stop();
                btnStart.Enabled = true;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string appdir = ConfigurationManager.AppSettings["APPLICATIONDIR"].ToString() ;
            try
            {
                Process.Start(appdir);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
