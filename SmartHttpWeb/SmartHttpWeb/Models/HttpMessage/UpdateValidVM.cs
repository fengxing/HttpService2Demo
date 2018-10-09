using System;
using System.Collections.Generic;

namespace SmartHttpWeb.Models
{

    public class UpdateValidVM
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }
       

        /// <summary>
        /// 是否使用验证
        /// </summary>
        public int IsValid { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public List<string> Messages { get; set; }


        public UpdateValidVM()
        {

        }

        public void Valid()
        {
            foreach (var item in Messages)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    throw new BException("映射的异常不能为空");
                }
            }
        }
    }
}