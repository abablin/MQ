using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RY.H3Hybrid.MQ.MsgModels
{
    /// <summary>
    /// 用來移除快取項目的訊息
    /// </summary>
    public class RemoveCacheMsg
    {
        private List<string> cacheKeys;
        private string cacheKey;

        public RemoveCacheMsg()
        {
            if (this.cacheKeys == null)
            {
                this.cacheKeys = new List<string>();
            }
        }

        /// <summary>
        /// 要從快取裡面移除的 Key
        /// 2018/06/22,daniel修改,
        /// 因為宣告為object的話,從JSON轉回來後,Key的前後會被加上", 變成 "H3Hybrid_Main_TSP_Site_Product_GetOne"
        /// 導致無法移除快取
        /// </summary>
        public string CacheKey 
        {
            get { return this.cacheKey; }
            set 
            { 
                if(!string.IsNullOrEmpty(value))
                {
                    this.cacheKey = value;
                    
                    if(!this.cacheKeys.Contains(value))
                    {
                        this.cacheKeys.Add(value);
                    }
                }
            }
        }

        /// <summary>
        /// 要從快取裡面移除的 Key 名稱集合
        /// </summary>
        public List<string> CacheKeys 
        { 
            get { return this.cacheKeys; }
            set { this.cacheKeys=value; }
        }
    }
}
