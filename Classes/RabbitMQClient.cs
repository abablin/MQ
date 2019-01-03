using System;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;

//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
using RY.H3Hybrid.MQ.Enums;

namespace RY.H3Hybrid.MQ.Classes
{
    /// <summary>
    /// 使用第三方的 RabbitMQ
    /// </summary>
    internal class RabbitMQClient //: MQClientBase
    {
//        private ConnectionFactory cnFactory;
//        private IConnection connection;
//        private IModel channel;
//        private EventingBasicConsumer consumer;

//        private string sHostName;
//        private string sPassword;
//        private string sExchangeName;
//        private int iPort;

//        protected override void Init()
//        {
//            if (ConfigurationManager.AppSettings["MQ_HostIP"] != null)
//            {
//                this.sHostName = ConfigurationManager.AppSettings["MQ_HostIP"].ToString();

//                // 抓 IP 的最後一位再加上前綴 5 當作 Port
//                this.iPort = int.Parse(string.Format("5{0}", this.sHostName.Split('.')[3]));

//                this.sPassword = string.Format("rabbitmqqaz123@WSX{0}!", this.sHostName.Split('.')[3]);
//            }

//            if (cnFactory == null)
//            {
//                cnFactory = new ConnectionFactory() { HostName = this.sHostName, Port = this.iPort, UserName = "daniel", Password = this.sPassword };
//            }
//        }

//        internal override void Send(ApplicationPort fromPort, ApplicationPort toPort, string toSiteName, string jsonMsg, bool multicast = false)
//        {
//            Thread thPublish = new Thread(() => SendInOtherThread(toPort, toSiteName, jsonMsg));
//            thPublish.Start();
//        }

//        internal override void Receive(ApplicationPort listenPort, string listenSiteName)
//        {
//            this.sExchangeName = string.Format("For{0}", listenPort.ToString());

//            connection = cnFactory.CreateConnection(string.Format("{0}.{1}", listenPort.ToString(), listenSiteName));

//            channel = connection.CreateModel();
//            channel.ExchangeDeclare(this.sExchangeName, "topic");

//            var queueName = channel.QueueDeclare().QueueName;

//            channel.QueueBind(queueName, this.sExchangeName, string.Format("{0}.{1}", listenPort.ToString(), listenSiteName));

//            consumer = new EventingBasicConsumer(channel);
//            consumer.Received += MQMsgReceived;

//            channel.BasicConsume(queueName, true, consumer);
//        }

//        private void MQMsgReceived(object sender, BasicDeliverEventArgs e)
//        {
//            this.TriggerMsgReceived(Encoding.UTF8.GetString(e.Body));
//        }

//        private void SendInOtherThread(ApplicationPort toPort, string toSiteName ,string jsonMsg)
//        {
//            var factory = new ConnectionFactory() { HostName = this.sHostName, Port = this.iPort, UserName = "daniel", Password = this.sPassword };
//            this.sExchangeName = string.Format("For{0}", toPort.ToString());

//            using (var connection = factory.CreateConnection())
//            {
//                using (var channel = connection.CreateModel())
//                {
//                    connection.AutoClose = true;
//                    channel.ExchangeDeclare(this.sExchangeName, "topic");
//                    channel.BasicPublish(this.sExchangeName, string.Format("{0}.{1}", toPort.ToString(), toSiteName), null, Encoding.UTF8.GetBytes(jsonMsg));
//                }
//            }
//        }
    } // end of class
}
