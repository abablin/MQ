using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RY.H3Hybrid.MQ.Enums
{
    /// <summary>
    /// 應用程式端口的列舉
    /// </summary>
    public enum ApplicationPort
    {
        /// <summary>
        /// 會員端,0
        /// </summary>
        Member=0,

        /// <summary>
        /// 總控端,1
        /// </summary>
        Manager=1,

        /// <summary>
        /// 通路端,2
        /// </summary>
        Agent=2,

        ///// <summary>
        ///// Window Form 後台的相關結算程式
        ///// </summary>
        //WindowForm,

        ///// <summary>
        ///// 其他種類的端口
        ///// </summary>
        //Other,

        /// <summary>
        /// 會員端,通路端,3
        /// </summary>
        MemberAndAgent=3,

        /// <summary>
        /// 會員端,總控端,4
        /// </summary>
        MemberAndManager=4,

        /// <summary>
        /// 通路端,總控端,5
        /// </summary>
        AgentAndManager=5,

        /// <summary>
        /// 會員端,通路端,總控端,6
        /// </summary>
        All=6,

        /// <summary>
        /// 實體主機 (目前用在產 Excel)
        /// </summary>
        PhysicalHost=7,

        /// <summary>
        /// 自定義的目的地
        /// 根據傳入的 IP, 決定到時要發送的目的地
        /// </summary>
        Custom=8
    }
}
