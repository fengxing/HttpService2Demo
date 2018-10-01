using RedisHelp;
using SmartBLL;
using SmartHttp.Network;
using SmartHttpEntity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SmartHttp
{
    public class HttpFacade
    {
        private static ConcurrentDictionary<string, string> httpConfigs = new ConcurrentDictionary<string, string>();
        private static DateTime time = DateTime.Now;
        private const string httpConfigPrefix = "httpconfig-";
        private static RedisHelper redis = Redis.GetHelper();


        public static HttpMessage GetHttpMessage(int appID, string method, HttpTag httpTag)
        {
            var httpMessage = HttpMessageBLL.GetByAppIDAndMethod(appID, method, httpTag.Version);
            return httpMessage;
        }

        public static HttpReturn Request(HttpRequest request, HttpMessage httpMessage, HttpTag httpTag)
        {
            var requestInfo = new RequestInfo()
            {
                HttpRequest = request,
                HttpTag = httpTag,
            };
            try
            {
                if (httpMessage == null)
                {
                    throw new Exception(string.Format("找不到相关接口配置,未调用接口.AppID:{0},Method:{1},Version:{2}", request.AppID, request.Method, httpTag.Version));
                }
                else
                {
                    requestInfo.HttpMessage = httpMessage;
                }
                #region 状态禁用禁止调用

                if (httpMessage.Status != Status.Normal)
                {
                    return new HttpReturn()
                    {
                        Method = httpMessage.Method,
                        Url = httpMessage.Url,
                        AppID = httpMessage.AppID,
                        HttpMethod = httpMessage.HttpType.ToString(),
                        RequestObjs = new List<string>(),
                        Exception = "",
                        ExceptionMessage = "接口被禁用,禁止调用",
                        Response = "",
                        ResponseEncrypt = "",
                        RequestEncrypt = "",
                        Request = "",
                        Moudle = httpMessage.Moudle,
                        StatusCode = 0,
                    };
                }

                #endregion

                if (httpMessage.InterfaceArgsCount != requestInfo.HttpRequest.RequestObjs.Count)
                {
                    throw new BException("请求参数数量异常!");
                }

                #region 判断参数类型和是否为空
                var args = httpMessage.GetInterfaceArgs();
                for (int i = 0; i < args.Count; i++)
                {
                    var arg = args[i];
                    var obj = requestInfo.HttpRequest.RequestObjs[i].Trim();
                    if (arg.IsAllowNull == false)
                    {
                        if (string.IsNullOrWhiteSpace(obj))
                        {
                            var err = arg.Name + "为空";
                            if (httpMessage.IsValid)
                            {
                                var e = httpMessage.GetWsExcepitons().FirstOrDefault(r => r.Name == err);
                                if (e != null)
                                {
                                    throw new BException(e.Value.Trim());
                                }
                            }
                            throw new BException(err);
                        }
                    }
                    if (string.IsNullOrWhiteSpace(obj) == false)
                    {
                        if (arg.Type != "String")
                        {
                            #region 类型判断
                            var isNoneError = true;
                            if (arg.Type == "Guid")
                            {
                                Guid t;
                                isNoneError &= Guid.TryParse(obj, out t);
                            }
                            else if (arg.Type == "Boolean")
                            {
                                isNoneError &= (obj.ToLower() == "true" || obj.ToLower() == "false");
                            }
                            else if (arg.Type == "Int32")
                            {
                                int t;
                                isNoneError &= int.TryParse(obj, out t);
                            }
                            else if (arg.Type == "Decimal")
                            {
                                decimal t;
                                isNoneError &= decimal.TryParse(obj, out t);
                            }
                            else if (arg.Type == "Double")
                            {
                                double t;
                                isNoneError &= double.TryParse(obj, out t);
                            }
                            else if (arg.Type == "Int32[]")
                            {
                                var arr = obj.Split(',');
                                foreach (var item in arr)
                                {
                                    int t;
                                    isNoneError &= int.TryParse(item, out t);
                                }
                            }
                            else if (arg.Type == "Guid[]")
                            {
                                var arr = obj.Split(',');
                                foreach (var item in arr)
                                {
                                    Guid t;
                                    isNoneError &= Guid.TryParse(item, out t);
                                }
                            }
                            if (isNoneError == false)
                            {
                                var err = arg.Name + "类型错误";
                                if (httpMessage.IsValid)
                                {
                                    var e = httpMessage.GetWsExcepitons().FirstOrDefault(r => r.Name == err);
                                    if (e != null)
                                    {
                                        throw new BException(e.Value.Trim());
                                    }
                                }
                                throw new BException(err);
                            }
                            #endregion
                        }
                        else
                        {
                            if (arg.MaxLength.HasValue)
                            {
                                if (obj.Length > arg.MaxLength.Value)
                                {
                                    throw new BException(arg.Name + "最大长度为:" + arg.MaxLength + ",当前长度为:" + obj.Length);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 计算配置

                if (httpConfigs.Count == 0 || DateTime.Now.Subtract(time).TotalMinutes > 3)
                {
                    ProcessConfig();
                }
                if (httpMessage.Url.Contains(httpConfigPrefix))
                {
                    var url = string.Join("/", httpMessage.Url.Split('/').Select(ProcessUrlSegment));
                    httpMessage.Url = url;
                }
                #endregion

                #region 缓存   
                requestInfo.Start();
                if (httpMessage.IsCache && httpMessage.CacheSeconds > 0 && redis != null)
                {
                    var key = request.RequestObjs.Count > 0 ? string.Join("", request.RequestObjs) : "";
                    var token = httpMessage.AppID + httpMessage.Method + httpTag.UID + key;
                    var cache = redis.StringGet(token);
                    if (!string.IsNullOrWhiteSpace(cache))
                    {
                        var ret = new HttpReturn()
                        {
                            IsServiceCache = true,
                            Response = cache,
                            HttpMethod = httpMessage.HttpType.ToString() + "Cache",
                            Exception = "",
                            ExceptionMessage = "",
                            Method = httpMessage.Method,
                            Request = "",
                            RequestObjs = request.RequestObjs,
                            AppID = httpMessage.AppID,
                            RequestEncrypt = "",
                            ResponseEncrypt = "",
                            StatusCode = 200,
                            Url = httpMessage.Url,
                            Moudle = httpMessage.Moudle,
                        };
                        requestInfo.HttpReturn = ret;
                        requestInfo.Stop();
                        requestInfo.Log();
                        return ret;
                    }
                    else
                    {
                        var ret = Processer.Process(requestInfo);
                        if (ret.IsSuccess)
                        {
                            try
                            {
                                redis.StringSet(token, ret.Response, TimeSpan.FromSeconds(httpMessage.CacheSeconds));
                            }
                            catch (Exception)
                            {

                            }
                        }
                        requestInfo.HttpReturn = ret;
                        requestInfo.Stop();
                        requestInfo.Log();
                        return ret;
                    }
                }
                #endregion
                return Processer.Process(requestInfo);
            }
            catch (Exception ex)
            {
                if (ex is BException)
                {
                    return new HttpReturn()
                    {
                        Exception = ex.GetType().Name,
                        ExceptionMessage = ex.Message.Replace("601|", ""),
                        HttpMethod = "",
                        AppID = request.AppID,
                        Method = request.Method,
                        Request = "",
                        RequestObjs = request.RequestObjs,
                        Response = "",
                        Url = "",
                        Moudle = "",
                        ResponseEncrypt = "",
                        RequestEncrypt = "",
                        StatusCode = 601
                    };
                }
                else if (ex is IDRefException)
                {
                    return new HttpReturn()
                    {
                        Exception = ex.GetType().Name,
                        ExceptionMessage = ex.Message.Replace("804|", ""),
                        HttpMethod = "",
                        AppID = request.AppID,
                        Method = request.Method,
                        Request = "",
                        RequestObjs = request.RequestObjs,
                        Response = "",
                        Url = "",
                        Moudle = "",
                        ResponseEncrypt = "",
                        RequestEncrypt = "",
                        StatusCode = 804
                    };
                }
                else
                {
                    return new HttpReturn()
                    {
                        Exception = ex.GetType().Name,
                        ExceptionMessage = ex.Message,
                        HttpMethod = "",
                        AppID = request.AppID,
                        Method = request.Method,
                        Request = "",
                        RequestObjs = request.RequestObjs,
                        Response = "",
                        Url = "",
                        Moudle = "",
                        ResponseEncrypt = "",
                        RequestEncrypt = "",
                        StatusCode = 0
                    };
                }
            }
        }

        /// <summary>
        /// 替换"httpconfig-key"为对应的真正url
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        private static string ProcessUrlSegment(string segment)
        {
            if (string.IsNullOrWhiteSpace(segment))
            {
                return string.Empty;
            }
            if (!segment.Contains(httpConfigPrefix))
            {
                return segment;
            }
            string key = segment.Replace(httpConfigPrefix, "").Trim();
            if (!httpConfigs.ContainsKey(key))
            {
                ProcessConfig(segment);
            }
            return httpConfigs[key].TrimEnd('/');
        }

        private static void ProcessConfig(string config)
        {
            var name = config.Split('-')[1];
            var configs = new HttpConfigBLL().GetAll();
            var isProduct = configs.FirstOrDefault(r => r.Name == "IsProduct");
            var entity = configs.FirstOrDefault(r => r.Name != "IsProduct");
            if (entity != null)
            {
                if (isProduct != null)
                {
                    httpConfigs.AddOrUpdate(entity.Name, entity.ProdcutValue, (key, existingVal) =>
                    {
                        if (existingVal != entity.ProdcutValue)
                        {
                            return entity.ProdcutValue;
                        }
                        else
                        {
                            return existingVal;
                        }
                    });
                }
                else
                {
                    httpConfigs.AddOrUpdate(entity.Name, entity.TestValue, (key, existingVal) =>
                    {
                        if (existingVal != entity.TestValue)
                        {
                            return entity.TestValue;
                        }
                        else
                        {
                            return existingVal;
                        }
                    });
                }
            }
            else
            {
                throw new Exception("httpconfig配置不存在");
            }
        }

        private static void ProcessConfig()
        {
            time = DateTime.Now;
            var configs = new HttpConfigBLL().GetAll();
            var isProduct = configs.FirstOrDefault(r => r.Name == "IsProduct");
            var entitys = configs.Where(r => r.Name != "IsProduct").ToList();
            foreach (var entity in entitys)
            {
                if (isProduct != null)
                {
                    httpConfigs.AddOrUpdate(entity.Name, entity.ProdcutValue, (key, existingVal) =>
                    {
                        if (existingVal != entity.ProdcutValue)
                        {
                            return entity.ProdcutValue;
                        }
                        else
                        {
                            return existingVal;
                        }
                    });
                }
                else
                {
                    httpConfigs.AddOrUpdate(entity.Name, entity.TestValue, (key, existingVal) =>
                    {
                        if (existingVal != entity.TestValue)
                        {
                            return entity.TestValue;
                        }
                        else
                        {
                            return existingVal;
                        }
                    });
                }
            }
        }
    }
}