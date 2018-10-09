using SmartBLL;
using SmartHttpEntity;
using SmartHttpWeb.Models.HttpNotify;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartHttpWeb.Controllers
{
    public class NotifyController : BaseController
    {
        public ActionResult Index(Guid id)
        {
            var httpMessage = HttpMessageBLL.GetByID(id);
            var httpNotify = new HttpNotifyRep().GetHttpNotify(httpMessage.AppID, httpMessage.Method);
            if (httpNotify != null)
            {
                return RedirectToAction("Update", new { id = httpNotify.ID });
            }
            else
            {
                return RedirectToAction("Add", new { id = httpMessage.ID });
            }
        }


        public ActionResult Update(Guid id)
        {
            var httpNotify = new HttpNotifyRep().GetByID(id);
            return View(httpNotify);
        }

        public ActionResult Add(Guid id)
        {
            ViewBag.httpmessageID = id;
            return View();
        }

        [HttpPost]
        public ActionResult Update(HttpNotifyAddUpdate model)
        {
            try
            {
                var entity = new HttpNotifyRep().GetByID(model.ID);
                if (model.Names == null || model.Names.Length == 0)
                {
                    new HttpNotifyRep().Delete(entity);
                }
                else
                {
                    entity = model.ToEntity(entity);
                    new HttpNotifyRep().Update(entity);
                }
                return Json(new { err = "", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }

        }

        [HttpPost]
        public ActionResult Add(HttpNotifyAddUpdate model)
        {
            try
            {
                var httpMessage = HttpMessageBLL.GetByID(model.HttpMessageID);
                var entity = model.ToEntity(null);
                entity.AppID = httpMessage.AppID;
                entity.Method = httpMessage.Method;
                entity.Version = httpMessage.Version;
                new HttpNotifyRep().Add(entity);
                return Json(new { err = "", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message, result = false });
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class GetUsersResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UserItem> UserItems { get; set; }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserItem
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
    }
}