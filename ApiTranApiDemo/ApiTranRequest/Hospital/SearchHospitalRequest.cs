using System;

namespace ApiTranRequest.Hospital
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchHospitalRequest
    {
        /// <summary>
        /// 页数索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序顺序
        /// <value>true:倒序</value>
        /// <value>false:顺序</value>
        /// </summary>
        public bool Sort { get; set; }

        /// <summary>
        /// 排序字段
        /// <value>0:创建时间</value>
        /// <value>1:医院名称</value>
        /// </summary>
        public int SortBy { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 关键字
        /// <value>maxlength:500</value>
        /// <null></null>
        /// </summary>
        public string KeyWords { get; set; }


        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        public string GetOrderByField()
        {
            if (this.SortBy == 0)
            {
                return "CreateTime";
            }
            else if (this.SortBy == 1)
            {
                return "Name";
            }
            return "CreateTime";
        }
    }
}
