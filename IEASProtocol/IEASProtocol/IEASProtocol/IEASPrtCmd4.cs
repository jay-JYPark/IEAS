using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEASProtocol
{
    /// <summary>
    /// 접속 인증 결과
    /// 접속 인증 요청에 대한 결과를 전송하기 위해 사용되는 명령. 인증 코드 검증하여 접속 승인/거부 한 내용이 담겨있음.
    /// </summary>
    public class IEASPrtCmd4 : IEASProtocolBase
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        protected byte authentiResult = 0x00;
        #endregion
        #region Properties
        /// <summary>
        /// 접속 인증 결과 ( 0x00 : 접속거부, 0x01 : 접속승인 )
        /// </summary>
        public byte AuthentiResult
        {
            get { return authentiResult; }
            set { authentiResult = value; }
        }
        #endregion
        /// <summary>
        /// IEASPrtCmd4 의 생성자. 커맨드를 0x04 로 초기화
        /// </summary>
        public IEASPrtCmd4()
        {
            this.CMD = 0x04;
        }
        /// <summary>
        /// 
        /// </summary>
        internal override void MakeData()
        {
            DataLength = 1;
            data = new byte[DataLength];
            Array.Clear(data, 0, data.Length);
            #region 데이터 부 셋팅
            int index = 0;
            //접속 승인/거부 내용
            Data[index++] = AuthentiResult;
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        internal override void ParseData()
        {
            if (Data == null || DataLength <= 0)
                throw new Exception("IEASProtocol.dll Exception - IEASPrtCmd4 - ParseData Fail. 데이터가 없습니다.");
            int index = 0;
            //접속 승인/거부 내용
            AuthentiResult = Data[index++];
        }
    }
}
