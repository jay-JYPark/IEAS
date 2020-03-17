using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEASProtocol
{
    /// <summary>
    /// 폴링
    /// Alive 상태 체크를 위한 명령
    /// 폴링에 대한 응답은 없으며, 이 명령을 수신한 시스템은 해당 명령에 대한 처리는 하지 않는다.
    /// </summary>
    public class IEASPrtCmdFF : IEASProtocolBase
    {
        #region Fields
        #endregion
        #region Properties
        #endregion
        /// <summary>
        /// IEASPrtCmdFF 의 생성자. 커맨드를 0xFF 로 초기화
        /// </summary>
        public IEASPrtCmdFF()
        {
            this.CMD = 0xFF;
        }
    }
}
