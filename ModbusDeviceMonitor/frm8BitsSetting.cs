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

    
    public partial class frm8BitsSetting : Form
    {
        public delegate void DelegAddStatus(int nAddr, string[] aStrHiStatus, string[] aStrLoStatus);
        public delegate void DelegSetStatus(int nAddr, string[] aStrHiStatus, string[] aStrLoStatus);
        public DelegAddStatus Add8InputStatusRegs;
        public DelegSetStatus Change8InputStatusReg;

        // 输出寄存器标志
        public bool _bOutputStatus = false; 
        /// <summary>
        /// 高位状态TextBox控件数组
        /// </summary>
        public TextBox[] _aHiTextBoxes = new TextBox[8];

        /// <summary>
        /// 低位状态TextBox控件数组
        /// </summary>
        public TextBox[] _aLoTextBoxes = new TextBox[8];

        /// <summary>
        /// 初始化状态-Hi
        /// </summary>
        private string[] _aInitHiSatatus = new string[8];


        /// <summary>
        /// 初始化状态-Lo
        /// </summary>
        private string[] _aInitLoSatatus = new string[8];

        public frm8BitsSetting()
        {
            InitializeComponent();

            PutTextBoxesToArray();
        }

        public frm8BitsSetting(uint uiAddr, string[] aHiStatus, string[] aLoStatus)
        {
            InitializeComponent();
            PutTextBoxesToArray();

            UpdateBitsStatusToCtrl(uiAddr, aHiStatus, aLoStatus);
            btnAdd.Text = "确定";
            textBoxAddr.Enabled = false;

            for (int nIndex = 0; nIndex < 8; nIndex++)
            {
                _aInitHiSatatus[nIndex] = aHiStatus[nIndex];
                _aInitLoSatatus[nIndex] = aLoStatus[nIndex];
            }
        }

        private void PutTextBoxesToArray()
        {
            _aHiTextBoxes[0] = textBoxHiStatus1;
            _aHiTextBoxes[1] = textBoxHiStatus2;
            _aHiTextBoxes[2] = textBoxHiStatus3;
            _aHiTextBoxes[3] = textBoxHiStatus4;
            _aHiTextBoxes[4] = textBoxHiStatus5;
            _aHiTextBoxes[5] = textBoxHiStatus6;
            _aHiTextBoxes[6] = textBoxHiStatus7;
            _aHiTextBoxes[7] = textBoxHiStatus8;

            _aLoTextBoxes[0] = textBoxLoStatus1;
            _aLoTextBoxes[1] = textBoxLoStatus2;
            _aLoTextBoxes[2] = textBoxLoStatus3;
            _aLoTextBoxes[3] = textBoxLoStatus4;
            _aLoTextBoxes[4] = textBoxLoStatus5;
            _aLoTextBoxes[5] = textBoxLoStatus6;
            _aLoTextBoxes[6] = textBoxLoStatus7;
            _aLoTextBoxes[7] = textBoxLoStatus8;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxAddr.Text == "")
            {
                MessageBox.Show("请输入寄存器地址!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            UInt32 uiAddr = 0;
            if (!HexToUint(textBoxAddr.Text, ref uiAddr))
            {
                MessageBox.Show("请正确输入寄存器地址!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!CheckStatusNameSetting())
            {
                return;
            }

            bool bChanged = false;
            string[]  aStrHiStatusName = new string[8];
            string[] aStrLoStatusName = new string[8];
            for (int nIndex = 0; nIndex < 8; nIndex++)
            {
                aStrHiStatusName[nIndex] =  _aHiTextBoxes[nIndex].Text;
                aStrLoStatusName[nIndex] = _aLoTextBoxes[nIndex].Text;

                if (aStrHiStatusName[nIndex] != _aInitHiSatatus[nIndex] || aStrLoStatusName[nIndex] != _aInitLoSatatus[nIndex])
                {
                    bChanged = true;
                }
            }

            if (this.Text.Contains("添加"))
                Add8InputStatusRegs((int)uiAddr, aStrHiStatusName, aStrLoStatusName);
            else
            {
                if (bChanged)
                {
                    Change8InputStatusReg((int)uiAddr, aStrHiStatusName, aStrLoStatusName);
                }
                 Close();
            }
        }

        public static bool HexToUint(string strHexNum, ref UInt32 uiNum)
        {
            if (strHexNum.Length == 0)
                return false;

            uiNum = 0;
            string upperHexNum = strHexNum.ToUpper();
            for (int nIndex = 0; nIndex < upperHexNum.Length; nIndex++)
            {
                char ch = upperHexNum.ElementAt(nIndex);
                if (ch >= '0' && ch <= '9')
                {
                    uiNum = (uiNum << 4) | (UInt32)(ch - '0'); 
                }
                else if (ch >= 'A' && ch <= 'F')
                {
                    uiNum = (uiNum << 4) | (UInt32)(ch - 'A' + 10); 
                }
                else 
                {
                    return false;
                }

            }

            return true;
        }

        private bool CheckStatusNameSetting()
        {
            bool bAllEmpty = true;
            int nRet = 0;

            for (int nIndex = 0; nIndex < 8; nIndex++)
            {
                nRet = CheckPairHiAndLoTextBox(_aHiTextBoxes[nIndex], _aLoTextBoxes[nIndex]);
                if (nRet < 0)
                    return false;
                else if (nRet == 1)
                    bAllEmpty = false;
            }

            if (bAllEmpty)
            {
                MessageBox.Show("在所有位状态名称中至少有一对不为空!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查位的高低名称设置
        /// </summary>
        /// <param name="tb1"></param>
        /// <param name="tb2"></param>
        /// <returns>0:Both empty 1:Ok -1:Hi or Lo is empty -2:Same value </returns>
        private int CheckPairHiAndLoTextBox(TextBox tb1, TextBox tb2)
        {
            if (tb1.Text == "" && tb2.Text == "")
                return 0;
            else if (tb1.Text == "" || tb2.Text == "")
            {
                MessageBox.Show("高位和低位状态名称必须成对出现!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }
            else if (tb1.Text == tb2.Text)
            {
                MessageBox.Show("高位和低位状态名称不能相同!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -2;
            }
            return 1;
        }

        private void frmBitsSetting_Load(object sender, EventArgs e)
        {

        }

        public void UpdateBitsStatusToCtrl(uint uiAddr, string[] aHiStatus, string[] aLoStatus)
        {
            textBoxAddr.Text = string.Format("{0:X4}", uiAddr);
            for (int nIndex = 0; nIndex < aHiStatus.Length; nIndex++)
            {
                _aHiTextBoxes[nIndex].Text = aHiStatus[nIndex];
                _aLoTextBoxes[nIndex].Text = aLoStatus[nIndex];
            }
        }
    }
}
