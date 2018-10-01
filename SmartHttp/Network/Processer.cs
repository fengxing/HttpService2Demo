using System.Collections.Generic;
using System;
using SmartHttpEntity;

namespace SmartHttp.Network
{
    public class Processer
    {
        public static HttpReturn Process(RequestInfo requestInfo)
        {
            var httpReturn = HttpExtendHelper.Request(requestInfo);
            if (httpReturn.IsSuccess == false)
            {
                if (requestInfo.HttpMessage.LoopTime >= 0 //允许重试
                    && requestInfo.HttpTag.ExecuteID.HasValue == false
                    && !string.IsNullOrWhiteSpace(httpReturn.ExceptionMessage)
                    && httpReturn.ExceptionMessage.Contains("请求接口超时"))    //接口超时)
                {
                    //默认值0，重试1次；其它值N，重试N次
                    var retryTimes = requestInfo.HttpMessage.LoopTime == 0 ? 1 : requestInfo.HttpMessage.LoopTime;

                    DateTime now = DateTime.Now;
                    for (int i = 0; i < retryTimes; i++)
                    {
                        if (requestInfo.HttpMessage.LoopWaitTime > 0)
                        {
                            now = DateTime.Now;
                            while ((DateTime.Now - now).TotalSeconds < requestInfo.HttpMessage.LoopWaitTime) { }
                        }
                        httpReturn = HttpExtendHelper.Request(requestInfo);
                        httpReturn.LoopTime += 1;
                        if (httpReturn.IsSuccess)
                        {
                            return httpReturn;
                        }
                    }
                }
            }
            if (httpReturn.LoopTime > 0)
            {
                httpReturn.ExceptionMessage = httpReturn.ExceptionMessage + "(重试" + httpReturn.LoopTime + "次)";
            }
            return httpReturn;
        }
    }
}
