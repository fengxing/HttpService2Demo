using System.Text;
using System.Web;

namespace SmartHttpWeb
{

    /// <summary>
    /// Modified by G. 2012/8/20
    /// </summary>
    public class PagerV2
    {
        private const string URLSPLIT = "_";

        public PagerV2()
        {
            GetPageNumber();
        }
        public PagerV2(int pgSize, int rdCount)
        {
            GetJumper(pgSize, rdCount);
        }
        public PagerV2(int pgNumber, int pgSize, int rdCount)
        {
            GetJumper(pgNumber, pgSize, rdCount);
        }


        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int RecordCount { get; set; }
        public int PageCount = -1;
        public string UidPrefix = string.Empty;

        public string GetJumper()
        {
            if (PageCount == -1 || RecordCount == -1) { return "请您先设置好相关数据。"; }
            if (RecordCount <= 0)
            {
                return string.Empty;
            }
            if (PageCount <= 1)
            {
                ComputePageNumber();
                if (PageCount <= 1)
                {
                    return string.Empty;
                }

            }
            string url = HttpContext.Current.Request.FilePath;
            StringBuilder printer = new StringBuilder("<div class=\"Pager_Normal\">");
            int iFirst = PageIndex - 5;
            int iLast = PageIndex + 5;
            if (iFirst < 1) { iFirst = 1; }
            if (PageIndex < 7) { iLast = 10; }
            if (iLast > PageCount) { iLast = PageCount; }

            url += "?";
            string _flag = "";
            foreach (string _str in HttpContext.Current.Request.QueryString)
            {
                if (_str == "pn") { continue; }
                url += _flag + _str + "=" + HttpContext.Current.Request.QueryString[_str];
                if (_flag == "") { _flag = "&"; }
            }

            url += _flag;

            if (PageIndex > 1)
            {
                printer.Append("<a href=\"" + url + "pn=1\">&lt;&lt;</a><a href=\"" + url + "pn=" + (PageIndex - 1).ToString() + "\">&lt;</a>");
            }

            for (int i = iFirst; i <= iLast; i++)
            {
                if (i == PageIndex) { printer.Append("<span class=\"current\">" + PageIndex.ToString() + "</span>"); }
                else printer.Append("<a href=\"" + url + "pn=" + i.ToString() + "\">" + i.ToString() + "</a>");
            }

            if (PageIndex < PageCount)
            {
                printer.Append("<a href=\"" + url + "pn=" + (PageIndex + 1).ToString() + "\">&gt;</a><a href=\"" + url + "pn=" + PageCount.ToString() + "\">&gt;&gt;</a>");
            }



            printer.Append("</div>");
            return printer.ToString();
        }

        /// <summary>
        /// 根据URL参数输出pager代码
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public string GetJumper(int pageIndex, int pageSize, int recordCount, string className = "pagination")
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            RecordCount = recordCount;
            ComputePageNumber();

            if (PageCount == -1 || RecordCount == -1) { return "请您先设置好相关数据。"; }
            if (RecordCount <= 0)
            {
                return string.Empty;
            }

            StringBuilder printer = new StringBuilder();
            int iFirst = PageIndex - 2;
            int iLast = PageIndex + 2;
            if (iFirst < 1) { iFirst = 1; }
            if (PageIndex < 3) { iLast = 5; }
            if (iLast > PageCount)
            {
                iLast = PageCount;
                //if (iFirst - 5 >=1)
                //{
                //    iFirst=-5;
                //}
            }

            printer.AppendFormat("<div><ul class=\"{0} fl\">", className);

            bool isFirst = PageIndex <= 1;
            bool isLast = PageIndex >= PageCount;


            string url = HttpContext.Current.Request.FilePath;

            url += "?";
            string _flag = "";
            foreach (string _str in HttpContext.Current.Request.QueryString)
            {
                if (_str == "pn") { continue; }
                url += _flag + _str + "=" + HttpContext.Current.Request.QueryString[_str];
                if (_flag == "") { _flag = "&"; }
            }

            url += _flag;


            printer.AppendFormat("<li {0}><a {1}>首页</a></li>", isFirst ? "class=\"disabled\" " : "", isFirst ? "" : string.Format("title=\"转到首页\" href=\"{0}pn={1}\"", url, 1));
            printer.AppendFormat("<li {0}><a {1}>« </a></li>", isFirst ? "class=\"disabled\" " : "", isFirst ? "" : string.Format("title=\"转到上一页\" href=\"{0}pn={1}\"", url, pageIndex - 1));

            for (int i = iFirst; i <= iLast; i++)
            {
                if (i == PageIndex)
                {
                    printer.AppendFormat("<li class=\"active\" title=\"第{0}页\"><a href=\"####\">{0}</a></li>", i);
                }
                else
                {
                    printer.AppendFormat("<li><a title=\"转到第{1}页\" href=\"{0}pn={1}\">{1}</a></li>", url, i);
                }
            }
            printer.AppendFormat("<li {0}><a {1}>»</a></li>", isLast ? "class=\"disabled\" " : "", isLast ? "" : string.Format("title=\"转到下一页\" href=\"{0}pn={1}\"", url, pageIndex + 1));
            printer.AppendFormat("<li {0}><a {1}>尾页</a></li>", isLast ? "class=\"disabled\" " : "", isLast ? "" : string.Format("title=\"转到尾页\" href=\"{0}pn={1}\"", url, PageCount));
            //<span class=\"goto\">到第<input type=\"text\" value=\"{1}\" size=\"3\" id=\"txtGoPage" + code + "\" />页</span><a href=\"javascript:void(0);\" title=\"跳转页面\" onclick=\"{2}\">GO</a>", RecordCount, pageIndex, string.Format(ajaxProviderFormat, "0"));
            printer.Append("</ul>");

            printer.Append("</div>");
            printer.AppendFormat("<span class=\"record-count\">共{0}条数据</span>", RecordCount);
            //printer.AppendFormat("<div class=\"Sum\">共{0}条数据,每页显示{1}条,共{2}页</div>", RecordCount, PageSize, PageCount);

            return printer.ToString();
        }


        /// <summary>
        /// 根据ajax参数方法等输出pager代码
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="ajaxProviderFormat"></param>
        /// <returns></returns>
        public string GetJumperForAjax(int pageIndex, int pageSize, int recordCount, string ajaxProviderFormat, string className = "pagination", string code = "")
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            RecordCount = recordCount;
            ComputePageNumber();

            if (PageCount == -1 || RecordCount == -1) { return "请您先设置好相关数据。"; }
            if (RecordCount <= 0)
            {
                return string.Empty;
            }

            StringBuilder printer = new StringBuilder();
            int iFirst = PageIndex - 5;
            int iLast = PageIndex + 5;
            if (iFirst < 1) { iFirst = 1; }
            if (PageIndex < 7) { iLast = 10; }
            if (iLast > PageCount) { iLast = PageCount; }
            printer.AppendFormat("<div><ul class=\"{0} fl\">", className);

            bool isFirst = PageIndex <= 1;
            bool isLast = PageIndex >= PageCount;
            printer.AppendFormat("<li {0}><a {1}>首页</a></li>", isFirst ? "class=\"disabled\" " : "", isFirst ? "" : string.Format("title=\"转到首页\" href=\"javascript:void(0);\" onclick=\"{0}\"", string.Format(ajaxProviderFormat, 1)));
            printer.AppendFormat("<li {0}><a {1}>« </a></li>", isFirst ? "class=\"disabled\" " : "", isFirst ? "" : string.Format("title=\"转到上一页\" href=\"javascript:void(0);\" onclick=\"{0}\"", string.Format(ajaxProviderFormat, PageIndex - 1)));

            for (int i = iFirst; i <= iLast; i++)
            {
                if (i == PageIndex)
                {
                    printer.AppendFormat("<li class=\"active\" title=\"第{0}页\"><a href=\"####\">{0}</a></li>", i);
                }
                else
                {
                    printer.AppendFormat("<li><a title=\"转到第{0}页\" href=\"javascript:void(0);\" onclick=\"{1}\">{0}</a></li>", i, string.Format(ajaxProviderFormat, i));
                }
            }
            printer.AppendFormat("<li {0}><a {1}>»</a></li>", isLast ? "class=\"disabled\" " : "", isLast ? "" : string.Format("title=\"转到下一页\" href=\"javascript:void(0);\" onclick=\"{0}\"", string.Format(ajaxProviderFormat, PageIndex + 1)));
            printer.AppendFormat("<li {0}><a {1}>尾页</a></li>", isLast ? "class=\"disabled\" " : "", isLast ? "" : string.Format("title=\"转到尾页\" href=\"javascript:void(0);\" onclick=\"{0}\"", string.Format(ajaxProviderFormat, PageCount)));
            //<span class=\"goto\">到第<input type=\"text\" value=\"{1}\" size=\"3\" id=\"txtGoPage" + code + "\" />页</span><a href=\"javascript:void(0);\" title=\"跳转页面\" onclick=\"{2}\">GO</a>", RecordCount, pageIndex, string.Format(ajaxProviderFormat, "0"));
            printer.Append("</ul>");

            printer.Append("</div>");
            printer.AppendFormat("<span class=\"record-count\">共{0}条数据, 共{1}页</span>", RecordCount, PageCount);
            //printer.AppendFormat("<div class=\"Sum\">共{0}条数据,每页显示{1}条,共{2}页</div>", RecordCount, PageSize, PageCount);

            return printer.ToString();
        }

        /// <summary>
        /// 输出页码
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="houseID"></param>
        /// <returns></returns>
        public string GetPagerHtml(int pageIndex, int pageSize, int recordCount, int houseID)
        {

            PageIndex = pageIndex;
            PageSize = pageSize;
            RecordCount = recordCount;
            ComputePageNumber();
            if (PageCount < 2)
                return string.Empty;
            //pageCount 从零开始 
            StringBuilder pagerHtml = new StringBuilder();

            //页码开始  显示5个页码 只能是 0 5 10 15……
            int pageNumFrom = (pageIndex / 5) * 5;


            if (pageIndex >= PageCount || pageIndex < 0)
            {
                return string.Empty;
            }

            if (pageNumFrom == 0)
            {
                pagerHtml.Append("<a href=\"javascript:void(0);\" class=\"pre\" title=\"上一页\"></a>");
                // pagerHtml.AppendFormat("<a>{0}</a><br/>", 0);
            }
            else
            {
                pagerHtml.AppendFormat("<a href=\"/Office/House/?id={0}&pid={1}\" class=\"pre\" title=\"上一页\"></a>", houseID, pageNumFrom - 1);
                //pagerHtml.AppendFormat("<a>{0}</a><br/>", pageNumFrom - 1);
            }

            for (int i = pageNumFrom; i < pageNumFrom + 5; i++)
            {
                if (pageIndex == i)
                {
                    pagerHtml.AppendFormat("<span class=\"item\">{1}</span>", i, i + 1);
                    // pagerHtml.AppendFormat("<span>{0}</span>", i);
                }
                else
                {
                    if (i < PageCount)
                        pagerHtml.AppendFormat("<a class=\"item\" title=\"{0}\" href=\"/Office/House/?id={0}&pid={1}\">{2}</a>", houseID, i, i + 1);
                    //pagerHtml.AppendFormat("<a>{0}</a>", i);
                }
            }
            if (pageIndex + 1 == PageCount)//最后一页
            {
                pagerHtml.Append("<a href=\"javascript:void(0);\" class=\"next\" title=\"没有了\"></a>");
                // pagerHtml.AppendFormat("<a>{0}</a>", pageIndex);
            }
            else if (PageCount - (pageNumFrom + 1) > 5)
            {
                pagerHtml.AppendFormat("<a href=\"/Office/House/?id={0}&pid={1}\" class=\"next\" title=\"下一页\"></a>", houseID, pageNumFrom + 5);
            }
            else if (PageCount - (pageNumFrom + 1) <= 5)
            {
                pagerHtml.AppendFormat("<a href=\"/Office/House/?id={0}&pid={1}\" class=\"next\" title=\"下一页\"></a>", houseID, pageNumFrom);
                // pagerHtml.AppendFormat("<a>{0}</a>", pageNumFrom + 5);
            }

            return pagerHtml.ToString();
        }

        public string GetJumper(int pgNumber, int pgSize, int rdCount)
        {
            PageIndex = pgNumber;
            PageSize = pgSize;
            RecordCount = rdCount;
            return GetJumper();
        }

        public string GetJumper(int pgSize, int rdCount)
        {
            GetPageNumber();
            PageSize = pgSize;
            RecordCount = rdCount;
            return GetJumper();
        }
        private void GetPageNumber()
        {
            int pn = 0;
            int.TryParse(HttpContext.Current.Request.QueryString["pn"], out pn);
            if (pn > 0) { PageIndex = pn; }
            else PageIndex = 1;
        }

        private void ComputePageNumber()
        {
            if (RecordCount <= 0 || PageSize <= 0)
            {
                PageCount = 0;
                return;
            }
            PageCount = RecordCount / PageSize;
            if (PageCount * PageSize < RecordCount) PageCount++;
            if (PageIndex < 1) { PageIndex = 1; }
            else if (PageIndex > PageCount) { PageIndex = PageCount; }
        }
    }
}