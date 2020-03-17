using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEASProtocol
{
    /// <summary>
    /// IEASProtocol 모듈의 Utility 클래스
    /// </summary>
    internal static class IEASProtocolUtil
    {
        #region Fields
        #endregion
        #region Properties
        #endregion
        /// <summary>
        /// IEASSenderType 을 Byte 형식으로 변환
        /// </summary>
        /// <param name="type">IEASSenderType</param>
        /// <returns>byte 형식의 값</returns>
        public static byte ConvertSenderTypeToByte(IEASSenderType type)
        {
            byte value = 0x00;
            switch (type)
            {
                case IEASSenderType.SWI:
                    value = 0x01;
                    break;
                case IEASSenderType.IITG:
                    value = 0x02;
                    break;
                case IEASSenderType.SAS:
                    value = 0x03;
                    break;
                default:
                    value = 0x00;
                    break;
            }
            return value;
        }
        /// <summary>
        /// byte 값을 IEASSenderType 형식으로 변환
        /// </summary>
        /// <param name="value">byte 형식의 값</param>
        /// <returns>IEASSenderType</returns>
        public static IEASSenderType ConvertByteToSenderType(byte value)
        {
            IEASSenderType type = IEASSenderType.None;
            switch (value)
            {
                case 0x00:
                    type = IEASSenderType.None;
                    break;
                case 0x01:
                    type = IEASSenderType.SWI;
                    break;
                case 0x02:
                    type = IEASSenderType.IITG;
                    break;
                case 0x03:
                    type = IEASSenderType.SAS;
                    break;
                default:
                    type = IEASSenderType.None;
                    break;
            }
            return type;
        }
        public static byte[] MakeCRC16(byte[] frame)
        {
            byte[] crc16 = new byte[2];
            Array.Clear(crc16, 0, crc16.Length);
            //CRC 생성 처리하기
            return crc16;
        }

        public static bool CheckCRC16(byte[] frame, byte[] crc16)
        {
            //CRC 16 체크 처리
            return true;
        }
    }
}
