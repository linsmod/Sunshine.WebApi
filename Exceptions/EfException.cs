using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.WebApiLib.Exceptions
{
    /// <summary>
    /// 表示entityframework的异常
    /// </summary>
    public class EfException : Exception
    {
        /// <summary>
        /// 表示entityframework的异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public EfException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
