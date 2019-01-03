using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RY.H3Hybrid.MQ.Enums
{
    [Serializable]
    public enum QueryParameterOperator
    {
        /// <summary>
        /// 等於
        /// </summary>
        Equal,

        /// <summary>
        /// 不等於
        /// </summary>
        NotEqual,

        /// <summary>
        /// 大於
        /// </summary>
        GreaterThan,

        /// <summary>
        /// 大於等於
        /// </summary>
        GreaterThanEqual,

        /// <summary>
        /// 小於
        /// </summary>
        LessThan,

        /// <summary>
        /// 小於等於
        /// </summary>
        LessThanEqual
    }
}
