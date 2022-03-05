
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
            this.grpState = new System.Windows.Forms.GroupBox();
            this.lblBehavior = new System.Windows.Forms.Label();
            this.lblClientInfo = new System.Windows.Forms.Label();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnForceDisconnect = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnSendText = new System.Windows.Forms.Button();
            this.splContent = new System.Windows.Forms.SplitContainer();
            this.cmsListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmTextDetail = new System.Windows.Forms.ToolStripMenuItem();
            this.tssFirst = new System.Windows.Forms.ToolStripSeparator();
            this.tsmCopyInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCopyText = new System.Windows.Forms.ToolStripMenuItem();
            this.cboSendTo = new System.Windows.Forms.ComboBox();
            this.lblSendTo = new System.Windows.Forms.Label();
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.lblRecord = new System.Windows.Forms.Label();
            this.btnClientInfo = new System.Windows.Forms.Button();
            this.lsvRecord = new LanDataTransmitter.Frm.View.MessageRecordListView();
            this.edtText = new LanDataTransmitter.Frm.View.PlaceholderTextBox();
            this.grpState.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splContent)).BeginInit();
            this.splContent.Panel1.SuspendLayout();
            this.splContent.Panel2.SuspendLayout();
            this.splContent.SuspendLayout();
            this.cmsListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpState
            // 
            this.grpState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpState.Controls.Add(this.lblBehavior);
            this.grpState.Controls.Add(this.lblClientInfo);
            this.grpState.Controls.Add(this.btnClientInfo);
            this.grpState.Controls.Add(this.btnRestart);
            this.grpState.Controls.Add(this.btnForceDisconnect);
            this.grpState.Controls.Add(this.btnStop);
            this.grpState.Location = new System.Drawing.Point(12, 12);
            this.grpState.Name = "grpState";
            this.grpState.Size = new System.Drawing.Size(360, 100);
            this.grpState.TabIndex = 0;
            this.grpState.TabStop = false;
            this.grpState.Text = "服务器状态";
            // 
            // lblBehavior
            // 
            this.lblBehavior.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBehavior.AutoEllipsis = true;
            this.lblBehavior.Location = new System.Drawing.Point(12, 22);
            this.lblBehavior.Name = "lblBehavior";
            this.lblBehavior.Size = new System.Drawing.Size(336, 17);
            this.lblBehavior.TabIndex = 1;
            this.lblBehavior.Text = "当前作为服务器，正在监听 0.0.0.0:10240";
            this.lblBehavior.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblClientInfo
            // 
            this.lblClientInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClientInfo.AutoEllipsis = true;
            this.lblClientInfo.Location = new System.Drawing.Point(12, 44);
            this.lblClientInfo.Name = "lblClientInfo";
            this.lblClientInfo.Size = new System.Drawing.Size(336, 17);
            this.lblClientInfo.TabIndex = 2;
            this.lblClientInfo.Text = "未连接任何客户端";
            this.lblClientInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRestart
            // 
            this.btnRestart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRestart.Location = new System.Drawing.Point(102, 69);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(75, 25);
            this.btnRestart.TabIndex = 4;
            this.btnRestart.Text = "重新监听";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnForceDisconnect
            // 
            this.btnForceDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnForceDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnForceDisconnect.Location = new System.Drawing.Point(183, 69);
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
            this.btnStop.Location = new System.Drawing.Point(279, 69);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 25);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "结束监听";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSendFile.Location = new System.Drawing.Point(297, 329);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 25);
            this.btnSendFile.TabIndex = 14;
            this.btnSendFile.Text = "发送文件...";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnSendText
            // 
            this.btnSendText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendText.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSendText.Location = new System.Drawing.Point(216, 329);
            this.btnSendText.Name = "btnSendText";
            this.btnSendText.Size = new System.Drawing.Size(75, 25);
            this.btnSendText.TabIndex = 13;
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
            this.splContent.Location = new System.Drawing.Point(12, 142);
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
            this.splContent.Size = new System.Drawing.Size(360, 150);
            this.splContent.SplitterDistance = 101;
            this.splContent.TabIndex = 8;
            // 
            // cmsListView
            // 
            this.cmsListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmTextDetail,
            this.tssFirst,
            this.tsmCopyInfo,
            this.tsmCopyText});
            this.cmsListView.Name = "cmsListView";
            this.cmsListView.Size = new System.Drawing.Size(178, 76);
            // 
            // tsmTextDetail
            // 
            this.tsmTextDetail.Name = "tsmTextDetail";
            this.tsmTextDetail.Size = new System.Drawing.Size(177, 22);
            this.tsmTextDetail.Tag = "depend";
            this.tsmTextDetail.Text = "消息详情(&D)...";
            this.tsmTextDetail.Click += new System.EventHandler(this.tsmTextDetail_Click);
            // 
            // tssFirst
            // 
            this.tssFirst.Name = "tssFirst";
            this.tssFirst.Size = new System.Drawing.Size(174, 6);
            // 
            // tsmCopyInfo
            // 
            this.tsmCopyInfo.Name = "tsmCopyInfo";
            this.tsmCopyInfo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.tsmCopyInfo.Size = new System.Drawing.Size(177, 22);
            this.tsmCopyInfo.Tag = "depend";
            this.tsmCopyInfo.Text = "复制信息(&I)";
            this.tsmCopyInfo.Click += new System.EventHandler(this.tsmCopyInfo_Click);
            // 
            // tsmCopyText
            // 
            this.tsmCopyText.Name = "tsmCopyText";
            this.tsmCopyText.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.tsmCopyText.Size = new System.Drawing.Size(177, 22);
            this.tsmCopyText.Tag = "depend";
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
            this.cboSendTo.Location = new System.Drawing.Point(69, 298);
            this.cboSendTo.Name = "cboSendTo";
            this.cboSendTo.Size = new System.Drawing.Size(303, 25);
            this.cboSendTo.TabIndex = 12;
            // 
            // lblSendTo
            // 
            this.lblSendTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSendTo.AutoSize = true;
            this.lblSendTo.Location = new System.Drawing.Point(12, 301);
            this.lblSendTo.Name = "lblSendTo";
            this.lblSendTo.Size = new System.Drawing.Size(51, 17);
            this.lblSendTo.TabIndex = 11;
            this.lblSendTo.Text = "发送至 :";
            // 
            // lblRecord
            // 
            this.lblRecord.AutoSize = true;
            this.lblRecord.Location = new System.Drawing.Point(12, 119);
            this.lblRecord.Name = "lblRecord";
            this.lblRecord.Size = new System.Drawing.Size(286, 17);
            this.lblRecord.TabIndex = 7;
            this.lblRecord.Text = "消息收发记录：(共收到 0 条消息，已发送 0 条消息)";
            // 
            // btnClientInfo
            // 
            this.btnClientInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClientInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClientInfo.Location = new System.Drawing.Point(6, 69);
            this.btnClientInfo.Name = "btnClientInfo";
            this.btnClientInfo.Size = new System.Drawing.Size(75, 25);
            this.btnClientInfo.TabIndex = 3;
            this.btnClientInfo.Text = "客户端信息";
            this.btnClientInfo.UseVisualStyleBackColor = true;
            this.btnClientInfo.Click += new System.EventHandler(this.btnClientInfo_Click);
            // 
            // lsvRecord
            // 
            this.lsvRecord.ContextMenuStrip = this.cmsListView;
            this.lsvRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvRecord.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lsvRecord.FullRowSelect = true;
            this.lsvRecord.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsvRecord.HideSelection = false;
            this.lsvRecord.Location = new System.Drawing.Point(0, 0);
            this.lsvRecord.MultiSelect = false;
            this.lsvRecord.Name = "lsvRecord";
            this.lsvRecord.OwnerDraw = true;
            this.lsvRecord.Size = new System.Drawing.Size(360, 101);
            this.lsvRecord.TabIndex = 9;
            this.lsvRecord.UseCompatibleStateImageBehavior = false;
            this.lsvRecord.View = System.Windows.Forms.View.Details;
            this.lsvRecord.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsvRecord_MouseDoubleClick);
            // 
            // edtText
            // 
            this.edtText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edtText.Location = new System.Drawing.Point(0, 0);
            this.edtText.Multiline = true;
            this.edtText.Name = "edtText";
            this.edtText.PlaceholderText = "此处输入需要发送的内容...";
            this.edtText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.edtText.Size = new System.Drawing.Size(360, 45);
            this.edtText.TabIndex = 10;
            this.edtText.TextChanged += new System.EventHandler(this.edtText_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.grpState);
            this.Controls.Add(this.lblRecord);
            this.Controls.Add(this.splContent);
            this.Controls.Add(this.lblSendTo);
            this.Controls.Add(this.cboSendTo);
            this.Controls.Add(this.btnSendText);
            this.Controls.Add(this.btnSendFile);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "LAN Data Transmitter (Server)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpState.ResumeLayout(false);
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

        private System.Windows.Forms.GroupBox grpState;
        private LanDataTransmitter.Frm.View.MessageRecordListView lsvRecord;
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
        private System.Windows.Forms.ToolStripSeparator tssFirst;
        private System.Windows.Forms.Label lblRecord;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnClientInfo;
    }
}

