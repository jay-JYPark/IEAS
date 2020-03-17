using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEASProtocol
{
    /// <summary>
    /// 발령
    /// 발령에 해당하는 CAP 메시지를 데이터로 가지는 명령
    /// </summary>
    public class IEASPrtCmd1 : CAPMSGBase
    {
        #region Fields
        #endregion
        #region Properties
        #endregion
        /// <summary>
        /// IEASPrtCmd1 의 생성자. 커맨드를 0x01 로 초기화
        /// </summary>
        public IEASPrtCmd1()
        {
            this.CMD = 0x01;
        }
    }
}
