using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VideoDemo
{
    public partial class settingsForm : Form
    {
        private string m_host = "";
        private string m_videoZone = "";
        private string m_walkupZone = "";
        private bool m_enableNames = false;
        public  Int32 secondsPre = 5;
        public  Int32 secondsPost = 10;

        public string Host
        {
            get { return m_host; }
        }

        public string VideoZone
        {
            get { return m_videoZone; }
        }

        public string WalkUpZone
        {
            get { return m_walkupZone; }
        }

        public bool EnableNames
        {
            get { return m_enableNames; }
        }

        public settingsForm()
        {
            InitializeComponent();
            this.Text = "Configuration " + cVersion.Version;
            loadSettings();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveClick();
        }

        public void saveClick()
        {
            m_host = txtHost.Text;
            m_videoZone = txtVideoZone.Text;
            m_walkupZone = txtWalkupZone.Text;
            m_enableNames = cbEnableNames.Checked;
            try
            {
                secondsPre = Convert.ToInt32(txtSecondsPre.Text);
                secondsPost = Convert.ToInt32(txtSecondsPost.Text);
            }
            catch (Exception)
            { }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /////////////////////////////////////////////////
        public void loadSettings()
        {

            
            try
            {
                string[] lines = System.IO.File.ReadAllLines(frmMain.settingsFile);
                foreach (string line in lines)
                {
                    if (line.StartsWith("host="))
                    {
                        txtHost.Text = line.Substring(line.IndexOf("=") + 1).Trim();
                    }
                    if (line.StartsWith("zone="))
                    {
                        txtVideoZone.Text = line.Substring(line.IndexOf("=") + 1).Trim();
                    }
                    if (line.StartsWith("touchZone="))
                    {
                        txtWalkupZone.Text = line.Substring(line.IndexOf("=") + 1).Trim();
                    }
                    if (line.StartsWith("enableNames="))
                    {
                        String t = line.Substring(line.IndexOf("=") + 1).Trim();
                        cbEnableNames.Checked = Convert.ToBoolean(t);
                    }
                    if (line.StartsWith("secondsPre="))
                    {
                        txtSecondsPre.Text = line.Substring(line.IndexOf("=") + 1).Trim();
                    }
                    if (line.StartsWith("secondsPost="))
                    {
                        txtSecondsPost.Text = line.Substring(line.IndexOf("=") + 1).Trim();
                    }
                }
            }
            catch (Exception e)
            {
                String err = e.Message;
            }
        }

        private void settingsForm_Load(object sender, EventArgs e)
        {

        }

        private void txtWalkupZone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                saveClick();
            }
        }

        private void txtVideoZone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                saveClick();
            }
        }

        private void txtHost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                saveClick();
            }
        }
    }
}
