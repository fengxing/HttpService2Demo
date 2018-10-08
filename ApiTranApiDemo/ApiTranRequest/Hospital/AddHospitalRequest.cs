namespace ApiTranRequest.Hospital
{
    /// <summary>
    /// 创建医院
    /// </summary>
    public class AddHospitalRequest
    {
        /// <summary>
        /// 医院名称
        /// <value>maxlength:200</value>
        /// </summary>
        public string Name { get; set; }
    }
}
