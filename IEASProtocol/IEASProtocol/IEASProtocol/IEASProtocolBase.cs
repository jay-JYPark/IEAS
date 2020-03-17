using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEASProtocol
{
    /// <summary>
    /// IEASProtocol 의 최상위 클래스
    /// </summary>
    public class IEASProtocolBase
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        protected byte header1 = Convert.ToByte('K');
        /// <summary>
        /// 
        /// </summary>
        protected byte header2 = Convert.ToByte('C');
        /// <summary>
        /// 
        /// </summary>
        protected byte header3 = Convert.ToByte('A');
        /// <summary>
        /// 
        /// </summary>
        protected byte header4 = Convert.ToByte('P');
        /// <summary>
        /// 
        /// </summary>
        protected int dataLength = 0;
        /// <summary>
        /// 
        /// </summary>
        protected byte[] data = null;
        /// <summary>
        /// 
        /// </summary>
        protected byte cmd = 0x00;
        /// <summary>
        /// 
        /// </summary>
        protected IEASSenderType senderType = IEASSenderType.None;
        /// <summary>
        /// 
        /// </summary>
        protected byte[] reserved = new byte[2] { 0x00, 0x00 };
        /*/// <summary>
        /// 
        /// </summary>
        protected byte[] crc16 = new byte[2] { 0x00, 0x00 };
        /// <summary>
        /// 
        /// </summary>
        protected bool crcCheckResult = true;*/
        #endregion
        #region Properties
        /// <summary>
        /// 패킷의 Header 1번
        /// </summary>
        public byte Header1
        {
            get { return header1; }
            internal set { header1 = value; }
        }
        /// <summary>
        /// 패킷의 Header 2번
        /// </summary>
        public byte Header2
        {
            get { return header2; }
            internal set { header2 = value; }
        }
        /// <summary>
        /// 패킷의 Header 3번
        /// </summary>
        public byte Header3
        {
            get { return header3; }
            internal set { header3 = value; }
        }
        /// <summary>
        /// 패킷의 Header 4번
        /// </summary>
        public byte Header4
        {
            get { return header4; }
            internal set { header4 = value; }
        }
        /// <summary>
        /// 패킷의 데이터 부 길이
        /// </summary>
        public int DataLength
        {
            get { return dataLength; }
            internal set { dataLength = value; }
        }
        /// <summary>
        /// 패킷의 데이터 부
        /// </summary>
        public byte[] Data
        {
            get { return data; }
            set
            {
                data = value;
                DataLength = data.Length;
            }
        }
        /// <summary>
        /// 명령어 코드
        /// </summary>
        public byte CMD
        {
            get { return cmd; }
            internal set { cmd = value; }
        }
        /// <summary>
        /// 명령을 최초 전송한 시스템의 종류 ( 0x01 : 표준발령대, 0x02 : 통합게이트웨이, 0x03 : 표준경보시스템, 0x04 : 단말 )
        /// </summary>
        public IEASSenderType SenderType
        {
            get { return senderType; }
            set { senderType = value; }
        }
        /// <summary>
        /// 예약
        /// </summary>
        public byte[] Reserved
        {
            get { return reserved; }
            internal set { reserved = value; }
        }
        /*/// <summary>
        /// CRC 16
        /// </summary>
        public byte[] CRC16
        {
            get { return crc16; }
            internal set { crc16 = value; }
        }
        /// <summary>
        /// CRC 16 검사 결과
        /// </summary>
        public bool CRCCheckResult
        {
            get { return crcCheckResult; }
            internal set { crcCheckResult = value; }
        }*/
        #endregion
        /// <summary>
        /// IEASProtocol 의 생성자
        /// </summary>
        public IEASProtocolBase()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        internal virtual void MakeData()
        {
            DataLength = 0;
            data = new byte[DataLength];
        }
        /// <summary>
        /// 
        /// </summary>
        internal virtual void ParseData()
        {
            
        }
    }
}
