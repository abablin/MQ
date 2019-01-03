using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RY.H3Hybrid.MQ.Enums
{
    /// <summary>
    /// 有提供 Excel 匯出的資料種類列舉
    /// 
    /// 1. 列舉的名稱以 Controller 開頭, 底線後面接 Action 名稱,
    ///    若同一 Action 底下有多個種類 Excel 資料的回傳, 再以結尾的名稱自己做分類(例如 Type1)
    ///    
    /// 2. Report_CashFlowNewReportType1, 以底線做分隔
    ///    Report : 對應到 RY.H3Hybrid.ExcelService.Core.DataFetcher 底下的資料夾名稱
    ///    CashFlowNewReportType1 : 對應到 RY.H3Hybrid.ExcelService.Core.DataFetcher 底下某個資料夾中的類別名稱
    /// </summary>
    public enum ExcelDataType
    {
        #region 報表
        /// <summary>
        /// 金流清單 > 存款清單
        /// </summary>
        Report_CashFlowNewReportType1,

        /// <summary>
        /// 金流清單 > 取款清單
        /// </summary>
        Report_CashFlowNewReportType2,

        /// <summary>
        /// 金流清單 > 存取款清單
        /// </summary>
        Report_CashFlowNewReportType3,
        #endregion

        #region 金流管理
        /// <summary>
        /// 存款單 > 申請單
        /// </summary>
        CashFlow_DepositListStatus1,

        /// <summary>
        /// 存款單 > 收支款單
        /// </summary>
        CashFlow_DepositListStatus2,

        /// <summary>
        /// 存款單 > 作廢單
        /// </summary>
        CashFlow_DepositListStatus3

        #endregion
    }
}
