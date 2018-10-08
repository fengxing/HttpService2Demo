using SmartBaseEntity;
using System.Collections.Generic;

namespace SmartOnlineEntity
{
    /// <summary>
    /// 医院
    /// </summary>
    [History]
    public class Hospital : BaseEntity, ISame, IName
    {
        /// <summary>
        /// 医院名称
        /// <value>maxlength:200</value>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetCols()
        {
            return nameof(Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetNameFileds()
        {
            return new List<string>() { "Name" };
        }

        /// <summary>
        /// 
        /// </summary>
        public Hospital()
        {
            this.Name = "";
        }
    }
}
