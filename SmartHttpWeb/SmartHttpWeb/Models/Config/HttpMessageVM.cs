using System;

namespace SmartHttpWeb.Models
{
    public class HttpMessageVM
    {
        public string Method { get; set; }

        public string Moudle { get; set; }
        public string SubMoudle { get; set; }

        public int PageIndex { get; set; }

        public int? AppID { get; set; }

        public string IsSuccess { get; set; }

        public string Args { get; set; }

        public int? Status { get; set; }

        public string ExecuteID { get; set; }

        public string UID { get; set; }

        public int?  Code { get; set; }

        public bool? IsNotify { get; set; }
    }
}