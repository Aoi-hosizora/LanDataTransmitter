
namespace LanDataTransmitter.Frm {
    partial class InitForm {
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
            this.rdoServer = new System.Windows.Forms.RadioButton();
            this.rdoClient = new System.Windows.Forms.RadioButton();
            this.grpServer = new System.Windows.Forms.GroupBox();
            this.lblServeInterface = new System.Windows.Forms.Label();
            this.cboServeInterface = new System.Windows.Forms.ComboBox();
            this.lblServeAddress = new System.Windows.Forms.Label();
            this.edtServeAddress = new System.Windows.Forms.TextBox();
            this.lblServePort = new System.Windows.Forms.Label();
            this.numServePort = new System.Windows.Forms.NumericUpDown();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.lblState = new System.Windows.Forms.Label();
            this.grpBehavior = new System.Windows.Forms.GroupBox();
            this.grpClient = new System.Windows.Forms.GroupBox();
            this.lblTargetAddress = new System.Windows.Forms.Label();
            this.cboTargetAddress = new System.Windows.Forms.ComboBox();
            this.lblTargetPort = new System.Windows.Forms.Label();
            this.numTargetPort = new System.Windows.Forms.NumericUpDown();
            this.lblClientName = new System.Windows.Forms.Label();
            this.cboClientName = new System.Windows.Forms.ComboBox();
            this.grpServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numServePort)).BeginInit();
            this.grpBehavior.SuspendLayout();
            this.grpClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetPort)).BeginInit();
            this.SuspendLayout();
            // 
            // rdoServer
            // 
            this.rdoServer.AutoSize = true;
            this.rdoServer.Checked = true;
            this.rdoServer.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdoServer.Location = new System.Drawing.Point(35, 25);
            this.rdoServer.Name = "rdoServer";
            this.rdoServer.Size = new System.Drawing.Size(92, 22);
            this.rdoServer.TabIndex = 2;
            this.rdoServer.TabStop = true;
            this.rdoServer.Text = "作为服务器";
            this.rdoServer.UseVisualStyleBackColor = true;
            this.rdoServer.CheckedChanged += new System.EventHandler(this.rdoBehavior_CheckedChanged);
            // 
            // rdoClient
            // 
            this.rdoClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdoClient.AutoSize = true;
            this.rdoClient.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdoClient.Location = new System.Drawing.Point(153, 25);
            this.rdoClient.Name = "rdoClient";
            this.rdoClient.Size = new System.Drawing.Size(92, 22);
            this.rdoClient.TabIndex = 3;
            this.rdoClient.Text = "作为客户端";
            this.rdoClient.UseVisualStyleBackColor = true;
            this.rdoClient.CheckedChanged += new System.EventHandler(this.rdoBehavior_CheckedChanged);
            // 
            // grpServer
            // 
            this.grpServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpServer.Controls.Add(this.lblServeInterface);
            this.grpServer.Controls.Add(this.cboServeInterface);
            this.grpServer.Controls.Add(this.lblServeAddress);
            this.grpServer.Controls.Add(this.edtServeAddress);
            this.grpServer.Controls.Add(this.lblServePort);
            this.grpServer.Controls.Add(this.numServePort);
            this.grpServer.Location = new System.Drawing.Point(12, 76);
            this.grpServer.Name = "grpServer";
            this.grpServer.Size = new System.Drawing.Size(280, 112);
            this.grpServer.TabIndex = 4;
            this.grpServer.TabStop = false;
            this.grpServer.Text = "服务器配置";
            // 
            // lblServeInterface
            // 
            this.lblServeInterface.AutoSize = true;
            this.lblServeInterface.Location = new System.Drawing.Point(6, 25);
            this.lblServeInterface.Name = "lblServeInterface";
            this.lblServeInterface.Size = new System.Drawing.Size(39, 17);
            this.lblServeInterface.TabIndex = 5;
            this.lblServeInterface.Text = "接口 :";
            // 
            // cboServeInterface
            // 
            this.cboServeInterface.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboServeInterface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboServeInterface.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboServeInterface.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboServeInterface.FormattingEnabled = true;
            this.cboServeInterface.Location = new System.Drawing.Point(51, 21);
            this.cboServeInterface.Name = "cboServeInterface";
            this.cboServeInterface.Size = new System.Drawing.Size(223, 24);
            this.cboServeInterface.TabIndex = 6;
            this.cboServeInterface.SelectedIndexChanged += new System.EventHandler(this.cboInterface_SelectedIndexChanged);
            // 
            // lblServeAddress
            // 
            this.lblServeAddress.AutoSize = true;
            this.lblServeAddress.Location = new System.Drawing.Point(6, 54);
            this.lblServeAddress.Name = "lblServeAddress";
            this.lblServeAddress.Size = new System.Drawing.Size(63, 17);
            this.lblServeAddress.TabIndex = 7;
            this.lblServeAddress.Text = "监听地址 :";
            // 
            // edtServeAddress
            // 
            this.edtServeAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtServeAddress.Location = new System.Drawing.Point(75, 51);
            this.edtServeAddress.Name = "edtServeAddress";
            this.edtServeAddress.ReadOnly = true;
            this.edtServeAddress.Size = new System.Drawing.Size(199, 23);
            this.edtServeAddress.TabIndex = 8;
            this.edtServeAddress.Text = "0.0.0.0";
            // 
            // lblServePort
            // 
            this.lblServePort.AutoSize = true;
            this.lblServePort.Location = new System.Drawing.Point(6, 83);
            this.lblServePort.Name = "lblServePort";
            this.lblServePort.Size = new System.Drawing.Size(63, 17);
            this.lblServePort.TabIndex = 9;
            this.lblServePort.Text = "监听端口 :";
            // 
            // numServePort
            // 
            this.numServePort.Location = new System.Drawing.Point(75, 80);
            this.numServePort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numServePort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numServePort.Name = "numServePort";
            this.numServePort.Size = new System.Drawing.Size(92, 23);
            this.numServePort.TabIndex = 10;
            this.numServePort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numServePort.Value = new decimal(new int[] {
            10240,
            0,
            0,
            0});
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStart.Location = new System.Drawing.Point(136, 312);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 25);
            this.btnStart.TabIndex = 19;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(217, 312);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblState
            // 
            this.lblState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(65, 316);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(65, 17);
            this.lblState.TabIndex = 18;
            this.lblState.Text = "尝试监听...";
            this.lblState.Visible = false;
            // 
            // grpBehavior
            // 
            this.grpBehavior.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBehavior.Controls.Add(this.rdoServer);
            this.grpBehavior.Controls.Add(this.rdoClient);
            this.grpBehavior.Location = new System.Drawing.Point(12, 12);
            this.grpBehavior.Name = "grpBehavior";
            this.grpBehavior.Size = new System.Drawing.Size(280, 58);
            this.grpBehavior.TabIndex = 1;
            this.grpBehavior.TabStop = false;
            this.grpBehavior.Text = "Windows 端的行为";
            // 
            // grpClient
            // 
            this.grpClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpClient.Controls.Add(this.lblTargetAddress);
            this.grpClient.Controls.Add(this.cboTargetAddress);
            this.grpClient.Controls.Add(this.lblTargetPort);
            this.grpClient.Controls.Add(this.numTargetPort);
            this.grpClient.Controls.Add(this.lblClientName);
            this.grpClient.Controls.Add(this.cboClientName);
            this.grpClient.Location = new System.Drawing.Point(12, 194);
            this.grpClient.Name = "grpClient";
            this.grpClient.Size = new System.Drawing.Size(280, 112);
            this.grpClient.TabIndex = 11;
            this.grpClient.TabStop = false;
            this.grpClient.Text = "客户端配置";
            // 
            // lblTargetAddress
            // 
            this.lblTargetAddress.AutoSize = true;
            this.lblTargetAddress.Location = new System.Drawing.Point(6, 25);
            this.lblTargetAddress.Name = "lblTargetAddress";
            this.lblTargetAddress.Size = new System.Drawing.Size(63, 17);
            this.lblTargetAddress.TabIndex = 12;
            this.lblTargetAddress.Text = "目的地址 :";
            // 
            // cboTargetAddress
            // 
            this.cboTargetAddress.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboTargetAddress.FormattingEnabled = true;
            this.cboTargetAddress.ItemHeight = 17;
            this.cboTargetAddress.Location = new System.Drawing.Point(75, 22);
            this.cboTargetAddress.Name = "cboTargetAddress";
            this.cboTargetAddress.Size = new System.Drawing.Size(199, 23);
            this.cboTargetAddress.TabIndex = 13;
            this.cboTargetAddress.Text = "127.0.0.1";
            // 
            // lblTargetPort
            // 
            this.lblTargetPort.AutoSize = true;
            this.lblTargetPort.Location = new System.Drawing.Point(6, 54);
            this.lblTargetPort.Name = "lblTargetPort";
            this.lblTargetPort.Size = new System.Drawing.Size(63, 17);
            this.lblTargetPort.TabIndex = 14;
            this.lblTargetPort.Text = "目的端口 :";
            // 
            // numTargetPort
            // 
            this.numTargetPort.Location = new System.Drawing.Point(75, 51);
            this.numTargetPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numTargetPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numTargetPort.Name = "numTargetPort";
            this.numTargetPort.Size = new System.Drawing.Size(92, 23);
            this.numTargetPort.TabIndex = 15;
            this.numTargetPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTargetPort.Value = new decimal(new int[] {
            10240,
            0,
            0,
            0});
            // 
            // lblClientName
            // 
            this.lblClientName.AutoSize = true;
            this.lblClientName.Location = new System.Drawing.Point(6, 83);
            this.lblClientName.Name = "lblClientName";
            this.lblClientName.Size = new System.Drawing.Size(75, 17);
            this.lblClientName.TabIndex = 16;
            this.lblClientName.Text = "名称 (可选) :";
            // 
            // cboClientName
            // 
            this.cboClientName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboClientName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboClientName.FormattingEnabled = true;
            this.cboClientName.IntegralHeight = false;
            this.cboClientName.ItemHeight = 17;
            this.cboClientName.Location = new System.Drawing.Point(87, 80);
            this.cboClientName.Name = "cboClientName";
            this.cboClientName.Size = new System.Drawing.Size(187, 23);
            this.cboClientName.TabIndex = 17;
            // 
            // InitForm
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(304, 344);
            this.Controls.Add(this.grpBehavior);
            this.Controls.Add(this.grpServer);
            this.Controls.Add(this.grpClient);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnExit);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "InitForm";
            this.Text = "LAN Data Transmitter";
            this.Load += new System.EventHandler(this.InitForm_Load);
            this.grpServer.ResumeLayout(false);
            this.grpServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numServePort)).EndInit();
            this.grpBehavior.ResumeLayout(false);
            this.grpBehavior.PerformLayout();
            this.grpClient.ResumeLayout(false);
            this.grpClient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoServer;
        private System.Windows.Forms.RadioButton rdoClient;
        private System.Windows.Forms.GroupBox grpServer;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblServePort;
        private System.Windows.Forms.Label lblServeAddress;
        private System.Windows.Forms.TextBox edtServeAddress;
        private System.Windows.Forms.NumericUpDown numServePort;
        private System.Windows.Forms.ComboBox cboServeInterface;
        private System.Windows.Forms.ToolTip tipMain;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.GroupBox grpBehavior;
        private System.Windows.Forms.Label lblServeInterface;
        private System.Windows.Forms.GroupBox grpClient;
        private System.Windows.Forms.Label lblClientName;
        private System.Windows.Forms.Label lblTargetAddress;
        private System.Windows.Forms.Label lblTargetPort;
        private System.Windows.Forms.NumericUpDown numTargetPort;
        private System.Windows.Forms.ComboBox cboClientName;
        private System.Windows.Forms.ComboBox cboTargetAddress;
    }
}