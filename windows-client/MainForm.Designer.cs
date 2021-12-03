
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
            this.lbRecord = new System.Windows.Forms.ListBox();
            this.tbText = new System.Windows.Forms.TextBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.sctnContent = new System.Windows.Forms.SplitContainer();
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
            this.gpbConnect.Location = new System.Drawing.Point(12, 12);
            this.gpbConnect.Name = "gpbConnect";
            this.gpbConnect.Size = new System.Drawing.Size(310, 94);
            this.gpbConnect.TabIndex = 0;
            this.gpbConnect.TabStop = false;
            this.gpbConnect.Text = "连接信息";
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
            this.lbRecord.Size = new System.Drawing.Size(310, 100);
            this.lbRecord.TabIndex = 3;
            // 
            // tbText
            // 
            this.tbText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbText.Location = new System.Drawing.Point(0, 0);
            this.tbText.Multiline = true;
            this.tbText.Name = "tbText";
            this.tbText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbText.Size = new System.Drawing.Size(310, 75);
            this.tbText.TabIndex = 4;
            // 
            // btnFile
            // 
            this.btnFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFile.Location = new System.Drawing.Point(154, 81);
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
            this.btnSend.Location = new System.Drawing.Point(235, 81);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 25);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // sctnContent
            // 
            this.sctnContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sctnContent.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.sctnContent.Location = new System.Drawing.Point(12, 116);
            this.sctnContent.Name = "sctnContent";
            this.sctnContent.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // sctnContent.Panel1
            // 
            this.sctnContent.Panel1.Controls.Add(this.lbRecord);
            // 
            // sctnContent.Panel2
            // 
            this.sctnContent.Panel2.Controls.Add(this.tbText);
            this.sctnContent.Panel2.Controls.Add(this.btnSend);
            this.sctnContent.Panel2.Controls.Add(this.btnFile);
            this.sctnContent.Size = new System.Drawing.Size(310, 210);
            this.sctnContent.SplitterDistance = 100;
            this.sctnContent.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 338);
            this.Controls.Add(this.sctnContent);
            this.Controls.Add(this.gpbConnect);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 377);
            this.Name = "MainForm";
            this.Text = "LAN Data Transmitter";
            this.Load += new System.EventHandler(this.MainForm_Load);
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
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.SplitContainer sctnContent;
    }
}

