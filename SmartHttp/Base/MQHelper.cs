//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using RabbitMQ.Client;
//using System;
//using System.Configuration;
//using System.Text;
//using System.Threading.Tasks;

//namespace SmartHttp
//{
//    public class MQHelper
//    {
//        private static readonly Lazy<ConnectionFactory> lazyFactory = new Lazy<ConnectionFactory>(
//            () => new ConnectionFactory()
//            {
//                Uri = ConfigurationManager.AppSettings["RabbitMQUrl"],
//                VirtualHost = ConfigurationManager.AppSettings["RabbitMQVirtualHost"],
//            });




//        /// <summary>
//        /// Rabbit MQ ConnectionFactory对象
//        /// </summary>
//        public static ConnectionFactory Factory
//        {
//            get { return lazyFactory.Value; }
//        }

//        private static object syncObject = new object();
//        private IConnection connection;

//        /// <summary>
//        /// Rabbit MQ Connection对象
//        /// </summary>
//        private IConnection Connection
//        {
//            get
//            {
//                if (connection == null)
//                {
//                    lock (syncObject)
//                    {
//                        if (connection == null)
//                        {
//                            connection = Factory.CreateConnection();
//                        }
//                    }
//                }
//                return connection;
//            }
//        }


//        /// <summary>
//        /// 批量入队
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="data"></param>
//        /// <param name="exchangeType"></param>
//        /// <param name="failAction">失败回调</param>
//        public bool Enqueue<T>(T data, Action<T> failAction = null)
//        {
//            string exchangeName = string.Empty;
//            try
//            {
//                using (IModel channel = Connection.CreateModel())
//                {
//                    //申明并创建队列(生产者必须)
//                    channel.QueueDeclare("httplog", true, false, false, null);
//                    string json = JsonConvert.SerializeObject(data, new IsoDateTimeConverter());
//                    //发送消息到Exchange
//                    channel.BasicPublish(exchangeName, "httplog", (IBasicProperties)null, Encoding.UTF8.GetBytes(json));
//                }
//                return true;
//            }
//            catch (Exception)
//            {
//                failAction?.Invoke(data);
//                return false;
//            }
//        }

//        /// <summary>
//        /// 异步压入日志队列
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="data"></param>
//        /// <param name="exchangeType"></param>
//        /// <param name="failAction"></param>
//        public async Task<bool> EnqueueAsync<T>(T data, Action<T> failAction = null)
//        {
//            return await Task.Run(() => Enqueue(data, failAction));
//        }

//    }
//}
