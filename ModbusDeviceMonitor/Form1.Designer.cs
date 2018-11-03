namespace ModbusDeviceMonitor
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemDevice = new System.Windows.Forms.MenuItem();
            this.menuItemNew = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemMonitor = new System.Windows.Forms.MenuItem();
            this.menuItemStop = new System.Windows.Forms.MenuItem();
            this.menuItemSetting = new System.Windows.Forms.MenuItem();
            this.menuItemComm = new System.Windows.Forms.MenuItem();
            this.menuItemAddOutputStatus = new System.Windows.Forms.MenuItem();
            this.menuItemSingle = new System.Windows.Forms.MenuItem();
            this.menuItem16Bits = new System.Windows.Forms.MenuItem();
            this.menuItemAddInputStatus = new System.Windows.Forms.MenuItem();
            this.menuItemCont8Regs = new System.Windows.Forms.MenuItem();
            this.menuItem16BitsStatus = new System.Windows.Forms.MenuItem();
            this.menuItemAddInputRegistrer = new System.Windows.Forms.MenuItem();
            this.menuItemAddHoldReg = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItemLayout = new System.Windows.Forms.MenuItem();
            this.menuItemArrangeIcons = new System.Windows.Forms.MenuItem();
            this.menuItemCascade = new System.Windows.Forms.MenuItem();
            this.menuItemHorizontalArrange = new System.Windows.Forms.MenuItem();
            this.menuItemVerticalArrange = new System.Windows.Forms.MenuItem();
            this.menuItemMaximize = new System.Windows.Forms.MenuItem();
            this.menuItemMinimize = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDevice,
            this.menuItemSetting,
            this.menuItem9,
            this.menuItemLayout,
            this.menuItemAbout});
            // 
            // menuItemDevice
            // 
            this.menuItemDevice.Index = 0;
            this.menuItemDevice.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemNew,
            this.menuItem1,
            this.menuItemOpen,
            this.menuItemSave,
            this.menuItemMonitor,
            this.menuItemStop});
            this.menuItemDevice.Text = "设备(&D)";
            // 
            // menuItemNew
            // 
            this.menuItemNew.Index = 0;
            this.menuItemNew.Text = "新建(&N)";
            this.menuItemNew.Click += new System.EventHandler(this.menuItemNew_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "添加(&A)";
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Index = 2;
            this.menuItemOpen.Text = "打开(&O)";
            this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
            // 
            // menuItemSave
            // 
            this.menuItemSave.Index = 3;
            this.menuItemSave.Text = "保存(&S)";
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItemMonitor
            // 
            this.menuItemMonitor.Index = 4;
            this.menuItemMonitor.Text = "监控(&M)";
            this.menuItemMonitor.Click += new System.EventHandler(this.menuItemMonitor_Click);
            // 
            // menuItemStop
            // 
            this.menuItemStop.Index = 5;
            this.menuItemStop.Text = "停止(&H)";
            this.menuItemStop.Click += new System.EventHandler(this.menuItemStop_Click);
            // 
            // menuItemSetting
            // 
            this.menuItemSetting.Index = 1;
            this.menuItemSetting.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemComm,
            this.menuItemAddOutputStatus,
            this.menuItemAddInputStatus,
            this.menuItemAddInputRegistrer,
            this.menuItemAddHoldReg});
            this.menuItemSetting.Text = "设置(&S)";
            // 
            // menuItemComm
            // 
            this.menuItemComm.Index = 0;
            this.menuItemComm.Text = "通信(&C)";
            this.menuItemComm.Click += new System.EventHandler(this.menuItemComm_Click);
            // 
            // menuItemAddOutputStatus
            // 
            this.menuItemAddOutputStatus.Index = 1;
            this.menuItemAddOutputStatus.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSingle,
            this.menuItem16Bits});
            this.menuItemAddOutputStatus.Text = "添加输出状态寄存器(&O)";
            // 
            // menuItemSingle
            // 
            this.menuItemSingle.Index = 0;
            this.menuItemSingle.Text = "单寄存器单状态";
            this.menuItemSingle.Click += new System.EventHandler(this.menuItemSingle_Click);
            // 
            // menuItem16Bits
            // 
            this.menuItem16Bits.Index = 1;
            this.menuItem16Bits.Text = "单寄存器16位状态";
            this.menuItem16Bits.Click += new System.EventHandler(this.menuItem16Bits_Click);
            // 
            // menuItemAddInputStatus
            // 
            this.menuItemAddInputStatus.Index = 2;
            this.menuItemAddInputStatus.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCont8Regs,
            this.menuItem16BitsStatus});
            this.menuItemAddInputStatus.Text = "添加输入状态寄存器(&I)";
            // 
            // menuItemCont8Regs
            // 
            this.menuItemCont8Regs.Index = 0;
            this.menuItemCont8Regs.Text = "连续8个寄存器状态";
            this.menuItemCont8Regs.Click += new System.EventHandler(this.menuItemCont8Regs_Click);
            // 
            // menuItem16BitsStatus
            // 
            this.menuItem16BitsStatus.Index = 1;
            this.menuItem16BitsStatus.Text = "单寄存器16位状态";
            this.menuItem16BitsStatus.Click += new System.EventHandler(this.menuItem16BitsStatus_Click);
            // 
            // menuItemAddInputRegistrer
            // 
            this.menuItemAddInputRegistrer.Index = 3;
            this.menuItemAddInputRegistrer.Text = "添加输入寄存器(&R)";
            this.menuItemAddInputRegistrer.Click += new System.EventHandler(this.menuItemAddInputRegistrer_Click);
            // 
            // menuItemAddHoldReg
            // 
            this.menuItemAddHoldReg.Index = 4;
            this.menuItemAddHoldReg.Text = "添加保持寄存器(&H)";
            this.menuItemAddHoldReg.Click += new System.EventHandler(this.menuItemAddHoldReg_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 2;
            this.menuItem9.MdiList = true;
            this.menuItem9.Text = "窗口(&W)";
            // 
            // menuItemLayout
            // 
            this.menuItemLayout.Index = 3;
            this.menuItemLayout.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemArrangeIcons,
            this.menuItemCascade,
            this.menuItemHorizontalArrange,
            this.menuItemVerticalArrange,
            this.menuItemMaximize,
            this.menuItemMinimize});
            this.menuItemLayout.Text = "排版(&L)";
            // 
            // menuItemArrangeIcons
            // 
            this.menuItemArrangeIcons.Index = 0;
            this.menuItemArrangeIcons.Text = "排列图标(&I)";
            this.menuItemArrangeIcons.Click += new System.EventHandler(this.menuItemArrange_Click);
            // 
            // menuItemCascade
            // 
            this.menuItemCascade.Index = 1;
            this.menuItemCascade.Text = "层叠(&C)";
            this.menuItemCascade.Click += new System.EventHandler(this.menuItemCascade_Click);
            // 
            // menuItemHorizontalArrange
            // 
            this.menuItemHorizontalArrange.Index = 2;
            this.menuItemHorizontalArrange.Text = "水平平铺(&H)";
            this.menuItemHorizontalArrange.Click += new System.EventHandler(this.menuItemHorizontalArrange_Click);
            // 
            // menuItemVerticalArrange
            // 
            this.menuItemVerticalArrange.Index = 3;
            this.menuItemVerticalArrange.Text = "垂直平铺(&V)";
            this.menuItemVerticalArrange.Click += new System.EventHandler(this.menuItemVerticalArrange_Click);
            // 
            // menuItemMaximize
            // 
            this.menuItemMaximize.Index = 4;
            this.menuItemMaximize.Text = "最大化(&X)";
            this.menuItemMaximize.Click += new System.EventHandler(this.menuItemMaximize_Click);
            // 
            // menuItemMinimize
            // 
            this.menuItemMinimize.Index = 5;
            this.menuItemMinimize.Text = "最小化(&N)";
            this.menuItemMinimize.Click += new System.EventHandler(this.menuItemMinimize_Click);
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 4;
            this.menuItemAbout.Text = "关于(&A)";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 425);
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "ModBusDeviceMonitor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItemDevice;
        private System.Windows.Forms.MenuItem menuItemNew;
        private System.Windows.Forms.MenuItem menuItemOpen;
        private System.Windows.Forms.MenuItem menuItemSave;
        private System.Windows.Forms.MenuItem menuItemMonitor;
        private System.Windows.Forms.MenuItem menuItemStop;
        private System.Windows.Forms.MenuItem menuItemSetting;
        private System.Windows.Forms.MenuItem menuItemComm;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem menuItemAbout;
        private System.Windows.Forms.MenuItem menuItemAddOutputStatus;
        private System.Windows.Forms.MenuItem menuItemSingle;
        private System.Windows.Forms.MenuItem menuItem16Bits;
        private System.Windows.Forms.MenuItem menuItemAddInputStatus;
        private System.Windows.Forms.MenuItem menuItemCont8Regs;
        private System.Windows.Forms.MenuItem menuItem16BitsStatus;
        private System.Windows.Forms.MenuItem menuItemAddInputRegistrer;
        private System.Windows.Forms.MenuItem menuItemAddHoldReg;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItemLayout;
        private System.Windows.Forms.MenuItem menuItemArrangeIcons;
        private System.Windows.Forms.MenuItem menuItemCascade;
        private System.Windows.Forms.MenuItem menuItemHorizontalArrange;
        private System.Windows.Forms.MenuItem menuItemVerticalArrange;
        private System.Windows.Forms.MenuItem menuItemMaximize;
        private System.Windows.Forms.MenuItem menuItemMinimize;
        private System.Windows.Forms.Timer timer1;
    }
}

