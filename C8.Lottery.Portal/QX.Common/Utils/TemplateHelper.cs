using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace QX.Common.Utils
{
    /// <summary>
    /// 模板引擎
    /// </summary>
    public class TemplateHelper
    {
        static TemplateHelper()
        {
            var config = new TemplateServiceConfiguration();
            config.DisableTempFileLocking = true;
            config.CachingProvider = new DefaultCachingProvider(t => { });

            var service = RazorEngineService.Create(config);
            Engine.Razor = service;
        }

        /// <summary>
        /// 模板填充
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string Render<T>(string template, T model) 
        {
            if (template.IsNullOrEmpty())
            {
                return "";
            }
            var key = EncryptionHelper.MD5Encrypt(template, "");
            return Engine.Razor.RunCompile(template, key, typeof(T), model, null);
        }
    }
}
