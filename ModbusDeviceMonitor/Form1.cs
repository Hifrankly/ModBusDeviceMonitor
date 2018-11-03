using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

namespace ModbusDeviceMonitor
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 设备对象集合
        /// </summary>
        private Devices _devices = new Devices();

        private StatusBar _statusBar1;
        
        public Form1()
        {
            InitializeComponent();

            CreateMyStatusBar();

            timer1.Enabled = true;
        }

        private void menuItemNew_Click(object sender, EventArgs e)
        {
            //if (this.ActiveMdiChild != null && !SaveCurrentDevice())
            //{
            //    return;
            //}

            frmDeviceSetting frmDevice = new frmDeviceSetting();
            frmDevice._refDevices = _devices;
            frmDevice.ShowDialog();
            if (frmDevice._bOk)
            {
                ModBusDevice device = new ModBusDevice();
                device._bCreatNew = true;
                frmDevice.GetDeviceInfo(ref device);

                _devices.Add(device);
                ShowDeviceForm(device);
            }
        }

        private void ShowDeviceForm(ModBusDevice device)
        {
            frmDevice frmDev = new frmDevice(device);
            frmDev.MdiParent = this;
            frmDev.SetBarText = InvokeSetBarText;
            frmDev.Show();
        }

        private bool SaveCurrentDevice()
        {
            frmDevice frmDev = (frmDevice)this.ActiveMdiChild;
            if (frmDev._device._bModified || frmDev._device._bCreatNew)
            {
                int nRet = (int)MessageBox.Show("是否保存当前设备数据?", "设备" + frmDev._device.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (nRet == (int)DialogResult.Yes)
                {
                    if (frmDev._device._bCreatNew)
                    {
                        SaveDevice(frmDev._device);
                    }
                    else
                    {
                        try
                        {
                            XmlSerializer xs = new XmlSerializer(typeof(ModBusDevice));
                            Stream myStream = new FileStream(frmDev._device._strFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                            xs.Serialize(myStream, frmDev._device);
                            frmDev._device._bModified = false;
                            myStream.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "保存设备" + frmDev._device.Name + "到文件失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else if (nRet != (int)DialogResult.No)
                {
                    return false;
                }
            }
            return true;
        }

        private void SaveDevice(ModBusDevice device)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "xml files (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    XmlSerializer xs = new XmlSerializer(typeof(ModBusDevice));
                    xs.Serialize(myStream, device);
                    device._bModified = false;
                    device._bCreatNew = false;
                    device._strFilePath = saveFileDialog1.FileName;
                    myStream.Close();
                }
            }
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            ModBusDevice device = GetDeviceFromFile();
            if (device != null)
            {
                _devices.Add(device);
                device.ResetCurrentStatus();
                ShowDeviceForm(device);
            }
        }

        private ModBusDevice GetDeviceFromFile()
        {
            Stream myStream;
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "xml files (*.xml)|*.xml";
            openFileDlg.FilterIndex = 1;
            openFileDlg.RestoreDirectory = true;

            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDlg.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Insert code to read the stream here.
                            XmlSerializer xs = new XmlSerializer(typeof(ModBusDevice));
                            ModBusDevice device = xs.Deserialize(myStream) as ModBusDevice;
                            return device;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "打开文件错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            return null;
        }

        private void menuItemArrange_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.ArrangeIcons);
        }

        private void menuItemCascade_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.Cascade);
        }

        private void menuItemHorizontalArrange_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.TileHorizontal);
        }

        private void menuItemVerticalArrange_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.TileVertical);
        }

        private void menuItemMaximize_Click(object sender, EventArgs e)
        {
            //for each child form set the window state to Maximized
            foreach (Form chform in this.MdiChildren)
                chform.WindowState = FormWindowState.Maximized;
        }

        private void menuItemMinimize_Click(object sender, EventArgs e)
        {
            //for each child form set the window state to Minimized
            foreach (Form chform in this.MdiChildren)
                chform.WindowState = FormWindowState.Minimized;
        }

        private void CreateMyStatusBar()
        {
            // Create a StatusBar control.
            _statusBar1 = new StatusBar();

            // Create two StatusBarPanel objects to display in the StatusBar.
            StatusBarPanel panel1 = new StatusBarPanel();
            StatusBarPanel panel2 = new StatusBarPanel();

            // Display the first panel with a sunken border style.
            panel1.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            // Initialize the text of the panel.
            panel1.Text = "Ready...";
            // Set the AutoSize property to use all remaining space on the StatusBar.
            panel1.AutoSize = StatusBarPanelAutoSize.Spring;

            // Display the second panel with a raised border style.
            panel2.BorderStyle = StatusBarPanelBorderStyle.Raised;
            // Create ToolTip text that displays the current time.
            panel2.ToolTipText = System.DateTime.Now.ToShortTimeString();
            // Set the text of the panel to the current date.
            panel2.Text = System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString();
            // Set the AutoSize property to size the panel to the size of the contents.
            panel2.AutoSize = StatusBarPanelAutoSize.Contents;
            // Display panels in the StatusBar control.
            _statusBar1.ShowPanels = true;
            // Add both panels to the StatusBarPanelCollection of the StatusBar.                        
            _statusBar1.Panels.Add(panel1);
            _statusBar1.Panels.Add(panel2);

            // Add the StatusBar to the form.
            this.Controls.Add(_statusBar1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Set the text of the panel to the current date.
            string strText = System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString();
            if (_statusBar1.Panels[1].Text != strText)
                _statusBar1.Panels[1].Text = strText;
        }

        public void InvokeSetBarText(string strText)
        {
            Invoke(new DelegSetBarText(SetBarText), strText);
        }

        public delegate void DelegSetBarText(string strText);
        public void SetBarText(string strText)
        {
            _statusBar1.Panels[0].Text = strText;
        }

        private void menuItemComm_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                frmDevice frmDev = (frmDevice)(this.ActiveMdiChild);
                ModBusDevice device = frmDev._device;
                frmDeviceSetting commSetting = new frmDeviceSetting(device);
                commSetting.ShowDialog();
                if (commSetting._bOk)
                {
                    if (commSetting.PortIsChanged())
                    {
                        bool bCurMonitorStatus = device._bMonitor;
                        bool bPortIsOpen = (device._port != null) ? device._port.IsOpen : false;
                        device._bMonitor = false;
                        device.ClosePort();
                        commSetting.GetDeviceInfo(ref device);

                        if (bPortIsOpen)
                            device.OpenPort();

                        device._bMonitor = bCurMonitorStatus;
                    }
                    device._bModified = true;
                }
            }
        }

        private void menuItemSingle_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null)
            {
                MessageBox.Show("请先添加设备后再添加寄存器!", "添加输出状态寄存器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmOutputCoil frmOutputCoil = new frmOutputCoil();
            frmOutputCoil.AddOutputCoil = new frmOutputCoil.DelegateAddOuputCoil(((frmDevice)this.ActiveMdiChild).AddOutput);
            frmOutputCoil.Text = "添加输出状态寄存器";
            frmOutputCoil.ShowDialog();
        }

        private void menuItem16Bits_Click(object sender, EventArgs e)
        {
            frmMultiBitsSetting bitsDlg = new frmMultiBitsSetting();
            bitsDlg._bOutputStatus = true;
            bitsDlg.AddOutputBitsStatus = new frmMultiBitsSetting.DelegAddBitsStatus(((frmDevice)this.ActiveMdiChild).AddOutput);
            bitsDlg.ShowDialog();
        }

        private void menuItemCont8Regs_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null)
            {
                MessageBox.Show("请先添加设备后再添加寄存器!", "添加输出状态寄存器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frm8BitsSetting frmBitsRegister = new frm8BitsSetting();
            frmBitsRegister._bOutputStatus = false;
            frmBitsRegister.Add8InputStatusRegs = new frm8BitsSetting.DelegAddStatus(((frmDevice)(this.ActiveMdiChild)).AddContinuous8InputStatusRegs);
            frmBitsRegister.Text = "添加输入状态寄存器";
            frmBitsRegister.ShowDialog();
        }

        private void menuItem16BitsStatus_Click(object sender, EventArgs e)
        {
            frmMultiBitsSetting bitsDlg = new frmMultiBitsSetting();
            bitsDlg._bOutputStatus = false;
            bitsDlg.AddInputBitsStatus = new frmMultiBitsSetting.DelegAddBitsStatus(((frmDevice)(this.ActiveMdiChild)).Add16BitsInputStatusReg);
            bitsDlg.ShowDialog();
        }

        private void menuItemAddInputRegistrer_Click(object sender, EventArgs e)
        {
            ShowAddRegisterDlg(false);
        }

        private void menuItemAddHoldReg_Click(object sender, EventArgs e)
        {
            ShowAddRegisterDlg(true);
        }

        public void ShowAddRegisterDlg(bool bHoldReg)
        {
            if (this.ActiveMdiChild == null)
            {
                MessageBox.Show("请先添加设备后再添加寄存器!", "添加输入寄存器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmRegisterSetting dlgRegister = new frmRegisterSetting();
            dlgRegister._bHoldRegister = bHoldReg;
            dlgRegister._bAddRegister = true;
            dlgRegister._device = GetActivateDevice();
            dlgRegister.AddRegister = new frmRegisterSetting.DelegateAddRegister(((frmDevice)(this.ActiveMdiChild)).AddReister);
            dlgRegister.ShowDialog();
        }

        private void menuItemMonitor_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                ((frmDevice)(this.ActiveMdiChild))._device._bMonitor = true;
            }
        }

        private void menuItemStop_Click(object sender, EventArgs e)
        {

        }

        private void menuItemSave_Click(object sender, EventArgs e)
        {
            SaveDevice(GetActivateDevice());
        }

        private ModBusDevice GetActivateDevice()
        {
            return ((frmDevice)(this.ActiveMdiChild))._device;
        }

        public void ThreadUpdateInput()
        {
            ModBusDevice device = null;
            while (true)
            {
                try
                {
                    for (int nIndex = 0; nIndex < _devices.Count; nIndex++)
                    {
                        device = _devices[nIndex];
                        if (device._bMonitor)
                        {
                            ScanInputStatus(device);

                            ScanInputRegister(device);
                        }

                        Thread.Sleep(10);
                    }
                }
                catch (Exception ex)
                {
                    ModBusDevice.WriteLog(string.Format("Scan input from device {0} occurred exception:{1}", (device == null) ? "" : device.Name, ex.Message));
                }
            }
        }

        // 扫描输入状态
        void ScanInputStatus(ModBusDevice device)
        {
            int nAddr = 0;
            foreach (StatusRegister status in device._dicInputStatus.Values)
            {
                if (status._nStartAddr < nAddr)
                    continue;

                byte[] buf = null;
                nAddr = status._nStartAddr;
                ModBusDevice.WriteLog(String.Format("Read intput status from device {0} start at address 0x{1:4X}.", device.Name, nAddr));
                if (device.ReadInputStatus(ref nAddr, ref buf, true))
                {
                    //判断输入状态是否有变化，有变化则进行更新
                    bool bMultiBits = (status._lsHiName.Count == 16) ? true : false;
                    int nAddr1 = status._nStartAddr;
                    int nCount = 0;
                    while (device._dicInputStatus.ContainsKey(nAddr1) && device._dicInputStatus[nAddr1]._nStartAddr < nAddr)
                    {
                        //读取返回数据4个字节开始为寄存器数据
                        //单寄存器16位状态数据,取2个字节长度
                        bool bStatus = false;
                        if (bMultiBits)
                        {
                            for (int nBitIndex = 0; nBitIndex < 16; nBitIndex++)
                            {
                                //判断状态是否变化，变化则更新界面显示
                                bStatus = ((buf[nCount * 2 + 3 + nBitIndex / 8] & (1 << (nBitIndex % 8))) != 0);
                                if (bStatus != device._dicInputStatus[nAddr1]._lsCurStatus[nBitIndex])
                                {
                                    byte[] data = new byte[16];
                                    Array.Copy(buf, data, 16);
                                    UpdateInputStatus(nAddr1, data, device);
                                    break;
                                }
                            }
                        }
                        else// 1bit 对应一个地址寄存器的状态
                        {
                            for (int nBitIndex = 0; nBitIndex < 8; nBitIndex++)
                            {
                                //判断状态是否变化，变化则更新界面显示
                                bStatus = ((buf[nCount + 3] & (1 << nBitIndex)) != 0);
                                if (bStatus != device._dicInputStatus[nAddr1]._lsCurStatus[nBitIndex])
                                {
                                    byte[] data = new byte[8];
                                    Array.Copy(buf, data, 8);
                                    UpdateInputStatus(nAddr1, data, device);
                                    break;
                                }
                            }
                        }

                        nCount++;
                        nAddr1 += bMultiBits ? 1 : 8;
                    }
                }
            }
        }

        // 扫描输入寄存器
        void ScanInputRegister(ModBusDevice device)
        {
            foreach (RegisterItem reg in device._dicInputRegs.Values)
            {
                byte[] buf = null;
                ModBusDevice.WriteLog(String.Format("Read intput register from device {0} start at address 0x{1:4X}.", device.Name, reg._nStartAddr));
                if (device.ReadInputReg(reg._nStartAddr, ref buf))
                {
                    byte[] bufData = new byte[reg._nLength];
                    Array.Copy(buf, 3, bufData, 0, buf[2]);

                    string strVal = reg.BufDataToValueString(bufData);
                    if (strVal != reg._strData)
                    {     
                      GetFormByDeviceName(device.Name).InvokeUpdateInputRegValueToList(reg._nStartAddr, strVal);
                    }
                }
            }
        }

        private void UpdateInputStatus(int nAddr, byte[] data, ModBusDevice device)
        {
            frmDevice chFrmDev = GetFormByDeviceName(device.Name);
            if (chFrmDev._device != null)
            {
                chFrmDev.InvokeUpdateInputStatus(nAddr, data);
            }
        }

        private frmDevice GetFormByDeviceName(string strName)
        {
            foreach (frmDevice chFrmDev in this.MdiChildren)
            {
                if (chFrmDev._device.Name == strName)
                {
                    return chFrmDev;
                }
            }
            return null;
        }
    }
}
