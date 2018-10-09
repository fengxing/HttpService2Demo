using Newtonsoft.Json;
using SmartBLL;
using SmartHttpEntity;
using SmartHttpWeb.Helper;
using SmartHttpWeb.Models;
using SmartHttpWeb.Models.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace SmartHttpWeb.Controllers
{
    /// <summary>
    /// Http三方通信配置
    /// </summary>
    public class HttpMessageController : BaseController
    {
        #region 页面
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.apps = HttpAppRep.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult GetMoudles(int appID)
        {
            try
            {
                if (Convert.ToInt64(appID) > 0)
                {
                    var app = HttpAppRep.GetByAppID(appID);
                    return Json(new { err = "", result = true, moudles = app.GetMoudles() });
                }
                else
                {
                    throw new BException("应用不存在");
                }
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }

        [HttpPost]
        public ActionResult GetSubMoudles(int appID, string moudle)
        {
            try
            {
                if (Convert.ToInt64(appID) > 0)
                {
                    var app = HttpAppRep.GetByAppID(appID);
                    return Json(new { err = "", result = true, moudles = app.GetSubMoudles(moudle) });
                }
                else
                {
                    throw new BException("应用不存在");
                }
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }






        public ActionResult HttpMessageListView(HttpMessageVM model)
        {
            var ip = Request.ServerVariables.Get("Remote_Addr").ToString();
            var tip = GetHostIP();
            ViewBag.P = true;
            int count = 0;
            var list = new HttpMessageBLL().GetHttpMessageList(model.Method, model.AppID, model.PageIndex, model.Moudle, model.SubMoudle, model.Status, model.IsNotify, out count);
            ViewBag.pager = new PagerV2().GetJumperForAjax(model.PageIndex, 10, (int)count, "searchData({0})");
            return View(list.ToList());
        }
        #endregion

        #region 接口信息

        public ActionResult Add()
        {
            ViewBag.apps = HttpAppRep.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(HttpMessageAddUpdateVM model)
        {
            try
            {
                var entity = model.ToEntity();
                HttpMessageBLL.AddUpdate(entity);
                return Json(new { err = "", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }

        [HttpPost]
        public ActionResult GetHttpUrl(int appID)
        {
            try
            {
                var address = new HttpMessageBLL().GetHttpAddress(appID);
                return Json(new { err = "", data = address, result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }



        public ActionResult Update(Guid id)
        {
            ViewBag.apps = HttpAppRep.GetAll();
            HttpMessage entity = HttpMessageBLL.GetByID(id);
            return View(entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(HttpMessageAddUpdateVM model)
        {
            try
            {
                var entity = model.ToEntity();
                var e = HttpMessageBLL.GetByID(entity.ID);
                entity.Define = e.Define;
                var define = e.GetDefine();
                if (define != null)
                {
                    entity.IsNeedLogin = !define.IsAllowAnonymous;
                }
                var nowinputArgs = entity.GetInterfaceArgs();
                foreach (var item in nowinputArgs)
                {
                    if (define != null)
                    {
                        var d = define.Requests.FirstOrDefault(r => r.Name == item.Description);
                        if (d != null)
                        {
                            item.MaxLength = d.MaxLength;
                        }
                    }
                }
                entity.SetInterfaceArgs(nowinputArgs);
                HttpMessageBLL.AddUpdate(entity);
                return Json(new { err = "", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }


        public ActionResult UpdateValid(Guid id)
        {
            ViewBag.apps = HttpAppRep.GetAll();
            HttpMessage entity = HttpMessageBLL.GetByID(id);
            return View(entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateValid(UpdateValidVM model)
        {
            try
            {
                if (model.Messages == null || model.Messages.Count == 0)
                {
                    throw new BException("参数请求错误");
                }
                var e = HttpMessageBLL.GetByID(model.ID);
                e.IsValid = model.IsValid > 0 ? true : false;
                var es = e.GetWsExcepitons();
                if (es.Count != model.Messages.Count)
                {
                    throw new BException("异常数量变化,请刷新页面重试");
                }
                for (int i = 0; i < es.Count; i++)
                {
                    es[i].Value = model.Messages[i].Trim();
                }
                e.SetWsExcepitons(es);
                HttpMessageBLL.AddUpdate(e);
                return Json(new { err = "", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }


        public ActionResult RunValue(Guid id)
        {
            HttpMessage entity = HttpMessageBLL.GetByID(id);
            ViewBag.args = HttpInvokeArgResp.Get(entity.AppID, entity.Method).ToList();
            return View(entity);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult RunValue(RunValueVM model)
        {
            try
            {
                HttpMessage entity = HttpMessageBLL.GetByID(model.ID);
                var list = new List<HttpInvokeArg>();
                for (int i = 0; i < model.NameArgs.Count; i++)
                {
                    list.Add(new HttpInvokeArg()
                    {
                        AppID = entity.AppID,
                        Method = entity.Method,
                        Name = model.NameArgs[i],
                        Value = model.ValueArgs[i],
                    });
                }
                HttpInvokeArgResp.AddUpdate(entity.AppID, entity.Method, list);
                return Json(new { err = "", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }

        public ActionResult DeleteHttpMessage(Guid id)
        {
            try
            {
                HttpMessageBLL.Delete(id);
                return Json(new { err = "", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }
        #endregion

        #region 配置信息
        /// <summary>
        /// 默认测试环境
        /// </summary>
        /// <returns></returns>
        public ActionResult HttpConfigList()
        {
            return View();
        }

        public ActionResult HttpConfigListView(HttpConfigVM model)
        {
            var whereList = new List<string>();
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                whereList.Add("Name like '%'+@Name+'%'");
            }
            if (!string.IsNullOrWhiteSpace(model.Description))
            {
                whereList.Add("Description like '%'+@Description+'%'");
            }
            var pageT = HttpConfigRep.Search<HttpConfig>(model.PageIndex, 10, false, "CreateTime",
               whereList,
               new
               {
                   Name = model.Name,
                   Description = model.Description
               });

            ViewBag.pager = new PagerV2().GetJumperForAjax(model.PageIndex, 10, pageT.Count, "searchData({0})");
            var vm = new HttpConfigListVM()
            {
                HttpConfigs = pageT.Ts
            };
            return View(vm);
        }


        public ActionResult AddHttpConfig()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddHttpConfig(HttpConfig entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entity.Name))
                {
                    throw new BException("名称为空");
                }
                if (string.IsNullOrWhiteSpace(entity.ProdcutValue))
                {
                    throw new BException("正式值为空");
                }
                if (string.IsNullOrWhiteSpace(entity.TestValue))
                {
                    throw new BException("测试值为空");
                }
                HttpConfigRep.AddUpdate(entity);
                return Json(new { err = "", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }


        public ActionResult UpdateHttpConfig(string id)
        {
            var entity = HttpConfigRep.GetByID(Guid.Parse(id));
            return View(entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateHttpConfig(HttpConfigAddUpdateVM model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    throw new BException("名称为空");
                }
                if (string.IsNullOrWhiteSpace(model.ProdcutValue))
                {
                    throw new BException("正式值为空");
                }
                if (string.IsNullOrWhiteSpace(model.TestValue))
                {
                    throw new BException("测试值为空");
                }
                var entity = BaseRepository<HttpConfig>.GetByID(model.ID);
                entity.Description = model.Description;
                entity.TestValue = model.TestValue;
                entity.ProdcutValue = model.ProdcutValue;
                entity.Name = model.Name;
                HttpConfigRep.AddUpdate(entity);
                return Json(new { err = "", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }


        public ActionResult Clone(string id)
        {
            try
            {
                var entity = HttpMessageBLL.GetByID(Guid.Parse(id));
                entity.Method = entity.Method + "Clone";
                entity.SetID(new HttpMessage().ID);
                entity.RowVersion = -1;
                HttpMessageBLL.AddUpdate(entity);
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult CloneApp(int appID, int toAppID, string configName)
        {
            try
            {
                if (appID <= 0 ||
                    toAppID <= 0 ||
                    appID == toAppID ||
                    string.IsNullOrWhiteSpace(configName))
                {
                    throw new BException("参数不正确");
                }
                new HttpMessageBLL().DeleteApp(toAppID);
                var configs = HttpMessageBLL.GetHttpMessageList(appID);
                foreach (var config in configs)
                {
                    config.AppID = toAppID;
                    config.SetID(new HttpMessage().ID);
                    var arr = config.Url.Split('/').ToList();
                    if (arr.Count > 0 && arr[0].Contains("httpconfig-"))
                    {
                        arr[0] = "httpconfig-" + configName;
                    }
                    config.Url = string.Join("/", arr);
                    config.RowVersion = -1;
                    config.IsNotify = false;
                    HttpMessageBLL.Add(config);
                }
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        #endregion

        #region 应用信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult HttpAppList()
        {
            return View();
        }

        public ActionResult HttpAppListView(HttpConfigVM model)
        {
            var whereList = new List<string>();
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                whereList.Add("Name like '%'+@Name+'%'");
            }
            if (!string.IsNullOrWhiteSpace(model.Description))
            {
                whereList.Add("Description like '%'+@Description+'%'");
            }
            var pageT = HttpAppRep.Search<HttpApp>(model.PageIndex, 10, false, "AppID",
               whereList,
               new
               {
                   Name = model.Name,
                   Description = model.Description
               });
            ViewBag.pager = new PagerV2().GetJumperForAjax(model.PageIndex, 10, pageT.Count, "searchData({0})");
            return View(pageT.Ts);
        }


        public ActionResult AddHttpApp()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddHttpApp(HttpAppAddUpdateVM model)
        {
            try
            {
                var entity = model.ToEntity();
                HttpAppRep.AddUpdate(entity);
                return Json(new { err = "", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }


        public ActionResult UpdateHttpApp(Guid id)
        {
            ViewBag.apps = HttpAppRep.GetAll();
            var entity = HttpAppRep.GetByID(id);
            return View(entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateHttpApp(HttpAppAddUpdateVM model)
        {
            try
            {
                var entity = model.ToEntity();
                HttpAppRep.AddUpdate(entity);
                return Json(new { err = "", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }

        [HttpPost]
        public ActionResult GetHttpApp(int appID)
        {
            try
            {
                if (Convert.ToInt64(appID) > 0)
                {
                    var app = HttpAppRep.GetByAppID(appID);
                    return Json(new { err = "", result = true, app = app });
                }
                else
                {
                    throw new BException("应用不存在");
                }
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }

        public ActionResult DeleteHttpApp(Guid id)
        {
            try
            {
                var httpApp = HttpAppRep.GetByID(id);
                if (httpApp != null)
                {
                    HttpAppRep.Delete(httpApp);
                }
                return Json(new { err = "", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }
        #endregion

        #region 日志
        public ActionResult HttpMessageLogList()
        {
            ViewBag.apps = HttpAppRep.GetAll();
            return View();
        }

        public ActionResult HttpMessageLogListView(HttpMessageVM model)
        {
            int count = 0;
            var list = new HttpLogInterfaceCallBLL().GetHttpLogInterfaceCallList(model.AppID, model.Method, model.IsSuccess, model.Args, model.ExecuteID, model.UID, model.Code, model.PageIndex, out count);
            ViewBag.pager = new PagerV2().GetJumperForAjax(model.PageIndex, 10, count, "searchData({0})");
            return View(list.ToList());


        }
        #endregion

        #region 模拟调用
        public ActionResult Invoke(Guid? id, Guid? logid)
        {
            if (id.HasValue)
            {
                var entity = HttpMessageBLL.GetByID(id.Value);
                ViewBag.args = HttpInvokeArgResp.Get(entity.AppID, entity.Method).ToList();
                var d = entity.GetDefine();
                var jsonArgs = new Dictionary<string, string>();
                foreach (var arg in d.Requests)
                {
                    if (arg.IsNullable == false)
                    {
                        if (arg.Type == "Boolean")
                        {
                            jsonArgs.Add(arg.Name, "false");
                        }
                        else if (arg.Type == "Int32")
                        {
                            if (arg.Name == "PageIndex")
                            {
                                jsonArgs.Add(arg.Name, "1");
                            }
                            else if (arg.Name == "PageSize")
                            {
                                jsonArgs.Add(arg.Name, "10");
                            }
                            else
                            {
                                int value = 0;
                                if (arg.ValueDescriptions != null && arg.ValueDescriptions.Count() > 0)
                                {
                                    var arr = arg.ValueDescriptions.FirstOrDefault().Split(':');
                                    if (arr.Count() > 0)
                                    {
                                        var v = arr[0];
                                        int.TryParse(v, out value);
                                    }
                                }
                                jsonArgs.Add(arg.Name, value.ToString());
                            }
                        }
                        else if (arg.Type == "Decimal" ||
                                    arg.Type == "Double" ||
                                    arg.Type == "Int64")
                        {
                            jsonArgs.Add(arg.Name, "0");
                        }
                        else if (arg.Type == "DateTime")
                        {
                            jsonArgs.Add(arg.Name, DateTime.Now.ToString("yyyy-MM-dd"));
                        }
                        else if (arg.Type == "Guid")
                        {
                            var name = arg.Name.TrimEnd('D').TrimEnd('I');
                            if (arg.Name == "ID")
                            {
                                if (arg.ValueDescriptions != null && arg.ValueDescriptions.Count() > 0)
                                {
                                    var arr = arg.ValueDescriptions.FirstOrDefault();
                                    var s = new StringBuilder();
                                    var t = arr;
                                    for (int i = 0; i < t.Length; i++)
                                    {
                                        if ((int)t[i] >= 65 && (int)t[i] <= 90)
                                        {
                                            s.Append(t[i]);
                                        }
                                        if ((int)t[i] >= 97 && (int)t[i] <= 122)
                                        {
                                            s.Append(t[i]);
                                        }
                                    }
                                    name = s.ToString().TrimEnd('D').TrimEnd('I');
                                }
                            }
                            if (name.IsNotNull())
                            {
                                var ret = SmartSDKHelper.SmartOutSideHelper.Request(new SmartSDKHelper.HttpInvoke()
                                {
                                    AppID = 16,
                                    Method = "GetLastGuid",
                                    RequestObjs = new List<string>() {
                                                    entity.AppID.ToString(),
                                                    name
                                            }
                                });
                                if (ret.IsSuccess)
                                {
                                    var j = Newtonsoft.Json.Linq.JObject.Parse(ret.Data);
                                    if (j.ToString() != "00000000-0000-0000-0000-000000000000")
                                    {
                                        jsonArgs.Add(arg.Name, j["ID"].ToString());
                                    }
                                    else
                                    {
                                        jsonArgs.Add(arg.Name, "");
                                    }
                                }
                                else
                                {
                                    jsonArgs.Add(arg.Name, "");
                                }
                            }
                            else
                            {
                                jsonArgs.Add(arg.Name, "");
                            }
                        }
                        else
                        {
                            jsonArgs.Add(arg.Name, "");
                        }
                    }
                    else
                    {
                        jsonArgs.Add(arg.Name, "");
                    }
                }
                ViewBag.invokes = jsonArgs.Select(r => r.Value).ToArray();
                return View(entity);
            }
            if (logid.HasValue)
            {
                var log = new HttpLogInterfaceCallBLL().GetByID(logid.Value);
                var entity = HttpMessageBLL.GetByAppIDAndMethod(log.AppID, log.Method, log.Version);
                ViewBag.args = HttpInvokeArgResp.Get(entity.AppID, entity.Method).ToList();
                if (!string.IsNullOrWhiteSpace(log.Request))
                {
                    var jsonArgs = JsonConvert.DeserializeObject<IDictionary<string, string>>(log.Request);
                    ViewBag.invokes = jsonArgs.Select(r => r.Value).ToArray();
                }
                return View(entity);
            }
            return Content("error");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Invoke(Guid id, string token, string uid, List<string> ValueArgs, string version, string HashCode, int? Sort)
        {
            try
            {
                var ip = Request.ServerVariables.Get("Remote_Addr").ToString();
                var httpMessage = HttpMessageBLL.GetByID(id);
                var r = HttpRequestHelper.Request(ip, httpMessage, token, uid, ValueArgs, version, HashCode, Sort);
                if (r.result)
                {
                    return Json(new { err = "", result = true, request = r.request, response = r.response, url = r.url });
                }
                else
                {
                    return Json(new { err = r.err, result = false, request = r.request, response = r.response, url = r.url });
                }
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false, request = "", response = "", url = "" });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetRequestArg(Guid id)
        {
            try
            {
                //String,Guid,Int32,Boolean,Decimal,Double,DateTime,Int64
                var httpMessage = HttpMessageBLL.GetByID(id);
                var response = "";
                var args = httpMessage.GetDefine();
                if (args.Requests.Count >= 0)
                {
                    var name = httpMessage.Method;
                    response = "var " + name + " = {";
                    if (!args.Requests.Select(r => r.Name).Contains("AppID"))
                    {
                        response += "\r\nAppID:" + httpMessage.AppID + ",";
                    }
                    else
                    {
                        response += "\r\nTAppID:" + httpMessage.AppID + ",";
                    }
                    if (!args.Requests.Select(r => r.Name).Contains("Method"))
                    {
                        response += "\r\nMethod:'" + httpMessage.Method + "',";
                    }
                    else
                    {
                        response += "\r\nTMethod:'" + httpMessage.Method + "',";
                    }
                    foreach (var arg in args.Requests)
                    {
                        response += "\r\n";
                        response = response + arg.Name;
                        var des = "";
                        if (!string.IsNullOrWhiteSpace(arg.Description))
                        {
                            des += arg.Description;
                        }
                        if (arg.ValueDescriptions != null && arg.ValueDescriptions.Length > 0)
                        {
                            des += ":" + string.Join(",", arg.ValueDescriptions);
                        }
                        if (!string.IsNullOrWhiteSpace(des))
                        {
                            des = "//" + des;
                        }
                        if (arg.IsNullable)
                        {
                            response += ":''," + des;
                        }
                        else
                        {
                            #region String DateTime
                            if (arg.Type == "String")
                            {
                                response += ":''," + des;
                            }
                            else if (arg.Type == "DateTime")
                            {
                                response += ":'yyyy-MM-dd'," + des;
                            }
                            #endregion
                            #region Guid
                            else if (arg.Type == "Guid")
                            {
                                response += ":''," + des;
                            }
                            #endregion
                            #region Boolean
                            else if (arg.Type == "Boolean")
                            {
                                response += ":false," + des;
                            }
                            #endregion
                            #region Int32
                            else if (arg.Type == "Int32")
                            {
                                int value = 0;
                                if (arg.ValueDescriptions != null && arg.ValueDescriptions.Count() > 0)
                                {
                                    var arr = arg.ValueDescriptions.FirstOrDefault().Split(':');
                                    if (arr.Count() > 0)
                                    {
                                        var v = arr[0];
                                        int.TryParse(v, out value);
                                    }
                                }
                                response += ":" + value + "," + des;
                            }
                            #endregion
                            #region Decimal Double Int64
                            else if (arg.Type == "Decimal" ||
                                     arg.Type == "Double" ||
                                     arg.Type == "Int64")
                            {
                                response += ":0," + des;
                            }
                            #endregion
                        }
                    }
                    response += " \r\n};\r\n";
                }
                return Json(new { err = "", result = true, response = response });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false, request = "", response = "", url = "" });
            }
        }


        private static string GetHostIP()
        {
            try
            {
                return Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
            }
            catch
            {
                return "";
            }
        }

        private static string GetHostName()
        {
            try
            {
                return Dns.GetHostName();
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 模拟数据
        [HttpPost]
        public ActionResult GetToken(Guid id, string Login)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Login))
                {
                    var httpMessage = HttpMessageBLL.GetByID(id);
                    var arr = Login.Split(',');
                    if (arr.Length != 2)
                    {
                        throw new BException("帐号密码参数异常");
                    }
                    var account = arr[0].Trim();
                    var pwd = arr[1].Trim().ToMd532Upper();
                    var appid = httpMessage.AppID;
                    SmartSDKHelper.Result ret = null;
                    if (appid != 8 && appid != 9 && appid != 10 && appid != 11)
                    {
                        ret = SmartSDKHelper.SmartOutSideHelper.Request(new SmartSDKHelper.HttpInvoke()
                        {
                            AppID = 7,
                            Method = "Login",
                            Version = "1.0",
                            CanRetry = false,
                            RequestObjs = new List<string>() { "20", pwd, account },
                            RetryTimes = 0,
                            SenderTime = DateTime.Now,
                            ID = Guid.NewGuid().ToString()
                        });
                    }
                    else
                    {
                        ret = SmartSDKHelper.SmartOutSideHelper.Request(new SmartSDKHelper.HttpInvoke()
                        {
                            AppID = appid,
                            Method = "Login",
                            Version = "1.0",
                            CanRetry = false,
                            RequestObjs = new List<string>() { pwd, account },
                            RetryTimes = 0,
                            SenderTime = DateTime.Now,
                            ID = Guid.NewGuid().ToString()
                        });
                    }

                    if (!ret.IsSuccess)
                    {
                        throw new BException(ret.ToString());
                    }
                    return Json(new { err = "", result = true, data = ret });
                }
                else
                {
                    throw new BException("请输入帐号密码,用逗号分割");
                }
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }

        #endregion

        /// <summary>
        /// 服务发现
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetServices(string id)
        {
            try
            {
                var entity = HttpMessageBLL.GetByID(Guid.Parse(id));
                if (string.IsNullOrWhiteSpace(entity.Url))
                {
                    throw new BException("url地址为空");
                }
                else
                {
                    var url = entity.Url.Trim();
                    var route = "";
                    if (url.Contains("httpconfig"))
                    {
                        var arr = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        var first = arr[0].Replace("httpconfig-", "");
                        var config = HttpConfigRep.GetAll().Where(r => r.Name == first).FirstOrDefault();
                        url = config.TestValue + "/Find/SearchService";
                        arr.RemoveAt(0);
                        route = string.Join("/", arr).Trim();
                    }
                    else
                    {
                        var arr = url.Replace("http://", "").Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        url = "http://" + arr[0] + "/Find/SearchService";
                        arr.RemoveAt(0);
                        route = string.Join("/", arr).Trim();
                    }
                    HttpHelper http = new HttpHelper() { };
                    http.ContentType = "text/html";
                    http.datas = JsonConvert.SerializeObject(new { Route = route });
                    var ret = http.PostRequest(url);
                    if (ret.StatusCode == 200)
                    {
                        var response = JsonConvert.DeserializeObject<SearchServiceResponse>(ret.Return);
                        entity.ContentType = "text/html";
                        entity.Description = response.ServcieName;
                        entity.Method = response.RelativePath.Split('/')[1];
                        entity.HttpType = response.Method == "POST" ? HttpType.HttpPost : HttpType.HttpGet;
                        entity.IsValid = false;
                        if (entity.HttpType == HttpType.HttpPost)
                        {
                            entity.WebServiceTemplate = response.Request;
                        }
                        else
                        {
                            entity.WebServiceTemplate = "";
                        }
                        if (response.Requests != null && response.Requests.Count > 0)
                        {
                            if (entity.InterfaceArgsCount > 0 && entity.InterfaceArgsCount != response.Requests.Count)
                            {
                                throw new BException("参数数量不一致，禁止整体服务发现，建议手工清除参数再次重试");
                            }
                            var args = new List<InterfaceArg>();
                            foreach (var request in response.Requests)
                            {
                                args.Add(new InterfaceArg()
                                {
                                    Description = request.Name.ToTrim(),
                                    Name = request.Description.ToTrim() == "" ? request.Name.ToTrim() : request.Description.ToTrim().Replace("\n", "").Replace(" ", ""),
                                    Type = request.Type.ToTrim(),
                                    IsAllowNull = request.IsNullable,
                                    MaxLength = request.MaxLength,
                                });
                            }
                            entity.SetInterfaceArgs(args);
                        }
                        if (response.Request != null)
                        {
                            var newExs = new List<WsExcepiton>();
                            foreach (var request in response.Requests)
                            {
                                if (request.IsNullable == false)
                                {
                                    var a = request.Description + "为空";
                                    var t = newExs.FirstOrDefault(r => r.Name == a);
                                    if (t == null)
                                    {
                                        newExs.Add(new WsExcepiton()
                                        {
                                            Name = a,
                                            Value = a
                                        });
                                    }
                                }
                                if (request.Type.Contains("String") == false)
                                {
                                    var a = request.Description + "类型错误";
                                    var t = newExs.FirstOrDefault(r => r.Name == a);
                                    if (t == null)
                                    {
                                        newExs.Add(new WsExcepiton()
                                        {
                                            Name = a,
                                            Value = a,
                                        });
                                    }
                                }
                            }
                            if (response.Exceptions != null)
                            {
                                foreach (var item in response.Exceptions)
                                {
                                    newExs.Add(new WsExcepiton()
                                    {
                                        Name = item,
                                        Value = item
                                    });
                                }
                            }
                            var newAdds = new List<WsExcepiton>();
                            var existargs = entity.GetWsExcepitons();
                            foreach (var item in newExs)
                            {
                                var a = existargs.FirstOrDefault(r => r.Name == item.Name);
                                if (a != null)
                                {
                                    newAdds.Add(a);
                                }
                                else
                                {
                                    newAdds.Add(item);
                                }
                            }
                            entity.SetWsExcepitons(newAdds);
                        }
                        entity.Define = ret.Return;
                        var d = entity.GetDefine();
                        if (d != null)
                        {
                            entity.IsNeedLogin = !d.IsAllowAnonymous;
                        }
                        else
                        {
                            entity.IsNeedLogin = false;
                        }
                        HttpMessageBLL.AddUpdate(entity);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(ret.Return))
                        {
                            throw new BException(ret.Return);
                        }
                        else
                        {
                            throw new BException("未知异常");
                        }
                    }
                    return Content("ok");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult DownServices(string id)
        {
            try
            {
                var entity = HttpMessageBLL.GetByID(Guid.Parse(id));
                var interfaceArgs = entity.GetInterfaceArgs();
                var exceptionArgs = entity.GetWsExcepitons();
                if (string.IsNullOrWhiteSpace(entity.Url))
                {
                    throw new BException("url地址为空");
                }
                else
                {
                    var service = entity.GetDefine();
                    if (service == null)
                    {
                        service = new SearchServiceResponse();
                    }
                    #region 模板处理
                    string template = System.AppDomain.CurrentDomain.BaseDirectory + @"File\template.html";
                    if (!System.IO.File.Exists(template))
                    {
                        throw new BException("模板文件不存在");
                    }
                    var stream = new FileStream(template, FileMode.Open, FileAccess.Read);
                    var txt = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
                    stream.Close();
                    var replaces = new Dictionary<string, string>();
                    replaces.Add("Login", service.ServcieName.ToTrim());
                    replaces.Add("@Version", entity.Version.ToTrim());
                    replaces.Add("@AppID", entity.AppID.ToString());
                    replaces.Add("@IsAllowAnonymous", service.IsAllowAnonymous ? "否" : "是");
                    replaces.Add("@IsCache", entity.IsCache ? "是" : "否");
                    if (entity.IsCache)
                    {
                        replaces.Add("@CacheSecondTag", "<div class=\"red notice-title\">缓存时间:" + entity.CacheSeconds + "秒</div>");
                    }
                    else
                    {
                        replaces.Add("@CacheSecondTag", "");
                    }
                    replaces.Add("@Description", entity.Description.ToTrim());
                    var requestHtml = "无";
                    if (service.Requests.Count > 0)
                    {
                        requestHtml = "<tr><th width=\"5%\">序号</th><th width=\"20%\">参数名称</th><th width=\"20%\">类型</th><th width=\"20%\">是否可为空</th><th width=\"35%\">描述</th></tr>";
                        var i = 1;
                        foreach (var request in service.Requests)
                        {
                            if (interfaceArgs.Any(r => r.Description == request.Name || r.Name == request.Description))
                            {
                                var targs = interfaceArgs.FirstOrDefault(r => r.Description == request.Name || r.Name == request.Description);
                                var values = "";
                                if (request.ValueDescriptions != null)
                                {
                                    foreach (var item in request.ValueDescriptions)
                                    {
                                        var p = item;
                                        if (!item.StartsWith("maxlength:"))
                                        {
                                            if (string.IsNullOrWhiteSpace(values))
                                            {
                                                values += p;
                                            }
                                            else
                                            {
                                                values += "<br />" + p;
                                            }
                                        }
                                    }
                                }
                                var maxLength = "";
                                if (request.MaxLength.HasValue)
                                {
                                    maxLength = "<span style='color:red;'>" + "(" + request.MaxLength.Value + ")" + "</span>";
                                }
                                if (request.IsNullable)
                                {
                                    requestHtml += "<tr><td>" + i + "</td><td>" + targs.Name.ToTrim() + "</td><td>" + request.Type.ToTrim() + "</td><td style=\"color:red;\">" + request.IsNullable.ToString() + "</td><td>" + values.ToTrim() + "</td></tr>";
                                }
                                else
                                {
                                    requestHtml += "<tr><td>" + i + "</td><td>" + targs.Name.ToTrim() + "</td><td>" + request.Type.ToTrim() + maxLength + "</td><td>" + request.IsNullable.ToString() + "</td><td>" + values.ToTrim() + "</td></tr>";
                                }
                                i = i + 1;
                            }
                        }
                    }
                    replaces.Add("@Request", requestHtml.ToTrim());
                    var responseClass = "";
                    var responseHtml = "无";
                    if (service.Responses.Count > 0)
                    {
                        responseHtml = "<tr><th width=\"20%\">参数</th><th width=\"20%\">类型</th><th width=\"20%\">是否可为空</th><th width=\"40%\">描述</th></tr>";
                        foreach (var response in service.Responses)
                        {
                            var description = response.Description.ToTrim();
                            var maxLength = "";
                            if (response.ValueDescriptions != null)
                            {
                                foreach (var item in response.ValueDescriptions)
                                {
                                    if (!item.StartsWith("maxlength:"))
                                    {
                                        if (string.IsNullOrWhiteSpace(description))
                                        {
                                            description += item;
                                        }
                                        else
                                        {
                                            description += "<br />" + item;
                                        }
                                    }
                                    else
                                    {
                                        maxLength = item.Replace("maxlength:", "(") + ")";
                                    }
                                }
                            }
                            var type = response.Type;
                            if (response.ClassDescriptions != null && response.ClassDescriptions.Count > 0)
                            {
                                type = "List<" + type + ">";
                            }
                            if (response.IsNullable)
                            {
                                responseHtml += "<tr><td>" + response.Name.ToTrim() + "</td><td>" + response.Type.ToTrim() + "?" + "</td><td style='color:red;'>" + response.IsNullable.ToString() + "</td><td>" + description + "</td></tr>";
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(maxLength))
                                {
                                    maxLength = "<span style='color:red;'>" + maxLength + "</span>";
                                }
                                responseHtml += "<tr><td>" + response.Name.ToTrim() + "</td><td>" + response.Type.ToTrim() + maxLength + "</td><td>" + response.IsNullable.ToString() + "</td><td>" + description + "</td></tr>";
                            }
                            if (response.ClassDescriptions != null && response.ClassDescriptions.Count > 0 && response.Type != "DateTime")
                            {
                                if (responseClass == "")
                                {
                                    responseClass = "<div class=\"item-lft\">相关参数具体信息</div>";
                                }
                                responseClass += GetClassHtml(response);
                                var existClass = new List<string>();
                                var innerClass = response.GetInnerServices();
                                foreach (var item in innerClass)
                                {
                                    if (existClass.Contains(item.Name) == false)
                                    {
                                        existClass.Add(item.Name);
                                        responseClass += GetClassHtml(item);
                                    }
                                }
                                foreach (var item in innerClass)
                                {
                                    var innerServices = item.GetInnerServices();
                                    foreach (var innerService in innerServices)
                                    {
                                        if (existClass.Contains(innerService.Name) == false)
                                        {
                                            existClass.Add(innerService.Name);
                                            responseClass += GetClassHtml(innerService);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    replaces.Add("@ResponseClass", responseClass.ToTrim());
                    replaces.Add("@Response", responseHtml.ToTrim());
                    var exceptionHtml = "无";
                    if (exceptionArgs.Count > 0)
                    {
                        exceptionHtml = "";
                        foreach (var exception in exceptionArgs)
                        {
                            var error = "";
                            if (entity.IsValid)
                            {
                                error = exception.Value;
                            }
                            else
                            {
                                error = exception.Name;
                            }
                            exceptionHtml += "<tr><td>" + error.ToTrim() + "</td></tr>";
                        }
                    }
                    replaces.Add("@Exception", exceptionHtml.ToTrim());
                    foreach (var replace in replaces)
                    {
                        txt = txt.Replace(replace.Key, replace.Value);
                    }
                    #endregion
                    var bt = Encoding.UTF8.GetBytes(txt);
                    var result = new FileContentResult(bt, "text/html")
                    {
                        FileDownloadName = entity.Method + "[" + entity.AppID + "][" + entity.Version + "]接口文档.html",
                    };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        private string GetClassHtml(ServiceAttribute response)
        {
            var responseClass = "<div class=\"item-rt\"><table width=\"100%\"><tbody><tr><th colspan=\"4\" style=\"background-color:#95e2ff\">" + response.Type.ToTrim().Replace("[]", "") + "</th></tr>";
            responseClass += "<tr><th width=\"20%\">参数</th><th width=\"20%\">类型</th><th width=\"20%\">是否可为空</th><th width=\"40%\">描述</th></tr>";
            foreach (var classDescription in response.ClassDescriptions)
            {
                var d = classDescription.Description.ToTrim();
                var maxLength = "";
                if (classDescription.ValueDescriptions != null)
                {
                    foreach (var item in classDescription.ValueDescriptions)
                    {
                        if (!item.StartsWith("maxlength:"))
                        {
                            if (string.IsNullOrWhiteSpace(d))
                            {
                                d += item;
                            }
                            else
                            {
                                d += "<br />" + item;
                            }
                        }
                        else
                        {
                            maxLength = item.Replace("maxlength:", "(") + ")";
                        }
                    }
                }
                if (classDescription.IsNullable)
                {
                    responseClass += "<tr><td>" + classDescription.Name.ToTrim() + "</td><td>" + classDescription.Type.ToTrim() + "?" + "</td><td style='color:red;'>" + classDescription.IsNullable + "</td><td>" + d + "</td></tr>";
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(maxLength))
                    {
                        maxLength = "<span style='color:red;'>" + maxLength + "</span>";
                    }
                    responseClass += "<tr><td>" + classDescription.Name.ToTrim() + "</td><td>" + classDescription.Type.ToTrim() + maxLength + "</td><td>" + classDescription.IsNullable + "</td><td>" + d + "</td></tr>";
                }
            }
            responseClass += "</tbody></table></div><div class=\"item-lft\"></div>";
            return responseClass;
        }

        [HttpGet]
        public ActionResult DownInterfaces(string id)
        {
            try
            {
                var entity = HttpMessageBLL.GetByID(Guid.Parse(id));
                var interfaceArgs = entity.GetInterfaceArgs();
                var exceptionArgs = entity.GetWsExcepitons();
                if (string.IsNullOrWhiteSpace(entity.Url))
                {
                    throw new BException("url地址为空");
                }
                else
                {
                    var service = entity.GetDefine();
                    if (service == null)
                    {
                        service = new SearchServiceResponse();
                    }
                    #region 模板处理
                    string template = System.AppDomain.CurrentDomain.BaseDirectory + @"File\interface.html";
                    if (!System.IO.File.Exists(template))
                    {
                        throw new BException("模板文件不存在");
                    }
                    var stream = new FileStream(template, FileMode.Open, FileAccess.Read);
                    var txt = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
                    stream.Close();
                    var replaces = new Dictionary<string, string>();
                    var inteface = entity.Method + "(";
                    if (service.Requests != null && service.Requests.Count > 0)
                    {
                        if (service.Requests.Count == 1)
                        {
                            inteface += service.Requests[0].Type + " " + service.Requests[0].Name;
                        }
                        else
                        {
                            inteface += "Request request";
                        }
                    }
                    inteface += ")";
                    if (service.Responses != null && service.Responses.Count > 0)
                    {
                        if (service.Responses.Count == 1)
                        {
                            inteface += " : " + service.Responses[0].Type;
                        }
                        else
                        {
                            inteface += " :Response";
                        }
                    }
                    else
                    {
                        inteface += " :void";
                    }
                    replaces.Add("@Interface", inteface);
                    replaces.Add("@MethodDesc", service.ServcieName.ToTrim());
                    if (service.Requests != null && service.Requests.Count > 1)
                    {
                        var dd = "";
                        foreach (var item in service.Requests)
                        {
                            if (interfaceArgs.Any(r => r.Description == item.Name))
                            {
                                var inter = interfaceArgs.FirstOrDefault(r => r.Description == item.Name);
                                var type = item.Type.ToTrim() + (item.IsNullable ? "?" : "");
                                dd += "<dd>" + item.Name.ToTrim() + " { get; set; } : " + type + "<span>" + inter.Name.ToTrim() + "</span></dd>";
                            }
                        }
                        var request = "<div><div class=\"data data_2\"><div class=\"tit\"><p>Request</p><p>类</p></div><dl><dt>属性</dt>" + dd + "</dl></div></div>";
                        replaces.Add("@Request", request.ToTrim());
                    }
                    else
                    {
                        replaces.Add("@Request", "");
                    }
                    if (service.Responses != null && service.Responses.Count > 1)
                    {
                        var response = new StringBuilder();
                        var dd = "";
                        foreach (var item in service.Responses)
                        {
                            var type = item.Type.ToTrim() + (item.IsNullable ? "?" : "");
                            dd += "<dd>" + item.Name.ToTrim() + " { get; set; } : " + type + "<span>" + item.Description.ToTrim() + "</span></dd>";
                        }
                        response.Append("<div><div class=\"data data_2\"><div class=\"tit\"><p>Response</p><p>类</p></div><dl><dt>属性</dt>" + dd + "</dl></div></div>");
                        foreach (var item in service.Responses)
                        {
                            var classd = "";
                            if (item.ClassDescriptions != null && item.Type != "DateTime")
                            {
                                foreach (var classD in item.ClassDescriptions)
                                {
                                    var type = classD.Type.ToTrim() + (classD.IsNullable ? "?" : "");
                                    classd += "<dd>" + classD.Name.ToTrim() + " { get; set; } : " + type + "<span>" + classD.Description.ToTrim() + "</span></dd>";
                                }
                                response.Append("<div><div class=\"data data_2\"><div class=\"tit\"><p>" + item.Type.Replace("[]", "") + "</p><p>类</p></div><dl><dt>属性</dt>" + classd + "</dl></div></div>");
                            }
                        }
                        replaces.Add("@Response", response.ToString().ToTrim());
                    }
                    else if (service.Responses != null && service.Responses.Count == 1)
                    {
                        if (service.Responses[0].ClassDescriptions != null)
                        {
                            var item = service.Responses[0];
                            var classd = "";
                            foreach (var classD in (item.ClassDescriptions))
                            {
                                var type = classD.Type.ToTrim() + (classD.IsNullable ? "?" : "");
                                classd += "<dd>" + classD.Name.ToTrim() + " { get; set; } : " + type + "<span>" + classD.Description.ToTrim() + "</span></dd>";
                            }
                            replaces.Add("@Response", "<div><div class=\"data data_2\"><div class=\"tit\"><p>" + item.Type.Replace("[]", "") + "</p><p>类</p></div><dl><dt>属性</dt>" + classd + "</dl></div></div>");
                        }
                        else
                        {
                            replaces["@MethodDesc"] = replaces["@MethodDesc"] + "(返回" + service.Responses[0].Name + ")";
                            replaces.Add("@Response", "");
                        }
                    }
                    else
                    {
                        replaces.Add("@Response", "");
                    }
                    foreach (var replace in replaces)
                    {
                        txt = txt.Replace(replace.Key, replace.Value);
                    }
                    #endregion
                    var bt = Encoding.UTF8.GetBytes(txt);
                    var result = new FileContentResult(bt, "text/html")
                    {
                        FileDownloadName = entity.Method + "[" + entity.AppID + "][" + entity.Version + "]UML文档.html",
                    };
                    return result;
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult DownAppError(int appID)
        {
            var httpMessages = HttpMessageBLL.GetHttpMessageList(appID);
            var info = new ErrorInfo()
            {
                AppID = appID,
            };
            foreach (var item in httpMessages)
            {
                var error = new ServerError()
                {
                    M = item.Method.ToTrim(),
                    ES = new List<Error>()
                };
                var exs = item.GetWsExcepitons();
                foreach (var ex in exs)
                {
                    error.ES.Add(new Error()
                    {
                        S = ex.Name,
                        C = ex.Value
                    });
                }
                info.Errors.Add(error);
            }
            var js = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            var bt = Encoding.UTF8.GetBytes(js);
            var result = new FileContentResult(bt, "application/x-javascript")
            {
                FileDownloadName = appID + ".js"
            };
            return result;

        }




        public class ErrorInfo
        {
            public int AppID { get; set; }

            public List<ServerError> Errors { get; set; }
            public ErrorInfo()
            {
                this.Errors = new List<ServerError>();
            }

        }

        public class ServerError
        {
            /// <summary>
            /// 
            /// </summary>
            public string M { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public List<Error> ES { get; set; }

            public ServerError()
            {
                this.ES = new List<Error>();
            }
        }

        public class Error
        {
            public string S { get; set; }

            public string C { get; set; }
        }

    }
}

