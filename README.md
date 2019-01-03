# MQ
This project is used to send or receive message queue for applications. Currently I built and test it for MSMQ because our platform is mainly built on Microsoft Windows Server. There are four main folders for this program and I'll give some help as following.

1. Classes: This is the main part of this program. Client application will call any functions available from QueueManager class. And it contains the abstract object MQClientBase. Finally the concrete class will be MSMQClient or RabbitMQClient.

2. MsgModels: I define all the messages that will be transfered between client and server in this folder. All classes inherient from a base class called MQMsgModel.

3. MessageAction: All receiving applications will parse MsgModels object they received and decide what to do next based on this property.

4. Enums: Defined all kinds of enums needed here.

