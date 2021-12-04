
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
            this.gpbConnect = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.lbRecord = new System.Windows.Forms.ListBox();
            this.edtText = new System.Windows.Forms.TextBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.sctnContent = new System.Windows.Forms.SplitContainer();
            this.lblClientId = new System.Windows.Forms.Label();
            this.lblBehavior = new System.Windows.Forms.Label();
            this.btnForceDisconnect = new System.Windows.Forms.Button();
            this.gpbConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sctnContent)).BeginInit();
            this.sctnContent.Panel1.SuspendLayout();
            this.sctnContent.Panel2.SuspendLayout();
            this.sctnContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpbConnect
            // 
            this.gpbConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpbConnect.Controls.Add(this.lblBehavior);
            this.gpbConnect.Controls.Add(this.lblClientId);
            this.gpbConnect.Controls.Add(this.btnForceDisconnect);
            this.gpbConnect.Controls.Add(this.btnStop);
            this.gpbConnect.Location = new System.Drawing.Point(12, 12);
            this.gpbConnect.Name = "gpbConnect";
            this.gpbConnect.Size = new System.Drawing.Size(340, 100);
            this.gpbConnect.TabIndex = 0;
            this.gpbConnect.TabStop = false;
            this.gpbConnect.Text = "连接信息";
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
            // lbRecord
            // 
            this.lbRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbRecord.FormattingEnabled = true;
            this.lbRecord.IntegralHeight = false;
            this.lbRecord.ItemHeight = 17;
            this.lbRecord.Location = new System.Drawing.Point(0, 0);
            this.lbRecord.Name = "lbRecord";
            this.lbRecord.ScrollAlwaysVisible = true;
            this.lbRecord.Size = new System.Drawing.Size(340, 100);
            this.lbRecord.TabIndex = 3;
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
            // btnFile
            // 
            this.btnFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFile.Location = new System.Drawing.Point(277, 301);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(75, 25);
            this.btnFile.TabIndex = 5;
            this.btnFile.Text = "发送文件...";
            this.btnFile.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSend.Location = new System.Drawing.Point(196, 301);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 25);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // sctnContent
            // 
            this.sctnContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sctnContent.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.sctnContent.Location = new System.Drawing.Point(12, 118);
            this.sctnContent.Name = "sctnContent";
            this.sctnContent.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // sctnContent.Panel1
            // 
            this.sctnContent.Panel1.Controls.Add(this.lbRecord);
            // 
            // sctnContent.Panel2
            // 
            this.sctnContent.Panel2.Controls.Add(this.edtText);
            this.sctnContent.Size = new System.Drawing.Size(340, 174);
            this.sctnContent.SplitterDistance = 100;
            this.sctnContent.TabIndex = 7;
            // 
            // lblClientId
            // 
            this.lblClientId.AutoSize = true;
            this.lblClientId.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblClientId.Location = new System.Drawing.Point(12, 44);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(80, 17);
            this.lblClientId.TabIndex = 6;
            this.lblClientId.Text = "未绑定客户端";
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
            // btnForceDisconnect
            // 
            this.btnForceDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnForceDisconnect.Enabled = false;
            this.btnForceDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnForceDisconnect.Location = new System.Drawing.Point(178, 69);
            this.btnForceDisconnect.Name = "btnForceDisconnect";
            this.btnForceDisconnect.Size = new System.Drawing.Size(75, 25);
            this.btnForceDisconnect.TabIndex = 5;
            this.btnForceDisconnect.Text = "断开连接";
            this.btnForceDisconnect.UseVisualStyleBackColor = true;
            this.btnForceDisconnect.Click += new System.EventHandler(this.btnForceDisconnect_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 335);
            this.Controls.Add(this.sctnContent);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.gpbConnect);
            this.Controls.Add(this.btnFile);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 350);
            this.Name = "MainForm";
            this.Text = "LAN Data Transmitter";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gpbConnect.ResumeLayout(false);
            this.gpbConnect.PerformLayout();
            this.sctnContent.Panel1.ResumeLayout(false);
            this.sctnContent.Panel2.ResumeLayout(false);
            this.sctnContent.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sctnContent)).EndInit();
            this.sctnContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbConnect;
        private System.Windows.Forms.ListBox lbRecord;
        private System.Windows.Forms.TextBox edtText;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.SplitContainer sctnContent;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblClientId;
        private System.Windows.Forms.Label lblBehavior;
        private System.Windows.Forms.Button btnForceDisconnect;
    }
}

