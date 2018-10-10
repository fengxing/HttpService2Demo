using System.Collections.Generic;

namespace ApiTranRequest.Hospital.SearchHospital
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchHospitalResponse
    {
        /// <summary>
        /// 医院信息列表
        /// </summary>
        public IEnumerable<SearchHospitalItem> SearchHospitalItems { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int Count { get; set; }
    }
}
