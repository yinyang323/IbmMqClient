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
                Send("Hello World");
                Console.WriteLine($"Recieved message: {Receive()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine(">>> END <<<");
            Console.ReadKey();
        }

        static void Send(string message)
        {
            using (var context = CreateJmsConnectionFactory().createContext())
            {
                var queue = context.createQueue(string.Concat("queue:///", QUEUE_NAME));
                var producer = context.createProducer();
                producer.send(queue, message);
            }
        }

        static string Receive()
        {
            using (var context = CreateJmsConnectionFactory().createContext())
            using (var consumer = context.createConsumer(context.createQueue(string.Concat("queue:///", QUEUE_NAME))))
            {
                var message = consumer.receive(15000L);
                var body = message?.getBody(typeof(String)) as string;
                return body;
            }
        }

        static JmsConnectionFactory CreateJmsConnectionFactory()
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

            return cf;
        }
    }
}
