using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.WebApi
{
    /// <summary>
    /// Api 请求
    /// </summary>
    public class ApiRequest
    {
        /// <summary>
        /// 请求名称
        /// </summary>
        public string Name { get; set; }

        public DateTime IssureAt { get; set; }

        System.Threading.CancellationTokenSource cancellationTokenSource;

        public void CancelRequest() {
            if (this.cancellationTokenSource == null)
                throw new NotSupportedException("");
        }
    }
}
