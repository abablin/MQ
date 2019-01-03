using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RY.H3Hybrid.MQ.MsgModels;
using RY.H3Hybrid.MQ.Enums;

namespace RY.H3Hybrid.MQ.Classes
{
    /// <summary>
    /// 用來真正連線到 MQ 的 Client 基礎物件
    /// 目前是用微軟的 MSMQ
    /// </summary>
    public abstract class MQClientBase
    {
        protected string multicastIP = string.Empty;
        protected string sendRemoteIP_Member = string.Empty;
        protected string sendRemoteIP_Agent = string.Empty;
        protected string sendRemoteIP_Manager = string.Empty;
        protected string sendRemoteIP_PhysicalHost = string.Empty;
        protected string sendRemoteIP_Custom = string.Empty;
        protected bool stopReceiving = false;

        public delegate void MsgReceive(string jsonMsg);
        public event MsgReceive MsgReceived;

        public delegate void MsgReceive_Model(MQMsgModel msgModel);
        public event MsgReceive_Model MsgReceived_Model;

        public MQClientBase()
        {
            Init();
        }

        /// <summary>
        /// 做初始化
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// 發送訊息
        /// </summary>
        /// <param name="fromPort">來源端口</param>
        /// <param name="toPort">目的地端口</param>
        /// <param name="toSiteName">目的地站台名稱</param>
        /// <param name="jsonMsg">JSON 格式的訊息內容</param>
        internal virtual void Send(ApplicationPort fromPort, ApplicationPort toPort, string toSiteName, string jsonMsg, bool multicast = false) { }

        internal virtual void Send(ApplicationPort fromPort, ApplicationPort toPort,string toServerIP, string toSiteName, string jsonMsg, bool multicast = false) { }

        internal virtual void Send(MQMsgModel msg, bool multicast = false) { }

        /// <summary>
        /// 接收訊息
        /// </summary>
        /// <param name="listenPort">要接收的端口</param>
        /// <param name="listenSiteName">要接收的站台名稱</param>
        internal virtual void Receive(ApplicationPort listenPort, string listenSiteName) { }

        /// <summary>
        /// 停止接收訊息
        /// </summary>
        internal virtual void Stop() { }

        /// <summary>
        /// 當接收到訊息之後,進行觸發
        /// </summary>
        /// <param name="jsonMsg">JSON 格式的訊息</param>
        protected void TriggerMsgReceived(string jsonMsg)
        {
            if (MsgReceived != null)
            {
                MsgReceived(jsonMsg);
            }
        }

        /// <summary>
        /// 當接收到訊息之後,進行觸發
        /// </summary>
        /// <param name="msgModel">物件格式的訊息</param>
        protected void TriggerMsgReceived_Model(MQMsgModel msgModel)
        {
            if (MsgReceived_Model != null)
            {
                MsgReceived_Model(msgModel);
            }
        }

        /// <summary>
        /// 設定是否要停止接收訊息
        /// </summary>
        /// <param name="stop">true:停止接收</param>
        public void SetReceivingVariable(bool stop)
        {
            stopReceiving = stop;
        }
    } // end of class
}
