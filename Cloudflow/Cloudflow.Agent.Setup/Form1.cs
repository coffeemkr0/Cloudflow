using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Cloudflow.Core.Configuration;

namespace Cloudflow.Agent.Setup
{
    public partial class Form1 : Form
    {
        #region Private Members
        private TcpListener _listener;
        private AgentLocalConfigurationSettings _agentLocalConfigurationSettings;

        #endregion

        #region Initialization
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "Cloudflow", "Agent", "Agent.config");
                _agentLocalConfigurationSettings = new AgentLocalConfigurationSettings(configFilePath);

                //Check to see if the agent has been configured previously and set the port
                numPort.Value = _agentLocalConfigurationSettings.Port;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("There was a problem initialize the local configuration store for the agent. {0}", ex.Message),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Private Methods
        private void btnConfigure_Click(object sender, EventArgs e)
        {
            try
            {
                var port = numPort.Value;

                //Delete any registered url in case it was registered with another user
                ExecuteNetshCommand("http delete urlacl http://+:" + port + "/CloudflowAgent/");

                //Register the url with the correct user or group
                var output = ExecuteNetshCommand("http add urlacl http://+:" + port + "/CloudflowAgent/ user=" + GetUser());

                if (output.Contains("Error"))
                {
                    MessageBox.Show(string.Format("Could not register the agent for the specified user or port. {1}", output),
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Set the port value and save the local config
                _agentLocalConfigurationSettings.Port = (int)numPort.Value;
                _agentLocalConfigurationSettings.Save();

                //Create desktop shortcut
                if (chkCreateShortcut.Checked)
                {
                    var desktopAgentPath = Path.GetFullPath("Cloudflow.Agent.Desktop.exe");
                    if (!File.Exists(desktopAgentPath))
                    {
                        desktopAgentPath = Path.GetFullPath("..\\..\\..\\Cloudflow.Agent.Desktop\\bin\\Debug\\Cloudflow.Agent.Desktop.exe");
                    }
                    CreateApplicationShortcut(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory), "Cloudflow Agent", desktopAgentPath);
                }

                MessageBox.Show(this, "Configuration complete.", "Complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ExecuteNetshCommand(string commandText)
        {
            var command = new Process();
            command.StartInfo = new ProcessStartInfo("netsh");
            command.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            command.StartInfo.CreateNoWindow = true;
            command.StartInfo.UseShellExecute = false;
            command.StartInfo.RedirectStandardOutput = true;
            command.StartInfo.Arguments = commandText;
            command.Start();
            var output = command.StandardOutput.ReadToEnd();
            command.WaitForExit();
            return output;
        }

        private string GetUser()
        {
            var user = "";

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
            var t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
            dynamic shell = Activator.CreateInstance(t);
            try
            {
                var shortcutPath = Path.Combine(shortcutLocation, name + ".lnk");
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

        private void btnGetFreePort_Click(object sender, EventArgs e)
        {
            numPort.Value = Core.Communication.TcpIp.GetLocalAvailablePort();
        }

        private void btnStartListening_Click(object sender, EventArgs e)
        {
            try
            {
                _listener = Core.Communication.TcpIp.GetListener((int)numPort.Value);
                btnStopListening.Enabled = true;
                btnStartListening.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("Could not listen on port {0}. {1}", numPort.Value, ex.Message),
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnStopListening_Click(object sender, EventArgs e)
        {
            if (_listener != null)
            {
                try
                {
                    _listener.Stop();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format("There was a problem stopping the listener. {0}", ex.Message),
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    btnStartListening.Enabled = true;
                    btnStopListening.Enabled = false;
                }
            }
        }
        #endregion
    }
}
