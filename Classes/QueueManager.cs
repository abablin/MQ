using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Jil;
using RY.H3Hybrid.MQ.Enums;
using RY.H3Hybrid.MQ.MsgModels;

namespace RY.H3Hybrid.MQ.Classes
{
    /// <summary>
    /// 提供給前端呼叫的物件
    /// </summary>
    public class QueueManager
    {

        private static QueueManager instance;
        private MQClientBase client;

        public delegate void MsgReceive(string jsonMsg);
        public event MsgReceive MsgReceived;

        public delegate void MsgReceive_Model(MQMsgModel msgModel);
        public event MsgReceive_Model MsgReceived_Model;

        private static object syncLock = new object();

        private QueueManager(MQClientBase c)
        {
            if (this.client == null)
            {
                this.client = c;
                this.client.MsgReceived += QueueMsgReceived;
                this.client.MsgReceived_Model += QueueMsgReceived_Model;
            }
        }

        private void QueueMsgReceived_Model(MQMsgModel msgModel)
        {
            if (this.MsgReceived_Model != null)
            {
                this.MsgReceived_Model(msgModel);
            }
        }

        private void QueueMsgReceived(string jsonMsg)
        {
            if (this.MsgReceived != null)
            {
                this.MsgReceived(jsonMsg);
            }
        }

        public static QueueManager GetInstance(bool isMulticast=false)
        {
            if (instance == null)
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        instance = new QueueManager(new MSMQClient(isMulticast));
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 發送訊息
        /// </summary>
        /// <param name="from">來源端口</param>
        /// <param name="to">目的地端口</param>
        /// <param name="jsonMsg">JSON 格式的訊息內容</param>
        /// <param name="multicast">是否為多點傳送,預設為 false</param>
        private void Send(ApplicationPort fromPort, ApplicationPort toPort, string toSiteQueueName, string jsonMsg, bool multicast = false)
        {
            if (this.client != null)
            {
                this.client.Send(fromPort, toPort, toSiteQueueName, jsonMsg, multicast);
            }
        }

        private void Send(ApplicationPort fromPort, ApplicationPort toPort,string toServerIP, string toSiteQueueName, string jsonMsg, bool multicast = false)
        {
            if (this.client != null)
            {
                this.client.Send(fromPort, toPort,toServerIP, toSiteQueueName, jsonMsg, multicast);
            }
        }

        /// <summary>
        /// 發送訊息
        /// </summary>
        /// <param name="jsonMsg">JSON 格式的訊息內容</param>
        /// <param name="multicast">是否為多點傳送,預設為 false</param>
        public void Send(string jsonMsg, bool multicast = false)
        {
            if (this.client != null)
            {
                // 收到前端的訊息後,解析目的地的端口與站台名稱
                MQMsgModel msg = JSON.Deserialize<MQMsgModel>(jsonMsg);

                // 改為另開執行緒去傳送
                //Thread sendThread = new Thread(() => Send(msg.FromPort, msg.ToPort, msg.ToSiteName, jsonMsg, multicast));
                //sendThread.Start();

                // 根據是否傳入目的地主機的 IP 來決定要呼叫的方法
                if (string.IsNullOrEmpty(msg.ToServerIP))
                {
                    this.Send(msg.FromPort, msg.ToPort, msg.ToSiteQueueName, jsonMsg, multicast);
                }
                else
                {
                    this.Send(msg.FromPort, msg.ToPort,msg.ToServerIP, msg.ToSiteQueueName, jsonMsg, multicast);
                }
            }
        }

        public void Send(MQMsgModel msg, bool multicast = false)
        {
            if (this.client != null)
            {
                this.client.Send(msg, multicast);
            }
        }

        public void Receive(ApplicationPort listenPort, string listenSiteName)
        {
            if (this.client != null)
            {
                this.client.SetReceivingVariable(false);
                this.client.Receive(listenPort, listenSiteName);
            }
        }

        /// <summary>
        /// 停止接收訊息
        /// </summary>
        public void StopReceiving()
        {
            if (this.client != null)
            {
                this.client.SetReceivingVariable(true);
                this.client.Stop();
            }
        }

    } // end of class
}
