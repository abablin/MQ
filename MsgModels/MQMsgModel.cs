using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RY.H3Hybrid.MQ.Enums;

namespace RY.H3Hybrid.MQ.MsgModels
{
    /// <summary>
    /// 所有訊息物件的基本類別
    /// </summary>
    [Serializable]
    public class MQMsgModel
    {
        private DateTime sentTime=DateTime.Now;
        private DateTime arrivedTime = DateTime.Now;

        /// <summary>
        /// 收到該訊息後要做的動作
        /// 區分 Member,Agent,Manager 等端口
        /// 對應到 RY.H3Hybrid.MQ.MessageAction
        /// </summary>
        public int ActionToDo { get; set; }

        /// <summary>
        /// 訊息的傳送端口
        /// </summary>
        public ApplicationPort FromPort { get; set; }
        
        /// <summary>
        /// 訊息的接收端口
        /// </summary>
        public ApplicationPort ToPort{ get; set; }

        /// <summary>
        /// 訊息的接收站台名稱,例如 Caesar
        /// </summary>
        public string ToSiteName { get; set; }

        /// <summary>
        /// 站台對應的 Queue 結尾名稱,例如泰A,泰B都對應到 member_demosite
        /// 因為站台名稱不一定會跟 Queue 名稱對應(舊的站台資料有的會,新建立的站台不會)
        /// </summary>
        public string ToSiteQueueName { get; set; }

        /// <summary>
        /// 目的地主機的 IP
        /// 此屬性用在目的地可能不固定的情況
        /// </summary>
        public string ToServerIP { get; set; }

        public string ReturnServerIP { get; set; }

        public string ReturnQueueName { get; set; }

        /// <summary>
        /// 訊息真正的內容, 也是JSON格式
        /// </summary>
        public object Body { get; set; }

        /// <summary>
        /// 訊息送出時間
        /// </summary>
        public DateTime SentTime 
        {
            get { return this.sentTime.ToLocalTime(); }
            set { this.sentTime = value; }
        }

        /// <summary>
        /// 訊息接收時間
        /// </summary>
        public DateTime ArrivedTime
        {
            get { return this.arrivedTime.ToLocalTime(); }
            set { this.arrivedTime = value; }
        }
    } // end of class
}
