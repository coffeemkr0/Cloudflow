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
                Process deleteCommand = new Process();
                deleteCommand.StartInfo = new ProcessStartInfo("netsh");
                deleteCommand.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                deleteCommand.StartInfo.CreateNoWindow = true;
                deleteCommand.StartInfo.UseShellExecute = false;
                deleteCommand.StartInfo.RedirectStandardOutput = true;
                deleteCommand.StartInfo.Arguments = "http delete urlacl http://+:80/Service1";
                deleteCommand.Start();
                string output = deleteCommand.StandardOutput.ReadToEnd();
                deleteCommand.WaitForExit();

                //Register the url with the correct user or group
                Process registerCommand = new Process();
                registerCommand.StartInfo = new ProcessStartInfo("netsh");
                registerCommand.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                registerCommand.StartInfo.CreateNoWindow = true;
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
