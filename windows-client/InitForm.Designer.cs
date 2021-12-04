
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
            this.rbtnServer = new System.Windows.Forms.RadioButton();
            this.rbtnClient = new System.Windows.Forms.RadioButton();
            this.lblSelect = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbbInterface = new System.Windows.Forms.ComboBox();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.edtAddress = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblHint = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.SuspendLayout();
            // 
            // rbtnServer
            // 
            this.rbtnServer.AutoSize = true;
            this.rbtnServer.Checked = true;
            this.rbtnServer.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbtnServer.Location = new System.Drawing.Point(15, 31);
            this.rbtnServer.Name = "rbtnServer";
            this.rbtnServer.Size = new System.Drawing.Size(92, 22);
            this.rbtnServer.TabIndex = 1;
            this.rbtnServer.TabStop = true;
            this.rbtnServer.Text = "作为服务器";
            this.rbtnServer.UseVisualStyleBackColor = true;
            this.rbtnServer.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // rbtnClient
            // 
            this.rbtnClient.AutoSize = true;
            this.rbtnClient.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbtnClient.Location = new System.Drawing.Point(15, 56);
            this.rbtnClient.Name = "rbtnClient";
            this.rbtnClient.Size = new System.Drawing.Size(92, 22);
            this.rbtnClient.TabIndex = 2;
            this.rbtnClient.Text = "作为客户端";
            this.rbtnClient.UseVisualStyleBackColor = true;
            this.rbtnClient.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.Location = new System.Drawing.Point(12, 9);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(189, 17);
            this.lblSelect.TabIndex = 0;
            this.lblSelect.Text = "请选择 Windows 客户端的行为：";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbbInterface);
            this.groupBox1.Controls.Add(this.numPort);
            this.groupBox1.Controls.Add(this.edtAddress);
            this.groupBox1.Controls.Add(this.lblPort);
            this.groupBox1.Controls.Add(this.lblAddress);
            this.groupBox1.Location = new System.Drawing.Point(12, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(271, 114);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "连接配置";
            // 
            // cbbInterface
            // 
            this.cbbInterface.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbInterface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbInterface.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbbInterface.FormattingEnabled = true;
            this.cbbInterface.Location = new System.Drawing.Point(6, 22);
            this.cbbInterface.Name = "cbbInterface";
            this.cbbInterface.Size = new System.Drawing.Size(259, 25);
            this.cbbInterface.TabIndex = 4;
            this.cbbInterface.SelectedIndexChanged += new System.EventHandler(this.cbbInterface_SelectedIndexChanged);
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(84, 82);
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
            this.numPort.Size = new System.Drawing.Size(83, 23);
            this.numPort.TabIndex = 8;
            this.numPort.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // edtAddress
            // 
            this.edtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtAddress.Location = new System.Drawing.Point(84, 53);
            this.edtAddress.Name = "edtAddress";
            this.edtAddress.Size = new System.Drawing.Size(181, 23);
            this.edtAddress.TabIndex = 6;
            this.edtAddress.Text = "0.0.0.0";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(6, 84);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(68, 17);
            this.lblPort.TabIndex = 7;
            this.lblPort.Text = "监听端口：";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(6, 56);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(72, 17);
            this.lblAddress.TabIndex = 5;
            this.lblAddress.Text = "IPv4 地址：";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStart.Location = new System.Drawing.Point(127, 207);
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
            this.btnExit.Location = new System.Drawing.Point(208, 207);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.Location = new System.Drawing.Point(56, 211);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(65, 17);
            this.lblHint.TabIndex = 9;
            this.lblHint.Text = "尝试监听...";
            this.lblHint.Visible = false;
            // 
            // InitForm
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(295, 241);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblSelect);
            this.Controls.Add(this.rbtnClient);
            this.Controls.Add(this.rbtnServer);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "InitForm";
            this.Text = "LAN Data Transmitter";
            this.Load += new System.EventHandler(this.InitForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtnServer;
        private System.Windows.Forms.RadioButton rbtnClient;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox edtAddress;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.ComboBox cbbInterface;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label lblHint;
    }
}