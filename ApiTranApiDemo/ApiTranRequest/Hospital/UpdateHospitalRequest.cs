using System;

namespace ApiTranRequest.Hospital
{
    /// <summary>
    /// 修改医院
    /// </summary>
    public class UpdateHospitalRequest
    {
        /// <summary>
        /// 医院编号
        /// </summary>
        public Guid HospitalID { get; set; }

        /// <summary>
        /// 医院名称
        /// <value>maxlength:200</value>
        /// </summary>
        public string Name { get; set; }
    }
}
