using SmartSDKHelper;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonConvert = new JsonConvert();
            var loginProcess = new LoginProcess();
            SmartOutSideHelper.Register(jsonConvert, loginProcess);
            MutilRun();
        }

        private static void MutilRun()
        {
            var invoke = new HttpInvokes()
            {
                ExexuteID = Guid.NewGuid(),
                ID = new Guid("954e0d11-e6e8-4720-bb7d-0b24dab4ac94"),
                AppID = 3,
                CanRetry = false,
                RetryTimes = 0,
                HttpCommands = new List<HttpCommand>() {
                     new HttpCommand () {
                           AppID=27,
                           Method="AddGoodReceive",
                           RequestObjs=new List<string> () {
                               "eeb956a1-545a-ff11-a124-043b02010000",
                               "94bfe31e-146a-ff11-9a63-077f03010000",
                               "1111",
                               "1",
                               "2018-05-28T16:00:00.000Z",
                               "1866cd0e-0f79-ff11-a7ec-0c0d07010000",
                               "2",
                               ""
                           },
                     },
                     new HttpCommand () {
                           AppID=27,
                           Method="AddModelReceiveGood",
                           RequestObjs=new List<string> () {
                               "$.0.ReceiveID",
                               "1",
                               "1",
                               "false",
                               "true",
                               "true",
                               "true",
                               "1232123"
                           },
                     },
                },
                SenderTime = DateTime.Now,
            };
            var result = invoke.Requests();
            Console.WriteLine(result.IsSuccess);
            Console.WriteLine(result.Data);
            Console.WriteLine(result.Wrong);
            Console.ReadLine();
        }

        private static void SingleRun()
        {
            //测试成功
            for (int i = 0; i < 10; i++)
            {
                SmartOutSideHelper.Set3rdHttp("http://192.168.2.201:7000/");
                Console.WriteLine("第[{0}]次请求", i + 1);
                var ret = SmartOutSideHelper.Request(new HttpInvoke()
                {
                    AppID = 16,
                    CanRetry = false,
                    ID = new Guid().ToString(),
                    Method = "GetAppIDs",
                    RequestObjs = new List<string>()
                    {

                    },
                    RetryTimes = 0,
                    SenderTime = DateTime.Now,
                    Version = "1.0"
                });
                SmartOutSideHelper.Set3rdHttp("http://116.62.232.36:7000/");
            }
        }

        public static void ProcessError(Result result)
        {
            Console.WriteLine("error");
        }
    }/// <summary>
     /// 阿里云授权返回
     /// </summary>
    public class AuthorityResponse
    {
        /// <summary>
        /// 阿里云Key
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// 阿里云密钥
        /// </summary>
        public string AccessKeySecret { get; set; }

        /// <summary>
        /// 授权过期时间
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// 授权Token
        /// </summary>
        public string SecurityToken { get; set; }

        /// <summary>
        /// 授权访问的Bucket
        /// </summary>
        public string Bucket { get; set; }
    }
}
