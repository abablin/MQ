using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RY.H3Hybrid.MQ.MsgModels.Excel
{
    [Serializable]
    public class ExcelData
    {
        private bool isBatchFile;

        /// <summary>
        /// 用戶端當前的 ASP.NET Session ID
        /// </summary>
        public string SessionID { get; set; }

        /// <summary>
        /// 此次傳遞的檔案 byte[] 內容
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// 檔案轉為 byte[] 後的總長度, 用在批次檔案
        /// </summary>
        public int TotalArrayLength;

        /// <summary>
        /// 此次傳遞的陣列長度
        /// </summary>
        public int CurrentArrayLength;

        /// <summary>
        /// 已傳遞的陣列長度
        /// 假設批次傳遞的長度為1000
        /// 第1筆資料的值會是 0
        /// 第2筆資料的值會是 1000
        /// </summary>
        public int SendedArrayLength;

        /// <summary>
        /// 表示該檔案是否為批次資料,預設為否
        /// </summary>
        [DefaultValue(false)]
        public bool IsBatchFile
        {
            get { return isBatchFile; }
            set
            {
                isBatchFile = value;
                if (value == false)
                {
                    //BatchID = string.Empty;
                    BatchRowsCount = 0;
                    BatchRowIndex = 0;
                }
            }
        }

        /// <summary>
        /// 如果是批次檔案,表示該筆資料是第幾筆資料
        /// 起始值是 1
        /// </summary>
        public int BatchRowIndex;

        /// <summary>
        /// 如果是批次檔案,表示該批次共有幾筆資料
        /// </summary>
        public int BatchRowsCount;

        /// <summary>
        /// 批次編號,用來找出哪些資料是在同一個批次的識別值
        /// 由前端傳入
        /// </summary>
        public string BatchID;
    } // end of class
}
