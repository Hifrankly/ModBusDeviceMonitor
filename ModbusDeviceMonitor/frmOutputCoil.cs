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
    public partial class frmOutputCoil : Form
    {
        public delegate void DelegateAddOuputCoil(int nAddr, string strHiName, string strLoName);
        public delegate void DelegateSetOutputCoil(int nAddr, string strHiName, string strLoName);

        public DelegateAddOuputCoil AddOutputCoil;
        public DelegateSetOutputCoil SetOutputCoil;

        public frmOutputCoil()
        {
            InitializeComponent();
        }

        public frmOutputCoil(int nAddr, string strHiName, string strLoName)
        {
            InitializeComponent();

            tbAddr.Text = String.Format("{0:X4}", nAddr);
            tbHiName.Text = strHiName;
            tbLoName.Text = strLoName;
            tbAddr.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uint uiAddr = 0;
            if (!frm8BitsSetting.HexToUint(tbAddr.Text, ref uiAddr))
            {
                MessageBox.Show("请输入正确的地址!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tbHiName.Text == "" || tbLoName.Text == "")
            {
                MessageBox.Show("状态对应名称不能为空!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (button1.Text.Contains("添加"))
            {
                AddOutputCoil((int)uiAddr, tbHiName.Text, tbLoName.Text);
            }
            else
            {
                SetOutputCoil((int)uiAddr, tbHiName.Text, tbLoName.Text);
                this.Close();
            }
        }

        private void frmOutputCoil_Load(object sender, EventArgs e)
        {
            if (this.Text.Contains("添加"))
            {
                button1.Text = "添加";
            }
            else
            {
                button1.Text = "设置";
            }
        }


    }
}
