using System;
using System.IO;
using System.Text;
using System.Messaging;
using System.Configuration;
using System.Collections.Generic;

using Jil;
using RY.H3Hybrid.MQ.Enums;
using RY.H3Hybrid.MQ.MsgModels;

namespace RY.H3Hybrid.MQ.Classes
{
    /// <summary>
    /// 使用微軟的 Message Queue
    /// </summary>
    public class MSMQClient : MQClientBase
    {
        private string queuePathSendRemote_Member = string.Empty;
        private string queuePathSendRemote_Agent = string.Empty;
        private string queuePathSendRemote_Manager = string.Empty;
        private string queuePathSendRemote_PhysicalHost = string.Empty;
        private string queuePathSendRemote_Custom = string.Empty;

        private string queuePathMulticast = string.Empty;
        private string queuePathReceiveLocal = string.Empty; // 非總控端的Queue路徑
        private string queuePathReceiveLocal_Manager = string.Empty; // 總控端的Queue路徑
        private string queuePathReceiveLocal_PhysicalHost = string.Empty; // 實體機的Queue路徑

        private MessageQueue queueReceiveLocal;// 接收本機的
        private MessageQueue queueMulticast;   // 傳送到本機的 多點傳送
        private MessageQueue queueSendRemote_Member;  // 傳送到遠端主機(會員端)
        private MessageQueue queueSendRemote_Agent;  // 傳送到遠端主機(通路端)
        private MessageQueue queueSendRemote_Manager;  // 傳送到遠端主機(總控端)
        private MessageQueue queueSendRemote_PhysicalHost;  // 傳送到遠端的實體機
        private MessageQueue queueSendRemote_Custom;  // 傳送到遠端的不固定主機
        private Message msmqMessage;
        private Message msgToSend;
        private bool isMulticast = false;

        private Dictionary<MessageQueue, string> dicQueueAndPath;
        private Object thisLock = new Object();

        public MSMQClient(bool multicast = false)
        {
            this.isMulticast = multicast;
        }

        protected override void Init()
        {
            // 路徑格式會是類似以下的值
            // FormatName:DIRECT=TCP:210.66.176.225\private$\Agent_THSiteA
            // {0}: Message Queue 主機的 IP
            // {1}: 目的地端口種類 (Member,Agent,Manager等)
            // {2}: 目的地站台名稱 (Carsar,Bestwin等)
            this.queuePathSendRemote_Member = @"FormatName:DIRECT=TCP:{0}\private$\{1}_{2}";
            this.queuePathSendRemote_Agent = @"FormatName:DIRECT=TCP:{0}\private$\{1}_{2}";
            this.queuePathSendRemote_Manager = @"FormatName:DIRECT=TCP:{0}\private$\{1}";
            this.queuePathSendRemote_PhysicalHost = @"FormatName:DIRECT=TCP:{0}\private$\{1}";
            this.queuePathSendRemote_Custom = @"FormatName:DIRECT=TCP:{0}\private$\{1}";

            this.queuePathMulticast = @"FormatName:multicast={0}";
            this.queuePathReceiveLocal = @".\Private$\{0}_{1}";
            this.queuePathReceiveLocal_PhysicalHost = @".\Private$\{0}";
            this.queuePathReceiveLocal_Manager = @".\Private$\Manager";

            this.sendRemoteIP_Member = ConfigurationManager.AppSettings["MQ.SendRemote.Member"] != null ? ConfigurationManager.AppSettings["MQ.SendRemote.Member"].ToString() : string.Empty;
            this.sendRemoteIP_Agent = ConfigurationManager.AppSettings["MQ.SendRemote.Agent"] != null ? ConfigurationManager.AppSettings["MQ.SendRemote.Agent"].ToString() : string.Empty;
            this.sendRemoteIP_Manager = ConfigurationManager.AppSettings["MQ.SendRemote.Manager"] != null ? ConfigurationManager.AppSettings["MQ.SendRemote.Manager"].ToString() : string.Empty;
            this.sendRemoteIP_PhysicalHost = ConfigurationManager.AppSettings["MQ.SendRemote.PhysicalHost"] != null ? ConfigurationManager.AppSettings["MQ.SendRemote.PhysicalHost"].ToString() : string.Empty;
            this.multicastIP = ConfigurationManager.AppSettings["MQ.Multicast.TargetIP"] != null ? ConfigurationManager.AppSettings["MQ.Multicast.TargetIP"].ToString() : string.Empty;

            if (this.queueSendRemote_Member == null)
            {
                this.queueSendRemote_Member = new MessageQueue();
                this.queueSendRemote_Member.Formatter = new BinaryMessageFormatter();
            }

            if (this.queueSendRemote_Agent == null)
            {
                this.queueSendRemote_Agent = new MessageQueue();
                this.queueSendRemote_Agent.Formatter = new BinaryMessageFormatter();
            }

            if (this.queueSendRemote_Manager == null)
            {
                this.queueSendRemote_Manager = new MessageQueue();
                this.queueSendRemote_Manager.Formatter = new BinaryMessageFormatter();
            }

            if (this.queueSendRemote_PhysicalHost == null)
            {
                this.queueSendRemote_PhysicalHost = new MessageQueue();
                this.queueSendRemote_PhysicalHost.Formatter = new BinaryMessageFormatter();
            }

            if (this.queueSendRemote_Custom == null)
            {
                this.queueSendRemote_Custom = new MessageQueue();
                this.queueSendRemote_Custom.Formatter = new BinaryMessageFormatter();
            }

            if (this.dicQueueAndPath == null)
            {
                this.dicQueueAndPath = new Dictionary<MessageQueue, string>();
            }

            if (this.queueMulticast == null)
            {
                this.queueMulticast = new MessageQueue();
                this.queueMulticast.Formatter = new BinaryMessageFormatter();
            }

            this.InitQueueReceiveLocal();

            if (this.msgToSend == null)
            {
                // 設定 Message 物件的相關屬性
                this.msgToSend = new Message();
                this.msgToSend.Formatter = new BinaryMessageFormatter();
                this.msgToSend.AttachSenderId = false;
                this.msgToSend.Recoverable = false;
            }
        }

        private void InitQueueReceiveLocal()
        {
            if (this.queueReceiveLocal == null)
            {
                this.queueReceiveLocal = new MessageQueue();
                this.queueReceiveLocal.Formatter = new BinaryMessageFormatter();
                this.queueReceiveLocal.ReceiveCompleted += MsgReceiveCompleted;
            }
        }

        private void MsgReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            if (this.queueReceiveLocal != null)
            {
                this.msmqMessage = this.queueReceiveLocal.EndReceive(e.AsyncResult);
                MQMsgModel msg = this.msmqMessage.Body as MQMsgModel;

                // 根據接收的資料類型,決定要觸發對應的處理程序
                if (msg == null)
                {
                    msg = JSON.Deserialize<MQMsgModel>((string)this.msmqMessage.Body);
                    msg.ArrivedTime = DateTime.Now;

                    this.TriggerMsgReceived(JSON.SerializeDynamic(msg));
                }
                else
                {
                    msg.ArrivedTime = DateTime.Now;
                    this.TriggerMsgReceived_Model(msg);
                }

                if (!this.stopReceiving)
                {
                    this.queueReceiveLocal.BeginReceive();
                }
            }
        }

        internal override void Send(ApplicationPort fromPort, ApplicationPort toPort, string toSiteName, string jsonMsg, bool multicast = false)
        {
            //this.isMulticast = multicast;
            //this.msgToSend.Body = jsonMsg;
            this.isMulticast = multicast;

            MQMsgModel msg = JSON.Deserialize<MQMsgModel>(jsonMsg);
            msg.SentTime = DateTime.Now;

            this.msgToSend.Body = JSON.SerializeDynamic(msg);

            if (this.isMulticast == false)
            {
                // 2018/08/16, 避免 foreach 在多執行緒可能發生的問題, 加上 lock
                lock (thisLock)
                {
                    this.GetTargetPorts(toPort, toSiteName);

                    foreach (KeyValuePair<MessageQueue, string> item in this.dicQueueAndPath)
                    {
                        // 發送之前檢查下是否有設定 IP,因為是內部主機,所以直接寫死192.168
                        if (item.Value.IndexOf("192.168") > -1)
                        {
                            item.Key.Path = item.Value;
                            item.Key.Send(this.msgToSend);
                        }
                        else
                        {
                            throw new Exception("MQ 主機的相關 IP 未設定,請確認");
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.multicastIP))
                {
                    // 改用 Message 物件來傳送
                    this.queueMulticast.Path = string.Format(this.queuePathMulticast, this.multicastIP);
                    this.queueMulticast.Send(this.msgToSend);
                }
                else
                {
                    throw new Exception("Message Queue 主機的 Multicast IP 未設定");
                }
            }
        }

        internal override void Send(MQMsgModel msg, bool multicast = false)
        {
            this.isMulticast = multicast;
            msg.SentTime = DateTime.Now;

            if (this.isMulticast == false)
            {
                // 2018/08/16, 避免 foreach 在多執行緒可能發生的問題, 加上 lock
                lock (thisLock)
                {
                    this.GetTargetPorts(msg.ToPort, msg.ToSiteName, msg.ToServerIP);

                    foreach (KeyValuePair<MessageQueue, string> item in this.dicQueueAndPath)
                    {
                        // 發送之前檢查下是否有設定 IP,因為是內部主機,所以直接寫死192.168
                        if (item.Value.IndexOf("192.168") > -1)
                        {
                            item.Key.Path = item.Value;
                            item.Key.Send(msg);
                        }
                        else
                        {
                            throw new Exception("MQ 主機的相關 IP 未設定,請確認");
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.multicastIP))
                {
                    this.queueMulticast.Path = string.Format(this.queuePathMulticast, this.multicastIP);
                    this.queueMulticast.Send(msg);
                }
                else
                {
                    throw new Exception("Message Queue 主機的 Multicast IP 未設定");
                }
            }
        }

        internal override void Send(ApplicationPort fromPort, ApplicationPort toPort, string toServerIP, string toSiteName, string jsonMsg, bool multicast = false)
        {
            this.isMulticast = multicast;

            MQMsgModel msg = JSON.Deserialize<MQMsgModel>(jsonMsg);
            msg.SentTime = DateTime.Now;

            this.msgToSend.Body = JSON.SerializeDynamic(msg);

            if (this.isMulticast == false)
            {
                // 2018/08/16, 避免 foreach 在多執行緒可能發生的問題, 加上 lock
                lock (thisLock)
                {
                    this.GetTargetPorts(toPort, toSiteName, toServerIP);

                    foreach (KeyValuePair<MessageQueue, string> item in this.dicQueueAndPath)
                    {
                        // 發送之前檢查下是否有設定 IP,因為是內部主機,所以直接寫死192.168
                        if (item.Value.IndexOf("192.168") > -1)
                        {
                            item.Key.Path = item.Value;
                            item.Key.Send(this.msgToSend);
                        }
                        else
                        {
                            throw new Exception("MQ 主機的相關 IP 未設定,請確認");
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.multicastIP))
                {
                    // 改用 Message 物件來傳送
                    this.queueMulticast.Path = string.Format(this.queuePathMulticast, this.multicastIP);
                    this.queueMulticast.Send(this.msgToSend);
                }
                else
                {
                    throw new Exception("Message Queue 主機的 Multicast IP 未設定");
                }
            }
        }


        internal override void Receive(ApplicationPort listenPort, string listenSiteName)
        {
            this.InitQueueReceiveLocal();

            if (this.queueReceiveLocal != null)
            {
                string path = string.Empty;

                if (listenPort == ApplicationPort.Manager)
                {
                    path = this.queuePathReceiveLocal_Manager;
                }
                else if (listenPort == ApplicationPort.PhysicalHost)
                {
                    // 實體機的 Queue 命名與通路端不同
                    // 通路端 : Agent_站台名稱
                    // 實體機 : 站台名稱 (給一個固定值就可以)
                    path = string.Format(this.queuePathReceiveLocal_PhysicalHost, listenSiteName);
                }
                else
                {
                    path = string.Format(this.queuePathReceiveLocal, listenPort.ToString(), listenSiteName);
                }


                if (MessageQueue.Exists(path))
                {
                    this.queueReceiveLocal.Path = path;
                    this.queueReceiveLocal.BeginReceive();
                }
            }
        }

        internal override void Stop()
        {
            if (this.queueReceiveLocal != null)
            {
                this.queueReceiveLocal.Close();
            }
        }

        private void GetTargetPorts(ApplicationPort toPorts, string toSiteName, string toServerIP = "")
        {
            this.dicQueueAndPath.Clear();

            if (toPorts == ApplicationPort.Member)
            {
                this.dicQueueAndPath.Add(this.queueSendRemote_Member, string.Format(this.queuePathSendRemote_Member, this.sendRemoteIP_Member, ApplicationPort.Member.ToString(), toSiteName));
            }
            else if (toPorts == ApplicationPort.Agent)
            {
                this.dicQueueAndPath.Add(this.queueSendRemote_Agent, string.Format(this.queuePathSendRemote_Agent, this.sendRemoteIP_Agent, ApplicationPort.Agent.ToString(), toSiteName));
            }
            else if (toPorts == ApplicationPort.Manager)
            {
                this.dicQueueAndPath.Add(this.queueSendRemote_Manager, string.Format(this.queuePathSendRemote_Manager, this.sendRemoteIP_Manager, ApplicationPort.Manager.ToString()));
            }
            else if (toPorts == ApplicationPort.PhysicalHost)
            {
                this.dicQueueAndPath.Add(this.queueSendRemote_PhysicalHost, string.Format(this.queuePathSendRemote_PhysicalHost, this.sendRemoteIP_PhysicalHost, toSiteName));
            }
            else if (toPorts == ApplicationPort.Custom)
            {
                this.dicQueueAndPath.Add(this.queueSendRemote_Custom, string.Format(this.queuePathSendRemote_Custom, toServerIP, toSiteName));
            }
            else if (toPorts == ApplicationPort.MemberAndAgent)
            {
                this.dicQueueAndPath.Add(this.queueSendRemote_Member, string.Format(this.queuePathSendRemote_Member, this.sendRemoteIP_Member, ApplicationPort.Member.ToString(), toSiteName));
                this.dicQueueAndPath.Add(this.queueSendRemote_Agent, string.Format(this.queuePathSendRemote_Agent, this.sendRemoteIP_Agent, ApplicationPort.Agent.ToString(), toSiteName));
            }
            else if (toPorts == ApplicationPort.MemberAndManager)
            {
                this.dicQueueAndPath.Add(this.queueSendRemote_Member, string.Format(this.queuePathSendRemote_Member, this.sendRemoteIP_Member, ApplicationPort.Member.ToString(), toSiteName));
                this.dicQueueAndPath.Add(this.queueSendRemote_Manager, string.Format(this.queuePathSendRemote_Manager, this.sendRemoteIP_Manager, ApplicationPort.Manager.ToString()));
            }
            else if (toPorts == ApplicationPort.AgentAndManager)
            {
                this.dicQueueAndPath.Add(this.queueSendRemote_Agent, string.Format(this.queuePathSendRemote_Agent, this.sendRemoteIP_Agent, ApplicationPort.Agent.ToString(), toSiteName));
                this.dicQueueAndPath.Add(this.queueSendRemote_Manager, string.Format(this.queuePathSendRemote_Manager, this.sendRemoteIP_Manager, ApplicationPort.Manager.ToString()));
            }
            else if (toPorts == ApplicationPort.All)
            {
                // 當接收端是 All 時, 不需要發給實體機的 Queue, 以及 Custom 的主機
                this.dicQueueAndPath.Add(this.queueSendRemote_Member, string.Format(this.queuePathSendRemote_Member, this.sendRemoteIP_Member, ApplicationPort.Member.ToString(), toSiteName));
                this.dicQueueAndPath.Add(this.queueSendRemote_Agent, string.Format(this.queuePathSendRemote_Agent, this.sendRemoteIP_Agent, ApplicationPort.Agent.ToString(), toSiteName));
                this.dicQueueAndPath.Add(this.queueSendRemote_Manager, string.Format(this.queuePathSendRemote_Manager, this.sendRemoteIP_Manager, ApplicationPort.Manager.ToString()));
            }
        }
    } // end of class
}
