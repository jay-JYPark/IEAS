using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using IEASProtocol;

namespace IEASProtocolSample
{
    public partial class Form1 : Form
    {
        #region Fields
        //발령 CAP 메시지 샘플 파일 명
        private string alertCAPMessageSampleFileName = "AlertCAPMessageSample.xml";
        //응답 CAP 메시지 샘플 파일 명
        private string ackCAPMessageSampleFileName = "AckCAPMessageSample.xml";
        //폴링 Thread 동작 제어용 Flag
        private bool pollingThreadFlag = false;
        //폴링 Thread 에서 사용될 ManualResetEvent
        private ManualResetEvent pollingThreadEvent = null;
        //폴링 Thread 핸들
        private Thread pollingThread = null;
        //파싱 Thread 동작 제어용 Flag
        private bool parsingThreadFlag = false;
        //파싱 Thread 에서 사용될 ManualResetEvent
        private ManualResetEvent parsingThreadEvent = null;
        //파싱 Thread 핸들
        private Thread parsingThread = null;
        //수신 데이터 Queue
        private Queue<byte[]> receiveDataQueue = null;
        //기억 버퍼
        private byte[] remainBuffer = new byte[0];
        #endregion
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            receiveDataQueue = new Queue<byte[]>();
            SessionManager.GetInstance().onConnect += new SessionManager.OnConnect(OnConnect);
            SessionManager.GetInstance().onError += new SessionManager.SocketError(OnError);
            SessionManager.GetInstance().onReceive += new SessionManager.OnReceive(OnReceive);
            SessionManager.GetInstance().onClose += new SessionManager.OnClose(OnClose);
        }      

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SessionManager.GetInstance().onConnect -= new SessionManager.OnConnect(OnConnect);
            SessionManager.GetInstance().onError -= new SessionManager.SocketError(OnError);
            SessionManager.GetInstance().onReceive -= new SessionManager.OnReceive(OnReceive);
            SessionManager.GetInstance().onClose -= new SessionManager.OnClose(OnClose);
            receiveDataQueue.Clear();
            receiveDataQueue = null;
        }
        #region 버튼 클릭
        /// <summary>
        /// [연결] 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string gatewayIP = gatewayIPTextBox.Text;
                string gatewayPort = gatewayPortTextBox.Text;
                string authentiCode = authentiCodeTextBox.Text;
                if (string.IsNullOrEmpty(gatewayIP) || string.IsNullOrEmpty(gatewayPort) || string.IsNullOrEmpty(authentiCode))
                    MessageBox.Show("연결 정보를 입력해 주세요");
                IPAddress gatewayAddress = null;
                if (IPAddress.TryParse(gatewayIP, out gatewayAddress))
                {
                    if (SessionManager.GetInstance().Connect(gatewayIP, Convert.ToInt32(gatewayPort)))
                        ListItemAdd("통합게이트웨이 연결 시도 - IP : " + gatewayIP.ToString() + " , Port : " + gatewayPort.ToString());
                }
                else
                    MessageBox.Show("유효한 IP 주소가 아닙니다.");
            }
            catch (Exception ex)
            {
                ListItemAdd("통합게이트웨이 연결 실패 - " + ex.Message);
                gatewayIPTextBox.Enabled = true;
                gatewayPortTextBox.Enabled = true;
                authentiCodeTextBox.Enabled = true;
                connectBtn.Enabled = true;
                disconnectBtn.Enabled = false;
            }
        }
        /// <summary>
        /// [연결 종료] 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                SessionManager.GetInstance().Close();
            }
            catch (Exception ex)
            {
                ListItemAdd("통합게이트웨이 연결 종료 실패 - " + ex.Message);
            }
        }
        /// <summary>
        /// [발령 CAP 시험] 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void alertCAPTestBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //발령 응답 명령 클래스 생성
                IEASProtocolBase protocolBase = IEASProtocolManager.CreateProtocol(0x01);
                //전송 시스템 종류 셋팅
                protocolBase.SenderType = IEASSenderType.SWI;
                //명령 코드에 해당하는 클래스로 캐스팅
                IEASPrtCmd1 prt1 = protocolBase as IEASPrtCmd1;
                //응답 CAP 메시지 셋팅
                string path = System.Environment.CurrentDirectory + @"\" + alertCAPMessageSampleFileName;
                prt1.CAPMessage = File.ReadAllText(path);
                //byte[] 형식의 frame 으로 변환
                byte[] frame = IEASProtocolManager.MakeFrame(prt1);
                SessionManager.GetInstance().Send(frame);
                //발령 Frame 수신처리
                //OnReceive(SessionManager.GetInstance(), frame);
            }
            catch (Exception ex)
            {
                ListItemAdd("발령 CAP 메시지 시험 실패 - " + ex.Message);
            }
        }
        /// <summary>
        /// [리스트 Clear] 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listClearBtn_Click(object sender, EventArgs e)
        {
            eventListView.Items.Clear();
        }
        #endregion
        #region Event Method
        /// <summary>
        /// SessionManager 연결 성공 이벤트
        /// </summary>
        /// <param name="sender"></param>
        private void OnConnect(object sender)
        {
            ListItemAdd("통합게이트웨이 연결 성공");
            gatewayIPTextBox.Invoke(new MethodInvoker(delegate() { gatewayIPTextBox.Enabled = false; }));
            gatewayPortTextBox.Invoke(new MethodInvoker(delegate() { gatewayPortTextBox.Enabled = false; ; }));
            authentiCodeTextBox.Invoke(new MethodInvoker(delegate() { authentiCodeTextBox.Enabled = false; }));
            connectBtn.Invoke(new MethodInvoker(delegate() { connectBtn.Enabled = false; }));
            disconnectBtn.Invoke(new MethodInvoker(delegate() { disconnectBtn.Enabled = true; }));
            CreateParsingThread();
            #region 접속 인증 요청
            //접속 인증 요청 명령 클래스 생성
            IEASProtocolBase protocolBase = IEASProtocolManager.CreateProtocol(0x03);
            //전송 시스템 종류 셋팅
            protocolBase.SenderType = IEASSenderType.SAS;
            //명령 코드에 해당하는 클래스로 캐스팅
            IEASPrtCmd3 prt3 = protocolBase as IEASPrtCmd3;
            //인증 코드 셋팅(32자)
            string authentiCode = authentiCodeTextBox.Text;
            prt3.AuthentiCode = authentiCode;
            //byte[] 형식의 frame 으로 변환
            byte[] sendData = IEASProtocolManager.MakeFrame(prt3);
            //통합게이트웨이로 전송
            if (SessionManager.GetInstance().Send(sendData))
                ListItemAdd("접속 인증 요청 전송");
            else
                ListItemAdd("접속 인증 요청 전송 실패");
            #endregion
        }
        /// <summary>
        /// SessionManager Error 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="errorMsg"></param>
        private void OnError(object sender, string errorMsg)
        {
            SessionManager.GetInstance().Close();
            ListItemAdd("TCP Error - " + errorMsg);
        }
        /// <summary>
        /// SessionManager 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void OnReceive(object sender, byte[] data)
        {
            lock (receiveDataQueue)
            {
                if (receiveDataQueue != null)
                    receiveDataQueue.Enqueue(data);
            }
        }
        /// <summary>
        /// SessionManager 연결 종료 이벤트
        /// </summary>
        /// <param name="sender"></param>
        private void OnClose(object sender)
        {
            ListItemAdd("TCP 연결 종료");
            DisposePollingThread();
            DisposeParsingThread();
            gatewayIPTextBox.Invoke(new MethodInvoker(delegate() { gatewayIPTextBox.Enabled = true; }));
            gatewayPortTextBox.Invoke(new MethodInvoker(delegate() { gatewayPortTextBox.Enabled = true; ; }));
            authentiCodeTextBox.Invoke(new MethodInvoker(delegate() { authentiCodeTextBox.Enabled = true; }));
            connectBtn.Invoke(new MethodInvoker(delegate() { connectBtn.Enabled = true; }));
            disconnectBtn.Invoke(new MethodInvoker(delegate() { disconnectBtn.Enabled = false; }));
        }
        #endregion

        #region Thread 관련
        #region 폴링 Thread
        private bool CreatePollingThread()
        {
            try
            {
                if (pollingThread == null)
                {
                    pollingThreadFlag = true;
                    pollingThread = new Thread(new ThreadStart(PollingThread));
                    pollingThread.IsBackground = true;
                    pollingThread.Start();
                    ListItemAdd("폴링 Thread 생성");
                }
                else
                    throw new Exception("Thread 가 이미 생성되어 있습니다.");
            }
            catch (Exception ex)
            {
                ListItemAdd("폴링 Thread 생성 실패 - " + ex.Message);
                return false;
            }
            return true;
        }
        private bool DisposePollingThread()
        {
            try
            {
                if (pollingThread != null)
                {
                    pollingThreadFlag = false;
                    pollingThreadEvent.Set();
                    pollingThread = null;
                    ListItemAdd("폴링 Thread 종료");
                }
                else
                    throw new Exception("Thread 가 생성되어 있지 않습니다.");
            }
            catch (Exception ex)
            {
                ListItemAdd("폴링 Thread 종료 실패 - " + ex.Message);
                return false;
            }
            return true;
        }
        private void PollingThread()
        {
            pollingThreadEvent = new ManualResetEvent(false);
            while (pollingThreadFlag)
            {
                try
                {
                    //폴링 명령 클래스 생성
                    IEASProtocolBase protocolBase = IEASProtocolManager.CreateProtocol(0xFF);
                    //전송 시스템 종류 셋팅
                    protocolBase.SenderType = IEASSenderType.SAS;
                    //byte[] 형식의 frame 으로 변환
                    byte[] sendData = IEASProtocolManager.MakeFrame(protocolBase);
                    //통합게이트웨이로 전송
                    if (SessionManager.GetInstance().Send(sendData))
                        ListItemAdd("폴링 데이터 전송");
                    else
                        ListItemAdd("폴링 데이터 전송 실패");
                    pollingThreadEvent.WaitOne(5000);
                    pollingThreadEvent.Reset();
                }
                catch (Exception ex)
                {
                    ListItemAdd("PollingThread Exception - " + ex.Message);
                }
            }
            pollingThreadEvent.Dispose();
            pollingThreadEvent = null;
        }
        #endregion
        #region 파싱 Thread
        private bool CreateParsingThread()
        {
            try
            {
                if (parsingThread == null)
                {
                    parsingThreadFlag = true;
                    parsingThread = new Thread(new ThreadStart(ParsingThread));
                    parsingThread.IsBackground = true;
                    parsingThread.Start();
                    ListItemAdd("파싱 Thread 생성");
                }
                else
                    throw new Exception("Thread 가 이미 생성되어 있습니다.");
            }
            catch (Exception ex)
            {
                ListItemAdd("파싱 Thread 생성 실패 - " + ex.Message);
                return false;
            }
            return true;
        }
        private bool DisposeParsingThread()
        {
            try
            {
                if (parsingThread != null)
                {
                    parsingThreadFlag = false;
                    parsingThreadEvent.Set();
                    parsingThread = null;
                    ListItemAdd("파싱 Thread 종료");
                }
                else
                    throw new Exception("Thread 가 생성되어 있지 않습니다.");
            }
            catch (Exception ex)
            {
                ListItemAdd("파싱 Thread 종료 실패 - " + ex.Message);
                return false;
            }
            return true;
        }
        private void ParsingThread()
        {
            parsingThreadEvent = new ManualResetEvent(false);
            while (parsingThreadFlag)
            {
                try
                {
                    int queueCount = 0;
                    lock (receiveDataQueue)
                        queueCount = receiveDataQueue.Count;
                    if (queueCount > 0)
                    {
                        byte[] receiveData = null;
                        lock (receiveDataQueue)
                            receiveData = receiveDataQueue.Dequeue();
                        if (receiveData != null)
                        {
                            byte[] totalBuffer = new byte[remainBuffer.Length + receiveData.Length];
                            Array.Clear(totalBuffer, 0, totalBuffer.Length);
                            int index = 0;
                            Buffer.BlockCopy(remainBuffer, 0, totalBuffer, 0, remainBuffer.Length);
                            index += remainBuffer.Length;
                            Buffer.BlockCopy(receiveData, 0, totalBuffer, index, receiveData.Length);
                            index += receiveData.Length;
                            if (totalBuffer.Length > 6)
                            {
                                for (int i = 0; i < totalBuffer.Length; i++)
                                {
                                    if (totalBuffer[i] == Convert.ToByte('K'))
                                    {
                                        if (i + 5 < totalBuffer.Length)
                                        {
                                            if (totalBuffer[i + 1] == Convert.ToByte('C') && totalBuffer[i + 2] == Convert.ToByte('A') && totalBuffer[i + 3] == Convert.ToByte('P'))
                                            {
                                                byte[] byDataLen = new byte[2];
                                                Array.Clear(byDataLen, 0, 2);
                                                byDataLen[0] = totalBuffer[i + 4];
                                                byDataLen[1] = totalBuffer[i + 5];
                                                int dataLen = BitConverter.ToInt16(byDataLen, 0);
                                                if (i + 9 + dataLen < totalBuffer.Length)
                                                {
                                                    byte[] frame = new byte[dataLen + 10];
                                                    Array.Clear(frame, 0, frame.Length);
                                                    Buffer.BlockCopy(totalBuffer, i, frame, 0, frame.Length);
                                                    i += frame.Length - 1;
                                                    try
                                                    {
                                                        IEASProtocolBase prtBase = IEASProtocolManager.ParseFrame(frame);
                                                        ProtocolProcessing(prtBase);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ListItemAdd("파싱 Exception - " + ex.Message);
                                                    }
                                                }
                                                else
                                                {
                                                    remainBuffer = new byte[totalBuffer.Length - i - 1];
                                                    Array.Clear(remainBuffer, 0, remainBuffer.Length);
                                                    Buffer.BlockCopy(totalBuffer, i, remainBuffer, 0, remainBuffer.Length);
                                                    break;
                                                }
                                            }
                                            else
                                                continue;
                                        }
                                        else
                                        {
                                            remainBuffer = new byte[totalBuffer.Length - i - 1];
                                            Array.Clear(remainBuffer, 0, remainBuffer.Length);
                                            Buffer.BlockCopy(totalBuffer, i, remainBuffer, 0, remainBuffer.Length);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (i + 6 < totalBuffer.Length)
                                            continue;
                                        else
                                        {
                                            remainBuffer = new byte[totalBuffer.Length - i - 2];
                                            Array.Clear(remainBuffer, 0, remainBuffer.Length);
                                            Buffer.BlockCopy(totalBuffer, i + 1, remainBuffer, 0, remainBuffer.Length);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                remainBuffer = new byte[totalBuffer.Length];
                                Array.Clear(remainBuffer, 0, remainBuffer.Length);
                                Buffer.BlockCopy(totalBuffer, 0, remainBuffer, 0, totalBuffer.Length);
                                continue;
                            }
                        }
                    }
                    else
                    {
                        parsingThreadEvent.WaitOne(500);
                        parsingThreadEvent.Reset();
                    }
                }
                catch (Exception ex)
                {
                    ListItemAdd("ParsingThread Exception - " + ex.Message);
                }
            }
            parsingThreadEvent.Dispose();
            parsingThreadEvent = null;
        }
        #endregion
        #endregion
        /// <summary>
        /// 파싱된 프로토콜 처리
        /// </summary>
        /// <param name="prtBase"></param>
        private void ProtocolProcessing(IEASProtocolBase prtBase)
        {
            switch (prtBase.CMD)
            {
                case 0x01:
                    {
                        IEASPrtCmd1 prt1 = prtBase as IEASPrtCmd1;
                        ListItemAdd("발령 CAP 메시지 수신 - CAP 메시지 : " + prt1.CAPMessage);
                        MessageBox.Show(prt1.CAPMessage, "CAP 메시지 수신", MessageBoxButtons.OK);
                        #region 응답 CAP 메시지 전송
                        //발령 응답 명령 클래스 생성
                        IEASProtocolBase protocolBase = IEASProtocolManager.CreateProtocol(0x02);
                        //전송 시스템 종류 셋팅
                        protocolBase.SenderType = IEASSenderType.SAS;
                        //명령 코드에 해당하는 클래스로 캐스팅
                        IEASPrtCmd2 prt2 = protocolBase as IEASPrtCmd2;
                        //응답 CAP 메시지 셋팅
                        string path = System.Environment.CurrentDirectory + @"\" + ackCAPMessageSampleFileName;
                        prt2.CAPMessage = File.ReadAllText(path);
                        //byte[] 형식의 frame 으로 변환
                        byte[] sendData = IEASProtocolManager.MakeFrame(prt2);
                        //frame 전송
                        if(SessionManager.GetInstance().Send(sendData))
                            ListItemAdd("응답 CAP 메시지 전송");
                        else
                            ListItemAdd("응답 CAP 메시지 전송 실패");
                        #endregion
                    }
                    break;
                case 0x02:
                    {
                        IEASPrtCmd2 prt2 = prtBase as IEASPrtCmd2;
                        ListItemAdd("응답 CAP 메시지 수신 - CAP 메시지 : " + prt2.CAPMessage);
                    }
                    break;
                case 0x03:
                    {
                        IEASPrtCmd3 prt3 = prtBase as IEASPrtCmd3;
                        ListItemAdd("접속 인증 요청 명령 수신 - 인증코드 : " + prt3.AuthentiCode);
                    }
                    break;
                case 0x04:
                    {
                        IEASPrtCmd4 prt4 = prtBase as IEASPrtCmd4;
                        if (prt4.AuthentiResult == 1)
                        {
                            ListItemAdd("접속 인증 결과 수신 - 접속승인");
                            CreatePollingThread();
                        }
                        else
                            ListItemAdd("접속 인증 결과 수신 - 접속거부");
                        
                    }
                    break;
                case 0xFF:
                    {
                        ListItemAdd("폴링 데이터 수신");
                    }
                    break;
                default:
                    {
                        ListItemAdd("Parsing 결과 - 지원하지 않는 명령어 코드 입니다");
                    }
                    break;
            }
        }

        private void ListItemAdd(string message)
        {
            ListViewItem item = new ListViewItem();
            item.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
            item.SubItems.Add(message);
            eventListView.Invoke(new MethodInvoker(delegate() { eventListView.Items.Add(item); }));
        }

        #region TextBox KeyPress
        private void gatewayIPTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (gatewayIPTextBox.Text.Split('.').Count() > 3 && e.KeyChar == Convert.ToChar(Keys.Delete))
                e.Handled = true;
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(Keys.Delete)))
            {
                e.Handled = true;
            }
        }

        private void gatewayPortTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }
        #endregion         

        
    }
}
