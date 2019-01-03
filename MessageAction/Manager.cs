using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RY.H3Hybrid.MQ.MessageAction
{
    /// <summary>
    /// 3000~4999
    /// 針對總控端的訊息動作
    /// 一律以3開頭,長度固定4碼
    /// </summary>
    public struct Manager
    {
        /// <summary>
        /// 重新載入存款單列表
        /// </summary>
        public const int ReloadDepositList = 3000;
    }
}
