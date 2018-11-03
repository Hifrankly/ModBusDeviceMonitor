using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModbusDeviceMonitor
{
    public partial class frmRegisterSetting : Form
    {

        /// <summary>
        /// 保持寄存器标志
        /// </summary>
        public bool _bHoldRegister = false;
        
        /// <summary>
        /// 添加寄存器标志
        /// </summary>
        public bool _bAddRegister = false;

        /// <summary>
        /// 保持或输入寄存器
        /// </summary>
        public RegisterItem _register = null;

        /// <summary>
        /// 添加寄存器委托申明
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        public delegate void DelegateAddRegister(bool bHoldReg, RegisterItem register);

        public delegate void DelegateSetRegister(bool bHoldReg, RegisterItem register);

        public DelegateAddRegister AddRegister = null;

        public DelegateSetRegister SetRegister = null;

        public string[] aType = { "Char", "Byte", "Int16", "UInt16", "Int32", "UInt32", "Int64", "UInt64", "Float", "Double", "String"};

        public ModBusDevice _device;

        public frmRegisterSetting()
        {
            InitializeComponent();

            for (int nIndex = 0; nIndex < aType.Length; nIndex++)
            {
                comboBoxType.Items.Add(aType[nIndex]);
            }
            textBoxCoefficient.Text = "1.0";
        }

        public frmRegisterSetting(RegisterItem register)
        {
            InitializeComponent();

            for (int nIndex = 0; nIndex < aType.Length; nIndex++)
            {
                comboBoxType.Items.Add(aType[nIndex]);
            }

            textBoxAddr.Text = string.Format("{0:X4}", register._nStartAddr);
            textBoxAddr.Enabled = false;
            textBoxName.Text = register._strName;
            comboBoxType.SelectedIndex = register._nType;
            textBoxUpperLimit.Text = register._strUpperLimit;
            textBoxLowerLimit.Text = register._strLowerLimit;
            textBoxCoefficient.Text = register._dCoefficient.ToString();
            _register = register;
        }

        private void frmRegisterSetting_Load(object sender, EventArgs e)
        {
            string strText = _bAddRegister ? "添加" : "设置";
            strText += _bHoldRegister ? "保持寄存器" : "输入寄存器";
            this.Text = strText;
             
            buttonAdd.Text = _bAddRegister ? "添加" : "确定";
            numUpDownLength.Enabled = false;

            labelNote.Text = "注：系数仅用于显示的浮点数和\r\n" +
                             "    寄存器中的整数(<=32位)转换\r\n" + 
                             "    显示值 = 系数 × 寄存器值";

            labelLength.Text = "(寄存器字节长度\r\n" + 
                                "2的整数倍)";
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (textBoxAddr.Text == "" || textBoxName.Text == "" || comboBoxType.SelectedIndex == -1)
            {
                MessageBox.Show("寄存器地址、名称、类型不能为空!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            uint uiAddr = 0;
            if (!frm8BitsSetting.HexToUint(textBoxAddr.Text, ref uiAddr))
            {
                MessageBox.Show("请输入正确的寄存器地址!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }       

            try 
            {
                Single sngCoefficient = Convert.ToSingle(textBoxCoefficient.Text);
                if (!_bHoldRegister)
                {
                    string strErr = "";
                    if ((textBoxUpperLimit.Text != "" && !CheckValue(textBoxUpperLimit.Text, comboBoxType.SelectedIndex, sngCoefficient, ref strErr)) ||
                        (textBoxLowerLimit.Text != "" && !CheckValue(textBoxLowerLimit.Text, comboBoxType.SelectedIndex, sngCoefficient, ref strErr)))
                    {
                        MessageBox.Show(string.Format("请输入正确的门限值({0})!", strErr), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                 if (_bAddRegister)
                    _register = new RegisterItem();
                               
                _register._nStartAddr = (int)uiAddr;
                _register._strName = textBoxName.Text;
                _register._strUpperLimit = textBoxUpperLimit.Text;
                _register._strLowerLimit = textBoxLowerLimit.Text;
                _register._nLength = (int)numUpDownLength.Value;
                _register._dCoefficient = sngCoefficient;
                _register._nType = comboBoxType.SelectedIndex;

                if (_bAddRegister)
                {
                    if ((_bHoldRegister && _device.CheckAddHoldRegAddr((int)uiAddr, _register._nLength)) || (!_bHoldRegister && _device.CheckAddInputRegAddr((int)uiAddr, _register._nLength)))
                    {
                        AddRegister(_bHoldRegister, _register);
                        if (_bHoldRegister)
                            _device.AddHoldRegister(_register);
                        else
                            _device.AddInputRegister(_register);
                    }
                }
                else
                {
                    SetRegister(_bHoldRegister, _register);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static bool CheckValue(string strValue, int nType, Single sngCoefficient, ref string strErr)
        {
            try
            {
                switch ((RegisterItem.DataType)nType)
                {
                    case RegisterItem.DataType.Type_Byte:
                        Convert.ToChar(strValue);
                        break;
                    case RegisterItem.DataType.Type_Char:
                        Convert.ToByte(strValue);
                        break;
                    case RegisterItem.DataType.Type_Int16:
                        Convert.ToInt16(Convert.ToSingle(strValue) / sngCoefficient);
                        break;
                    case RegisterItem.DataType.Type_UInt16:
                        Convert.ToUInt16(Convert.ToSingle(strValue) / sngCoefficient);
                        break;
                    case RegisterItem.DataType.Type_Int32:
                        Convert.ToInt32(Convert.ToSingle(strValue) / sngCoefficient);
                        break;
                    case RegisterItem.DataType.Type_UInt32:
                        Convert.ToUInt32(Convert.ToSingle(strValue) / sngCoefficient);
                        break;
                    case RegisterItem.DataType.Type_Float:
                        Convert.ToSingle(strValue);
                        break;
                    case RegisterItem.DataType.Type_Int64:
                        Convert.ToInt64(strValue);
                        break;
                    case RegisterItem.DataType.Type_UInt64:
                        Convert.ToUInt64(strValue);
                        break;
                    case RegisterItem.DataType.Type_Double:
                        Convert.ToDouble(strValue);
                        break;
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                return false;
            }
            return true;
        }

        public static bool CheckValueInRange(string strLoLimit, string strHiLimit, string strValue, int nType)
        {
            Single value;
            try
            {
                switch ((RegisterItem.DataType)nType)
                {
                    case RegisterItem.DataType.Type_Char:
                        char ch = Convert.ToChar(strValue);
                        if (strLoLimit != "")
                        {
                            if (ch < Convert.ToChar(strLoLimit))
                                return false;
                        }

                        if (strHiLimit != "")
                        {
                            if (ch > Convert.ToChar(strHiLimit))
                                return false;
                        }
                        break;
                    case RegisterItem.DataType.Type_Byte:
                        byte byValue = Convert.ToByte(strValue);
                        if (strLoLimit != "")
                        {
                            if (byValue < Convert.ToByte(strLoLimit))
                                return false;
                        }

                        if (strHiLimit != "")
                        {
                            if (byValue > Convert.ToByte(strHiLimit))
                                return false;
                        }

                        break;
                    case RegisterItem.DataType.Type_Int16:
                        value = Convert.ToSingle(strValue);
                        if (strLoLimit != "")
                        {
                            if (value < Convert.ToSingle(strLoLimit))
                                return false;
                        }

                        if (strHiLimit != "")
                        {
                            if (value > Convert.ToSingle(strHiLimit))
                                return false;
                        }
                        break;
                    case RegisterItem.DataType.Type_UInt16:
                        value = Convert.ToSingle(strValue);
                        if (strLoLimit != "")
                        {
                            if (value < Convert.ToSingle(strLoLimit))
                                return false;
                        }

                        if (strHiLimit != "")
                        {
                            if (value > Convert.ToSingle(strHiLimit))
                                return false;
                        }
                        break;
                    case RegisterItem.DataType.Type_Int32:
                        value = Convert.ToSingle(strValue);
                        if (strLoLimit != "")
                        {
                            if (value < Convert.ToSingle(strLoLimit))
                                return false;
                        }

                        if (strHiLimit != "")
                        {
                            if (value > Convert.ToSingle(strHiLimit))
                                return false;
                        }
                        break;
                    case RegisterItem.DataType.Type_UInt32:
                        value = Convert.ToSingle(strValue);
                        if (strLoLimit != "")
                        {
                            if (value < Convert.ToSingle(strLoLimit))
                                return false;
                        }

                        if (strHiLimit != "")
                        {
                            if (value > Convert.ToSingle(strHiLimit))
                                return false;
                        }
                        break;
                    case RegisterItem.DataType.Type_Float:
                        Single sValue =  Convert.ToSingle(strValue);

                        if (strLoLimit != "")
                        {
                            if (sValue < Convert.ToSingle(strLoLimit))
                                return false;
                        }

                        if (strHiLimit != "")
                        {
                            if (sValue > Convert.ToSingle(strHiLimit))
                                return false;
                        }
                        break;
                    case RegisterItem.DataType.Type_Int64:
                        Int64 lValue = Convert.ToInt64(strValue);
                        if (strLoLimit != "")
                        {
                            if (lValue < Convert.ToInt64(strLoLimit))
                                return false;
                        }

                        if (strHiLimit != "")
                        {
                            if (lValue > Convert.ToInt64(strHiLimit))
                                return false;
                        }
                        break;
                    case RegisterItem.DataType.Type_UInt64:
                       UInt64 ulVal =  Convert.ToUInt64(strValue);
                        if (strLoLimit != "")
                        {
                            if (ulVal < Convert.ToUInt64(strLoLimit))
                                return false;
                        }

                        if (strHiLimit != "")
                        {
                            if (ulVal > Convert.ToUInt64(strHiLimit))
                                return false;
                        }

                       break;
                    case RegisterItem.DataType.Type_Double:
                       Double dVal = Convert.ToDouble(strValue);
                       if (strLoLimit != "")
                        {
                            if (dVal < Convert.ToDouble(strLoLimit))
                                return false;
                        }

                        if (strHiLimit != "")
                        {
                            if (dVal > Convert.ToDouble(strHiLimit))
                                return false;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("输入值进行门限判断失败{0}!", ex.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nLength = -1;
            switch ((RegisterItem.DataType)comboBoxType.SelectedIndex)
            { 
                case RegisterItem.DataType.Type_Byte:
                case RegisterItem.DataType.Type_Char:
                case RegisterItem.DataType.Type_Int16:
                case RegisterItem.DataType.Type_UInt16:
                    nLength = 2;
                    break;
                case RegisterItem.DataType.Type_Int32:
                case RegisterItem.DataType.Type_UInt32:
                case RegisterItem.DataType.Type_Float:
                    nLength = 4;
                    break;
                case RegisterItem.DataType.Type_Int64:
                case RegisterItem.DataType.Type_UInt64:
                case RegisterItem.DataType.Type_Double:
                    nLength = 8;
                    break;
            }

            if (nLength > 0)
            {
                numUpDownLength.Value = nLength;
                numUpDownLength.Enabled = false;
            }
            else
            {
                numUpDownLength.Enabled = true;
            }
        }


    }
}
