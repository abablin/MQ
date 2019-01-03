using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RY.H3Hybrid.MQ.Enums;

namespace RY.H3Hybrid.MQ.MsgModels.Excel
{
    public class QueryParameter
    {
        /// <summary>
        /// 查詢條件的欄位名稱
        /// </summary>
        public string Key;

        /// <summary>
        /// 查詢條件的運算子
        /// </summary>
        public Enums.QueryParameterOperator Operator;

        /// <summary>
        /// 查詢條件的欄位值
        /// </summary>
        public string Value;
    }


    public class ExcelParameter
    {
        /// <summary>
        /// 查詢條件的集合
        /// </summary>
        public List<QueryParameter> QueryParameters { get; set; }

        /// <summary>
        /// 站台編號
        /// </summary>
        public string SiteID { get; set; }


        /// <summary>
        /// 用戶端的帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 此次請求的檔名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 用戶端當前的唯一識別值 (例如 ASP.NET Session ID)
        /// </summary>
        public string SessionID { get; set; }

        /// <summary>
        /// 此次請求檔案的 GUID
        /// </summary>
        public string FileGUID { get; set; }

        /// <summary>
        /// 此次請求的主機 IP
        /// </summary>
        public string SourceIP { get; set; }

        /// <summary>
        /// 此次請求的 Queue 名稱
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 此次請求的資料類型,值要對應到 RY.H3Hybrid.MQ.Enums.ExcelDataType 底下的列舉值
        /// </summary>
        public ExcelDataType ExcelDataType { get; set; }
    } // end of class
}
