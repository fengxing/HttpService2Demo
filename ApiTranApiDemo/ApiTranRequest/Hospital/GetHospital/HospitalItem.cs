using System;

namespace ApiTranRequest.Hospital.GetHospital
{
    /// <summary>
    /// 医院信息
    /// </summary>
    public class HospitalItem
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
