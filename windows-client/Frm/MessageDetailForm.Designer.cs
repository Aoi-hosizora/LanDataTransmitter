
namespace LanDataTransmitter.Frm {
    partial class MessageDetailForm {
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
            this.lblID = new System.Windows.Forms.Label();
            this.lblSender = new System.Windows.Forms.Label();
            this.lblReceiver = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblContent = new System.Windows.Forms.Label();
            this.edtID = new System.Windows.Forms.TextBox();
            this.edtSender = new System.Windows.Forms.TextBox();
            this.edtReceiver = new System.Windows.Forms.TextBox();
            this.edtTime = new System.Windows.Forms.TextBox();
            this.edtContent = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnNavigate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(12, 15);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(52, 17);
            this.lblID.TabIndex = 0;
            this.lblID.Text = "消息ID :";
            // 
            // lblSender
            // 
            this.lblSender.AutoSize = true;
            this.lblSender.Location = new System.Drawing.Point(12, 44);
            this.lblSender.Name = "lblSender";
            this.lblSender.Size = new System.Drawing.Size(51, 17);
            this.lblSender.TabIndex = 2;
            this.lblSender.Text = "发送方 :";
            // 
            // lblReceiver
            // 
            this.lblReceiver.AutoSize = true;
            this.lblReceiver.Location = new System.Drawing.Point(12, 73);
            this.lblReceiver.Name = "lblReceiver";
            this.lblReceiver.Size = new System.Drawing.Size(51, 17);
            this.lblReceiver.TabIndex = 4;
            this.lblReceiver.Text = "接收方 :";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(12, 102);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(63, 17);
            this.lblTime.TabIndex = 6;
            this.lblTime.Text = "发送时间 :";
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(12, 131);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(63, 17);
            this.lblContent.TabIndex = 8;
            this.lblContent.Text = "详细内容 :";
            // 
            // edtID
            // 
            this.edtID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtID.Location = new System.Drawing.Point(81, 12);
            this.edtID.Name = "edtID";
            this.edtID.ReadOnly = true;
            this.edtID.Size = new System.Drawing.Size(291, 23);
            this.edtID.TabIndex = 1;
            // 
            // edtSender
            // 
            this.edtSender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtSender.Location = new System.Drawing.Point(81, 41);
            this.edtSender.Name = "edtSender";
            this.edtSender.ReadOnly = true;
            this.edtSender.Size = new System.Drawing.Size(291, 23);
            this.edtSender.TabIndex = 3;
            // 
            // edtReceiver
            // 
            this.edtReceiver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtReceiver.Location = new System.Drawing.Point(81, 70);
            this.edtReceiver.Name = "edtReceiver";
            this.edtReceiver.ReadOnly = true;
            this.edtReceiver.Size = new System.Drawing.Size(291, 23);
            this.edtReceiver.TabIndex = 5;
            // 
            // edtTime
            // 
            this.edtTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtTime.Location = new System.Drawing.Point(81, 99);
            this.edtTime.Name = "edtTime";
            this.edtTime.ReadOnly = true;
            this.edtTime.Size = new System.Drawing.Size(291, 23);
            this.edtTime.TabIndex = 7;
            // 
            // edtContent
            // 
            this.edtContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtContent.Location = new System.Drawing.Point(12, 154);
            this.edtContent.Multiline = true;
            this.edtContent.Name = "edtContent";
            this.edtContent.ReadOnly = true;
            this.edtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.edtContent.Size = new System.Drawing.Size(360, 119);
            this.edtContent.TabIndex = 9;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(297, 279);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "关闭";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnNavigate
            // 
            this.btnNavigate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNavigate.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNavigate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnNavigate.Location = new System.Drawing.Point(201, 279);
            this.btnNavigate.Name = "btnNavigate";
            this.btnNavigate.Size = new System.Drawing.Size(90, 25);
            this.btnNavigate.TabIndex = 10;
            this.btnNavigate.Text = "在列表中显示";
            this.btnNavigate.UseVisualStyleBackColor = true;
            this.btnNavigate.Click += new System.EventHandler(this.btnNavigate_Click);
            // 
            // MessageDetailForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(384, 311);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.edtID);
            this.Controls.Add(this.lblSender);
            this.Controls.Add(this.edtSender);
            this.Controls.Add(this.lblReceiver);
            this.Controls.Add(this.edtReceiver);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.edtTime);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.edtContent);
            this.Controls.Add(this.btnNavigate);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(250, 300);
            this.Name = "MessageDetailForm";
            this.Text = "消息详情";
            this.Load += new System.EventHandler(this.MessageDetailForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label lblSender;
        private System.Windows.Forms.Label lblReceiver;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.TextBox edtID;
        private System.Windows.Forms.TextBox edtSender;
        private System.Windows.Forms.TextBox edtReceiver;
        private System.Windows.Forms.TextBox edtTime;
        private System.Windows.Forms.TextBox edtContent;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnNavigate;
    }
}