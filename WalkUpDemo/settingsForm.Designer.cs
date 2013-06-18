namespace VideoDemo
{
    partial class settingsForm
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
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVideoZone = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWalkupZone = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.cbEnableNames = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSecondsPost = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSecondsPre = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(96, 6);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(182, 20);
            this.txtHost.TabIndex = 0;
            this.txtHost.Text = "127.0.0.1:2090";
            this.txtHost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHost_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Host:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Video Zone:";
            // 
            // txtVideoZone
            // 
            this.txtVideoZone.Location = new System.Drawing.Point(96, 32);
            this.txtVideoZone.Name = "txtVideoZone";
            this.txtVideoZone.Size = new System.Drawing.Size(182, 20);
            this.txtVideoZone.TabIndex = 2;
            this.txtVideoZone.Text = "Far Zone";
            this.txtVideoZone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVideoZone_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Walk-up Zone:";
            // 
            // txtWalkupZone
            // 
            this.txtWalkupZone.Location = new System.Drawing.Point(96, 58);
            this.txtWalkupZone.Name = "txtWalkupZone";
            this.txtWalkupZone.Size = new System.Drawing.Size(182, 20);
            this.txtWalkupZone.TabIndex = 4;
            this.txtWalkupZone.Text = "Touch Zone";
            this.txtWalkupZone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWalkupZone_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(204, 110);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbEnableNames
            // 
            this.cbEnableNames.AutoSize = true;
            this.cbEnableNames.Checked = true;
            this.cbEnableNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableNames.Location = new System.Drawing.Point(97, 110);
            this.cbEnableNames.Name = "cbEnableNames";
            this.cbEnableNames.Size = new System.Drawing.Size(95, 17);
            this.cbEnableNames.TabIndex = 7;
            this.cbEnableNames.Text = "Enable Names";
            this.cbEnableNames.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(201, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Post:";
            // 
            // txtSecondsPost
            // 
            this.txtSecondsPost.Location = new System.Drawing.Point(238, 84);
            this.txtSecondsPost.Name = "txtSecondsPost";
            this.txtSecondsPost.Size = new System.Drawing.Size(40, 20);
            this.txtSecondsPost.TabIndex = 10;
            this.txtSecondsPost.Text = "10";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Video Seconds Pre Event:";
            // 
            // txtSecondsPre
            // 
            this.txtSecondsPre.Location = new System.Drawing.Point(155, 84);
            this.txtSecondsPre.Name = "txtSecondsPre";
            this.txtSecondsPre.Size = new System.Drawing.Size(40, 20);
            this.txtSecondsPre.TabIndex = 8;
            this.txtSecondsPre.Text = "5";
            // 
            // settingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 143);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSecondsPost);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSecondsPre);
            this.Controls.Add(this.cbEnableNames);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtWalkupZone);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtVideoZone);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "settingsForm";
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.settingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtVideoZone;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWalkupZone;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox cbEnableNames;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSecondsPost;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSecondsPre;
    }
}