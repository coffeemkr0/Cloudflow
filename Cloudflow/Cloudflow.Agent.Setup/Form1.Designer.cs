namespace Cloudflow.Agent.Setup
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConfigure = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.chkSpecific = new System.Windows.Forms.RadioButton();
            this.chkUsers = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConfigure
            // 
            this.btnConfigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigure.Location = new System.Drawing.Point(345, 211);
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Size = new System.Drawing.Size(75, 23);
            this.btnConfigure.TabIndex = 99;
            this.btnConfigure.Text = "Configure";
            this.btnConfigure.UseVisualStyleBackColor = true;
            this.btnConfigure.Click += new System.EventHandler(this.btnConfigure_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(79, 110);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(326, 20);
            this.textBox1.TabIndex = 5;
            // 
            // chkSpecific
            // 
            this.chkSpecific.AutoSize = true;
            this.chkSpecific.Location = new System.Drawing.Point(41, 61);
            this.chkSpecific.Name = "chkSpecific";
            this.chkSpecific.Size = new System.Drawing.Size(136, 17);
            this.chkSpecific.TabIndex = 3;
            this.chkSpecific.Text = "A specific user or group";
            // 
            // chkUsers
            // 
            this.chkUsers.AutoSize = true;
            this.chkUsers.Checked = true;
            this.chkUsers.Location = new System.Drawing.Point(41, 38);
            this.chkUsers.Name = "chkUsers";
            this.chkUsers.Size = new System.Drawing.Size(213, 17);
            this.chkUsers.TabIndex = 2;
            this.chkUsers.TabStop = true;
            this.chkUsers.Text = "Users in the User group on this machine\r\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(293, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Which uers will be allowed to run the agent on this machine?\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(76, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(319, 26);
            this.label2.TabIndex = 4;
            this.label2.Text = "Include the domain name when specifying a domain user or group.\r\nExample: domain\\" +
    "username or domain\\groupname.\r\n";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 246);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkUsers);
            this.Controls.Add(this.chkSpecific);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnConfigure);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConfigure;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton chkSpecific;
        private System.Windows.Forms.RadioButton chkUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

