using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEASProtocol
{
    /// <summary>
    /// IEASProtocol 의 인스턴스의 생성, 파싱, 언파싱 기능을 제공하는 매니져 클리스
    /// </summary>
    public class IEASProtocolManager
    {
        #region Fields
        #endregion
        #region Properties
        #endregion
        /// <summary>
        /// IEASProtocolManager 의 생성자
        /// </summary>
        public IEASProtocolManager()
        {
        }
        /// <summary>
        /// 인자로 수신된 명령어 코드에 해당하는 명령 클래스를 생성하여 반환
        /// </summary>
        /// <param name="cmd">명령어 코드</param>
        /// <returns>명령 클래스</returns>
        /// <example>
        /// <see cref="IEASProtocolBase">IEASProtocolBase</see><br/>
        /// <see cref="IEASPrtCmd1">IEASPrtCmd1</see>
        /// <code>
        /// using IEASProtocol;
        /// 
        /// public void MakeProtocol(string capMessage)
        /// {
        ///     //명령어 코드를 인자로 CreateProtocol 함수 호출
        ///     IEASProtocolBase protocolBase = IEASProtocolManager.CreateProtocol(0x01);
        ///     
        ///     //인스턴스를 사용할 프로토콜의 클래스로 캐스팅
        ///     IEASPrtCmd1 prt1 = protocolBase as IEASPrtCmd1;
        ///     
        ///     //사용(Ex:데이터 셋팅)
        ///     prt1.CAPMessage = capMessage;
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="IEASProtocolBase">IEASProtocolBase</seealso>
        /// <seealso cref="IEASPrtCmd1">IEASPrtCmd1</seealso>
        public static IEASProtocolBase CreateProtocol(byte cmd)
        {
            IEASProtocolBase prtBase = null;
            try
            {
                switch (cmd)
                {
                    #region 커맨드별 프로토콜 생성
                    case 0x01:
                        {
                            IEASPrtCmd1 prt = new IEASPrtCmd1();
                            prtBase = prt;
                        }
                        break;
                    case 0x02:
                        {
                            IEASPrtCmd2 prt = new IEASPrtCmd2();
                            prtBase = prt;
                        }
                        break;
                    case 0x03:
                        {
                            IEASPrtCmd3 prt = new IEASPrtCmd3();
                            prtBase = prt;
                        }
                        break;
                    case 0x04:
                        {
                            IEASPrtCmd4 prt = new IEASPrtCmd4();
                            prtBase = prt;
                        }
                        break;
                    case 0xFF:
                        {
                            IEASPrtCmdFF prt = new IEASPrtCmdFF();
                            prtBase = prt;
                        }
                        break;
                    #endregion
                    default:
                        throw new Exception("IEASProtocol.dll Exception - IEASProtocolManager - CreateProtocol Fail. 지원하지 않는 커맨드 입니다.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("IEASProtocol.dll Excption - IEASProtocolManager - CreateProtocol Fail. " + ex.Message);
            }
            return prtBase;
        }
        /// <summary>
        /// 명령 클래스를 byte[] 형식의 frame으로 반환
        /// </summary>
        /// <param name="prtBase">명령 클래스</param>
        /// <returns>byte[] 형식의 frame</returns>
        /// <example>
        /// <see cref="IEASProtocolBase">IEASProtocolBase</see><br/>
        /// <see cref="IEASPrtCmd1">IEASPrtCmd1</see>
        /// <code>
        /// using IEASProtocol;
        /// 
        /// public void MakeFrame(string capMessage)
        /// {
        ///     //명령어 코드를 인자로 CreateProtocol 함수 호출
        ///     IEASProtocolBase protocolBase = IEASProtocolManager.CreateProtocol(0x01);
        ///     
        ///     //인스턴스를 사용할 프로토콜의 클래스로 캐스팅
        ///     IEASPrtCmd1 prt1 = protocolBase as IEASPrtCmd1;
        ///     
        ///     //사용(Ex:데이터 셋팅)
        ///     prt1.CAPMessage = capMessage;
        ///     
        ///     byte[] frame = IEASProtocolManager.MakeFrame(prt1);
        /// }
        /// </code>
        /// </code>
        /// </example>
        /// <seealso cref="IEASProtocolBase">IEASProtocolBase</seealso>
        /// <seealso cref="IEASPrtCmd1">IEASPrtCmd1</seealso>
        public static byte[] MakeFrame(IEASProtocolBase prtBase)
        {
            if (prtBase == null)
                return null;
            try
            {
                prtBase.MakeData();
                int frameLen = 10 + prtBase.DataLength;
                byte[] frame = new byte[frameLen];
                Array.Clear(frame, 0, frame.Length);
                int index = 0;
                //Header1
                frame[index++] = prtBase.Header1;
                //Header2
                frame[index++] = prtBase.Header2;
                //Header3
                frame[index++] = prtBase.Header3;
                //Header4
                frame[index++] = prtBase.Header4;
                //Length
                Buffer.BlockCopy(BitConverter.GetBytes(prtBase.DataLength), 0, frame, index, 2);
                index += 2;
                //CMD
                frame[index++] = prtBase.CMD;
                //Sender Type
                frame[index++] = IEASProtocolUtil.ConvertSenderTypeToByte(prtBase.SenderType);
                //Data
                Buffer.BlockCopy(prtBase.Data, 0, frame, index, prtBase.DataLength);
                index += prtBase.DataLength;
                return frame;
            }
            catch (Exception ex)
            {
                throw new Exception("IEASProtocol.dll Exception - IEASProtocolManager - MakeFrame Faile. " + ex.Message);
            }
            
        }
        /// <summary>
        /// byte[] 형식의 frame을 명령 클래스 형식으로 반환
        /// </summary>
        /// <param name="frame">byte[] 형식의 frame</param>
        /// <returns>명령 클래스</returns>
        /// <example>
        /// <see cref="IEASProtocolBase">IEASProtocolBase</see><br/>
        /// <see cref="IEASPrtCmd1">IEASPrtCmd1</see><br/>
        /// <see cref="IEASPrtCmd2">IEASPrtCmd2</see><br/>
        /// <see cref="IEASPrtCmd3">IEASPrtCmd3</see><br/>
        /// <see cref="IEASPrtCmd4">IEASPrtCmd4</see><br/>
        /// <see cref="IEASPrtCmdFF">IEASPrtCmdFF</see>
        /// <code>
        /// using IEASProtocol;
        /// 
        /// public void ParseFrame(byte[] frame)
        /// {
        ///     //한 패킷의 frame 을 인자로 ParseFrame 함수 호출 
        ///     IEASProtocolBase protocolBase = IEASProtocolManager.ParseFrame(frame);
        ///     
        ///     //명령어 코드 확인 후 해당 클래스로 캐스팅하여 사용
        ///     switch (prtBase.CMD)
        ///     {
        ///         case 0x01:
        ///             IEASPrtCmd1 prt1 = protocolBase as IEASPrtCmd1;
        ///             break;
        ///         case 0x02:
        ///             IEASPrtCmd2 prt2 = protocolBase as IEASPrtCmd2;
        ///             break;
        ///         case 0x03:
        ///             IEASPrtCmd3 prt3 = protocolBase as IEASPrtCmd3;
        ///             break;
        ///         case 0x04:
        ///             IEASPrtCmd4 prt4 = protocolBase as IEASPrtCmd4;
        ///             break;
        ///         case 0xFF:
        ///             IEASPrtCmdFF prtFF = protocolBase as IEASPrtCmdFF;
        ///             break;
        ///         default:
        ///             break;
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="IEASProtocolBase">IEASProtocolBase</seealso>
        /// <seealso cref="IEASPrtCmd1">IEASPrtCmd1</seealso>
        /// <seealso cref="IEASPrtCmd2">IEASPrtCmd2</seealso>
        /// <seealso cref="IEASPrtCmd3">IEASPrtCmd3</seealso>
        /// <seealso cref="IEASPrtCmd4">IEASPrtCmd4</seealso>
        /// <seealso cref="IEASPrtCmdFF">IEASPrtCmdFF</seealso>
        public static IEASProtocolBase ParseFrame(byte[] frame)
        {
            if (frame == null || frame.Length < 10)
                throw new Exception("IEASProtocol.dll Exception - IEASProtocolManager - ParseFrame Fail. 데이터가 없거나 데이터의 길이가 충분하지 않습니다.");
            if (!(frame[0] == 'K' && frame[1] == 'C' && frame[2] == 'A' && frame[3] == 'P'))
                throw new Exception("IEASProtocol.dll Exception - IEASProtocolManager - ParseFrame Fail. 데이터가 올바르지 않습니다.");
            try
            {
                IEASProtocolBase prtBase = null;
                #region 프로토콜의 인스턴스 생성
                byte cmd = frame[6];
                switch (cmd)
                {
                    case 0x01:
                    case 0x02:
                    case 0x03:
                    case 0x04:
                    case 0xFF:
                        prtBase = CreateProtocol(cmd);
                        break;
                    default:
                        throw new Exception("IEASProtocol.dll Exception - IEASProtocolManager - ParseFrame Fail. 지원하지 않는 커맨드 입니다.");
                }
                if (prtBase == null)
                    return null;
                #endregion
                //데이터 길이
                byte[] byDataLen = new byte[2];
                Array.Clear(byDataLen, 0, 2);
                byDataLen[0] = frame[4];
                byDataLen[1] = frame[5];
                int dataLen = BitConverter.ToUInt16(byDataLen, 0);
                //Sender Type
                prtBase.SenderType = IEASProtocolUtil.ConvertByteToSenderType(frame[7]);
                //Data
                byte[] dataTemp = new byte[dataLen];
                Array.Clear(dataTemp, 0, dataTemp.Length);
                Buffer.BlockCopy(frame, 8, dataTemp, 0, dataLen);
                prtBase.Data = dataTemp;
                prtBase.ParseData();
                //Reserved
                byte[] reserved = new byte[2];
                Array.Clear(reserved, 0, reserved.Length);
                Buffer.BlockCopy(frame, 8 + dataLen, reserved, 0, reserved.Length);
                return prtBase;
            }
            catch (Exception ex)
            {
                throw new Exception("IEASProtocol.dll Exception - IEASProtocolManager - ParseFrame Fail. " + ex.Message);
            }
        }
    }
}
