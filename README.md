# MQ
This project is used to send or receive message queue for applications. Currently I built and test it for MSMQ because our platform is mainly built on Microsoft Windows Server. There are four main folders for this program and I'll give some help as following.

1. Classes: This is the main part of this program. Client application will call any functions available from QueueManager class. And it contains the abstract object MQClientBase. Finally the concrete class will be MSMQClient or RabbitMQClient or you can extend what you need.

2. MsgModels: I define all the messages that will be transfered between client and server in this folder. All classes inherit from a base class called MQMsgModel.

3. MessageAction: All receiving applications will parse MsgModels object they received and decide what to do next based on this property.

4. Enums: Define all kinds of enums needed here.


Real application senarios (I've used currently)

1. Web application cache duration:
When data changed(insert, update, delete) in backend system, it would send a message to client applications to notify them to remove current cached data. So next time when user visits the page again, it will try to load the data from database and caches it again until next notification.

2. Excel file output:
Some of our backend systems run on virtual machines and the old program put it all together in a asp.net mvc application's action. This mechanism results in long-time waiting when user clicks the download button or even nothing returned. This is really bad experience for users. So I split the old mechanism into two parts. The first part still runs on virtual machine but what it does is simplified. It will just send a message to remote server that's not on virtual machine. The second part will do a query from database and then generate the excel data content to a stream. Finally it will send data stream back to virtual machine in batch messages if needed. And client application will notify user and download the file automatically with the help from asp.net signalr.

3. Web page data refresh:
One of our backend system uses a very bad mechanism to get the newest data from database. It will query databse every 30 seconds. This won't be a problem when there are no huge amount of concurrent users. But if it's not the case, it will be a serious problem. So I changed this to a new pattern. When client application commits new data to database, it'll also send a message to backend system to tell it to get the data commited. This mechanism hugely decreases database procedure's query times and of course user will have better experience on system.


