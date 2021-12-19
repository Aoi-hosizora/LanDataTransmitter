
namespace LanDataTransmitter {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.grpConnect = new System.Windows.Forms.GroupBox();
            this.lblBehavior = new System.Windows.Forms.Label();
            this.lblClientId = new System.Windows.Forms.Label();
            this.btnForceDisconnect = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lsbRecord = new System.Windows.Forms.ListBox();
            this.edtText = new System.Windows.Forms.TextBox();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnSendText = new System.Windows.Forms.Button();
            this.splContent = new System.Windows.Forms.SplitContainer();
            this.grpConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splContent)).BeginInit();
            this.splContent.Panel1.SuspendLayout();
            this.splContent.Panel2.SuspendLayout();
            this.splContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpConnect
            // 
            this.grpConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpConnect.Controls.Add(this.lblBehavior);
            this.grpConnect.Controls.Add(this.lblClientId);
            this.grpConnect.Controls.Add(this.btnForceDisconnect);
            this.grpConnect.Controls.Add(this.btnStop);
            this.grpConnect.Location = new System.Drawing.Point(12, 12);
            this.grpConnect.Name = "grpConnect";
            this.grpConnect.Size = new System.Drawing.Size(340, 100);
            this.grpConnect.TabIndex = 0;
            this.grpConnect.TabStop = false;
            this.grpConnect.Text = "连接信息";
            // 
            // lblBehavior
            // 
            this.lblBehavior.AutoSize = true;
            this.lblBehavior.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblBehavior.Location = new System.Drawing.Point(12, 22);
            this.lblBehavior.Name = "lblBehavior";
            this.lblBehavior.Size = new System.Drawing.Size(224, 17);
            this.lblBehavior.TabIndex = 6;
            this.lblBehavior.Text = "当前作为服务器，正在监听 0.0.0.0:9999";
            // 
            // lblClientId
            // 
            this.lblClientId.AutoSize = true;
            this.lblClientId.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblClientId.Location = new System.Drawing.Point(12, 46);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(80, 17);
            this.lblClientId.TabIndex = 6;
            this.lblClientId.Text = "未绑定客户端";
            // 
            // btnForceDisconnect
            // 
            this.btnForceDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnForceDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnForceDisconnect.Location = new System.Drawing.Point(163, 69);
            this.btnForceDisconnect.Name = "btnForceDisconnect";
            this.btnForceDisconnect.Size = new System.Drawing.Size(90, 25);
            this.btnForceDisconnect.TabIndex = 5;
            this.btnForceDisconnect.Text = "断开所有连接";
            this.btnForceDisconnect.UseVisualStyleBackColor = true;
            this.btnForceDisconnect.Click += new System.EventHandler(this.btnForceDisconnect_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStop.Location = new System.Drawing.Point(259, 69);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 25);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "结束监听";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lsbRecord
            // 
            this.lsbRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsbRecord.FormattingEnabled = true;
            this.lsbRecord.IntegralHeight = false;
            this.lsbRecord.ItemHeight = 17;
            this.lsbRecord.Location = new System.Drawing.Point(0, 0);
            this.lsbRecord.Name = "lsbRecord";
            this.lsbRecord.ScrollAlwaysVisible = true;
            this.lsbRecord.Size = new System.Drawing.Size(340, 100);
            this.lsbRecord.TabIndex = 3;
            // 
            // edtText
            // 
            this.edtText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edtText.Location = new System.Drawing.Point(0, 0);
            this.edtText.Multiline = true;
            this.edtText.Name = "edtText";
            this.edtText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.edtText.Size = new System.Drawing.Size(340, 70);
            this.edtText.TabIndex = 4;
            // 
            // btnSendFile
            // 
            this.btnSendFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSendFile.Location = new System.Drawing.Point(277, 301);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 25);
            this.btnSendFile.TabIndex = 5;
            this.btnSendFile.Text = "发送文件...";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnSendText
            // 
            this.btnSendText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendText.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSendText.Location = new System.Drawing.Point(196, 301);
            this.btnSendText.Name = "btnSendText";
            this.btnSendText.Size = new System.Drawing.Size(75, 25);
            this.btnSendText.TabIndex = 6;
            this.btnSendText.Text = "发送";
            this.btnSendText.UseVisualStyleBackColor = true;
            this.btnSendText.Click += new System.EventHandler(this.btnSendText_Click);
            // 
            // splContent
            // 
            this.splContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splContent.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splContent.Location = new System.Drawing.Point(12, 118);
            this.splContent.Name = "splContent";
            this.splContent.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splContent.Panel1
            // 
            this.splContent.Panel1.Controls.Add(this.lsbRecord);
            // 
            // splContent.Panel2
            // 
            this.splContent.Panel2.Controls.Add(this.edtText);
            this.splContent.Size = new System.Drawing.Size(340, 174);
            this.splContent.SplitterDistance = 100;
            this.splContent.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 335);
            this.Controls.Add(this.splContent);
            this.Controls.Add(this.btnSendText);
            this.Controls.Add(this.grpConnect);
            this.Controls.Add(this.btnSendFile);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 350);
            this.Name = "MainForm";
            this.Text = "LAN Data Transmitter";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpConnect.ResumeLayout(false);
            this.grpConnect.PerformLayout();
            this.splContent.Panel1.ResumeLayout(false);
            this.splContent.Panel2.ResumeLayout(false);
            this.splContent.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splContent)).EndInit();
            this.splContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpConnect;
        private System.Windows.Forms.ListBox lsbRecord;
        private System.Windows.Forms.TextBox edtText;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnSendText;
        private System.Windows.Forms.SplitContainer splContent;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblClientId;
        private System.Windows.Forms.Label lblBehavior;
        private System.Windows.Forms.Button btnForceDisconnect;
    }
}

