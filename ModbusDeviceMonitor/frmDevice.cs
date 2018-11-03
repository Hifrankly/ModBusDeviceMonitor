using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace ModbusDeviceMonitor
{
    public partial class frmDevice : Form
    {
       public ModBusDevice _device;

       /// <summary>
       /// 单击在listview item上的索引
       /// </summary>
       private int _nClickIndex;

       /// <summary>
       /// 单击在listview subitem上的索引
       /// </summary>
       private int _nClickSubIndex;

       /// <summary>
       /// 编辑项标记
       /// </summary>
       private bool _bEditItem;

       /// <summary>
       /// 编辑项索引
       /// </summary>
       private int _nEiditItemIndex;

       /// <summary>
       /// 编辑子项索引
       /// </summary>
       private int _nEditSubItemItem;

       private bool _bClosing;

       public Form1.DelegSetBarText SetBarText;

        public frmDevice(ModBusDevice device)
        {
            InitializeComponent();

            MessageFilter msgFilter = new MessageFilter();
            msgFilter.MouseClick = new LButtonClick(msgMouseClick);
            Application.AddMessageFilter(msgFilter);

            _device = device;
            this.Text = "ModBus Device - " + _device.Name;

            InitListView();
            UpdateDeviceToPage();
        }

        public void msgMouseClick(IntPtr hWnd, int x, int y, bool bDoubleClick)
        {
           if(!_bClosing)
           {
               Invoke(new LButtonDownHandler(MouseClickHandler), hWnd, x, y, bDoubleClick);
           }
        }

        private void InitListView()
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.SetBounds(0, 0, this.Width - SystemInformation.FrameBorderSize.Width * 2, this.Height - SystemInformation.CaptionHeight - SystemInformation.FrameBorderSize.Height * 2);
            listView1.Columns.Add("A", 80);

            int nWidth = (listView1.Width - 80) / 8;
            for (byte index = 1; index < 9; index++)
            {
                byte[] header = {(byte)(65 + index)};
                listView1.Columns.Add(System.Text.Encoding.ASCII.GetString(header), nWidth);
                listView1.Columns[index].TextAlign = HorizontalAlignment.Center;
            }
        }

        /// <summary>
        /// 将device信息添加到当前页面的listView控件上
        /// </summary>
        private void UpdateDeviceToPage()
        {
            // 获取tabpage上的listview
            listView1.Items.Clear();

            //  将线圈状态信息更新到listview
            UpdateCoilsToListView();

            // 将输入状态寄存器更新到listview
            UpdateInputsStatusToListView();

            // 将输入寄存器更新到listview
            UpdateInputRegToListView();

            // 将保存寄存器更新到listview
            UpdateHoldRegToListView();
        }

        public void UpdateCoilsToListView()
        {
            if (_device._dicCoils == null)
                return;

            lock (_device._dicCoils._lock)
            {
                foreach (StatusRegister coil in _device._dicCoils.Values)
                {
                    if (coil._lsHiName.Count == 1)
                    {
                        listView1.AddOutputStatus(coil);
                    }
                    else
                    {
                        string[] aStrLoStatus = new string[coil._lsLoName.Count];
                        string[] aStrHiStatus = new string[coil._lsHiName.Count];
                        _device.GetBitsOutputStatus(coil._nStartAddr, ref aStrHiStatus, ref aStrLoStatus);
                        listView1.AddOutputStatus(coil._nStartAddr, aStrLoStatus);
                    }
                }
            }
        }

        public void UpdateInputsStatusToListView()
        {
            if (_device._dicInputStatus == null)
                return;

            string[] aStrHiStatus, aStrLoStatus;
            lock (_device._dicInputStatus._lock)
            {
                foreach (StatusRegister input in _device._dicInputStatus.Values)
                {
                    if (input._lsLoName.Count > 8)
                    {
                        aStrHiStatus = new string[16];
                        aStrLoStatus = new string[16];
                        listView1.AddInput16BitsStatus(input._nStartAddr, aStrLoStatus);
                    }
                    else
                    {
                        aStrHiStatus = new string[8];
                        aStrLoStatus = new string[8];
                        listView1.AddInput8BitsStatus(input._nStartAddr, aStrLoStatus);
                    }
                }
            }
        }

        public void UpdateInputRegToListView()
        {
            if (_device._dicInputRegs == null)
                return;

            foreach (RegisterItem reg in _device._dicInputRegs.Values)
            {
                listView1.AddInputRegister(reg);
            }
        }

        public void UpdateHoldRegToListView()
        {
            if (_device._dicHoldRegs == null)
                return;

            foreach (RegisterItem reg in _device._dicHoldRegs.Values)
            {
                listView1.AddHoldRegister(reg);
            }
        }

        private void frmDevice_SizeChanged(object sender, EventArgs e)
        {
            listView1.SetBounds(0, 0, this.Width - SystemInformation.FrameBorderSize.Width * 2, this.Height - SystemInformation.CaptionHeight - SystemInformation.FrameBorderSize.Height * 2);           
            for(int nIndex = 1; nIndex < listView1.Columns.Count; nIndex++)
            {
                listView1.Columns[nIndex].Width = (listView1.Width - 80) / 8;
            }
        }

        delegate void LButtonDownHandler(IntPtr hWnd, int x, int y, bool bDoubleClick);
        [DllImport("user32.dll", EntryPoint = "GetScrollPos")]
        public static extern int GetScrollPos(IntPtr hwnd, int nBar);
        public void MouseClickHandler(IntPtr hWnd, int x, int y, bool bDoubleClick)
        {
            if (listView1.Items.Count == 0 || hWnd != listView1.Handle)
                return;

            if (_bEditItem && listView1._tbEdit.Handle != hWnd)
            {
                _bEditItem = false;
                if (CheckEditItemText(listView1, _nEiditItemIndex, _nEditSubItemItem))
                {
                    listView1.Items[_nEiditItemIndex].SubItems[_nEditSubItemItem].Text = listView1.GetEditText();
                }

                listView1.HideEditBox();
            }

            if (listView1.Bounds.Contains(x, y + listView1.Items[0].Bounds.Height) && listView1.Focused)
            {
                bool bSelectedSubItem = false;
                int nHPos = GetScrollPos(listView1.Handle, 0);
                int nVPos = GetScrollPos(listView1.Handle, 1);
                ListViewItem listItem = listView1.GetItemAt(x, y);
                int nIndex = (y + nVPos - listView1.Items[0].Bounds.Height) / listView1.Items[0].Bounds.Height;
                if (nIndex >= 0 && nIndex < listView1.Items.Count)
                {
                    int nWidth = 0;
                    int nXOfListView = x + nHPos;
                    for (int nSubIndex = 0; nSubIndex < listView1.Items[nIndex].SubItems.Count; nSubIndex++)
                    {
                        nWidth += listView1.Columns[nSubIndex].Width;
                        if (nXOfListView < nWidth)
                        {
                            if (bDoubleClick)
                            {
                                ProcessDblClick(nIndex, nSubIndex);
                                return;
                            }
                            else
                            {
                                if (nSubIndex != 0)
                                {
                                    listView1.SelectSubItem(listView1.Items[nIndex].SubItems[nSubIndex]);
                                    bSelectedSubItem = true;
                                }
                            }
                            break;
                        }

                    }
                }

                if (!bSelectedSubItem)
                    listView1.SelectSubItem(null);
            }
        }

        public void ProcessDblClick(int nIndexOfList, int nSubIndex)
        {
            _nClickIndex = nIndexOfList;
            _nClickSubIndex = nSubIndex;
            if (nIndexOfList <= listView1._nMaxCoilIndex)// 设置Coil状态
            {
                DblClickOnCoilItem(nIndexOfList, nSubIndex);
            }
            else if (nIndexOfList <= listView1._nMaxSwitchIndex) // 设置输入状态
            {
                DblClickOnInInputStatusItem(nIndexOfList, nSubIndex);
            }
            else if (nIndexOfList <= listView1._nMaxInputRegIndex) // 设置输入寄存器
            {
                DblClickOnInputRegItem(nIndexOfList, nSubIndex);
            }
            else
            {
                DblClickOnHoldRegItem(nIndexOfList, nSubIndex);
            }
        }

        public void DblClickOnCoilItem(int nIndexOfList, int nSubIndex)
        {
            uint uiAddr = 0;
            int nAddrIndex = 0;
            int nIndexOfStatus = 0;
            bool bMultiBits = listView1.Items[nIndexOfList].SubItems[nAddrIndex].Text.Contains("-");
            if (bMultiBits)
            {
                string strAddrText = listView1.Items[nIndexOfList].SubItems[0].Text;
                strAddrText = strAddrText.Substring(0, strAddrText.IndexOf("-"));
                frm8BitsSetting.HexToUint(strAddrText, ref uiAddr);
                nIndexOfStatus = listView1.Items[nIndexOfList].SubItems[nAddrIndex].Text.Contains("-L8") ? (nSubIndex - 1) : (nSubIndex + 8 - 1);
            }
            else
            {
                frm8BitsSetting.HexToUint(listView1.Items[nIndexOfList].SubItems[((int)(nSubIndex / 2)) * 2].Text, ref uiAddr);
                nIndexOfStatus = 0;
            }

            if ((nSubIndex == 0) || ((nSubIndex % 2) == 0 && !bMultiBits))//弹出设置对话框进行修改
            {
                if (bMultiBits)
                    ShowSetMultiOutputDlg((int)uiAddr);
                else
                    ShowSetOutputDlg((int)uiAddr);
            }
            else //双击在子项上，设置高低状态
            {
                if (_device._dicCoils.ContainsKey((int)uiAddr))
                {
                    StatusRegister coil = _device._dicCoils[(Int32)uiAddr];

                    bool bHiStatus = (listView1.Items[nIndexOfList].SubItems[nSubIndex].Text == coil._lsHiName[nIndexOfStatus]);
                    if (bHiStatus)
                    {
                        coil._lsCurStatus[nIndexOfStatus] = false;
                        listView1.Items[nIndexOfList].SubItems[nSubIndex].Text = coil._lsLoName[nIndexOfStatus];
                        listView1.Items[nIndexOfList].SubItems[nSubIndex].BackColor = Color.White;
                    }
                    else
                    {
                        coil._lsCurStatus[nIndexOfStatus] = true;
                        listView1.Items[nIndexOfList].SubItems[nSubIndex].Text = coil._lsHiName[nIndexOfStatus];
                        listView1.Items[nIndexOfList].SubItems[nSubIndex].BackColor = Color.Red;
                    }

                    if (listView1.Items[nIndexOfList].SubItems[nSubIndex] == listView1._selectedSubItem)
                    {
                        listView1._colorBak = bHiStatus ? Color.White : Color.Red;
                    }

                    //_statusBar1.Panels[0].Text = string.Format("设置\"{0}\"", listView1.Items[nIndexOfList].SubItems[nSubIndex].Text);

                    //ThreadStart thStart = new ThreadStart(ThreadSetOutput);
                    Thread thSetOutput = new Thread(new ParameterizedThreadStart(ThreadSetOutput));

                    object[] param = new object[3];
                    param[0] = uiAddr;
                    param[1] = _device;
                    param[2] = listView1.Items[nIndexOfList].SubItems[nSubIndex].Text;
                    thSetOutput.Start(param);
                }
            }
        }

        public void DblClickOnInInputStatusItem(int nIndexOfList, int nSubIndex)
        {
            uint uiAddr = 0;
            frm8BitsSetting.HexToUint(listView1.Items[nIndexOfList].SubItems[0].Text, ref uiAddr);
            if (listView1.Items[nIndexOfList].SubItems[nSubIndex].Text != "")//弹出设置对话框进行修改
            {
                if (listView1.Items[nIndexOfList].SubItems[0].Text.Contains("-"))
                    ShowChange16BitsStatusDlg(false, uiAddr);
                else
                    ShowChange8StatusRegDlg(uiAddr);
            }
        }

        public void DblClickOnInputRegItem(int nIndexOfList, int nSubIndex)
        {
            RegisterItem register = null;
            int nAddrIndex = (int)(nSubIndex / 3) * 3;
            uint uiAddr = 0;
            frm8BitsSetting.HexToUint(listView1.Items[nIndexOfList].SubItems[nAddrIndex].Text, ref uiAddr);
            if (_device._dicInputRegs.ContainsKey((int)uiAddr))
            {
                register = _device._dicInputRegs[(int)uiAddr];
                ShowChangeRegisterDlg(register, false);
            }
        }

        public void DblClickOnHoldRegItem(int nIndexOfList, int nSubIndex)
        {
            RegisterItem register = null;
            int nAddrIndex = (int)(nSubIndex / 3) * 3;
            uint uiAddr = 0;
            frm8BitsSetting.HexToUint(listView1.Items[nIndexOfList].SubItems[nAddrIndex].Text, ref uiAddr);
            if (_device._dicHoldRegs.ContainsKey((int)uiAddr))
            {
                if ((nSubIndex % 3) == 2) //双击在数据上
                {
                    _bEditItem = true;
                    _nEiditItemIndex = nIndexOfList;
                    _nEditSubItemItem = nSubIndex;
                    listView1.ShowTextBoxOnItem(nIndexOfList, nSubIndex);
                    //timer1.Interval = 200;
                    //timer1.Start();
                }
                else
                {
                    register = _device._dicHoldRegs[(int)uiAddr];
                    ShowChangeRegisterDlg(register, true);
                }
            }
        }

        /// <summary>
        /// 具有编辑功能的只有保持寄存器，检查输入字符是否是指定类型以及上下限判断
        /// </summary>
        /// <param name="listView1"></param>
        /// <param name="nIndex"></param>
        /// <param name="nSubIndex"></param>
        /// <returns></returns>
        public bool CheckEditItemText(MyListView listView1, int nIndex, int nSubIndex)
        {
            string strText = listView1.GetEditText();
            if (nIndex > listView1._nMaxInputRegIndex && nIndex <= listView1._nMaxHoldRegIndex)
            {
                uint uiAddr = 0;
                frm8BitsSetting.HexToUint(listView1.Items[nIndex].SubItems[(int)(nSubIndex / 3) * 3].Text, ref uiAddr);
                if (_device._dicHoldRegs.ContainsKey((int)uiAddr))
                {
                    string strErr = "";
                    if (!frmRegisterSetting.CheckValue(strText, _device._dicHoldRegs[(int)uiAddr]._nType,
                                                       _device._dicHoldRegs[(int)uiAddr]._dCoefficient, ref strErr))
                    {
                        //MessageBox.Show("请输入正确的值!", "输入值检查", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    if (frmRegisterSetting.CheckValueInRange(_device._dicHoldRegs[(int)uiAddr]._strLowerLimit,
                                                              _device._dicHoldRegs[(int)uiAddr]._strUpperLimit, strText,
                                                               _device._dicHoldRegs[(int)uiAddr]._nType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void ShowSetOutputDlg(int nAddr)
        {
            frmOutputCoil dlgOutputSetting = new frmOutputCoil(nAddr, _device._dicCoils[nAddr]._lsHiName[0],
                                                                _device._dicCoils[nAddr]._lsLoName[0]);
            dlgOutputSetting.SetOutputCoil = new frmOutputCoil.DelegateSetOutputCoil(ChangeOutputStatus);
            dlgOutputSetting.ShowDialog();
        }

        public void ShowSetMultiOutputDlg(int nAddr)
        {
            string[] aHiStatus = new string[16];
            string[] aLoStatus = new string[16];
            _device.GetBitsOutputStatus(nAddr, ref aHiStatus, ref aLoStatus);

            frmMultiBitsSetting dlgMultiBits = new frmMultiBitsSetting((uint)nAddr, aHiStatus, aLoStatus, true);
            dlgMultiBits.Text = "修改输入状态寄存器设置";
            dlgMultiBits.ChangeOutputBitsStatus = new frmMultiBitsSetting.DelegSetBitsStatus(Change16OuputStatus);
            dlgMultiBits.ShowDialog();
        }

        public void ShowChange8StatusRegDlg(uint uiAddr)
        {
            string[] aHiStatus = new string[8];
            string[] aLoStatus = new string[8];
            _device.GetContinuousInputStatus(uiAddr, ref aHiStatus, ref aLoStatus);
            frm8BitsSetting dlgBitsSetting = new frm8BitsSetting(uiAddr, aHiStatus, aLoStatus);
            dlgBitsSetting.Change8InputStatusReg = new frm8BitsSetting.DelegSetStatus(Change8InputStatus);
            dlgBitsSetting.Text = "修改输入状态寄存器设置";
            dlgBitsSetting.ShowDialog();
        }

        public void ShowChange16BitsStatusDlg(bool bOutput, uint uiAddr)
        {
            string[] aHiStatus = new string[16];
            string[] aLoStatus = new string[16];
            _device.GetBitsInputStatus(uiAddr, ref aHiStatus, ref aLoStatus);
            frmMultiBitsSetting dlgBitsSetting = new frmMultiBitsSetting(uiAddr, aHiStatus, aLoStatus, bOutput);
            dlgBitsSetting.ChangeInputBitsStatus = new frmMultiBitsSetting.DelegSetBitsStatus(Change16BitsInputStatus);
            dlgBitsSetting.Text = "修改输入状态寄存器设置";
            dlgBitsSetting.ShowDialog();
        }

        public void ShowChangeRegisterDlg(RegisterItem register, bool bHoldReg)
        {
            frmRegisterSetting dlgRegisterSetting = new frmRegisterSetting(register);
            dlgRegisterSetting._bHoldRegister = bHoldReg;
            dlgRegisterSetting._bAddRegister = false;
            dlgRegisterSetting.SetRegister = new frmRegisterSetting.DelegateSetRegister(ChangeRegister);
            dlgRegisterSetting.ShowDialog();
        }

        public void ChangeRegister(bool bHoldReg, RegisterItem register)
        {
            int nIndexOfName = (int)(_nClickSubIndex / 3) * 3 + 1;
            if (listView1.Items[_nClickIndex].SubItems[nIndexOfName].Text != register._strName)
                listView1.Items[_nClickIndex].SubItems[nIndexOfName].Text = register._strName;
        }

        public void ChangeOutputStatus(int nAddr, string strHiName, string strLoName)
        {
            int nIndexOfName = _nClickSubIndex + (((_nClickSubIndex % 2) == 0) ? 1 : 0);
            if (listView1.Items[_nClickIndex].SubItems[nIndexOfName].Text == _device._dicCoils[nAddr]._lsHiName[0])
                listView1.Items[_nClickIndex].SubItems[nIndexOfName].Text = strHiName;
            else
                listView1.Items[_nClickIndex].SubItems[nIndexOfName].Text = strLoName;
            _device._dicCoils[nAddr]._lsHiName[0] = strHiName;
            _device._dicCoils[nAddr]._lsLoName[0] = strLoName;
        }

        public void Change16OuputStatus(int nAddr, string[] aStrHiName, string[] aStrLoName)
        {
            int nLIndex, nHIndex;
            if (listView1.Items[_nClickIndex].SubItems[0].Text.Contains("L8"))
            {
                nLIndex = _nClickIndex;
                nHIndex = _nClickIndex + 1;
            }
            else
            {
                nLIndex = _nClickIndex - 1;
                nHIndex = _nClickIndex;
            }

            for (int nIndex = 0; nIndex < 8; nIndex++)
            {
                // 低8位
                if (listView1.Items[nLIndex].SubItems[nIndex + 1].Text != "" && listView1.Items[nLIndex].SubItems[nIndex + 1].Text == _device._dicCoils[nAddr]._lsHiName[nIndex])
                    listView1.Items[nLIndex].SubItems[nIndex + 1].Text = aStrHiName[nIndex];
                else
                    listView1.Items[nLIndex].SubItems[nIndex + 1].Text = aStrLoName[nIndex];

                // 高8位
                int nHiIndex = nIndex + 8;
                if (listView1.Items[nHIndex].SubItems[nIndex + 1].Text != "" && listView1.Items[nHIndex].SubItems[nIndex + 1].Text == _device._dicCoils[nAddr]._lsHiName[nIndex + 8])
                    listView1.Items[nHIndex].SubItems[nIndex + 1].Text = aStrHiName[nIndex + 8];
                else
                    listView1.Items[nHIndex].SubItems[nIndex + 1].Text = aStrLoName[nIndex + 8];
            }

            _device.SetOutputStatus(nAddr, aStrHiName, aStrLoName);
        }

        public void Change8InputStatus(int nAddr, string[] aStrHiNames, string[] aStrLoNames)
        {
            string[] aStrOldHiName = new string[8];
            string[] aStrOldLoName = new string[8];
            _device.GetContinuousInputStatus((uint)nAddr, ref aStrOldHiName, ref aStrOldLoName);
            for (int nSubIndex = 0; nSubIndex < 8; nSubIndex++)
            {
                if (listView1.Items[_nClickIndex].SubItems[nSubIndex + 1].Text == aStrOldHiName[nSubIndex])
                {
                    listView1.Items[_nClickIndex].SubItems[nSubIndex + 1].Text = aStrHiNames[nSubIndex];
                }
                else if (listView1.Items[_nClickIndex].SubItems[nSubIndex + 1].Text == aStrOldLoName[nSubIndex])
                {
                    listView1.Items[_nClickIndex].SubItems[nSubIndex + 1].Text = aStrLoNames[nSubIndex];
                }
            }

            _device.SetInputStatus(nAddr, aStrHiNames, aStrLoNames);
        }

        public void Change16BitsInputStatus(int nAddr, string[] aStrHiName, string[] aStrLoName)
        {
            int nLIndex, nHIndex;
            if (listView1.Items[_nClickIndex].SubItems[0].Text.Contains("L8"))
            {
                nLIndex = _nClickIndex;
                nHIndex = _nClickIndex + 1;
            }
            else
            {
                nLIndex = _nClickIndex - 1;
                nHIndex = _nClickIndex;
            }

            for (int nIndex = 0; nIndex < 8; nIndex++)
            {
                // 低8位
                if (listView1.Items[nLIndex].SubItems[nIndex + 1].Text != "" && listView1.Items[nLIndex].SubItems[nIndex + 1].Text == _device._dicInputStatus[nAddr]._lsHiName[nIndex])
                    listView1.Items[nLIndex].SubItems[nIndex + 1].Text = aStrHiName[nIndex];
                else
                    listView1.Items[nLIndex].SubItems[nIndex + 1].Text = aStrLoName[nIndex];

                // 高8位
                if (listView1.Items[nHIndex].SubItems[nIndex + 1].Text != "" && listView1.Items[nHIndex].SubItems[nIndex + 1].Text == _device._dicInputStatus[nAddr]._lsHiName[nIndex + 8])
                    listView1.Items[nHIndex].SubItems[nIndex + 1].Text = aStrHiName[nIndex + 8];
                else
                    listView1.Items[nHIndex].SubItems[nIndex + 1].Text = aStrLoName[nIndex + 8];
            }

            _device.SetInputStatus(nAddr, aStrHiName, aStrLoName);
        }

        public void ThreadSetOutput(object list)
        {
            object[] param = (object[])list;
            string strAddr = param[0].ToString();
            int nAddr = Convert.ToInt32(strAddr);
            ModBusDevice device = (ModBusDevice)param[1];
            string strOutput = (string)param[2];
            bool bRet = device.SetOutputStatusToDevice(nAddr);
            SetBarText("设置\"" + strOutput + (bRet ? "成功\"" : "失败\""));
        }

        public void AddOutput(int nAddr, string strHiName, string strLoName)
        {
            if (_device._dicCoils != null && _device._dicCoils.ContainsKey(nAddr))
            {
                MessageBox.Show("寄存器地址已经添加!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _device.AddCoil(nAddr, strHiName, strLoName);
            listView1.AddOutputStatus(_device._dicCoils[nAddr]);
        }

        public void AddOutput(int nAddr, string[] aStrHiName, string[] aStrLoName)
        {
            if (_device._dicCoils != null && _device._dicCoils.ContainsKey(nAddr))
            {
                MessageBox.Show("寄存器地址已经添加!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _device.AddOutputStatus(nAddr, true, aStrHiName, aStrLoName);
            listView1.AddOutputStatus(nAddr, aStrLoName);
        }

        private void frmDevice_FormClosing(object sender, FormClosingEventArgs e)
        {
            _bClosing = true;
        }

        public void AddContinuous8InputStatusRegs(int nAddr, string[] aStrHiName, string[] aStrLoName)
        {
            if (CheckStatusWasAdded(nAddr, _device._dicInputStatus))
                return;

            _device.AddInputStatus(nAddr, false, aStrHiName, aStrLoName);
            listView1.AddInput8BitsStatus(nAddr, aStrLoName);
        }

        public void Add16BitsInputStatusReg(int nAddr, string[] aStrHiName, string[] aStrLoName)
        {
            if (CheckStatusWasAdded(nAddr, _device._dicInputStatus))
                return;

            _device.AddInputStatus(nAddr, true, aStrHiName, aStrLoName);

            listView1.AddInput16BitsStatus(nAddr, aStrLoName);
        }

        private bool CheckStatusWasAdded(int nAddr, SerializableDictionary<Int32, StatusRegister> dic)
        {
            if (dic != null)
            {
                bool bAdded = false;
                if (dic.ContainsKey(nAddr))
                {
                    bAdded = true;
                }
                else
                {
                    for (int nCount = 1; nCount < 8; nCount++)
                    {
                        if (dic.ContainsKey(nAddr - nCount) && dic[nAddr - nCount]._lsHiName.Count >= nCount && !dic[nAddr - nCount]._bMultiBits)
                            bAdded = true;
                    }
                }

                if (bAdded)
                {
                    MessageBox.Show("寄存器地址已经添加!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }
            return false;
        }

        public void AddReister(bool bHoldReg, RegisterItem register)
        {
            if (bHoldReg)
                listView1.AddHoldRegister(register);
            else
                listView1.AddInputRegister(register);
        }

        public void InvokeUpdateInputRegValueToList(int nAddr, string strValue)
        {
            if (!_bClosing)
            {
                Invoke(new DelegUpdateInputRegDataToList(UpdateInputRegValueToList), nAddr, strValue);
            }
        }

        public delegate void DelegUpdateInputRegDataToList(int nAddr, string strValue);
        private void UpdateInputRegValueToList(int nAddr, string strValue)
        {
            string strAddr = string.Format("{0:4X}", nAddr);
            int nStartIndex = (listView1._nMaxSwitchIndex > 0) ? listView1._nMaxSwitchIndex : listView1._nMaxCoilIndex;
            nStartIndex += 1;
            for (int nIndex = 0; nIndex < listView1._nMaxInputRegIndex; nIndex++)
            {
                for (int nSubIndex = 0; nSubIndex < 3; nSubIndex++)
                {
                    if (listView1.Items[nIndex].SubItems[nSubIndex * 3].Text == strAddr)
                    {
                        if (frmRegisterSetting.CheckValueInRange(_device._dicInputRegs[nAddr]._strLowerLimit,
                                                _device._dicInputRegs[nAddr]._strUpperLimit, strValue, _device._dicInputRegs[nAddr]._nType))
                        {
                            listView1.Items[nIndex].SubItems[nSubIndex * 3].ForeColor = Color.Black;
                        }
                        else
                        {
                            listView1.Items[nIndex].SubItems[nSubIndex * 3].ForeColor = Color.Red;
                        }
                        listView1.Items[nIndex].SubItems[nSubIndex * 3].Text = strValue;
                        return;
                    }
                }
            }
        }

        public void InvokeUpdateInputStatus(int nAddr, byte[] data)
        {
            if (!_bClosing)
            {
                Invoke(new DelegUpdateInputStatus(UpdateInputStatusToList), nAddr, data);
            }
        }   

        public delegate void DelegUpdateInputStatus(int nAddr, byte[] data);
        public void UpdateInputStatusToList(int nAddr, byte[] data)
        {
            bool bMultiBits = (data.Length == 16);
            string strAddr = string.Format("{0:X4}{1}", nAddr, (bMultiBits ? "-L8" : ""));
            int nStartIndex = listView1._nMaxCoilIndex > 0 ? (listView1._nMaxCoilIndex + 1) : 0;
            for (int nIndex = nStartIndex; nIndex <= listView1._nMaxSwitchIndex; nIndex++)
            {
                if (listView1.Items[nIndex].SubItems[0].Text == strAddr)
                {
                    bool bStatus = false; 
                    for (int nSubIndex = 0; nSubIndex < 8; nSubIndex++)
                    {
                        bStatus = (data[nSubIndex] == 1);
                        if (_device._dicInputStatus[nAddr]._lsCurStatus[nSubIndex] != bStatus)
                        {
                            listView1.Items[nIndex].SubItems[nSubIndex].Text = bStatus ? _device._dicInputStatus[nAddr]._lsHiName[nSubIndex] :
                                                                                _device._dicInputStatus[nAddr]._lsLoName[nSubIndex];
                            _device._dicInputStatus[nAddr]._lsCurStatus[nSubIndex] = bStatus;
                        }

                        if (bMultiBits)
                        {
                            bStatus = (data[nSubIndex + 8] == 1);
                            if (_device._dicInputStatus[nAddr]._lsCurStatus[nSubIndex + 8] != bStatus)
                            {
                                listView1.Items[nIndex + 1].SubItems[nSubIndex].Text = bStatus ? _device._dicInputStatus[nAddr]._lsHiName[nSubIndex + 8] :
                                                                                    _device._dicInputStatus[nAddr]._lsLoName[nSubIndex + 8];
                                _device._dicInputStatus[nAddr]._lsCurStatus[nSubIndex + 8] = bStatus;
                            }
                        }
                    }
                    break;
                }
            }
        }



    }

    public delegate void LButtonClick(IntPtr hWnd, int x, int y, bool bDoubleClick);
    public class MessageFilter : IMessageFilter
    {
        public const int WM_HSCROLL = 0x114;
        public const int WM_VSCROLL = 0x115;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_PAINT = 0x000F;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_CLICK = -2;

        public LButtonClick MouseClick;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_LBUTTONDBLCLK)
            {
                if (MouseClick != null)
                {
                    MouseClick(m.HWnd, (int)m.LParam & 0xFFFF, (int)m.LParam >> 16, m.Msg == WM_LBUTTONDBLCLK);
                }
            }
            return false;
        }
    }
}
