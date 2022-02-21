
namespace LanDataTransmitter.Frm {
    partial class ClientInfoForm {
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
            this.lblClientId = new System.Windows.Forms.Label();
            this.lblClientName = new System.Windows.Forms.Label();
            this.lblServerAddress = new System.Windows.Forms.Label();
            this.lblConnectTime = new System.Windows.Forms.Label();
            this.edtClientId = new System.Windows.Forms.TextBox();
            this.edtClientName = new System.Windows.Forms.TextBox();
            this.edtConnectTime = new System.Windows.Forms.TextBox();
            this.edtServerAddress = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblClientId
            // 
            this.lblClientId.AutoSize = true;
            this.lblClientId.Location = new System.Drawing.Point(12, 15);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(75, 17);
            this.lblClientId.TabIndex = 0;
            this.lblClientId.Text = "客户端标识 :";
            // 
            // lblClientName
            // 
            this.lblClientName.AutoSize = true;
            this.lblClientName.Location = new System.Drawing.Point(12, 44);
            this.lblClientName.Name = "lblClientName";
            this.lblClientName.Size = new System.Drawing.Size(75, 17);
            this.lblClientName.TabIndex = 2;
            this.lblClientName.Text = "客户端名称 :";
            // 
            // lblServerAddress
            // 
            this.lblServerAddress.AutoSize = true;
            this.lblServerAddress.Location = new System.Drawing.Point(12, 102);
            this.lblServerAddress.Name = "lblServerAddress";
            this.lblServerAddress.Size = new System.Drawing.Size(75, 17);
            this.lblServerAddress.TabIndex = 6;
            this.lblServerAddress.Text = "服务器地址 :";
            // 
            // lblConnectTime
            // 
            this.lblConnectTime.AutoSize = true;
            this.lblConnectTime.Location = new System.Drawing.Point(12, 73);
            this.lblConnectTime.Name = "lblConnectTime";
            this.lblConnectTime.Size = new System.Drawing.Size(63, 17);
            this.lblConnectTime.TabIndex = 4;
            this.lblConnectTime.Text = "连接时间 :";
            // 
            // edtClientId
            // 
            this.edtClientId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtClientId.Location = new System.Drawing.Point(93, 12);
            this.edtClientId.Name = "edtClientId";
            this.edtClientId.ReadOnly = true;
            this.edtClientId.Size = new System.Drawing.Size(279, 23);
            this.edtClientId.TabIndex = 1;
            // 
            // edtClientName
            // 
            this.edtClientName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtClientName.Location = new System.Drawing.Point(93, 41);
            this.edtClientName.Name = "edtClientName";
            this.edtClientName.ReadOnly = true;
            this.edtClientName.Size = new System.Drawing.Size(279, 23);
            this.edtClientName.TabIndex = 3;
            // 
            // edtConnectTime
            // 
            this.edtConnectTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtConnectTime.Location = new System.Drawing.Point(93, 70);
            this.edtConnectTime.Name = "edtConnectTime";
            this.edtConnectTime.ReadOnly = true;
            this.edtConnectTime.Size = new System.Drawing.Size(279, 23);
            this.edtConnectTime.TabIndex = 5;
            // 
            // edtServerAddress
            // 
            this.edtServerAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtServerAddress.Location = new System.Drawing.Point(93, 99);
            this.edtServerAddress.Name = "edtServerAddress";
            this.edtServerAddress.ReadOnly = true;
            this.edtServerAddress.Size = new System.Drawing.Size(279, 23);
            this.edtServerAddress.TabIndex = 7;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(297, 128);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "关闭";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ClientInfoForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(384, 160);
            this.Controls.Add(this.lblClientId);
            this.Controls.Add(this.edtClientId);
            this.Controls.Add(this.lblClientName);
            this.Controls.Add(this.edtClientName);
            this.Controls.Add(this.lblConnectTime);
            this.Controls.Add(this.edtConnectTime);
            this.Controls.Add(this.lblServerAddress);
            this.Controls.Add(this.edtServerAddress);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "ClientInfoForm";
            this.Text = "客户端信息";
            this.Load += new System.EventHandler(this.ClientInfoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblClientId;
        private System.Windows.Forms.Label lblClientName;
        private System.Windows.Forms.Label lblServerAddress;
        private System.Windows.Forms.Label lblConnectTime;
        private System.Windows.Forms.TextBox edtClientId;
        private System.Windows.Forms.TextBox edtClientName;
        private System.Windows.Forms.TextBox edtConnectTime;
        private System.Windows.Forms.TextBox edtServerAddress;
        private System.Windows.Forms.Button btnOK;
    }
}