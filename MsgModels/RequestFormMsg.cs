using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RY.H3Hybrid.MQ.MsgModels
{
    /// <summary>
    /// 存取款單基本資料
    /// </summary>
    public class RequestFormMsg
    {
        /// <summary>
        /// 資料流水號, 對應到 RequestForm 的 rd_key 欄位
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// 單號
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 會員帳號
        /// </summary>
        public string MemberAccount { get; set; }

        /// <summary>
        /// 存款或取款 (直接寫入存款或取款, 不是寫1,-1)
        /// </summary>
        public string MoneyType { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 是否有上傳存款收據 (針對存款單)
        /// </summary>
        [DefaultValue(false)]
        public bool HasUploadedReceipt { get; set; }

        /// <summary>
        /// 轉為 Base64 後的存款收據圖片
        /// </summary>
        public string ReceiptContent { get; set; }
    }
}
