using SmartBaseEntity;
using System;
using System.Collections.Generic;

namespace SmartOnlineEntity
{
    /// <summary>
    /// 医院
    /// </summary>
    [History]
    public class HospitalAddress : BaseEntity, IName
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid HospitalID { get; set; }

        /// <summary>
        /// 医院地址
        /// <value>maxlength:300</value>
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetNameFileds()
        {
            return new List<string>() { "Address" };
        }

        /// <summary>
        /// 
        /// </summary>
        public HospitalAddress()
        {
            this.Address = "";
        }
    }
}
