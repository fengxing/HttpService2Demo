using System;
using System.Collections.Generic;

namespace SmartHttpWeb.Models.HttpNotify
{
    public class HttpNotifyAddUpdate
    {
        public Guid HttpMessageID { get; set; }

        public Guid ID { get; set; }

        public string[] Names { get; set; }
        public string[] DingUserIDs { get; set; }

        /// <summary>
        /// 要通知的应用编号
        /// <value>maxlength:2000</value>
        /// </summary>
        public string AppIDs { get; set; }

        /// <summary>
        /// 通知的消息模板
        /// <value>maxlength:500</value>
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 通知的消息模板参数路径
        /// <value>maxlength:2000</value>
        /// </summary>
        public string MessagePaths { get; set; }


        public SmartHttpEntity.HttpNotify ToEntity(SmartHttpEntity.HttpNotify entity)
        {
            if (entity == null)
            {
                entity = new SmartHttpEntity.HttpNotify() { };
            }
            var arr = new List<NotifyUser>();
            if (this.Names != null)
            {
                for (int i = 0; i < this.Names.Length; i++)
                {
                    arr.Add(new NotifyUser()
                    {
                        DingUserID = this.DingUserIDs[i].ToTrim(),
                        Name = this.Names[i].ToTrim()
                    });
                }
            }
            entity.NotifyUsers = Newtonsoft.Json.JsonConvert.SerializeObject(arr);
            entity.AppIDs = AppIDs.ToTrim();
            entity.Message = Message.ToTrim();
            entity.MessagePaths = MessagePaths.ToTrim();
            return entity;
        }

    }
}