using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace ModbusDeviceMonitor
{
    public partial class frmDeviceSetting : Form
    {
        public string[] _aStrBaudRate = { "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "38400", "56000", "57600", "115200", "128000", "256000" };
        public bool _bOk;
        public Devices _refDevices;
        public ModBusDevice _device;

        public frmDeviceSetting()
        {
            InitializeComponent();
            
            InitControls();
        }

        public frmDeviceSetting(ModBusDevice device)
        {
            InitializeComponent();

            _device = device;
            InitControls();
            this.Text = "通信设置";
            tbDeviceName.Text = _device.Name;
            tbAddress.Text = _device.Address.ToString();
        }

        public void InitControls()
        {
            string[] aStrPorts = SerialPort.GetPortNames();
            for (int nIndex = 0; nIndex < aStrPorts.Length; nIndex++)
            {
                comboBoxPort.Items.Add(aStrPorts[nIndex]);
            }

           string strText = comboBoxStopBits.GetItemText(0);

            // 计算机上无端口，添加10个端口用于设置
            if (comboBoxPort.Items.Count == 0)
            {
                for (int nIndex = 1; nIndex <= 10; nIndex++)
                {
                    comboBoxPort.Items.Add("COM" + nIndex.ToString());
                }
            }

            for (int nIndex = 0; nIndex < _aStrBaudRate.Length; nIndex++)
            {
                comboBoxBaud.Items.Add(_aStrBaudRate[nIndex]);
            }
            
            comboBoxStopBits.Items.Add("1");
            comboBoxStopBits.Items.Add("1.5");
            comboBoxStopBits.Items.Add("2");

            comboBoxDataBits.Items.Add("8");
            comboBoxDataBits.Items.Add("7");
            comboBoxDataBits.Items.Add("6");
            comboBoxDataBits.Items.Add("5");     

            comboBoxCheck.Items.Add("无");
            comboBoxCheck.Items.Add("奇校验");
            comboBoxCheck.Items.Add("偶检验");

            comboBoxMode.Items.Add("RTU");
            comboBoxMode.Items.Add("ASCII");        

            numUpDownReadCoil.Value = 1;
            numUpDownReadSwitch.Value = 2;
            numUpDownReadHoldReg.Value = 3;
            numUpDownReadInputReg.Value = 4;

            numUpDownWriteCoil.Value = 5;
            numUpDownWriteHoldReg.Value = 6;
            numUpDownWriteCoils.Value = 15;
            numUpDownWriteHoldRegs.Value = 16;

            if (_device != null)
            {
                tbAddress.Text = _device.Address.ToString();
                tbDeviceName.Text = _device.Name;
                comboBoxPort.SelectedIndex = comboBoxPort.FindStringExact(_device.PortName);
                comboBoxBaud.SelectedIndex = comboBoxBaud.FindStringExact(_device.BaudRate.ToString());                
                comboBoxDataBits.SelectedIndex = comboBoxDataBits.FindStringExact(_device.DataBits.ToString());
                comboBoxStopBits.SelectedIndex = (int)_device.StopBits;
                comboBoxCheck.SelectedIndex = (int)_device.Parity;
            }
            else
            {
                comboBoxBaud.SelectedIndex = 7;
                comboBoxStopBits.SelectedIndex = 0;
                comboBoxDataBits.SelectedIndex = 0;
                comboBoxCheck.SelectedIndex = 0;

                numUpDownReadCoil.Value = 1;
                numUpDownReadSwitch.Value = 2;
                numUpDownReadHoldReg.Value = 3;
                numUpDownReadInputReg.Value = 4;

                numUpDownWriteCoil.Value = 5;
                numUpDownWriteHoldReg.Value = 6;
                numUpDownWriteCoils.Value = 15;
                numUpDownWriteHoldRegs.Value = 16;
            }

            comboBoxMode.SelectedIndex = 0;
            _bOk = false;
        }

        private void frmDeviceSetting_Load(object sender, EventArgs e)
        {
            _bOk = false;
            //tbAddress.Text = "";
            //tbDeviceName.Text = "";
            tbDeviceName.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbDeviceName.Text == "" || tbAddress.Text == "" || comboBoxPort.Text == "")
            {
                MessageBox.Show("请输入完整的设备信息!", "添加新设备", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int nAddress = Convert.ToInt32(tbAddress.Text);
                if (nAddress < 0 && nAddress > 247)
                {
                    MessageBox.Show("请输入正确的设备地址(0~247)!", "添加新设备", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("请输入正确的设备地址({0})!", ex.Message), "添加新设备", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!CheckFunctionCode())
                return;

            if (_device == null)
            {
               if (!CheckDevices())
                   return;
            }

            _bOk = true;

            Close();
        }

        /// <summary>
        /// 检查设备名称、地址是否有重复
        /// </summary>
        /// <returns></returns>
        public bool CheckDevices()
        {
            if (_refDevices != null)
            {
                foreach (ModBusDevice device in _refDevices)
                {
                    if (device.Name == tbDeviceName.Text)
                    {
                        MessageBox.Show("名称同已有设备重复!", "添加新设备", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    if (device.PortName == comboBoxPort.Text && device.Address.ToString() == tbAddress.Text)
                    {
                        MessageBox.Show("设备地址和端口号同已有设备重复!", "添加新设备", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 检查功能码是否为空或者重复
        /// </summary>
        /// <returns></returns>
        public bool CheckFunctionCode()
        {
            foreach (Control ctrl in groupBox2.Controls)
            {
                if (ctrl.GetType() == typeof(NumericUpDown))
                {
                    if (ctrl.Text == "")
                    {
                        MessageBox.Show("功能码不能为空!", "添加新设备", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    foreach (Control ctrl1 in groupBox2.Controls)
                    {
                        if (ctrl.GetType().ToString() == "System.Windows.Forms.NumericUpDown" && ctrl != ctrl1)
                        {
                            if (ctrl.Text == ctrl1.Text)
                            {
                                MessageBox.Show("功能码不能重复!", "添加新设备", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool PortIsChanged()
        {
            //  判断端口设置是否已改变
            if (_device.PortName != comboBoxPort.Text || _device.BaudRate.ToString() != comboBoxBaud.Text ||
                _device.DataBits.ToString() != comboBoxDataBits.Text || _device.StopBits.ToString() != comboBoxStopBits.Text ||
                (int)_device.Parity != comboBoxCheck.SelectedIndex)
                return true;

            return false;
        }

        /// <summary>
        ///  获取设置的设备信息
        /// </summary>
        /// <param name="device"></param>
        public void GetDeviceInfo(ref ModBusDevice device)
        {
            device.Name = tbDeviceName.Text;
            device.Address = Convert.ToByte(tbAddress.Text);
            device.SetPort(string.Format("PortName:{0};BaudRate:{1};DataBits:{2};StopBits:{3};Parity:{4}",
                                          comboBoxPort.Text, comboBoxBaud.Text, comboBoxDataBits.Text,
                                          comboBoxStopBits.Text, comboBoxCheck.Text));
            
            // 读功能码
            device.FuncReadCoilStatus = (byte)numUpDownReadCoil.Value;
            device.FuncReadSwitch = (byte)numUpDownReadSwitch.Value;
            device.FuncReadHoldReg = (byte)numUpDownReadHoldReg.Value;
            device.FuncReadInputReg = (byte)numUpDownReadInputReg.Value;

            // 写功能码
            device.FuncReadCoilStatus = (byte)numUpDownReadCoil.Value;
            device.FuncReadSwitch = (byte)numUpDownReadSwitch.Value;
            device.FuncReadHoldReg = (byte)numUpDownReadHoldReg.Value;
            device.FuncReadInputReg = (byte)numUpDownReadInputReg.Value;
        }
    }
}
