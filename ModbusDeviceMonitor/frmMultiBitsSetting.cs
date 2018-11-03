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
    public partial class frmMultiBitsSetting : Form
    {
        public delegate void DelegAddBitsStatus(int nAddr, string[] aStrHiStatus, string[] aStrLoStatus);
        public delegate void DelegSetBitsStatus(int nAddr, string[] aStrHiStatus, string[] aStrLoStatus);
        public DelegAddBitsStatus AddInputBitsStatus;
        public DelegSetBitsStatus ChangeInputBitsStatus;
        public DelegAddBitsStatus AddOutputBitsStatus;
        public DelegSetBitsStatus ChangeOutputBitsStatus;


        // 输出寄存器标志
        public bool _bOutputStatus = false; 

        /// <summary>
        /// 初始化状态-Hi
        /// </summary>
        private string[] _aInitHiSatatus = new string[16];

        /// <summary>
        /// 初始化状态-Lo
        /// </summary>
        private string[] _aInitLoSatatus = new string[16];

        /// <summary>
        /// 双击中的item
        /// </summary>
        private ListViewItem _CurrentItem;

        /// <summary>
        /// 双击中的subitem
        /// </summary>
        private ListViewItem.ListViewSubItem _CurrentSB;

        /// <summary>
        /// TextBox输入无效
        /// </summary>
        private bool _bCancelEdit;

        /// <summary>
        /// 默认添加构造函数
        /// </summary>
        public frmMultiBitsSetting()
        {
            InitializeComponent();
            InitListView();

            listView1.Controls.Add(textBoxEmbedded);
            textBoxEmbedded.Visible = false;
            this.Text = "添加寄存器16位状态";
        }

        /// <summary>
        /// 设置时构造函数
        /// </summary>
        /// <param name="uiAddr"></param>
        /// <param name="aHiStatus"></param>
        /// <param name="aLoStatus"></param>
        public frmMultiBitsSetting(uint uiAddr, string[] aHiStatus, string[] aLoStatus, bool bOutput)
        {
            InitializeComponent();
            InitListView();

            button1.Text = "确定";
            textBoxAddr.Enabled = false;
            textBoxAddr.Text = string.Format("{0:X4}", uiAddr);
            textBoxEmbedded.Visible = false;
            _bOutputStatus = bOutput;

            for (int nIndex = 0; nIndex < 16; nIndex++)
            {
                _aInitHiSatatus[nIndex] = aHiStatus[nIndex];
                _aInitLoSatatus[nIndex] = aLoStatus[nIndex];

                listView1.Items[nIndex].SubItems[1].Text = aLoStatus[nIndex];
                listView1.Items[nIndex].SubItems[2].Text = aHiStatus[nIndex];
            }

            listView1.Controls.Add(textBoxEmbedded);
            this.Text = "设置寄存器16位状态";
        }

        public void InitListView()
        {
            listView1.View = View.Details;
            listView1.LabelEdit = true;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("编号", 50);
            listView1.Columns.Add("0状态名称", 170);
            listView1.Columns.Add("1状态名称", 170);

            for (int nIndex = 0; nIndex < 16; nIndex++)
            {
                string [] aItemsText = {(nIndex + 1).ToString(), "", ""};
                ListViewItem item = new ListViewItem(aItemsText);
                listView1.Items.Add(item);
            }
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
            string[] aStrHiStatusName = new string[16];
            string[] aStrLoStatusName = new string[16];
            for (int nIndex = 0; nIndex < 16; nIndex++)
            {
                aStrHiStatusName[nIndex] = listView1.Items[nIndex].SubItems[2].Text;
                aStrLoStatusName[nIndex] = listView1.Items[nIndex].SubItems[1].Text;

                if (aStrHiStatusName[nIndex] != _aInitHiSatatus[nIndex] || aStrLoStatusName[nIndex] != _aInitLoSatatus[nIndex])
                {
                    bChanged = true;
                }
            }

            if (this.Text.Contains("添加"))
            {
                if (_bOutputStatus)
                     AddOutputBitsStatus((int)uiAddr, aStrHiStatusName, aStrLoStatusName);
                else
                     AddInputBitsStatus((int)uiAddr, aStrHiStatusName, aStrLoStatusName);                 
            }
            else
            {
                if (bChanged)
                {
                    if (_bOutputStatus)
                        ChangeOutputBitsStatus((int)uiAddr, aStrHiStatusName, aStrLoStatusName);
                    else
                        ChangeInputBitsStatus((int)uiAddr, aStrHiStatusName, aStrLoStatusName);
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

            for (int nIndex = 0; nIndex < 16; nIndex++)
            {
                nRet = CheckPairHiAndLoName(listView1.Items[nIndex].SubItems[1].Text, listView1.Items[nIndex].SubItems[2].Text);
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
        
        private int CheckPairHiAndLoName(string strText1, string strText2)
        {
            if (strText1 == "" && strText2 == "")
                return 0;
            else if (strText1 == "" || strText2 == "")
            {
                MessageBox.Show("高位和低位状态名称必须成对出现!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }
            else if (strText1 == strText2)
            {
                MessageBox.Show("高位和低位状态名称不能相同!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -2;
            }
            return 1;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _CurrentItem =  listView1.GetItemAt(e.X, e.Y);
            _CurrentSB = _CurrentItem.GetSubItemAt(e.X, e.Y);

            int nSubIndex = _CurrentItem.SubItems.IndexOf(_CurrentSB);
            if (nSubIndex > 0)
            {
                textBoxEmbedded.MaximumSize = new Size(_CurrentSB.Bounds.Width, _CurrentSB.Bounds.Height);
                textBoxEmbedded.SetBounds(_CurrentSB.Bounds.Left, _CurrentSB.Bounds.Top, _CurrentSB.Bounds.Width, _CurrentSB.Bounds.Height);
                textBoxEmbedded.Visible = true;
                textBoxEmbedded.Text = _CurrentSB.Text;
                textBoxEmbedded.BringToFront();
                textBoxEmbedded.Focus();
                textBoxEmbedded.Select(textBoxEmbedded.Text.Length, 0);
            }
        }

        private void textBoxEmbedded_Leave(object sender, EventArgs e)
        {
             textBoxEmbedded.Visible = false;
            if (!_bCancelEdit)
            {
              // update listviewitem
              if (textBoxEmbedded.Text != "")
              {
                 _CurrentSB.Text = textBoxEmbedded.Text;
              }
            }
            else
            {
                // Editing was cancelled by user
                _bCancelEdit = false;
            }
           listView1.Focus();
        }

        private void textBoxEmbedded_KeyDown(object sender, KeyEventArgs e)
        {
               switch (e.KeyCode)
              {
                   case Keys.Return:    // Enter key is pressed
                   case Keys.Escape:    // Escape key is pressed
                    _bCancelEdit = false;//  editing completed
                    e.Handled = true;
                    textBoxEmbedded.Visible = false;
                   break;             
              }
        }

    }
}
