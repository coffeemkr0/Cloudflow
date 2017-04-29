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
                string output = ExecuteNetshCommand("http delete urlacl http://+:80/CloudflowAgentService/");
                output += ExecuteNetshCommand("http delete urlacl http://+:80/CloudflowMessaging/");

                //Register the url with the correct user or group
                output += ExecuteNetshCommand("http add urlacl http://+:80/CloudflowAgentService/ user=" + GetUser());
                output += ExecuteNetshCommand("http add urlacl http://+:80/CloudflowMessaging/ user=" + GetUser());

                if (output.Contains("Error"))
                {
                    MessageBox.Show(string.Format("Could not register the agent for the specified user {0}. {1}",GetUser(), output),
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (chkCreateShortcut.Checked)
                {
                    string desktopAgentPath = Path.GetFullPath("Cloudflow.Agent.Desktop.exe");
                    if (!File.Exists(desktopAgentPath))
                    {
                        desktopAgentPath = Path.GetFullPath("..\\..\\..\\Cloudflow.Agent.Desktop\\bin\\Debug\\Cloudflow.Agent.Desktop.exe");
                    }
                    CreateApplicationShortcut(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory), "Cloudflow Agent", desktopAgentPath);
                }

                MessageBox.Show("Configuration complete.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ExecuteNetshCommand(string commandText)
        {
            Process command = new Process();
            command.StartInfo = new ProcessStartInfo("netsh");
            command.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            command.StartInfo.CreateNoWindow = true;
            command.StartInfo.UseShellExecute = false;
            command.StartInfo.RedirectStandardOutput = true;
            command.StartInfo.Arguments = commandText;
            command.Start();
            string output = command.StandardOutput.ReadToEnd();
            command.WaitForExit();
            return output;
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

        private void CreateApplicationShortcut(string shortcutLocation, string name, string target)
        {
            Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
            dynamic shell = Activator.CreateInstance(t);
            try
            {
                string shortcutPath = Path.Combine(shortcutLocation, name + ".lnk");
                var lnk = shell.CreateShortcut(shortcutPath);
                try
                {
                    lnk.TargetPath = target;
                    lnk.IconLocation = target + " ,0";
                    lnk.Save();
                }
                finally
                {
                    Marshal.FinalReleaseComObject(lnk);
                }
            }
            finally
            {
                Marshal.FinalReleaseComObject(shell);
            }
        }
    }
}
