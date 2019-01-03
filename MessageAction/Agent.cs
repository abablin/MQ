using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RY.H3Hybrid.MQ.MessageAction
{
    /// <summary>
    /// 5000 以後
    /// 針對通路端的訊息動作
    /// 一律以5開頭,長度固定4碼
    /// </summary>
    public struct Agent
    {
        /// <summary>
        /// 重新載入存款單列表
        /// </summary>
        public const int ReloadDepositList = 5000;

        /// <summary>
        /// 重新載入取款單列表
        /// </summary>
        public const int ReloadWithdrawalList = 5001;


        /// <summary>
        /// 移除快取項目
        /// </summary>
        public const int RemoveCacheItem = 5002;

        ///// <summary>
        ///// 下載 Excel 檔案
        ///// </summary>
        //public const int DownloadExcel = 5003;
    } // end of struct
}
