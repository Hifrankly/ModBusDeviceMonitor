using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO.Ports;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Threading;
using System.IO;

 namespace ModbusDeviceMonitor
{
    [Serializable]
    public class Devices : IList<ModBusDevice>, ICollection<ModBusDevice>, IEnumerable<ModBusDevice>
    {
        [XmlIgnoreAttribute]
        private bool _bReadOnly = false;
        /// <summary>
        ///  设备列表
        /// </summary>
        private List<ModBusDevice> _listDevices;


        public Devices()
        {
            if (_listDevices == null)
            {
               _listDevices = new List<ModBusDevice>();
           }
        }

        public bool IsReadOnly
        {
            get
            {
                return _bReadOnly;
            }
        }

        public int Count
        {
            get
            {
                return _listDevices.Count();
            }
        }

        public ModBusDevice this[int index]
        {
            get
            {
                return _listDevices[index];
            }
            set { }
        }

        public IEnumerator<ModBusDevice> GetEnumerator()
        {
            for (int nIndex = 0; nIndex < _listDevices.Count(); nIndex++)
            {
                yield return _listDevices[nIndex];
            }
        }

        // 必须实现此方法，因为
        // IEnumerable<T> 继承 IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool Remove(ModBusDevice item)
        {
            return _listDevices.Remove(item);
        }

        public void CopyTo(ModBusDevice[] dest, int index)
        {

        }

        public bool Contains(ModBusDevice item)
        {
            return _listDevices.Contains(item);
        }

        public void Clear()
        {
            _listDevices.Clear();
        }

        public void RemoveAt(int nIndex)
        {
            _listDevices.RemoveAt(nIndex);
        }

        public void Insert(int nIndex, ModBusDevice item)
        {
            _listDevices.Insert(nIndex, item);
        }

        public int IndexOf(ModBusDevice item)
        {
            return _listDevices.IndexOf(item);
        }

        public void Add(ModBusDevice device)
        {
            _listDevices.Add(device);
        }
    }

    [Serializable]
    public class ModBusDevice
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  从机地址
        /// </summary>
        public Byte  Address { get; set; }

        /// <summary>
        ///  端口号
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        ///  波特率
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        ///  数据位
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits { get; set; }

        /// <summary>
        ///  校验
        /// </summary>
        public Parity Parity { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        [XmlIgnore]
        public SerialPort _port = null;

        /// <summary>
        ///  输出状态对象
        /// </summary>
        public SerializableDictionary<Int32, StatusRegister> _dicCoils;

        /// <summary>
        ///  输入状态
        /// </summary>
        public SerializableDictionary<Int32, StatusRegister> _dicInputStatus;

        /// <summary>
        /// 保持寄存器
        /// </summary>
        public SerializableDictionary<Int32, RegisterItem> _dicHoldRegs;

        /// <summary>
        /// 输入寄存器
        /// </summary>
        public SerializableDictionary<Int32, RegisterItem> _dicInputRegs;

        /// <summary>
        ///  读线圈状态功能码
        /// </summary>
        public byte FuncReadCoilStatus { get; set; }

        /// <summary>
        ///  读开关状态
        /// </summary>
        public byte FuncReadSwitch { get; set; }

        /// <summary>
        /// 读保持寄存器
        /// </summary>
        public byte FuncReadHoldReg { get; set; }

        /// <summary>
        /// 读输入寄存器
        /// </summary>
        public byte FuncReadInputReg { get; set; }
        
        /// <summary>
        ///  写单个线圈
        /// </summary>
        public byte FuncWriteSingleCoil { get; set; }
        
        /// <summary>
        ///  写单个保持寄存器
        /// </summary>
        public byte FuncWriteSingleHoldReg { get; set; }
        
        /// <summary>
        ///  写多个线圈
        /// </summary>
        public byte FuncWriteCoils { get; set; }

        /// <summary>
        /// 写多个保持寄存器
        /// </summary>
        public byte FuncWriteHoldRegs { get; set; }

        /// <summary>
        /// 超时时间(ms)
        /// </summary>
        public static int _nTimeOut = 2000; 

        /// <summary>
        /// 退出事件
        /// </summary>
        [XmlIgnore]
        public ManualResetEvent _mrQuitEvent;

        /// <summary>
        /// 设备被修改标志
        /// </summary>
        [XmlIgnore]
        public bool _bModified;

        /// <summary>
        /// 是否是新增标志
        /// </summary>
        [XmlIgnore]
        public bool _bCreatNew;

        /// <summary>
        /// 序列化文件路径
        /// </summary>
        [XmlIgnore]
        public string _strFilePath;

        /// <summary>
        /// 是否监控标志
        /// </summary>
        public bool _bMonitor;

        /// <summary>
        /// CRC多项式因子
        /// </summary>
        [XmlIgnore]
        public const UInt16 CRC_POLYNOMIAL_FACTOR = 0xA001;

        public ModBusDevice()
        {
            _bModified = false;
            _bCreatNew = false;
            _bMonitor = false;
            _mrQuitEvent = new ManualResetEvent(false);
        }

        public void SetPort(string strPortData)
        {
            string[] aSeparator = {";", ":", "PortName", "BaudRate", "DataBits", "StopBits", "Parity"};
            string[] aData = strPortData.Split(aSeparator, StringSplitOptions.RemoveEmptyEntries);
            if (aData.Length == 5)
            {
                PortName = aData[0];
                BaudRate = Convert.ToInt32(aData[1]);
                DataBits = Convert.ToInt32(aData[2]);

                // 停止位
                switch (aData[3])
                {
                    case "1":
                        StopBits = StopBits.One;
                        break;
                    case "1.5":
                        StopBits = StopBits.OnePointFive;
                        break;
                    case "2":
                        StopBits = StopBits.Two;
                        break;
                }

                // 校验方式
                switch (aData[4])
                {
                    case "无":
                        Parity = Parity.None;
                        break;
                    case "奇校验":
                        Parity = Parity.Odd;
                        break;
                    case "偶校验":
                        Parity = Parity.Even;
                        break;
                }
            }
            _bModified = true;
        }

        public void AddCoil(int nAddr, string strHiName, string strLoName)
        {
            StatusRegister coil = new StatusRegister();
            coil._nStartAddr = nAddr;
            coil._lsHiName.Add(strHiName);
            coil._lsLoName.Add(strLoName);
            coil._lsCurStatus.Add(false);

            if (_dicCoils == null)
            {
                _dicCoils = new SerializableDictionary<Int32, StatusRegister>();
            }
            lock (_dicCoils)
            {
                _dicCoils.Add(nAddr, coil);
            }
            _bModified = true;
        }

        public StatusRegister GetCoil(int nAddr)
        {
            return _dicCoils[nAddr];
        }

        public void AddOutputStatus(int nAddr, bool bMultiBits, string[] aStrHiName, string[] aStrLoName)
        {
            StatusRegister outputStatus = new StatusRegister();
            outputStatus._nStartAddr = nAddr;
            outputStatus._bMultiBits = bMultiBits;
            for (int nIndex = 0; nIndex < aStrHiName.Length; nIndex++)
            {
                outputStatus._lsHiName.Add(aStrHiName[nIndex]);
                outputStatus._lsLoName.Add(aStrLoName[nIndex]);
                outputStatus._lsCurStatus.Add(false);
            }

            if (_dicCoils == null)
            {
                _dicCoils = new SerializableDictionary<Int32, StatusRegister>();
            }
            _dicCoils.Add(nAddr, outputStatus);
            _bModified = true;
        }

        public void AddInputStatus(int nAddr, bool bMultiBits, string[] aStrHiName, string[] aStrLoName)
        {
            StatusRegister intputStatus = new StatusRegister();
            intputStatus._nStartAddr = nAddr;
            intputStatus._bMultiBits = bMultiBits;
            for (int nIndex = 0; nIndex < aStrHiName.Length; nIndex++)
            {
                intputStatus._lsHiName.Add(aStrHiName[nIndex]);
                intputStatus._lsLoName.Add(aStrLoName[nIndex]);
                intputStatus._lsCurStatus.Add(false);
            }

            if (_dicInputStatus == null)
            {
                _dicInputStatus = new SerializableDictionary<Int32, StatusRegister>();
            }
            _dicInputStatus.Add(nAddr, intputStatus);
            _bModified = true;
        }

        //public void AddInputStatus(int nAddr, int nBitPos, string strHiName, string strLoName)
        //{
        //    StatusRegister intputStatus = new StatusRegister();
        //    intputStatus._nStartAddr = nAddr;
        //    intputStatus._byBitPos = (byte)nBitPos;
        //    intputStatus._lsHiName = strHiName;
        //    intputStatus._lsLoName = strLoName;

        //    if (_dicInputStatus == null)
        //    {
        //        _dicInputStatus = new SerializableDictionary<UInt32, StatusRegister>();
        //    }
        //    _dicInputStatus.Add((UInt32)((nAddr << 8) | (nBitPos & 0xFF)), intputStatus);
        //    _bModified = true;
        //}

        //public void AddInputStatus(int nAddr, string strHiName, string strLoName)
        //{
        //    StatusRegister intputStatus = new StatusRegister();
        //    intputStatus._nStartAddr = nAddr;
        //    intputStatus._lsHiName = strHiName;
        //    intputStatus._lsLoName = strLoName;

        //    if (_dicInputStatus == null)
        //    {
        //        _dicInputStatus = new SerializableDictionary<UInt32, StatusRegister>();
        //    }
        //    _dicInputStatus.Add((UInt32)(nAddr << 8), intputStatus);
        //    _bModified = true;
        //}

        public StatusRegister GetInputStatus(int nAddr, int nBitPos)
        {
            return _dicInputStatus[nAddr];
        }

        public void AddHoldRegister(RegisterItem register)
        {
            if (_dicHoldRegs == null)
                _dicHoldRegs = new SerializableDictionary<int, RegisterItem>();
            _dicHoldRegs.Add(register._nStartAddr, register);
            register._aData = new byte[register._nLength];
            _bModified = true;
        }

        public void AddInputRegister(RegisterItem register)
        {
            if (_dicInputRegs == null)
                _dicInputRegs = new SerializableDictionary<int, RegisterItem>();
            _dicInputRegs.Add(register._nStartAddr, register);
            register._aData = new byte[register._nLength];
            _bModified = true;
        }

        //public void Get8BitsOutputStatus(uint uiStartAddr, ref string[] aHiStats, ref string[] aLoStatus)
        //{
        //   for (int nIndex = 0; nIndex < 8; nIndex++)
        //   {
        //       uint uiKey = uiStartAddr << 8 | (uint)nIndex;
        //       if (_dicCoils.ContainsKey(uiKey))
        //       {
        //           aHiStats[nIndex] = _dicCoils[uiKey]._strHiBitName;
        //           aLoStatus[nIndex] = _dicCoils[uiKey]._strLoBitName;
        //       }
        //       else
        //       {
        //           aHiStats[nIndex] = "";
        //           aLoStatus[nIndex] = "";
        //       }
        //   }
        //}

        public void GetContinuousInputStatus(uint uiStartAddr, ref string[] aHiStats, ref string[] aLoStatus)
        {
            for (int nIndex = 0; nIndex < aHiStats.Length; nIndex++)
            {
                uint uiKey = (uiStartAddr + (uint)nIndex);
                if (_dicInputStatus.ContainsKey((int)uiKey))
                {
                    aHiStats[nIndex] = _dicInputStatus[(int)uiKey]._lsHiName[nIndex];
                    aLoStatus[nIndex] = _dicInputStatus[(int)uiKey]._lsLoName[nIndex];
                }
                else
                {
                    aHiStats[nIndex] = "";
                    aLoStatus[nIndex] = "";
                }
            }
        }

        public void GetBitsInputStatus(uint uiStartAddr, ref string[] aHiStats, ref string[] aLoStatus)
        {
            for (int nIndex = 0; nIndex < aHiStats.Length; nIndex++)
            {
                uint uiKey = uiStartAddr ;
                if (_dicInputStatus.ContainsKey((int)uiKey))
                {
                    aHiStats[nIndex] = _dicInputStatus[(int)uiKey]._lsHiName[nIndex];
                    aLoStatus[nIndex] = _dicInputStatus[(int)uiKey]._lsLoName[nIndex];
                }
                else
                {
                    aHiStats[nIndex] = "";
                    aLoStatus[nIndex] = "";
                }
            }
        }

        public void GetBitsOutputStatus(int nStartAddr, ref string[] aHiStats, ref string[] aLoStatus)
        {
            if (_dicCoils.ContainsKey(nStartAddr))
            {
                for (int nIndex = 0; nIndex < aHiStats.Length; nIndex++)
                {
                    aHiStats[nIndex] = _dicCoils[nStartAddr]._lsHiName[nIndex];
                    aLoStatus[nIndex] = _dicCoils[nStartAddr]._lsLoName[nIndex];
                }
            }
        }

        //public void Set8Coils(uint uiStartAddr, string[] aHiStats, string[] aLoStatus)
        //{
        //    for (int nIndex = 0; nIndex < 8; nIndex++)
        //    {
        //        uint uiKey = uiStartAddr << 8 | (uint)nIndex;
        //        if (_dicCoils.ContainsKey(uiKey))
        //        {
        //          if(aHiStats[nIndex] == "")
        //          {
        //              _dicCoils.Remove(uiKey);
        //          }
        //          else
        //          {
        //            _dicCoils[uiKey]._strHiBitName = aHiStats[nIndex];
        //            _dicCoils[uiKey]._strLoBitName = aLoStatus[nIndex];
        //          }
        //        }
        //        else if(aHiStats[nIndex] != "" && aLoStatus[nIndex] != "")
        //        {
        //            StatusItem coil = new StatusItem();
        //            coil._strHiBitName = aHiStats[nIndex];
        //            coil._strLoBitName = aLoStatus[nIndex];
        //            coil._nAddr = (int)uiStartAddr;
        //            coil._byBitPos = (byte)nIndex;
        //            _dicCoils.Add(uiKey, coil);
        //        }
        //    }
        //    _bModified = true;
        //}

        public void SetInputStatus(int nStartAddr, string[] aHiStats, string[] aLoStatus)
        {
            if (_dicInputStatus.ContainsKey((int)nStartAddr))
             {
                for (int nIndex = 0; nIndex < aHiStats.Length; nIndex++)
                {
                    _dicInputStatus[nStartAddr]._lsHiName[nIndex] = aHiStats[nIndex];
                    _dicInputStatus[nStartAddr]._lsLoName[nIndex] = aLoStatus[nIndex];
                }
            }
            _bModified = true;
        }

        public void SetOutputStatus(int nStartAddr, string[] aHiStats, string[] aLoStatus)
        {
            if (_dicCoils.ContainsKey(nStartAddr))
             {
                for (int nIndex = 0; nIndex < aHiStats.Length; nIndex++)
                {
                    _dicCoils[nStartAddr]._lsHiName[nIndex] = aHiStats[nIndex];
                    _dicCoils[nStartAddr]._lsLoName[nIndex] = aLoStatus[nIndex];
                }
            }
            _bModified = true;
        }

        /// <summary>
        /// 检查输入寄存器的地址是否与已有的设置有冲突
        /// </summary>
        /// <param name="nAddr"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public bool CheckAddInputRegAddr(int nAddr, int nLen)
        {
            if (_dicInputRegs == null)
                return true;
            
            if (_dicInputRegs.ContainsKey(nAddr))
            {
                MessageBox.Show(string.Format("地址{0:X4}已添加!", nAddr), "添加输入寄存器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            foreach (RegisterItem item in _dicInputRegs.Values)
            {
                if (nAddr >= item._nStartAddr && nAddr <= (item._nStartAddr + item._nLength - 1) || (item._nStartAddr >= nAddr && item._nStartAddr <= (nAddr + nLen - 1)))
                {
                    MessageBox.Show(string.Format("寄存器地址{0:X4}与已添加寄存器{1}有重叠!", nAddr, item._strName), "添加输入寄存器");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检查保持寄存器是否与已有地址有冲突
        /// </summary>
        /// <param name="nAddr"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public bool CheckAddHoldRegAddr(int nAddr, int nLen)
        {
            if (_dicHoldRegs == null)
                return true;    
            
            if (_dicHoldRegs.ContainsKey(nAddr))
            {
                MessageBox.Show(string.Format("地址{0:X4}已添加!", nAddr), "添加保持寄存器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            foreach (RegisterItem item in _dicHoldRegs.Values)
            {
                if (nAddr >= item._nStartAddr && nAddr <= (item._nStartAddr + item._nLength - 1) || (item._nStartAddr >= nAddr && item._nStartAddr <= (nAddr + nLen - 1)))
                {
                    MessageBox.Show(string.Format("寄存器地址{0:X4}与已添加寄存器{1}有重叠!", nAddr, item._strName), "添加保持寄存器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// CRC计算
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public UInt16 CalculateCRC(byte[] buf, int nLen)
        {
            UInt16 uiCRCFull = 0xFFFF;
            for (int nIndex = 0; nIndex < nLen; nIndex++)
            {
                uiCRCFull ^= buf[nIndex];
                for (int nSubIndex = 0; nSubIndex < 8; nSubIndex++)
                {
                    UInt16 uiLSB = (UInt16)(uiCRCFull & 0x0001);
                    uiCRCFull = (UInt16)((uiCRCFull >> 1) & 0x7FFF);

                    if (uiLSB != 0)
                    {
                        uiCRCFull ^= CRC_POLYNOMIAL_FACTOR;
                    }
                }
            }

            return uiCRCFull;
        }

        /// <summary>
        /// 读输出状态寄存器值
        /// </summary>
        /// <param name="uiKey"></param>
        /// <returns></returns>
        public bool ReadOutputStatusReg(uint uiKey)
        {
            if (_port != null)
            {
                //Key中包含Addr
                UInt16 uiAddr = (UInt16)(uiKey >> 8);


                lock (_port)
                {

                }
            }

            return false;
        }

        public void ResetCurrentStatus()
        {
            if (_dicCoils != null)
            {
                foreach (StatusRegister reg in _dicCoils.Values)
                {
                    if (reg._lsCurStatus.Count == 0)
                    {
                        for (int nIndex = 0; nIndex < reg._lsHiName.Count; nIndex++)
                        {
                            reg._lsCurStatus.Add(false);
                        }
                    }
                }
            }

            if (_dicInputStatus != null)
            {
                foreach (StatusRegister reg in _dicInputStatus.Values)
                {
                    if (reg._lsCurStatus.Count == 0)
                    {
                        for (int nIndex = 0; nIndex < reg._lsHiName.Count; nIndex++)
                        {
                            reg._lsCurStatus.Add(false);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 读保持寄存器值
        /// </summary>
        public bool ReadHoldRegVlaue(int nKey)
        {
            if (_port != null)
            {
                                   
            }
            return false;
        }



        /// <summary>
        /// 扫描输入状态寄存器值
        /// </summary>
        public void ScanInputStatusReg()
        {

        }

        /// <summary>
        /// 扫描输入寄存器的值
        /// </summary>
        public void ScanInputReg()
        {

        }

        public static void WriteLog(string strText)
        {
            try
            {
                DateTime time = DateTime.Now;
                string strFile = Application.StartupPath + string.Format("\\Logs\\{0}{1:D2}\\{2:D2}", time.Year, time.Month, time.Day);
                StreamWriter writer = new StreamWriter(strFile, true);
                string strTime = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", time.Hour, time.Minute, time.Second, time.Millisecond);
                writer.Write(strTime + ": " + strText);
                writer.Close();
            }
            catch (System.Exception ex)
            {
            }
        }

        public string DataOfBufToHexString(byte[] buf)
        {
            string strHexString = string.Format("{0:X2}", buf[0]);
            for (int nIndex = 1; nIndex < buf.Length; nIndex++)
            {
                strHexString += " " + string.Format("{0:X2}", buf[nIndex]);

                if ((nIndex % 24) == 0)
                {
                    strHexString += "\r\n";
                    if (nIndex != (buf.Length - 1))
                    {
                        strHexString += "       ";
                    }
                }
            }

            return strHexString;
        }

        public bool SetOutputStatusToDevice(int nAddr)
        {
            if (_dicCoils.ContainsKey(nAddr))
            {
                byte[] buf = new byte[8];
                UInt16 data = 0;
                buf[0] = (byte)Address;//设备地址
                buf[1] = (byte)FuncWriteSingleCoil;//功能码
                buf[2] = (byte)((nAddr >> 8) & 0xFF);//地址高字节
                buf[3] = (byte)(nAddr & 0xFF);//地址低字节

                // 16位寄存器对应16位状态
                if (_dicCoils[nAddr]._bMultiBits)
                {
                    for (int nIndex = 0; nIndex < 16; nIndex++)
                    {
                        data |= (UInt16)(_dicCoils[nAddr]._lsCurStatus[nIndex] ? (1 << nIndex) : 0);
                    }
                }
                else
                {
                    data = (UInt16)(_dicCoils[nAddr]._lsCurStatus[0] ? 0xFF00 : 0x0000);
                }

                buf[4] = (byte)((data >> 8) & 0xFF);
                buf[5] = (byte)(data & 0xFF);

                UInt16 nCRC = CalculateCRC(buf, 6);
                buf[6] = (byte)((nCRC >> 8) & 0xFF);
                buf[7] = (byte)(nCRC & 0xFF);

                byte[] aByRegBuf = null;
                if (Communication(buf, FuncWriteSingleCoil, 8, 2000, ref aByRegBuf))
                {
                    if (aByRegBuf[6] == buf[6] && aByRegBuf[7] == buf[7])
                        return true;
               }
            }
            return false;
        }

        public bool SetHoldRegData(int nAddr)
        {
            if (_dicHoldRegs.ContainsKey(nAddr))
            {
                // 数据长度 = 设备地址1字节 + 功能码1字节 + 寄存器起始地址2字节 +  数据长度 + CRC 2字节
                byte[] buf = new byte[6 + _dicHoldRegs[nAddr]._nLength];
                buf[0] = Address;
                buf[1] = FuncWriteHoldRegs;
                buf[2] = (byte)((nAddr >> 8) & 0xFF);
                buf[3] = (byte)(nAddr & 0xFF);
                buf[4] = (byte)((_dicHoldRegs[nAddr]._nLength >> 8) & 0xFF);
                buf[5] = (byte)(_dicHoldRegs[nAddr]._nLength & 0xFF);

                // 数据部分
                byte[] bufData = _dicHoldRegs[nAddr].GetDataBuf();
                for (int nIndex = 0; nIndex < bufData.Length; nIndex++)
                {
                    buf[6 + nIndex] = bufData[nIndex];
                }

                // CRC
                UInt16 uiCRC = CalculateCRC(buf, buf.Length - 2);
                buf[buf.Length - 2] = (byte)((uiCRC >> 8) & 0xFF);
                buf[buf.Length - 1] = (byte)(uiCRC & 0xFF);

                byte[] bufResponse = null;
                if (Communication(buf, FuncWriteHoldRegs, 8, _nTimeOut, ref bufResponse))
                {
                    // 判断返回的地址和个数是否与写入的相同
                    if (buf[2] == bufResponse[2]  && buf[4] == bufResponse[4] && buf[5] == bufResponse[5])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool ReadInputStatus(ref int nStartAddr, ref byte[] status, bool bContinuous)
        {
            if (_dicInputStatus.ContainsKey(nStartAddr))
            {
                byte[] buf = new byte[8];
                buf[0] = Address;
                buf[1] = FuncReadSwitch;
                buf[2] = (byte)(nStartAddr >> 8);
                buf[3] = (byte)(nStartAddr & 0xFF);
                buf[4] = 0;

                int nCount = 0;
                bool bMultiBitsOfReg = (_dicInputStatus[nStartAddr]._lsHiName.Count == 16);
                while (_dicInputStatus.ContainsKey(nStartAddr))
                {
                    if ((_dicInputStatus[nStartAddr]._lsHiName.Count == 16 && !bMultiBitsOfReg) || (_dicInputStatus[nStartAddr]._lsHiName.Count == 8 && bMultiBitsOfReg))
                    {
                        break;
                    }

                    nCount += bMultiBitsOfReg ? 1 : 8;

                    if (!bContinuous || nCount == 255 || (!bMultiBitsOfReg && nCount == 248))
                        break;

                    nStartAddr += bMultiBitsOfReg ? 1 : 8;
                }

                buf[5] = (byte)(nCount >> 8);
                buf[6] = (byte)(nCount & 0xFF);

                //计算CRC
                UInt16 nCRC = CalculateCRC(buf, 6);
                buf[6] = (byte)(nCRC >> 8);
                buf[7] = (byte)(nCRC & 0xFF);

                int nRetCount = bMultiBitsOfReg ? nCount * 2 : ((nCount - 1) / 16 + 1) * 2; 
                return Communication(buf, FuncReadSwitch, nRetCount, 2000, ref status);
            }

            return false;
        }

        public bool ReadInputReg(int nAddr, ref byte[] bufData)
        {
            if (_dicInputRegs.ContainsKey(nAddr))
            {
                byte[] buf = new byte[8];
                buf[0] = Address;
                buf[1] = FuncReadInputReg;
                buf[2] = (byte)((nAddr >> 8) & 0xFF);
                buf[3] = (byte)(nAddr & 0xFF);
                buf[4] = (byte)(((_dicInputRegs[nAddr]._nLength / 2) >> 8) & 0xFF);
                buf[5] = (byte)((_dicInputRegs[nAddr]._nLength / 2) & 0xFF);

                UInt16 uiCRC = CalculateCRC(buf, 6);
                buf[6] = (byte)((uiCRC >> 8) & 0xFF);
                buf[7] = (byte)(uiCRC & 0xFF);

                return Communication(buf, FuncReadInputReg, _dicInputRegs[nAddr]._nLength, 2000, ref bufData);//返回字节数 功能码1字节 + 字节数 1字节 + 数据长度 + CRC 2字节
            }

            return false;
        }

        public bool Communication(byte[] aBySendData, int nRetFunc, int nRetCount, int nTimeout, ref byte[] aByRetBuf)
        {
            if (_port != null && _port.IsOpen)
            {
                lock (_port)
                {
                    int nLoop = 0;
                    string strText;
                    string strData = DataOfBufToHexString(aBySendData);
                    while (nLoop < 3)
                    {
                        strText = string.Format("Send data to modebus device {0}:\r\n{1}", Name, strData);
                        WriteLog(strText);

                        _port.Write(aBySendData, 0, aBySendData.Length);
                    
                        // 按波特率稍加延时, 3个半bit的延时和发送数据的时间
                        int nMillSec = (int)((double)(DataBits + (int)StopBits) * aBySendData.Length * 1000 / BaudRate); 
                        Thread.Sleep(nMillSec + (int)(3.5 * 1000 / BaudRate));
                    
                        //读取端口输入
                        byte[] buf = new byte[2048];
                        _port.ReadTimeout = nTimeout;
                   
                        int nCount = _port.Read(buf, 0, nRetCount);
                        if (nCount > 0)
                        {
                            aByRetBuf = new byte[nCount];
                            Array.Copy(buf, aByRetBuf, nCount);
                            strText = string.Format("Received data from port {0}:\r\n{1}", PortName, DataOfBufToHexString(buf));
                            WriteLog(strText);

                            // CRC校验
                            UInt16 nCRC = CalculateCRC(buf, buf.Length - 2);
                            if (nCRC == BitConverter.ToUInt16(buf, buf.Length - 2))
                            {
                                if (nCount >= nRetCount && buf[1] == nRetFunc)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                WriteLog("CRC value is invalid of response data");
                            }
                        }


                        if (_mrQuitEvent.WaitOne(3000))
                            return false;   
                    }

                }
            }
            return false;
        }

        public bool WriteData(byte[] buf)
        {
            if (_port != null && _port.IsOpen)
            {
                lock (_port)
                {
                    _port.Write(buf, 0, buf.Length);
                    
                    // 按波特率稍加延时, 3个bit的延时和发送数据的时间
                    int nMillSec = (int)((double)(DataBits + (int)StopBits) * buf.Length * 1000 / BaudRate); 
                    Thread.Sleep(nMillSec + (int)(3.0 * 1000 / BaudRate));
                    
 
                    return true;
                }
            }
            return false;
        }

        public void ClosePort()
        {
            if (_port != null && _port.IsOpen)
            {
                lock (_port)
                {
                    try
                    {
                        _port.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public void OpenPort()
        {
            if (_port != null)
            {
                lock (_port)
                {
                    try
                    {
                        _port.Open();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
    }

    public class PortCommucication
    {

    }

    [Serializable]
    public class StatusRegister
    {   
        [XmlElement("RegisterAddr")]
        public int _nStartAddr = 0;

        [XmlElement("FlagMultiBits")]
        public bool _bMultiBits = false;

        [XmlElement("BitPosition")]
        public byte _byBitPos = 0;
        //public string _strName = "";
        
        [XmlElement("HiStatusName")]
        public List<string> _lsHiName = new List<string>();

        [XmlElement("LoStatusName")]
        public List<string> _lsLoName = new List<string>();

        [XmlElement("CurStatus")]
        public List<bool> _lsCurStatus = new List<bool>();

        [XmlElement("InitStatus")]
        public byte _byInit = 0;

        [XmlIgnore]
        public byte _byData = 0;

        public StatusRegister()
        {

        }
    }

    [Serializable]
    public class RegisterItem
    {
        public enum DataType
        { 
            Type_Char,
            Type_Byte,
            Type_Int16,
            Type_UInt16,
            Type_Int32,
            Type_UInt32,
            Type_Int64,
            Type_UInt64,
            Type_Float,
            Type_Double,
            Type_String
        }
        
        [XmlElement("StartAddress")]
        public int _nStartAddr;

        [XmlElement("Name")]
        public string _strName;

        [XmlElement("DataType")]
        public int _nType;

        //[XmlElement("Value")]
        //public ValueType _Value;

        [XmlElement("UpperLimit")]
        public string _strUpperLimit;

        [XmlElement("LowerLimit")]
        public string _strLowerLimit;

        [XmlElement("Length")]
        public int _nLength;

        [XmlElement("StringData")]
        public string _strData;

        [XmlElement("Coefficient")]
        public Single _dCoefficient;

        [XmlIgnore]
        public byte[] _aData;

        public RegisterItem()
        {
            _dCoefficient = 1; 
        }

        public byte[] GetDataBuf()
        {
            switch ((DataType)_nType)
            {
                case DataType.Type_Char:
                    _aData = BitConverter.GetBytes(Convert.ToChar(_strData));
                    break;
                case DataType.Type_Byte:
                    _aData = BitConverter.GetBytes(Convert.ToByte(_strData));
                    break;
                case DataType.Type_Int16:
                    _aData = BitConverter.GetBytes((Int16)(Convert.ToDouble(_strData) / _dCoefficient));
                    break;
                case DataType.Type_UInt16:
                    _aData = BitConverter.GetBytes((UInt16)(Convert.ToDouble(_strData) / _dCoefficient));
                    break;
                case DataType.Type_Int32:
                    _aData = BitConverter.GetBytes((Int32)(Convert.ToDouble(_strData) / _dCoefficient));
                    break;
                case DataType.Type_UInt32:
                    _aData = BitConverter.GetBytes((UInt32)(Convert.ToDouble(_strData) / _dCoefficient));
                    break;
                case DataType.Type_Int64:
                    _aData = BitConverter.GetBytes(Convert.ToInt64(_strData));
                    break;
                case DataType.Type_UInt64:
                    _aData = BitConverter.GetBytes(Convert.ToUInt64(_strData));
                    break;
                case DataType.Type_Float:
                    _aData = BitConverter.GetBytes(Convert.ToSingle(_strData));
                    break;
                case DataType.Type_Double:
                    _aData = BitConverter.GetBytes(Convert.ToDouble(_strData));
                    break;
                case DataType.Type_String:
                    if (_aData == null)
                        _aData = new byte[_nLength];

                    for (int nIndex = 0; nIndex < _strData.Length; nIndex++)
                    {
                        _aData[nIndex] = Convert.ToByte(_strData.ElementAt(nIndex));
                    }
                    break;
            }
            
            return _aData;
        }

        public string BufDataToValueString(byte[] buf)
        {
            string strVal = "";
            switch ((DataType)_nType)
            {
                case DataType.Type_Char:
                    strVal = Convert.ToString(BitConverter.ToChar(buf, 1));
                    break;
                case DataType.Type_Byte:
                    strVal = Convert.ToString(buf[1]);
                    break;
                case DataType.Type_Int16:
                    strVal = BitConverter.ToInt16(buf, 0).ToString();
                    break;
                case DataType.Type_UInt16:
                    strVal = BitConverter.ToUInt16(buf, 0).ToString();
                    break;
                case DataType.Type_Int32:
                    strVal = BitConverter.ToInt32(buf, 0).ToString();
                    break;
                case DataType.Type_UInt32:
                    strVal = BitConverter.ToUInt32(buf, 0).ToString();
                    break;
                case DataType.Type_Int64:
                    strVal = BitConverter.ToInt64(buf, 0).ToString();
                    break;
                case DataType.Type_UInt64:
                    strVal = BitConverter.ToUInt64(buf, 0).ToString();
                    break;
                case DataType.Type_Float:
                    strVal = string.Format("{0:R}", BitConverter.ToSingle(buf, 0));
                    break;
                case DataType.Type_Double:
                    strVal = string.Format("{0:R}", BitConverter.ToDouble(buf, 0));
                    break;
                case DataType.Type_String:
                    strVal = BitConverter.ToString(buf, 0);
                    break;
            }

            return strVal;
        }
    }

    /// <summary>
    /// 加入同步锁可多线程操作
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
     public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable  
    {
       /// <summary>
       /// Synchronous lock
       /// </summary>
       public Object _lock = new Object();

        public SerializableDictionary() 
        {
        }  

        public void WriteXml(XmlWriter write)       // Serializer  
        {  
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));  
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));  
  
            foreach (KeyValuePair<TKey, TValue> kv in this)  
            {  
                write.WriteStartElement("SerializableDictionary");  
                write.WriteStartElement("key");  
                KeySerializer.Serialize(write, kv.Key);  
                write.WriteEndElement();  
                write.WriteStartElement("value");  
                ValueSerializer.Serialize(write, kv.Value);  
                write.WriteEndElement();  
                write.WriteEndElement();  
            }  
        } 
 
        public void ReadXml(XmlReader reader)       // Deserializer  
        {  
            reader.Read();  
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));  
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));  
  
            while (reader.NodeType != XmlNodeType.EndElement)  
            {  
                reader.ReadStartElement("SerializableDictionary");  
                reader.ReadStartElement("key");  
                TKey tk = (TKey)KeySerializer.Deserialize(reader);  
                reader.ReadEndElement();  
                reader.ReadStartElement("value");  
                TValue vl = (TValue)ValueSerializer.Deserialize(reader);  
                reader.ReadEndElement();  
                reader.ReadEndElement();  
                this.Add(tk, vl);  
                reader.MoveToContent();  
            }  
            reader.ReadEndElement();  
  
        }
  
        public XmlSchema GetSchema()  
        {  
            return null;  
        }

        /// <summary>
        /// Key 进行索引
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new TValue this[TKey key]
        {
            get
            {
                lock (_lock)
                {
                    return base[key];
                }
            }
            set
            {
                lock (_lock)
                {
                    base[key] = value;
                }
            }
        }

        public new bool ContainsKey(TKey key)
        {
            lock (_lock)
            {
                return base.ContainsKey(key);
            }
        }

        public new bool Remove(TKey key)
        {
            lock (_lock)
            {
                return base.Remove(key);
            }
        }

        public new void Add(TKey key, TValue value)
        {
            lock (_lock)
            {
                base.Add(key, value);
            }
        }

        public new Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                return base.Values;
            }
        }
    }  
}
