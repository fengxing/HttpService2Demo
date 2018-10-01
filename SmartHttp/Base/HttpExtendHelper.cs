using Newtonsoft.Json;
using SmartBLL;
using SmartHttpEntity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SmartHttp
{
    public static class HttpExtendHelper
    {
        public static HttpReturn Request(RequestInfo requestInfo)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var httpReturn = new HttpReturn()
            {
                Method = requestInfo.HttpMessage.Method,
                Url = requestInfo.HttpMessage.Url,
                AppID = requestInfo.HttpMessage.AppID,
                HttpMethod = requestInfo.HttpMessage.HttpType.ToString(),
                RequestObjs = new List<string>(),
                Exception = "",
                ExceptionMessage = "",
                Response = "",
                ResponseEncrypt = "",
                RequestEncrypt = "",
                Request = "",
                Moudle = requestInfo.HttpMessage.Moudle,
                StatusCode = 0
            };
            requestInfo.HttpReturn = httpReturn;
            try
            {
                //var encrypt = Smart.Http.AllEncrypt.Facade.GetOperator(httpReturn.AppID);
                //if (encrypt != null && encrypt.IsNeedEncrypt(requestInfo.HttpMessage.Moudle))
                //{
                //    requestInfo.HttpReturn.Request = string.Join(",", requestInfo.HttpRequest.RequestObjs);
                //    requestInfo.Encrypt = encrypt;
                //    requestInfo.HttpRequest.RequestObjs = encrypt.EncryptRequest(requestInfo.HttpRequest.RequestObjs, requestInfo.HttpRequest.Method);
                //}
                return HttpRequest(requestInfo);
            }
            catch (Exception ex)
            {
                sw.Stop();
                httpReturn.Exception = JsonConvert.SerializeObject(ex);
                httpReturn.ExceptionMessage = ex.Message;
                requestInfo.HttpReturn = httpReturn;
                requestInfo.Log(ex.GetType().Name);
                return httpReturn;
            }
        }

        private static HttpReturn HttpRequest(RequestInfo requestInfo)
        {
            var httpMessage = requestInfo.HttpMessage;
            var httpReturn = requestInfo.HttpReturn;
            var objs = requestInfo.HttpRequest.RequestObjs;
            #region 生成参数          
            if (httpMessage.HttpType == HttpType.HttpGet)
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    objs[i] = System.Web.HttpUtility.UrlEncode(objs[i]);
                }
            }
            #endregion

            #region 构造http头
            var http = new HttpHelper();
            http.DHeaders.Add("appid", requestInfo.HttpRequest.AppID.ToString());
            http.DHeaders.Add("token", requestInfo.HttpTag.Token);
            http.DHeaders.Add("sendertime", requestInfo.HttpTag.SenderTime);
            http.DHeaders.Add("cip", requestInfo.HttpTag.CIP);
            http.DHeaders.Add("sip", requestInfo.HttpTag.IP);
            http.DHeaders.Add("rowversion", requestInfo.HttpTag.RowVersion);
            http.DHeaders.Add("uid", requestInfo.HttpTag.UID);
            http.DHeaders.Add("other", requestInfo.HttpTag.Other);
            http.DHeaders.Add("channel", requestInfo.HttpTag.Channel);
            http.DHeaders.Add("isAynsc", requestInfo.HttpTag.IsAynsc.ToString());
            http.DHeaders.Add("method", requestInfo.HttpMessage.Method);
            if (requestInfo.HttpTag.Retry)
            {
                http.DHeaders.Add("retry", requestInfo.HttpTag.Retry.ToString());
            }
            if (requestInfo.HttpTag.IsPreLoad)
            {
                http.DHeaders.Add("preload", requestInfo.HttpTag.IsPreLoad.ToString());
            }
            if (requestInfo.HttpTag.ExecuteID.HasValue)
            {
                http.DHeaders.Add("executeid", requestInfo.HttpTag.ExecuteID.Value.ToString());
            }
            if (!string.IsNullOrWhiteSpace(requestInfo.HttpTag.RequestID))
            {
                http.DHeaders.Add("requestid", requestInfo.HttpTag.RequestID);
            }
            if (httpMessage.TimeOut > 0)
            {
                http.Timeout = httpMessage.TimeOut;
            }
            if (!string.IsNullOrWhiteSpace(httpMessage.ContentType))
            {
                http.ContentType = httpMessage.ContentType;
            }
            if (!string.IsNullOrWhiteSpace(httpMessage.Headers))
            {
                var headers = httpMessage.Headers.Split(',');
                http.Headers.AddRange(headers);
            }
            if (!string.IsNullOrWhiteSpace(httpMessage.HttpEncoding))
            {
                http.Encoding = Encoding.GetEncoding(httpMessage.HttpEncoding);
            }
            if (!string.IsNullOrWhiteSpace(httpMessage.UserAgent))
            {
                if (httpMessage.UserAgent == "空")
                {
                    http.UserAgent = null;
                }
                else
                {
                    http.UserAgent = httpMessage.UserAgent;
                }
            }

            #endregion

            #region 构造地址和请求内容
            if (httpMessage.HttpType == HttpType.HttpGet)
            {
                var url = httpMessage.Url;
                for (int i = 0; i < objs.Count; i++)
                {
                    url = url.Replace("{" + i.ToString() + "}", objs[i].Trim());
                }
                httpReturn.Url = url;
            }
            else
            {
                var webServiceTemplate = httpMessage.WebServiceTemplate;
                if (requestInfo.HttpMessage.AppID < 500 && webServiceTemplate.StartsWith("{") && webServiceTemplate.EndsWith("}"))
                {
                    var jsonArgs = JsonConvert.DeserializeObject<IDictionary<string, string>>(httpMessage.WebServiceTemplate);
                    var index = 0;
                    var dic = new Dictionary<string, string>();
                    foreach (var jsonArg in jsonArgs)
                    {
                        if (jsonArg.Value.StartsWith("{") && jsonArg.Value.EndsWith("}"))
                        {
                            if (int.TryParse(jsonArg.Value.TrimStart('{').TrimEnd('}'), out index))
                            {
                                dic.Add(jsonArg.Key, objs[index].Trim());
                            }
                            else
                            {
                                dic.Add(jsonArg.Key, jsonArg.Value);
                            }
                        }
                        else
                        {
                            dic.Add(jsonArg.Key, jsonArg.Value);
                        }
                    }
                    webServiceTemplate = JsonConvert.SerializeObject(dic);
                }
                else
                {
                    for (int i = 0; i < objs.Count; i++)
                    {
                        webServiceTemplate = webServiceTemplate.Replace("{" + i.ToString() + "}", objs[i].Trim());
                    }
                }
                if (httpReturn.Url.Contains("{"))
                {
                    var url = httpMessage.Url;
                    for (int i = 0; i < objs.Count; i++)
                    {
                        url = url.Replace("{" + i.ToString() + "}", objs[i].Trim());
                    }
                    httpMessage.Url = url;
                    httpReturn.Url = url;
                }
                http.RequestData = httpReturn.Request = webServiceTemplate;
            }
            #endregion

            #region http请求
            HttpHelperReturn helperReturn;
            if (httpMessage.HttpType == HttpType.HttpGet)
            {
                helperReturn = http.GetRequest(httpReturn.Url);
            }
            else
            {
                if (httpMessage.HttpType == HttpType.HttpPut)
                {
                    helperReturn = http.PutRequest(httpMessage.Url);
                }
                else
                {
                    helperReturn = http.PostRequest(httpMessage.Url);
                }
            }
            httpReturn.StatusCode = helperReturn.StatusCode;
            httpReturn.Response = helperReturn.Return;
            #endregion
            #region 是否结果映射
            if (requestInfo.HttpReturn.IsSuccess == false && httpMessage.IsValid)
            {
                var errorMppers = httpMessage.GetWsExcepitons();

                var mapper = errorMppers.FirstOrDefault(r => r.Name.Trim() == httpReturn.Response.Trim());
                if (mapper != null)
                {
                    httpReturn.WsExcepiton = mapper;
                    httpReturn.Response = mapper.Value;
                    helperReturn.Return = mapper.Value;
                }
            }
            #endregion
            requestInfo.Stop();
            requestInfo.HttpReturn = httpReturn;
            requestInfo.Log();
            return httpReturn;
        }

        public static void Log(this RequestInfo requestInfo, string exceptionType = "")
        {
            var httpmessage = requestInfo.HttpMessage;
            //不关闭日志
            if (httpmessage.IsLog != true)
            {
                var httpReturn = requestInfo.HttpReturn;
                var log = new HttpLogInterfaceCall()
                {
                    Exception = httpReturn.Exception,
                    ExceptionMessage = httpReturn.ExceptionMessage,
                    Moudle = httpReturn.Moudle,
                    HttpMethod = httpReturn.HttpMethod,
                    IsSuccess = requestInfo.HttpTag.ExecuteID.HasValue ? false : httpReturn.IsSuccess,
                    AppID = httpReturn.AppID,
                    Method = httpReturn.Method,
                    Mills = requestInfo.Stopwatch.ElapsedMilliseconds,
                    Request = httpReturn.Request,
                    RequestArgs =  string.Join(",", requestInfo.HttpRequest.RequestObjs),
                    RequestEncrypt = httpReturn.RequestEncrypt,
                    Response = httpReturn.WsExcepiton != null ? httpReturn.WsExcepiton.Name : httpReturn.Response,
                    ResponseEncrypt = httpReturn.ResponseEncrypt,
                    CreateTime = DateTime.Now,
                    Url = httpReturn.Url,
                    StatusCode = httpReturn.StatusCode,
                    ExceptionType = string.IsNullOrWhiteSpace(exceptionType) ? httpReturn.Exception : exceptionType,
                    Channel = requestInfo.HttpTag.Channel,
                    Version = httpmessage.Version,
                    SenderTime = requestInfo.HttpTag.SenderTime,
                    IsAynsc = requestInfo.HttpTag.IsAynsc,
                    Browser = requestInfo.HttpTag.Browser,
                    CIP = requestInfo.HttpTag.CIP,
                    IP = requestInfo.HttpTag.IP,
                    IsPreLoad = requestInfo.HttpTag.IsPreLoad,
                    Other = requestInfo.HttpTag.Other,
                    RequestID = requestInfo.HttpTag.RequestID,
                    Retry = requestInfo.HttpTag.Retry,
                    ExecuteID = requestInfo.HttpTag.ExecuteID.HasValue ? requestInfo.HttpTag.ExecuteID.Value : Guid.Empty,
                    Token = requestInfo.HttpTag.Token,
                    UID = requestInfo.HttpTag.UID,
                    ComputerName = requestInfo.HttpTag.ComputerName,
                    IsNotify = httpmessage.IsNotify,
                    IsProcess = false,
                    ExecuteResult = 0
                };
                if (log.AppID != 99 && log.Method != "ApiCommit" && log.ExecuteID != Guid.Empty)
                {
                    log.IsSuccess = false;
                }
                HttpLogBLL.SaveLog(log);
            }
        }
    }
}