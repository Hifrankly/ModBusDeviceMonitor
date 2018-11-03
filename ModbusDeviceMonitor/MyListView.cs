using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ModbusDeviceMonitor
{
   public class MyListView : ListView
    {
       /// <summary>
       /// list的第一个子项用不同颜色进行区分
       /// </summary>
       public static Color[] aItemsHeadColor = { Color.GreenYellow, Color.LightBlue, Color.Orange, Color.Pink };

       public enum ListItemType
        { 
            Coils,
            InputStatus,
            InputRegister,
            HoldRegister
        }
        
        /// <summary>
        /// 选中的子项
        /// </summary>
        public ListViewItem.ListViewSubItem _selectedSubItem;

        /// <summary>
        /// 选中子项原来的背景色
        /// </summary>
        public Color _colorBak;

        /// <summary>
        /// 最后一条coil的行号
        /// </summary>
        public int _nMaxCoilIndex;

        /// <summary>
        /// 最后一条输入开关状态的行号
        /// </summary>
        public int _nMaxSwitchIndex;

        /// <summary>
        /// 最后一条输入寄存器行号
        /// </summary>
        public int _nMaxInputRegIndex;

        /// <summary>
        /// 最后一条保持寄存器行号
        /// </summary>
        public int _nMaxHoldRegIndex;

       /// <summary>
       /// 输出状态子项索引
       /// </summary>
        public int _nSubIndexOfOutputStatus;
       
       /// <summary>
       /// 保持寄存器的子项索引
       /// </summary>
        public int _nSubIndexOfHoldRegItem;

       /// <summary>
       /// 输入寄存器的子项索引
       /// </summary>
        public int _nSubIndexOfInputRegItem;

       /// <summary>
       /// 编辑控件
       /// </summary>
        public TextBox _tbEdit;

        /// <summary>
        /// TextBox输入无效
        /// </summary>
        private bool _bCancelEdit;

        public MyListView()
        {
            ResetIndexs();
            ShowItemToolTips = true;
            _nSubIndexOfHoldRegItem = 0;
            _nSubIndexOfInputRegItem = 0;
            _nSubIndexOfOutputStatus = 0;
            _tbEdit = new TextBox();
            _tbEdit.Visible = false;
            _tbEdit.KeyDown += textBoxEmbedded_KeyDown;
            this.Controls.Add(_tbEdit);
        }

        public void SelectSubItem(ListViewItem.ListViewSubItem subItem)
        {
            if (_selectedSubItem != null)
            {
                _selectedSubItem.BackColor = _colorBak;
            }
            _selectedSubItem = subItem;

            if (subItem != null)
            {
                _colorBak = subItem.BackColor;
                _selectedSubItem.BackColor = Color.LightGray;
            }
        }

        public void IncMaxCoilIndex()
        {
            _nMaxCoilIndex++;

            if (_nMaxSwitchIndex >= 0)
            {
                _nMaxSwitchIndex++;
            }

            if (_nMaxInputRegIndex >= 0)
            {
                _nMaxInputRegIndex++;
            }

            if (_nMaxHoldRegIndex >= 0)
            {
                _nMaxHoldRegIndex++;
            }
        }

      public void IncMaxInputStatusIndex()
        {
           if (_nMaxSwitchIndex < 0 && _nMaxCoilIndex >= 0)
           {
                _nMaxSwitchIndex = _nMaxCoilIndex;
           }
          
           _nMaxSwitchIndex++;

            if (_nMaxInputRegIndex >= 0)
            {
                _nMaxInputRegIndex++;
           }

            if (_nMaxHoldRegIndex >= 0)
            {
                _nMaxHoldRegIndex++;
           }
        }

      public void IncMaxInputRegIndex()
       {
           if (_nMaxInputRegIndex < 0)
           {
               if (_nMaxSwitchIndex >= 0)
               {
                   _nMaxInputRegIndex = _nMaxSwitchIndex;
               }
               else if (_nMaxCoilIndex >= 0)
               {
                   _nMaxInputRegIndex = _nMaxCoilIndex;
               }
           }
          
          _nMaxInputRegIndex++;

           if (_nMaxHoldRegIndex >= 0)
           {
               _nMaxHoldRegIndex++;
           }
       }

        public void ResetIndexs()
        {
            _nMaxCoilIndex = -1;
            _nMaxSwitchIndex = -1;
            _nMaxInputRegIndex = -1;
            _nMaxHoldRegIndex = -1;
        }

       //public int GetInsertIndexOfList(ListItemType listItemType)
       // {
            
       // }

       //public void Add(RegisterItem register)
       // {

       // }

       public void AddOutputStatus(StatusRegister status)
        {
            ListViewItem listItem = null;
            if (_nMaxCoilIndex < 0 || _nSubIndexOfOutputStatus > 7)
            {
                listItem = new ListViewItem();
                _nSubIndexOfOutputStatus = 0;
                listItem.UseItemStyleForSubItems = false;

                if ((_nMaxCoilIndex >= 0 && this.Items.Count > (_nMaxCoilIndex + 1)))
                {
                    this.Items.Insert(_nMaxCoilIndex + 1, listItem);
                }
                else if (_nMaxCoilIndex < 0 && this.Items.Count > 0)
                {
                    this.Items.Insert(0, listItem);
                }
                else
                {
                    this.Items.Add(listItem);
                }

                IncMaxCoilIndex();
            }
            else
            {
                listItem = this.Items[_nMaxCoilIndex];
            }

            for (int nIndex = 0; nIndex < status._lsLoName.Count; nIndex++)
            {
                if (_nSubIndexOfOutputStatus == 0)
                {
                    listItem.SubItems[0].BackColor = aItemsHeadColor[(int)ListItemType.Coils];
                    listItem.SubItems[0].Text = string.Format("{0:X4}", status._nStartAddr);
                    listItem.SubItems.Add(status._lsLoName[nIndex]);
                }
                else
                {
                    listItem.SubItems.Add(string.Format("{0:X4}", status._nStartAddr));
                    listItem.SubItems.Add(status._lsLoName[nIndex]);
                }
                _nSubIndexOfOutputStatus += 2;
            }
        }

       // 一个寄存器地址对应十六位输出
       public void AddOutputStatus(int nAddr, string[] aStrLoNames)
       {
           ListViewItem listItem = new ListViewItem();
           ListViewItem listItem1 = new ListViewItem();
           listItem.UseItemStyleForSubItems = false;
           listItem1.UseItemStyleForSubItems = false;

           if ((_nMaxCoilIndex >= 0 && this.Items.Count > (_nMaxCoilIndex + 1)))
           {
               this.Items.Insert(_nMaxCoilIndex, listItem);
           }
           else if (_nMaxCoilIndex < 0 && this.Items.Count > 0)
           {
               this.Items.Insert(0, listItem);
           }
           else
           {
               this.Items.Add(listItem);
           }
           IncMaxCoilIndex();

           this.Items.Add(listItem1);
           IncMaxCoilIndex();

           listItem.SubItems[0].BackColor = aItemsHeadColor[(int)ListItemType.Coils];
           listItem1.SubItems[0].BackColor = aItemsHeadColor[(int)ListItemType.Coils];
           listItem.SubItems[0].Text = string.Format("{0:X4}-L8", nAddr);
           listItem1.SubItems[0].Text = string.Format("{0:X4}-H8", nAddr);

           for (int nIndex = 0; nIndex < 8; nIndex++)
           {
               listItem.SubItems.Add(aStrLoNames[nIndex]);
               listItem1.SubItems.Add(aStrLoNames[nIndex + 8]);
           }
           _nSubIndexOfOutputStatus = 8;
       }

       // 添加输入状态8位连续寄存器
       public void AddInput8BitsStatus(int nAddr, string[] aStrLoNames)
       {
           ListViewItem listItem = new ListViewItem();
           listItem.UseItemStyleForSubItems = false;

           if (this.Items.Count == 0 || this.Items.Count == (_nMaxCoilIndex + 1) || this.Items.Count == (_nMaxSwitchIndex + 1))
           {
               this.Items.Add(listItem);
           }
           else if (_nMaxSwitchIndex > 0)
           {
               this.Items.Insert(_nMaxSwitchIndex, listItem);
           }
           else if (_nMaxCoilIndex >= 0)
           {
               this.Items.Insert(_nMaxCoilIndex + 1, listItem);
           }
           else
           {
               this.Items.Insert(0, listItem);
           }

           listItem.SubItems[0].BackColor = aItemsHeadColor[(int)ListItemType.InputStatus];
           listItem.SubItems[0].Text = string.Format("{0:X4}", nAddr);

           for (int nIndex = 0; nIndex < 8; nIndex++)
           {
               listItem.SubItems.Add(aStrLoNames[nIndex]);
           }

           IncMaxInputStatusIndex();
       }

       // 添加输入状态单寄存器16位状态
       public void AddInput16BitsStatus(int nAddr, string[] aStrLoNames)
       {
           ListViewItem listItem = new ListViewItem();
           ListViewItem listItem1 = new ListViewItem();
           listItem.UseItemStyleForSubItems = false;
           listItem1.UseItemStyleForSubItems = false;

           if (this.Items.Count == 0 || this.Items.Count == (_nMaxCoilIndex + 1) || this.Items.Count == (_nMaxSwitchIndex + 1))
           {
               this.Items.Add(listItem);
               this.Items.Add(listItem1);
           }
           else if (_nMaxSwitchIndex > 0)
           {
               this.Items.Insert(_nMaxSwitchIndex, listItem);
               this.Items.Insert(_nMaxSwitchIndex + 1, listItem1);
           }
           else if (_nMaxCoilIndex >= 0)
           {
               this.Items.Insert(_nMaxCoilIndex + 1, listItem);
               this.Items.Insert(_nMaxCoilIndex + 2, listItem1);
           }
           else
           {
               this.Items.Insert(0, listItem);
               this.Items.Insert(1, listItem1);
           }

           listItem.SubItems[0].BackColor = aItemsHeadColor[(int)ListItemType.InputStatus];
           listItem1.SubItems[0].BackColor = aItemsHeadColor[(int)ListItemType.InputStatus];
           listItem.SubItems[0].Text = string.Format("{0:X4}-L8", nAddr);
           listItem1.SubItems[0].Text = string.Format("{0:X4}-H8", nAddr);
           for (int nIndex = 0; nIndex < 8; nIndex++)
           {
               listItem.SubItems.Add(aStrLoNames[nIndex]);
               listItem1.SubItems.Add(aStrLoNames[nIndex + 8]);
           }

           IncMaxInputStatusIndex();
           IncMaxInputStatusIndex();
       }

       public void AddInputRegister(RegisterItem register)
        {
            ListViewItem listItem = null;
            int nInsertIndex = -1;

           if (_nMaxInputRegIndex >= 0)
            {
                nInsertIndex = _nMaxInputRegIndex;
            }
           else if (_nMaxSwitchIndex >= 0)
           {
               nInsertIndex = _nMaxSwitchIndex;
           }
           else if (_nMaxCoilIndex >= 0)
           {
               nInsertIndex = _nMaxCoilIndex;
           }

           if (nInsertIndex == -1 || _nSubIndexOfInputRegItem > 8 || _nMaxInputRegIndex < 0)
           {
               listItem = new ListViewItem(); 
               _nSubIndexOfInputRegItem = 0;
               listItem.UseItemStyleForSubItems = false;
               IncMaxInputRegIndex();
           }
           else 
           {
               listItem = this.Items[nInsertIndex];
           }

           if (_nSubIndexOfInputRegItem == 0)
           {
               listItem.SubItems[0].BackColor = aItemsHeadColor[(int)ListItemType.InputRegister];
               listItem.SubItems[0].Text = string.Format("{0:X4}", register._nStartAddr);
               listItem.SubItems.Add(register._strName);
               listItem.SubItems.Add("");

               if (nInsertIndex == -1 || this.Items.Count == (nInsertIndex + 1))
               {
                   this.Items.Add(listItem);
               }
               else
               {
                   this.Items.Insert(nInsertIndex + 1, listItem);
               }
           }
           else
           {
               listItem.SubItems.Add(string.Format("{0:X4}", register._nStartAddr));
               listItem.SubItems.Add(register._strName);
               listItem.SubItems.Add("");
           }

           listItem.UseItemStyleForSubItems = false;
           _nMaxInputRegIndex = listItem.Index;
           _nSubIndexOfInputRegItem += 3;     
        }

       public void AddHoldRegister(RegisterItem register)
       {
           ListViewItem listItem = null;
           int nInsertIndex = -1;

           if(_nMaxHoldRegIndex >= 0)
           {
               nInsertIndex = _nMaxHoldRegIndex;
           }
           else if (_nMaxInputRegIndex >= 0)
           {
               nInsertIndex = _nMaxInputRegIndex;

           }
           else if (_nMaxSwitchIndex >= 0)
           {
               nInsertIndex = _nMaxSwitchIndex;
           }
           else if (_nMaxCoilIndex >= 0)
           {
               nInsertIndex = _nMaxCoilIndex;
           }

           if (nInsertIndex == -1 || _nSubIndexOfInputRegItem > 8 || _nMaxHoldRegIndex < 0)
           {
               listItem = new ListViewItem();
               _nSubIndexOfHoldRegItem = 0;
           }
           else
           {
               listItem = this.Items[nInsertIndex];
           }

           if (_nSubIndexOfHoldRegItem == 0)
           {
               listItem.SubItems[0].BackColor = aItemsHeadColor[(int)ListItemType.HoldRegister]; ;
               listItem.SubItems[0].Text =  string.Format("{0:X4}", register._nStartAddr);
               listItem.SubItems.Add(register._strName);
               listItem.SubItems.Add("");

               if (nInsertIndex == -1 || this.Items.Count == (nInsertIndex + 1))
               {
                   this.Items.Add(listItem);
               }
               else
               {
                   this.Items.Insert(nInsertIndex + 1, listItem);
               }
           }
           else
           {
               listItem.SubItems.Add(string.Format("{0:X4}", register._nStartAddr));
               listItem.SubItems.Add(register._strName);
               listItem.SubItems.Add("");
           }

           listItem.UseItemStyleForSubItems = false;
           _nSubIndexOfHoldRegItem += 3;
           _nMaxHoldRegIndex = listItem.Index;
       }

       public void ShowTextBoxOnItem(int nIndex, int nSubIndex)
       {
           if (nIndex < this.Items.Count && nSubIndex < this.Items[nIndex].SubItems.Count)
           {
               Rectangle rect = this.Items[nIndex].SubItems[nSubIndex].Bounds;

               _bCancelEdit = false;
               _tbEdit.MaximumSize = new System.Drawing.Size(rect.Width, rect.Height);// = rect.Height;
               _tbEdit.SetBounds(rect.Left, rect.Top, rect.Width, rect.Height);
               _tbEdit.Visible = true;
               _tbEdit.BringToFront();
               _tbEdit.Text = this.Items[nIndex].SubItems[nSubIndex].Text;
           }
       }

       public string GetEditText()
       {
           return _tbEdit.Text;
       }

       public void HideEditBox()
       {
           _tbEdit.Hide();
       }

       private void textBoxEmbedded_KeyDown(object sender, KeyEventArgs e)
       {
           switch (e.KeyCode)
           {
               case Keys.Return:    // Enter key is pressed
                   _bCancelEdit = false;//  editing completed
                   e.Handled = true;
                   _tbEdit.Visible = false;
                   break;
               case Keys.Escape:    // Escape key is pressed
                   _bCancelEdit = true;  // editing was cancelled
                   e.Handled = true;
                   _tbEdit.Visible = false;
                   break;
           }
       }
    }
}
