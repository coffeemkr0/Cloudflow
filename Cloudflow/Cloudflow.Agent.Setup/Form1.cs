using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cloudflow.Agent.Setup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            try
            {
                Process deleteCommand = new Process();
                deleteCommand.StartInfo = new ProcessStartInfo("netsh");
                deleteCommand.StartInfo.UseShellExecute = false;
                deleteCommand.StartInfo.RedirectStandardOutput = true;
                deleteCommand.StartInfo.Arguments = "http delete urlacl http://+:80/Service1";
                deleteCommand.Start();
                string output = deleteCommand.StandardOutput.ReadToEnd();
                deleteCommand.WaitForExit();

                //Register the url with the correct user or group

                Process registerCommand = new Process();
                registerCommand.StartInfo = new ProcessStartInfo("netsh");
                registerCommand.StartInfo.UseShellExecute = false;
                registerCommand.StartInfo.RedirectStandardOutput = true;
                registerCommand.StartInfo.Arguments = "http add urlacl http://+:80/Service1 user=" + GetUser();
                registerCommand.Start();
                output = registerCommand.StandardOutput.ReadToEnd();
                registerCommand.WaitForExit();
                if (output.Contains("Error"))
                {
                    MessageBox.Show(string.Format("Could not register the agent for the specified user {0}. {1}",GetUser(), output),
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("The agent has been successfully configured.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetUser()
        {
            string user = "";

            if (chkUsers.Checked)
            {
                user = "BUILTIN\\Users";
            }
            else
            {
                user = textBox1.Text;
                if (!user.Contains("\\"))
                {
                    user = Environment.MachineName + "\\" + user;
                }
            }
            return user;
        }
    }
}
