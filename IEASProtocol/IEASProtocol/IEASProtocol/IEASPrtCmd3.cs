using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEASProtocol
{
    /// <summary>
    /// 접속 인증 요청
    /// 통합게이트웨이로 TCP 연결 후 접속에 대한 인증을 요청하기 위해 사용되는 명령. 인증을 위한 32자리의 코드 정보가 담겨있음.
    /// </summary>
    public class IEASPrtCmd3 : IEASProtocolBase
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        protected string authentiCode = "00000000000000000000000000000000";
        #endregion
        #region Properties
        /// <summary>
        /// 32자리의 인증 코드
        /// </summary>
        public string AuthentiCode
        {
            get { return authentiCode; }
            set 
            {
                if (value.Length == 32)
                    authentiCode = value;
                else if (value.Length > 32)
                    authentiCode = value.Substring(0, 32);
                else
                {
                    int dumyLen = 32 - value.Length;
                    string strDumy = string.Empty;
                    for (int i = 0; i < dumyLen; i++)
                        strDumy += "0";
                    authentiCode = value + strDumy;
                }
            }
        }
        #endregion
        /// <summary>
        /// IEASPrtCmd3 의 생성자. 커맨드를 0x03 로 초기화
        /// </summary>
        public IEASPrtCmd3()
        {
            this.CMD = 0x03;
        }
        /// <summary>
        /// 
        /// </summary>
        internal override void MakeData()
        {
            byte[] byAuthenticode = Encoding.Default.GetBytes(AuthentiCode);
            if(byAuthenticode.Length != 32)
                throw new Exception("IEASProtocol.dll Exception - IEASPrtCmd3 - MakeData Fail - 인증 코드의 길이는 32 여야 합니다.");
            DataLength = 32;
            data = new byte[DataLength];
            Array.Clear(data, 0, data.Length);
            #region 데이터 부 셋팅
            int index = 0;
            //인증 코드
            Buffer.BlockCopy(byAuthenticode, 0, Data, index, 32);
            index += 32;
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        internal override void ParseData()
        {
            if (Data == null || DataLength <= 0)
                throw new Exception("IEASProtocol.dll Exception - IEASPrtCmd3 - ParseData Fail. 데이터가 없습니다.");
            int index = 0;
            int authenticodeLength = dataLength;
            //인증 코드
            byte[] byAuthenticode = new byte[authenticodeLength];
            Array.Clear(byAuthenticode, 0, byAuthenticode.Length);
            Buffer.BlockCopy(Data, index, byAuthenticode, 0, authenticodeLength);
            AuthentiCode = Encoding.Default.GetString(byAuthenticode);
            index += authenticodeLength;
        }
    }
}
