# IbmMqClient  [![NuGet version](https://badge.fury.io/nu/IbmMqClient.svg)](http://badge.fury.io/nu/IbmMqClient)
IBM MQ standalone client for .NET

```csharp
using com.ibm.msg.client.jms;
using com.ibm.msg.client.wmq.common;
```

```csharp
var ff = JmsFactoryFactory.getInstance(JmsConstants.__Fields.WMQ_PROVIDER);
var cf = ff.createConnectionFactory();

cf.setIntProperty(CommonConstants.__Fields.WMQ_CONNECTION_MODE, CommonConstants.__Fields.WMQ_CM_CLIENT);
cf.setStringProperty(CommonConstants.__Fields.WMQ_HOST_NAME, "127.0.0.1");
cf.setIntProperty(CommonConstants.__Fields.WMQ_PORT, 8010);
cf.setStringProperty(CommonConstants.__Fields.WMQ_CHANNEL, "EXAMPLE.CHANNEL.ONE");
cf.setStringProperty(CommonConstants.__Fields.WMQ_QUEUE_MANAGER, "EXAMPLE_QUEUE_MANAGER");
cf.setStringProperty(CommonConstants.__Fields.WMQ_APPLICATIONNAME, "JMS EXAMPLE");
cf.setStringProperty(CommonConstants.USERID, "EXAMPLE_USER");

var connection = cf.createConnection();
var session = connection.createSession(false, javax.jms.Session.__Fields.AUTO_ACKNOWLEDGE);
var queue = session.createQueue("queue:///EXAMPLE_QUEUE_NAME");
var producer = session.createProducer(queue);

var msg = session.createTextMessage();
msg.setStringProperty("JMSXGroupID", Guid.NewGuid().ToString());
msg.setIntProperty("JMSXGroupSeq", 1);
msg.setBooleanProperty("JMS_IBM_Last_Msg_In_Group", true);
msg.setText("Hello World");

connection.start();
producer.send(msg);

producer.close();
session.close();
connection.close();
```
