
namespace LanDataTransmitter.Frm {
    partial class ClientsInfoForm {
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
            this.cboClients = new System.Windows.Forms.ComboBox();
            this.lblClientId = new System.Windows.Forms.Label();
            this.edtClientId = new System.Windows.Forms.TextBox();
            this.lblClientName = new System.Windows.Forms.Label();
            this.edtClientName = new System.Windows.Forms.TextBox();
            this.lblConnectTime = new System.Windows.Forms.Label();
            this.edtConnectTime = new System.Windows.Forms.TextBox();
            this.lblServerAddress = new System.Windows.Forms.Label();
            this.edtServerAddress = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblClients = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboClients
            // 
            this.cboClients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboClients.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClients.FormattingEnabled = true;
            this.cboClients.Location = new System.Drawing.Point(12, 66);
            this.cboClients.Name = "cboClients";
            this.cboClients.Size = new System.Drawing.Size(360, 25);
            this.cboClients.TabIndex = 3;
            this.cboClients.SelectedIndexChanged += new System.EventHandler(this.cboClients_SelectedIndexChanged);
            // 
            // lblClientId
            // 
            this.lblClientId.AutoSize = true;
            this.lblClientId.Location = new System.Drawing.Point(12, 100);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(75, 17);
            this.lblClientId.TabIndex = 4;
            this.lblClientId.Text = "客户端标识 :";
            // 
            // edtClientId
            // 
            this.edtClientId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtClientId.Location = new System.Drawing.Point(93, 97);
            this.edtClientId.Name = "edtClientId";
            this.edtClientId.ReadOnly = true;
            this.edtClientId.Size = new System.Drawing.Size(279, 23);
            this.edtClientId.TabIndex = 5;
            // 
            // lblClientName
            // 
            this.lblClientName.AutoSize = true;
            this.lblClientName.Location = new System.Drawing.Point(12, 129);
            this.lblClientName.Name = "lblClientName";
            this.lblClientName.Size = new System.Drawing.Size(75, 17);
            this.lblClientName.TabIndex = 6;
            this.lblClientName.Text = "客户端名称 :";
            // 
            // edtClientName
            // 
            this.edtClientName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtClientName.Location = new System.Drawing.Point(93, 126);
            this.edtClientName.Name = "edtClientName";
            this.edtClientName.ReadOnly = true;
            this.edtClientName.Size = new System.Drawing.Size(279, 23);
            this.edtClientName.TabIndex = 7;
            // 
            // lblConnectTime
            // 
            this.lblConnectTime.AutoSize = true;
            this.lblConnectTime.Location = new System.Drawing.Point(12, 158);
            this.lblConnectTime.Name = "lblConnectTime";
            this.lblConnectTime.Size = new System.Drawing.Size(63, 17);
            this.lblConnectTime.TabIndex = 8;
            this.lblConnectTime.Text = "连接时间 :";
            // 
            // edtConnectTime
            // 
            this.edtConnectTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtConnectTime.Location = new System.Drawing.Point(93, 155);
            this.edtConnectTime.Name = "edtConnectTime";
            this.edtConnectTime.ReadOnly = true;
            this.edtConnectTime.Size = new System.Drawing.Size(279, 23);
            this.edtConnectTime.TabIndex = 9;
            // 
            // lblServerAddress
            // 
            this.lblServerAddress.AutoSize = true;
            this.lblServerAddress.Location = new System.Drawing.Point(12, 15);
            this.lblServerAddress.Name = "lblServerAddress";
            this.lblServerAddress.Size = new System.Drawing.Size(99, 17);
            this.lblServerAddress.TabIndex = 0;
            this.lblServerAddress.Text = "服务器监听地址 :";
            // 
            // edtServerAddress
            // 
            this.edtServerAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtServerAddress.Location = new System.Drawing.Point(117, 12);
            this.edtServerAddress.Name = "edtServerAddress";
            this.edtServerAddress.ReadOnly = true;
            this.edtServerAddress.Size = new System.Drawing.Size(255, 23);
            this.edtServerAddress.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(297, 184);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "关闭";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblClients
            // 
            this.lblClients.AutoSize = true;
            this.lblClients.Location = new System.Drawing.Point(12, 44);
            this.lblClients.Name = "lblClients";
            this.lblClients.Size = new System.Drawing.Size(195, 17);
            this.lblClients.TabIndex = 2;
            this.lblClients.Text = "当前已连接到服务器的所有客户端 :";
            // 
            // ClientsInfoForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(384, 216);
            this.Controls.Add(this.lblServerAddress);
            this.Controls.Add(this.edtServerAddress);
            this.Controls.Add(this.lblClients);
            this.Controls.Add(this.cboClients);
            this.Controls.Add(this.lblClientId);
            this.Controls.Add(this.edtClientId);
            this.Controls.Add(this.lblClientName);
            this.Controls.Add(this.edtClientName);
            this.Controls.Add(this.lblConnectTime);
            this.Controls.Add(this.edtConnectTime);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "ClientsInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "客户端信息";
            this.Load += new System.EventHandler(this.ClientsInfoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboClients;
        private System.Windows.Forms.Label lblClientId;
        private System.Windows.Forms.TextBox edtClientId;
        private System.Windows.Forms.Label lblClientName;
        private System.Windows.Forms.TextBox edtClientName;
        private System.Windows.Forms.Label lblConnectTime;
        private System.Windows.Forms.TextBox edtConnectTime;
        private System.Windows.Forms.Label lblServerAddress;
        private System.Windows.Forms.TextBox edtServerAddress;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblClients;
    }
}