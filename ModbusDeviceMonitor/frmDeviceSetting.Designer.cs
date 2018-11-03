namespace ModbusDeviceMonitor
{
    partial class frmDeviceSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.tbDeviceName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxCheck = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxStopBits = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxDataBits = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxBaud = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxPort = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numUpDownWriteHoldRegs = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.numUpDownWriteCoils = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.numUpDownWriteHoldReg = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.numUpDownWriteCoil = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.numUpDownReadInputReg = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numUpDownReadHoldReg = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.numUpDownReadSwitch = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numUpDownReadCoil = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownWriteHoldRegs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownWriteCoils)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownWriteHoldReg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownWriteCoil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownReadInputReg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownReadHoldReg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownReadSwitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownReadCoil)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "总线地址:";
            // 
            // tbAddress
            // 
            this.tbAddress.Location = new System.Drawing.Point(78, 53);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(124, 21);
            this.tbAddress.TabIndex = 1;
            // 
            // tbDeviceName
            // 
            this.tbDeviceName.Location = new System.Drawing.Point(78, 20);
            this.tbDeviceName.Name = "tbDeviceName";
            this.tbDeviceName.Size = new System.Drawing.Size(124, 21);
            this.tbDeviceName.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "设备名称:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(268, 301);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(283, 37);
            this.button1.TabIndex = 4;
            this.button1.Text = "添加";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "端口号:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxCheck);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.comboBoxStopBits);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.comboBoxDataBits);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboBoxBaud);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBoxPort);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(23, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(217, 208);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "端口设置";
            // 
            // comboBoxCheck
            // 
            this.comboBoxCheck.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCheck.FormattingEnabled = true;
            this.comboBoxCheck.Location = new System.Drawing.Point(78, 174);
            this.comboBoxCheck.Name = "comboBoxCheck";
            this.comboBoxCheck.Size = new System.Drawing.Size(101, 20);
            this.comboBoxCheck.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 178);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "校  验:";
            // 
            // comboBoxStopBits
            // 
            this.comboBoxStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStopBits.FormattingEnabled = true;
            this.comboBoxStopBits.Location = new System.Drawing.Point(78, 138);
            this.comboBoxStopBits.Name = "comboBoxStopBits";
            this.comboBoxStopBits.Size = new System.Drawing.Size(101, 20);
            this.comboBoxStopBits.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "停止位:";
            // 
            // comboBoxDataBits
            // 
            this.comboBoxDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDataBits.FormattingEnabled = true;
            this.comboBoxDataBits.Location = new System.Drawing.Point(78, 102);
            this.comboBoxDataBits.Name = "comboBoxDataBits";
            this.comboBoxDataBits.Size = new System.Drawing.Size(101, 20);
            this.comboBoxDataBits.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "数据位:";
            // 
            // comboBoxBaud
            // 
            this.comboBoxBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBaud.FormattingEnabled = true;
            this.comboBoxBaud.Location = new System.Drawing.Point(78, 66);
            this.comboBoxBaud.Name = "comboBoxBaud";
            this.comboBoxBaud.Size = new System.Drawing.Size(101, 20);
            this.comboBoxBaud.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "波特率:";
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Location = new System.Drawing.Point(78, 30);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(101, 20);
            this.comboBoxPort.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(206, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "(0-247)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numUpDownWriteHoldRegs);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.numUpDownWriteCoils);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.numUpDownWriteHoldReg);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.numUpDownWriteCoil);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.numUpDownReadInputReg);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.numUpDownReadHoldReg);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.numUpDownReadSwitch);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.numUpDownReadCoil);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(268, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(283, 261);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "功能码";
            // 
            // numUpDownWriteHoldRegs
            // 
            this.numUpDownWriteHoldRegs.Location = new System.Drawing.Point(209, 215);
            this.numUpDownWriteHoldRegs.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numUpDownWriteHoldRegs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownWriteHoldRegs.Name = "numUpDownWriteHoldRegs";
            this.numUpDownWriteHoldRegs.Size = new System.Drawing.Size(52, 21);
            this.numUpDownWriteHoldRegs.TabIndex = 16;
            this.numUpDownWriteHoldRegs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(21, 192);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(143, 12);
            this.label13.TabIndex = 15;
            this.label13.Text = "写多个线圈(00001-09999)";
            // 
            // numUpDownWriteCoils
            // 
            this.numUpDownWriteCoils.Location = new System.Drawing.Point(209, 188);
            this.numUpDownWriteCoils.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numUpDownWriteCoils.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownWriteCoils.Name = "numUpDownWriteCoils";
            this.numUpDownWriteCoils.Size = new System.Drawing.Size(52, 21);
            this.numUpDownWriteCoils.TabIndex = 14;
            this.numUpDownWriteCoils.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(21, 163);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(179, 12);
            this.label14.TabIndex = 13;
            this.label14.Text = "写单个保持寄存器(40001-49999)";
            // 
            // numUpDownWriteHoldReg
            // 
            this.numUpDownWriteHoldReg.Location = new System.Drawing.Point(209, 161);
            this.numUpDownWriteHoldReg.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numUpDownWriteHoldReg.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownWriteHoldReg.Name = "numUpDownWriteHoldReg";
            this.numUpDownWriteHoldReg.Size = new System.Drawing.Size(52, 21);
            this.numUpDownWriteHoldReg.TabIndex = 12;
            this.numUpDownWriteHoldReg.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(21, 219);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(179, 12);
            this.label15.TabIndex = 11;
            this.label15.Text = "写多个保持寄存器(40001-49999)";
            // 
            // numUpDownWriteCoil
            // 
            this.numUpDownWriteCoil.Location = new System.Drawing.Point(209, 134);
            this.numUpDownWriteCoil.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numUpDownWriteCoil.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownWriteCoil.Name = "numUpDownWriteCoil";
            this.numUpDownWriteCoil.Size = new System.Drawing.Size(52, 21);
            this.numUpDownWriteCoil.TabIndex = 10;
            this.numUpDownWriteCoil.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(21, 138);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(143, 12);
            this.label16.TabIndex = 9;
            this.label16.Text = "写单个线圈(00001-09999)";
            // 
            // numUpDownReadInputReg
            // 
            this.numUpDownReadInputReg.Location = new System.Drawing.Point(209, 107);
            this.numUpDownReadInputReg.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numUpDownReadInputReg.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownReadInputReg.Name = "numUpDownReadInputReg";
            this.numUpDownReadInputReg.Size = new System.Drawing.Size(52, 21);
            this.numUpDownReadInputReg.TabIndex = 8;
            this.numUpDownReadInputReg.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 113);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(155, 12);
            this.label11.TabIndex = 7;
            this.label11.Text = "读输入寄存器(30001-39999)";
            // 
            // numUpDownReadHoldReg
            // 
            this.numUpDownReadHoldReg.Location = new System.Drawing.Point(209, 80);
            this.numUpDownReadHoldReg.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numUpDownReadHoldReg.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownReadHoldReg.Name = "numUpDownReadHoldReg";
            this.numUpDownReadHoldReg.Size = new System.Drawing.Size(52, 21);
            this.numUpDownReadHoldReg.TabIndex = 6;
            this.numUpDownReadHoldReg.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 86);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(155, 12);
            this.label12.TabIndex = 5;
            this.label12.Text = "读保持寄存器(40001-49999)";
            // 
            // numUpDownReadSwitch
            // 
            this.numUpDownReadSwitch.Location = new System.Drawing.Point(209, 53);
            this.numUpDownReadSwitch.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numUpDownReadSwitch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownReadSwitch.Name = "numUpDownReadSwitch";
            this.numUpDownReadSwitch.Size = new System.Drawing.Size(52, 21);
            this.numUpDownReadSwitch.TabIndex = 4;
            this.numUpDownReadSwitch.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(143, 12);
            this.label10.TabIndex = 3;
            this.label10.Text = "读开关状态(10001-19999)";
            // 
            // numUpDownReadCoil
            // 
            this.numUpDownReadCoil.Location = new System.Drawing.Point(209, 26);
            this.numUpDownReadCoil.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numUpDownReadCoil.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownReadCoil.Name = "numUpDownReadCoil";
            this.numUpDownReadCoil.Size = new System.Drawing.Size(52, 21);
            this.numUpDownReadCoil.TabIndex = 2;
            this.numUpDownReadCoil.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(143, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "读线圈状态(00001-09999)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(21, 93);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 12);
            this.label17.TabIndex = 10;
            this.label17.Text = "通信模式:";
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.Enabled = false;
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Location = new System.Drawing.Point(78, 88);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(124, 20);
            this.comboBoxMode.TabIndex = 11;
            // 
            // frmDeviceSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 351);
            this.Controls.Add(this.comboBoxMode);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbDeviceName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbAddress);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDeviceSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加新设备";
            this.Load += new System.EventHandler(this.frmDeviceSetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownWriteHoldRegs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownWriteCoils)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownWriteHoldReg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownWriteCoil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownReadInputReg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownReadHoldReg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownReadSwitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownReadCoil)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.TextBox tbDeviceName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxCheck;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxStopBits;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxDataBits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxBaud;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxPort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numUpDownWriteHoldRegs;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numUpDownWriteCoils;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown numUpDownWriteHoldReg;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numUpDownWriteCoil;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown numUpDownReadInputReg;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numUpDownReadHoldReg;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numUpDownReadSwitch;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numUpDownReadCoil;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox comboBoxMode;
    }
}