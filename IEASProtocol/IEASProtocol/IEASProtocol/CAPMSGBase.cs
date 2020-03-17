using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEASProtocol
{
    /// <summary>
    /// CAP 메시지 전송을 위해 사용되는 프로토콜의 상위 클래스
    /// </summary>
    public class CAPMSGBase : IEASProtocolBase
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        protected string capMessage = string.Empty;
        #endregion
        #region Properties
        /// <summary>
        /// CAP 메시지
        /// </summary>
        public string CAPMessage
        {
            get { return capMessage; }
            set { capMessage = value; }
        }
        #endregion
        /// <summary>
        /// CAPMSGBase 의 생성자
        /// </summary>
        public CAPMSGBase()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        internal override void MakeData()
        {
            byte[] byCAPMessage = Encoding.Default.GetBytes(CAPMessage);
            DataLength = byCAPMessage.Length;
            data = new byte[DataLength];
            Array.Clear(data, 0, data.Length);
            #region 데이터 부 셋팅
            int index = 0;
            //CAP 메시지
            Buffer.BlockCopy(byCAPMessage, 0, Data, index, byCAPMessage.Length);
            index += byCAPMessage.Length;
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        internal override void ParseData()
        {
            if (Data == null || DataLength <= 0)
                throw new Exception("IEASProtocol.dll Exception - CAPMSGBase - ParseData Fail. 데이터가 없습니다.");
            int index = 0;
            int capMessageLength = dataLength;
            //CAP 메시지
            byte[] byCAPMessage = new byte[capMessageLength];
            Array.Clear(byCAPMessage, 0, byCAPMessage.Length);
            Buffer.BlockCopy(Data, index, byCAPMessage, 0, capMessageLength);
            CAPMessage = Encoding.Default.GetString(byCAPMessage);
            index += byCAPMessage.Length;
        }
    }
}
