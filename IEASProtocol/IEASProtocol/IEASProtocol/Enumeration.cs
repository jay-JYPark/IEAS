using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEASProtocol
{
    /// <summary>
    /// 명령의 최초 전송 시스템 종류
    /// </summary>
    public enum IEASSenderType
    {
        /// <summary>
        /// 없음
        /// </summary>
        None,
        /// <summary>
        /// 표준발령대(Standard Warning Installation)
        /// </summary>
        SWI,
        /// <summary>
        /// 통합게이트웨이(Interface from Installation To integrated alerting Gateway)
        /// </summary>
        IITG,
        /// <summary>
        /// 표준경보시스템(Standard Warning System)
        /// </summary>
        SAS
    }
}
