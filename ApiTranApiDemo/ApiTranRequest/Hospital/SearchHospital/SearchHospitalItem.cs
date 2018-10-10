namespace ApiTranRequest.Hospital.SearchHospital
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchHospitalItem
    {
        /// <summary>
        /// 医院信息
        /// </summary>
        public HospitalItem HospitalItem { get; set; }

        /// <summary>
        /// 创建人信息
        /// </summary>
        public CreateUserItem CreateUserItem { get; set; }
    }
}
