
namespace LanDataTransmitter {
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
            this.grpConnect = new System.Windows.Forms.GroupBox();
            this.cboInterface = new System.Windows.Forms.ComboBox();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.edtAddress = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.lblHint = new System.Windows.Forms.Label();
            this.grpBehavior = new System.Windows.Forms.GroupBox();
            this.grpConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.grpBehavior.SuspendLayout();
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
            this.rdoServer.TabIndex = 1;
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
            this.rdoClient.Location = new System.Drawing.Point(144, 25);
            this.rdoClient.Name = "rdoClient";
            this.rdoClient.Size = new System.Drawing.Size(92, 22);
            this.rdoClient.TabIndex = 2;
            this.rdoClient.Text = "作为客户端";
            this.rdoClient.UseVisualStyleBackColor = true;
            this.rdoClient.CheckedChanged += new System.EventHandler(this.rdoBehavior_CheckedChanged);
            // 
            // grpConnect
            // 
            this.grpConnect.Controls.Add(this.cboInterface);
            this.grpConnect.Controls.Add(this.numPort);
            this.grpConnect.Controls.Add(this.edtAddress);
            this.grpConnect.Controls.Add(this.lblPort);
            this.grpConnect.Controls.Add(this.lblAddress);
            this.grpConnect.Location = new System.Drawing.Point(12, 76);
            this.grpConnect.Name = "grpConnect";
            this.grpConnect.Size = new System.Drawing.Size(271, 111);
            this.grpConnect.TabIndex = 3;
            this.grpConnect.TabStop = false;
            this.grpConnect.Text = "连接配置";
            // 
            // cboInterface
            // 
            this.cboInterface.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboInterface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInterface.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboInterface.FormattingEnabled = true;
            this.cboInterface.Location = new System.Drawing.Point(6, 22);
            this.cboInterface.Name = "cboInterface";
            this.cboInterface.Size = new System.Drawing.Size(259, 25);
            this.cboInterface.TabIndex = 4;
            this.cboInterface.SelectedIndexChanged += new System.EventHandler(this.cboInterface_SelectedIndexChanged);
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(75, 82);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(92, 23);
            this.numPort.TabIndex = 8;
            this.numPort.Value = new decimal(new int[] {
            10240,
            0,
            0,
            0});
            // 
            // edtAddress
            // 
            this.edtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtAddress.Location = new System.Drawing.Point(75, 53);
            this.edtAddress.Name = "edtAddress";
            this.edtAddress.ReadOnly = true;
            this.edtAddress.Size = new System.Drawing.Size(190, 23);
            this.edtAddress.TabIndex = 6;
            this.edtAddress.Text = "0.0.0.0";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(6, 84);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(63, 17);
            this.lblPort.TabIndex = 7;
            this.lblPort.Text = "监听端口 :";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(6, 56);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(67, 17);
            this.lblAddress.TabIndex = 5;
            this.lblAddress.Text = "IPv4 地址 :";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStart.Location = new System.Drawing.Point(127, 196);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 25);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(208, 196);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblHint
            // 
            this.lblHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHint.AutoSize = true;
            this.lblHint.Location = new System.Drawing.Point(56, 200);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(65, 17);
            this.lblHint.TabIndex = 9;
            this.lblHint.Text = "尝试监听...";
            this.lblHint.Visible = false;
            // 
            // grpBehavior
            // 
            this.grpBehavior.Controls.Add(this.rdoServer);
            this.grpBehavior.Controls.Add(this.rdoClient);
            this.grpBehavior.Location = new System.Drawing.Point(12, 12);
            this.grpBehavior.Name = "grpBehavior";
            this.grpBehavior.Size = new System.Drawing.Size(271, 58);
            this.grpBehavior.TabIndex = 12;
            this.grpBehavior.TabStop = false;
            this.grpBehavior.Text = "Windows 客户端的行为";
            // 
            // InitForm
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(295, 230);
            this.Controls.Add(this.grpBehavior);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.grpConnect);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "InitForm";
            this.Text = "LAN Data Transmitter";
            this.Load += new System.EventHandler(this.InitForm_Load);
            this.grpConnect.ResumeLayout(false);
            this.grpConnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.grpBehavior.ResumeLayout(false);
            this.grpBehavior.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoServer;
        private System.Windows.Forms.RadioButton rdoClient;
        private System.Windows.Forms.GroupBox grpConnect;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox edtAddress;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.ComboBox cboInterface;
        private System.Windows.Forms.ToolTip tipMain;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.GroupBox grpBehavior;
    }
}