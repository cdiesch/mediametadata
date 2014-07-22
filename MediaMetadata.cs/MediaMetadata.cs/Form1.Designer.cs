namespace MediaMetadata.cs
{
    partial class fMetadata
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMetadata));
            this.btnStart = new System.Windows.Forms.Button();
            this.txtParentDir = new System.Windows.Forms.TextBox();
            this.lblRepositoryDir = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtRepositoryDir = new System.Windows.Forms.TextBox();
            this.lblMonitorDir = new System.Windows.Forms.Label();
            this.cbxRepository = new System.Windows.Forms.CheckBox();
            this.cbxFolders = new System.Windows.Forms.CheckBox();
            this.btnParentDir = new System.Windows.Forms.Button();
            this.btnRepository = new System.Windows.Forms.Button();
            this.prgUpdate = new System.Windows.Forms.ProgressBar();
            this.lblPercent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(424, 122);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtParentDir
            // 
            this.txtParentDir.Location = new System.Drawing.Point(134, 12);
            this.txtParentDir.Name = "txtParentDir";
            this.txtParentDir.Size = new System.Drawing.Size(334, 20);
            this.txtParentDir.TabIndex = 3;
            // 
            // lblRepositoryDir
            // 
            this.lblRepositoryDir.AutoSize = true;
            this.lblRepositoryDir.Location = new System.Drawing.Point(4, 42);
            this.lblRepositoryDir.Name = "lblRepositoryDir";
            this.lblRepositoryDir.Size = new System.Drawing.Size(124, 13);
            this.lblRepositoryDir.TabIndex = 8;
            this.lblRepositoryDir.Text = "Repository for XML Files:";
            // 
            // btnStop
            // 
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStop.Location = new System.Drawing.Point(424, 122);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtRepositoryDir
            // 
            this.txtRepositoryDir.Location = new System.Drawing.Point(134, 39);
            this.txtRepositoryDir.Name = "txtRepositoryDir";
            this.txtRepositoryDir.Size = new System.Drawing.Size(334, 20);
            this.txtRepositoryDir.TabIndex = 6;
            // 
            // lblMonitorDir
            // 
            this.lblMonitorDir.AutoSize = true;
            this.lblMonitorDir.Location = new System.Drawing.Point(26, 15);
            this.lblMonitorDir.Name = "lblMonitorDir";
            this.lblMonitorDir.Size = new System.Drawing.Size(102, 13);
            this.lblMonitorDir.TabIndex = 7;
            this.lblMonitorDir.Text = "Directory to Monitor:";
            // 
            // cbxRepository
            // 
            this.cbxRepository.AutoSize = true;
            this.cbxRepository.Location = new System.Drawing.Point(7, 65);
            this.cbxRepository.Name = "cbxRepository";
            this.cbxRepository.Size = new System.Drawing.Size(174, 17);
            this.cbxRepository.TabIndex = 9;
            this.cbxRepository.Text = "Save XML files in the repository";
            this.cbxRepository.UseVisualStyleBackColor = true;
            // 
            // cbxFolders
            // 
            this.cbxFolders.AutoSize = true;
            this.cbxFolders.Location = new System.Drawing.Point(7, 89);
            this.cbxFolders.Name = "cbxFolders";
            this.cbxFolders.Size = new System.Drawing.Size(259, 17);
            this.cbxFolders.TabIndex = 10;
            this.cbxFolders.Text = "Save XML files in the parent folders of the movies";
            this.cbxFolders.UseVisualStyleBackColor = true;
            // 
            // btnParentDir
            // 
            this.btnParentDir.Image = ((System.Drawing.Image)(resources.GetObject("btnParentDir.Image")));
            this.btnParentDir.Location = new System.Drawing.Point(474, 10);
            this.btnParentDir.Name = "btnParentDir";
            this.btnParentDir.Size = new System.Drawing.Size(25, 23);
            this.btnParentDir.TabIndex = 11;
            this.btnParentDir.UseVisualStyleBackColor = true;
            this.btnParentDir.Click += new System.EventHandler(this.btnParentDir_Click);
            // 
            // btnRepository
            // 
            this.btnRepository.Image = ((System.Drawing.Image)(resources.GetObject("btnRepository.Image")));
            this.btnRepository.Location = new System.Drawing.Point(474, 37);
            this.btnRepository.Name = "btnRepository";
            this.btnRepository.Size = new System.Drawing.Size(25, 23);
            this.btnRepository.TabIndex = 12;
            this.btnRepository.UseVisualStyleBackColor = true;
            this.btnRepository.Click += new System.EventHandler(this.btnRepository_Click);
            // 
            // prgUpdate
            // 
            this.prgUpdate.Location = new System.Drawing.Point(13, 122);
            this.prgUpdate.Name = "prgUpdate";
            this.prgUpdate.Size = new System.Drawing.Size(405, 23);
            this.prgUpdate.TabIndex = 13;
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.BackColor = System.Drawing.Color.Transparent;
            this.lblPercent.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPercent.Location = new System.Drawing.Point(206, 127);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(21, 17);
            this.lblPercent.TabIndex = 14;
            this.lblPercent.Text = "0%";
            this.lblPercent.UseCompatibleTextRendering = true;
            // 
            // fMetadata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 155);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.prgUpdate);
            this.Controls.Add(this.btnRepository);
            this.Controls.Add(this.btnParentDir);
            this.Controls.Add(this.cbxFolders);
            this.Controls.Add(this.cbxRepository);
            this.Controls.Add(this.lblRepositoryDir);
            this.Controls.Add(this.lblMonitorDir);
            this.Controls.Add(this.txtRepositoryDir);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.txtParentDir);
            this.Controls.Add(this.btnStart);
            this.Name = "fMetadata";
            this.Text = "Metadata Downloader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtParentDir;
        private System.Windows.Forms.Label lblRepositoryDir;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtRepositoryDir;
        private System.Windows.Forms.Label lblMonitorDir;
        private System.Windows.Forms.CheckBox cbxRepository;
        private System.Windows.Forms.CheckBox cbxFolders;
        private System.Windows.Forms.Button btnParentDir;
        private System.Windows.Forms.Button btnRepository;
        private System.Windows.Forms.ProgressBar prgUpdate;
        private System.Windows.Forms.Label lblPercent;

    }
}

