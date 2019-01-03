using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RY.H3Hybrid.MQ.MessageAction
{
    /// <summary>
    /// 1000~2999
    /// 針對會員端的訊息動作
    /// 一律以1開頭,長度固定4碼
    /// </summary>
    public struct Member
    {
        /// <summary>
        /// 移除快取項目
        /// </summary>
        public const int RemoveCacheItem = 1000;
    }
}
