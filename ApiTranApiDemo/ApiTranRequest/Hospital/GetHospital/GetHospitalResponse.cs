namespace ApiTranRequest.Hospital.GetHospital
{
    /// <summary>
    /// 
    /// </summary>
    public class GetHospitalResponse
    {
        /// <summary>
        /// 医院信息
        /// </summary>
        public HospitalItem HospitalItem { get; set; }

        /// <summary>
        /// 创建者信息
        /// </summary>
        public CreateUserItem CreateUserItem { get; set; }

        /// <summary>
        /// 更新者信息
        /// </summary>
        public UpdateUserItem UpdateUserItem { get; set; }
    }
}
