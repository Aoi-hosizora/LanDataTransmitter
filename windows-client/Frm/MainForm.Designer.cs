
namespace LanDataTransmitter.Frm {
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv");
            this.grpConnect = new System.Windows.Forms.GroupBox();
            this.lblBehavior = new System.Windows.Forms.Label();
            this.lblClientInfo = new System.Windows.Forms.Label();
            this.btnForceDisconnect = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnSendText = new System.Windows.Forms.Button();
            this.splContent = new System.Windows.Forms.SplitContainer();
            this.cmsListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmCopyText = new System.Windows.Forms.ToolStripMenuItem();
            this.cboSendTo = new System.Windows.Forms.ComboBox();
            this.lblSendTo = new System.Windows.Forms.Label();
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.tsmCopyInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.lsvRecord = new LanDataTransmitter.Frm.View.CustomListView();
            this.edtText = new LanDataTransmitter.Frm.View.PlaceholderTextBox();
            this.tsmTextDetail = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.grpConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splContent)).BeginInit();
            this.splContent.Panel1.SuspendLayout();
            this.splContent.Panel2.SuspendLayout();
            this.splContent.SuspendLayout();
            this.cmsListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpConnect
            // 
            this.grpConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpConnect.Controls.Add(this.lblBehavior);
            this.grpConnect.Controls.Add(this.lblClientInfo);
            this.grpConnect.Controls.Add(this.btnForceDisconnect);
            this.grpConnect.Controls.Add(this.btnStop);
            this.grpConnect.Location = new System.Drawing.Point(12, 12);
            this.grpConnect.Name = "grpConnect";
            this.grpConnect.Size = new System.Drawing.Size(360, 100);
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
            this.lblBehavior.TabIndex = 1;
            this.lblBehavior.Text = "当前作为服务器，正在监听 0.0.0.0:9999";
            // 
            // lblClientInfo
            // 
            this.lblClientInfo.AutoSize = true;
            this.lblClientInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblClientInfo.Location = new System.Drawing.Point(12, 46);
            this.lblClientInfo.Name = "lblClientInfo";
            this.lblClientInfo.Size = new System.Drawing.Size(80, 17);
            this.lblClientInfo.TabIndex = 2;
            this.lblClientInfo.Text = "未连接客户端";
            // 
            // btnForceDisconnect
            // 
            this.btnForceDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnForceDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnForceDisconnect.Location = new System.Drawing.Point(183, 69);
            this.btnForceDisconnect.Name = "btnForceDisconnect";
            this.btnForceDisconnect.Size = new System.Drawing.Size(90, 25);
            this.btnForceDisconnect.TabIndex = 3;
            this.btnForceDisconnect.Text = "断开所有连接";
            this.btnForceDisconnect.UseVisualStyleBackColor = true;
            this.btnForceDisconnect.Click += new System.EventHandler(this.btnForceDisconnect_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStop.Location = new System.Drawing.Point(279, 69);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 25);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "结束监听";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSendFile.Location = new System.Drawing.Point(297, 307);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 25);
            this.btnSendFile.TabIndex = 11;
            this.btnSendFile.Text = "发送文件...";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnSendText
            // 
            this.btnSendText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendText.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSendText.Location = new System.Drawing.Point(216, 307);
            this.btnSendText.Name = "btnSendText";
            this.btnSendText.Size = new System.Drawing.Size(75, 25);
            this.btnSendText.TabIndex = 10;
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
            this.splContent.Panel1.Controls.Add(this.lsvRecord);
            // 
            // splContent.Panel2
            // 
            this.splContent.Panel2.Controls.Add(this.edtText);
            this.splContent.Size = new System.Drawing.Size(360, 152);
            this.splContent.SplitterDistance = 101;
            this.splContent.TabIndex = 5;
            // 
            // cmsListView
            // 
            this.cmsListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmTextDetail,
            this.tssSeparator,
            this.tsmCopyInfo,
            this.tsmCopyText});
            this.cmsListView.Name = "cmsListView";
            this.cmsListView.Size = new System.Drawing.Size(181, 98);
            // 
            // tsmCopyText
            // 
            this.tsmCopyText.Name = "tsmCopyText";
            this.tsmCopyText.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.tsmCopyText.Size = new System.Drawing.Size(180, 22);
            this.tsmCopyText.Text = "复制文本(&C)";
            this.tsmCopyText.Click += new System.EventHandler(this.tsmCopyText_Click);
            // 
            // cboSendTo
            // 
            this.cboSendTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSendTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSendTo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboSendTo.FormattingEnabled = true;
            this.cboSendTo.Location = new System.Drawing.Point(65, 276);
            this.cboSendTo.Name = "cboSendTo";
            this.cboSendTo.Size = new System.Drawing.Size(307, 25);
            this.cboSendTo.TabIndex = 9;
            this.cboSendTo.SelectedIndexChanged += new System.EventHandler(this.cboSendTo_SelectedIndexChanged);
            // 
            // lblSendTo
            // 
            this.lblSendTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSendTo.AutoSize = true;
            this.lblSendTo.Location = new System.Drawing.Point(12, 280);
            this.lblSendTo.Name = "lblSendTo";
            this.lblSendTo.Size = new System.Drawing.Size(51, 17);
            this.lblSendTo.TabIndex = 8;
            this.lblSendTo.Text = "发送至 :";
            // 
            // tsmCopyInfo
            // 
            this.tsmCopyInfo.Name = "tsmCopyInfo";
            this.tsmCopyInfo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.tsmCopyInfo.Size = new System.Drawing.Size(180, 22);
            this.tsmCopyInfo.Text = "复制信息(&I)";
            this.tsmCopyInfo.Click += new System.EventHandler(this.tsmCopyInfo_Click);
            // 
            // lsvRecord
            // 
            this.lsvRecord.ContextMenuStrip = this.cmsListView;
            this.lsvRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvRecord.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lsvRecord.FullRowSelect = true;
            this.lsvRecord.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsvRecord.HideSelection = false;
            this.lsvRecord.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lsvRecord.Location = new System.Drawing.Point(0, 0);
            this.lsvRecord.MultiSelect = false;
            this.lsvRecord.Name = "lsvRecord";
            this.lsvRecord.OwnerDraw = true;
            this.lsvRecord.Size = new System.Drawing.Size(360, 101);
            this.lsvRecord.TabIndex = 6;
            this.lsvRecord.UseCompatibleStateImageBehavior = false;
            this.lsvRecord.View = System.Windows.Forms.View.Details;
            // 
            // edtText
            // 
            this.edtText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edtText.Location = new System.Drawing.Point(0, 0);
            this.edtText.Multiline = true;
            this.edtText.Name = "edtText";
            this.edtText.PlaceholderText = "Input here...";
            this.edtText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.edtText.Size = new System.Drawing.Size(360, 47);
            this.edtText.TabIndex = 7;
            this.edtText.TextChanged += new System.EventHandler(this.edtText_TextChanged);
            // 
            // tsmTextDetail
            // 
            this.tsmTextDetail.Name = "tsmTextDetail";
            this.tsmTextDetail.Size = new System.Drawing.Size(180, 22);
            this.tsmTextDetail.Text = "文本详情(&D)";
            this.tsmTextDetail.Click += new System.EventHandler(this.tsmTextDetail_Click);
            // 
            // tssSeparator
            // 
            this.tssSeparator.Name = "tssSeparator";
            this.tssSeparator.Size = new System.Drawing.Size(177, 6);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 341);
            this.Controls.Add(this.grpConnect);
            this.Controls.Add(this.splContent);
            this.Controls.Add(this.lblSendTo);
            this.Controls.Add(this.cboSendTo);
            this.Controls.Add(this.btnSendText);
            this.Controls.Add(this.btnSendFile);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 380);
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
            this.cmsListView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpConnect;
        private LanDataTransmitter.Frm.View.CustomListView lsvRecord;
        private LanDataTransmitter.Frm.View.PlaceholderTextBox edtText;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnSendText;
        private System.Windows.Forms.SplitContainer splContent;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblClientInfo;
        private System.Windows.Forms.Label lblBehavior;
        private System.Windows.Forms.Button btnForceDisconnect;
        private System.Windows.Forms.ComboBox cboSendTo;
        private System.Windows.Forms.Label lblSendTo;
        private System.Windows.Forms.ToolTip tipMain;
        private System.Windows.Forms.ContextMenuStrip cmsListView;
        private System.Windows.Forms.ToolStripMenuItem tsmCopyText;
        private System.Windows.Forms.ToolStripMenuItem tsmCopyInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmTextDetail;
        private System.Windows.Forms.ToolStripSeparator tssSeparator;
    }
}

