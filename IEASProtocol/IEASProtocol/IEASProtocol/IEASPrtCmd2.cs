using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEASProtocol
{
    /// <summary>
    /// 발령에 대한 응답
    /// 발령 명령에 대한 응답(Ack) 또는 에러(Error) 에 해당하는 CAP 메시지를 데이터로 가지는 명령
    /// </summary>
    public class IEASPrtCmd2 : CAPMSGBase
    {
        #region Fields
        #endregion
        #region Properties
        #endregion
        /// <summary>
        /// IEASPrtCmd2 의 생성자. 커맨드를 0x02 로 초기화
        /// </summary>
        public IEASPrtCmd2()
        {
            this.CMD = 0x02;
        }
    }
}
