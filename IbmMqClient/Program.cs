using com.ibm.msg.client.jms;
using com.ibm.msg.client.wmq.common;
using System;
using System.Text;
using System.Configuration;

namespace IbmMqClient
{
    class Program
    {
        static string HOST = ConfigurationManager.AppSettings.Get("HOST");
        static string PORT = ConfigurationManager.AppSettings.Get("PORT");
        static string CHANNEL = ConfigurationManager.AppSettings.Get("CHANNEL");
        static string APP_USER = ConfigurationManager.AppSettings.Get("APP_USER");
        static string APP_PASSWORD = ConfigurationManager.AppSettings.Get("APP_PASSWORD");
        static string QMGR = ConfigurationManager.AppSettings.Get("QUEUE_MANAGER");
        static string QUEUE_NAME = ConfigurationManager.AppSettings.Get("QUEUE_NAME");

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            try
            {
                Test();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine(">>> END <<<");
            Console.ReadKey();
        }

        static void Test()
        {
            var ff = JmsFactoryFactory.getInstance(JmsConstants.__Fields.WMQ_PROVIDER);
            var cf = ff.createConnectionFactory();

            cf.setIntProperty(CommonConstants.__Fields.WMQ_CONNECTION_MODE, CommonConstants.__Fields.WMQ_CM_CLIENT);
            cf.setStringProperty(CommonConstants.__Fields.WMQ_HOST_NAME, HOST);
            cf.setIntProperty(CommonConstants.__Fields.WMQ_PORT, int.Parse(PORT));
            cf.setStringProperty(CommonConstants.__Fields.WMQ_CHANNEL, CHANNEL);
            cf.setStringProperty(CommonConstants.__Fields.WMQ_QUEUE_MANAGER, QMGR);
            cf.setStringProperty(CommonConstants.__Fields.WMQ_APPLICATIONNAME, "Test JMS");
            cf.setStringProperty(CommonConstants.USERID, APP_USER);

            if (!string.IsNullOrEmpty(APP_PASSWORD))
            {
                cf.setBooleanProperty(CommonConstants.USER_AUTHENTICATION_MQCSP, true);
                cf.setStringProperty(CommonConstants.PASSWORD, APP_PASSWORD);
            }

            var connection = cf.createConnection();
            var session = connection.createSession(false, javax.jms.Session.__Fields.AUTO_ACKNOWLEDGE);
            var queue = session.createQueue("queue:///" + QUEUE_NAME);
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
        }
    }
}
