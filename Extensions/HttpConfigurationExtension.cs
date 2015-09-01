using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sunshine.WebApiLib.Extensions
{
    public static class HttpConfigurationExtension
    {
        /// <summary>
        /// 移除服务端对application/xml支持
        /// </summary>
        /// <param name="config"></param>
        public static void RemoveXmlMediaTypeSupport(this HttpConfiguration config)
        {
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            if (appXmlType != null)
                config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
