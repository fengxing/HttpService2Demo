using SmartHttpEntity;
using System;
using System.Collections.Generic;

namespace SmartHttpWeb.Models
{
    public class HttpAppAddUpdateVM
    {

        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 程序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }


        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }


        /// <summary>
        /// 版本
        /// </summary>
        public int RowVersion { get; set; }


        /// <summary>
        /// 名字参数
        /// </summary>
        public List<string> Moudles { get; set; }

        public HttpAppAddUpdateVM()
        {
            this.Moudles = new List<string>();
        }

        public HttpApp ToEntity()
        {

            var entity = new HttpApp()
            {
                Description = this.Description,
                AppID = this.AppID,
                Name = this.Name
            };

            if (!string.IsNullOrWhiteSpace(this.ID))
            {
                entity.SetID(Guid.Parse(this.ID));
                entity.RowVersion = this.RowVersion;
            }
            var moudleName = new List<string>();
            for (int i = 0; i < Moudles.Count; i++)
            {
                moudleName.Add(Moudles[i].ToString());
            }
            entity.SetMoudles(moudleName);
            return entity;
        }
    }
}