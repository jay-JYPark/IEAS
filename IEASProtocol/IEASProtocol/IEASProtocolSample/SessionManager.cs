using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace IEASProtocolSample
{
    public class SessionManager
    {
        #region Fields
        private static SessionManager sessionMng = null;
        //통합게이트웨이에 연결될 소켓
        private Socket soc = null;
        //수신 버퍼 사이즈
        private const int BUFFER_SIZE = 4096;
        //수신 버퍼
        private byte[] buffer = new byte[BUFFER_SIZE];
        #endregion
        #region Properties
        #endregion
        #region Delegate
        public delegate void OnConnect(object sender);
        public delegate void SocketError(object sender, string errorMsg);
        public delegate void OnReceive(object sender, byte[] data);
        public delegate void OnSend(object sender, int sendBytesLength);
        public delegate void OnClose(object sender);
        #endregion
        #region Event
        public event OnConnect onConnect = null;
        public event SocketError onError = null;
        public event OnReceive onReceive = null;
        public event OnSend onSend = null;
        public event OnClose onClose = null;
        #endregion

        private SessionManager()
        {
        }

        /// <summary>
        /// SessionManager 의 Instance 를 반환
        /// </summary>
        /// <returns></returns>
        public static SessionManager GetInstance()
        {
            if (sessionMng == null)
                sessionMng = new SessionManager();
            return sessionMng;
        }
        public bool GetConnectStatus()
        {
            if (soc != null && soc.Connected)
                return true;
            else
                return false;
        }
        /// <summary>
        /// TCP 연결
        /// </summary>
        /// <returns></returns>
        public bool Connect(string ip, int port)
        {
            try
            {
                IPAddress[] ipAddr = Dns.GetHostAddresses(ip);
                IPEndPoint endPoint = new IPEndPoint(ipAddr[0], port);
                soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                soc.BeginConnect(endPoint, new AsyncCallback(OnConnectCallBack), soc);
            }
            catch (Exception ex)
            {
                if (onError != null)
                    onError(sessionMng, "Connect Exception - " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// TCP 연결 종료
        /// </summary>
        public void Close()
        {
            try
            {
                if (soc.Connected)
                {
                    soc.Shutdown(SocketShutdown.Both);
                    soc.BeginDisconnect(false, new AsyncCallback(OnCloseCallBack), soc);
                }
                else
                {
                    if (onClose != null)
                        onClose(soc);
                }
            }
            catch (Exception ex)
            {
                if (onError != null)
                    onError(sessionMng, "Close Exception - " + ex.Message);
            }
        }

        /// <summary>
        /// 데이터 수신
        /// </summary>
        private void Receive()
        {
            try
            {
                Array.Clear(buffer, 0, buffer.Length);
                soc.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(OnReceiveCallBack), soc);
            }
            catch (Exception ex)
            {
                if (onError != null)
                    onError(sessionMng, "Receive Exception - " + ex.Message);
            }
        }

        /// <summary>
        /// 데이터 전송
        /// </summary>
        /// <param name="data">전송할 데이터</param>
        /// <returns></returns>
        public bool Send(byte[] data)
        {
            try
            {
                if (soc == null || !soc.Connected)
                    throw new Exception("소켓이 연결되어 있지 않습니다.");
                soc.BeginSend(data, 0, data.Length, 0, new AsyncCallback(OnSendCallBack), soc);
            }
            catch (Exception ex)
            {
                if (onError != null)
                    onError(sessionMng, "Send Exception - " + ex.Message);
                return false;
            }
            return true;
        }
        #region CallBack
        /// <summary>
        /// 연결 Callback 
        /// </summary>
        /// <param name="iar"></param>
        private void OnConnectCallBack(IAsyncResult iar)
        {
            try
            {
                Socket s = iar.AsyncState as Socket;
                if (s == null)
                    throw new Exception("OnConnectCallBack Exception - Client is null.");
                s.EndConnect(iar);
                if (s.Connected)
                {
                    Receive();
                    if (onConnect != null)
                        onConnect(sessionMng);
                }
            }
            catch (Exception ex)
            {
                if (onError != null)
                    onError(sessionMng, ex.Message);
            }
        }
        /// <summary>
        /// 데이터 수신 CallBack
        /// </summary>
        /// <param name="iar"></param>
        private void OnReceiveCallBack(IAsyncResult iar)
        {
            try
            {
                Socket s = iar.AsyncState as Socket;
                if (s == null)
                    throw new Exception("OnReceiveCallBack Exception - Client is null.");
                int bytesRead = 0;
                bytesRead = s.EndReceive(iar);
                if (bytesRead <= 0)
                    Close();
                byte[] receiveData = new byte[bytesRead];
                Array.Clear(receiveData, 0, receiveData.Length);
                Buffer.BlockCopy(buffer, 0, receiveData, 0, bytesRead);
                if (onReceive != null)
                    onReceive(sessionMng, receiveData);
                this.Receive();
            }
            catch (Exception ex)
            {
                if (onError != null)
                    onError(sessionMng, ex.Message);
            }
        }
        /// <summary>
        /// 데이터 전송 CallBack
        /// </summary>
        /// <param name="iar"></param>
        private void OnSendCallBack(IAsyncResult iar)
        {
            try
            {
                Socket s = iar.AsyncState as Socket;
                if (s == null)
                    throw new Exception("OnSendCallBack Exception - Client is null.");
                int bytesWritten = s.EndSend(iar);
                if (onSend != null)
                    onSend(sessionMng, bytesWritten);
            }
            catch (Exception ex)
            {
                if (onError != null)
                    onError(sessionMng, ex.Message);
            }
        }
        /// <summary>
        /// 소켓 연결 종료 CallBack
        /// </summary>
        /// <param name="iar"></param>
        private void OnCloseCallBack(IAsyncResult iar)
        {
            try
            {
                Socket s = (Socket)iar.AsyncState;
                if (s == null)
                    throw new Exception("OnCloseCallBack Exception - Client is null.");

                s.EndDisconnect(iar);

                if (onClose != null)
                    onClose(sessionMng);
            }
            catch (Exception ex)
            {
                if (onError != null)
                    onError(sessionMng, ex.Message);
            }
        }
        #endregion
    }
}
