
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QX.Common
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public static class SystemHelper
    {
        /// <summary>
        /// 加载应用程序的程序集
        /// </summary>
        /// <param name="matchName">过滤名字</param>
        /// <returns>系统中自定义的程序集</returns>
        public static Assembly[] LoadAppAssemblies(string matchName = "QX*.dll")
        {
            try
            {
                string binFolder = AppDomain.CurrentDomain.RelativeSearchPath;
                if (String.IsNullOrEmpty(binFolder))
                    binFolder = Environment.CurrentDirectory;
                DirectoryInfo binInfo = new DirectoryInfo(binFolder);
                string[] files = binInfo.GetFiles(matchName).Select(m => m.Name).ToArray();

                var assemblies = files.Select(m => Assembly.Load(Path.GetFileNameWithoutExtension(m))).ToArray();

                return assemblies;
            }
            catch (Exception ex)
            {
                Logger.Logger.Error("SystemHelper -> LoadAppAssemblies [批量加载应用程序集失败!]", ex);
                return null;
            }
        }
        /// <summary>
        /// 批量加载应用程序的程序集
        /// </summary>
        /// <param name="matchName">过滤名字</param>
        /// <returns>系统中自定义的程序集</returns>
        public static Assembly[] LoadAppAssemblies(params string[] matchNames)
        {
            try
            {
                string binFolder = AppDomain.CurrentDomain.RelativeSearchPath;
                if (String.IsNullOrEmpty(binFolder))
                    binFolder = Environment.CurrentDirectory;
                DirectoryInfo binInfo = new DirectoryInfo(binFolder);
                var files = new List<string>();
                matchNames.ToList().ForEach(f =>
                {
                    files.AddRange(binInfo.GetFiles(f).Select(m => m.Name).ToArray());
                });
                var loadedAssemblies = files.Select(m => Assembly.Load(Path.GetFileNameWithoutExtension(m))).ToArray();
                return loadedAssemblies;
            }
            catch (Exception ex)
            {
                Logger.Logger.Error("SystemHelper -> LoadAppAssemblies [批量加载应用程序集失败!]", ex);
                return null;
            }

        }

        /// <summary>
        /// 将字符串转化为Int值
        /// </summary>
        /// <param name="strVal">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>整数</returns>
        public static int ToInt(this string strVal, int defaultValue = 0)
        {
            int result;
            return Int32.TryParse(strVal, out result) ? result : defaultValue;
        }

        /// <summary>
        /// 将字符串转化为Int64
        /// </summary>
        /// <param name="strVal">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>Int64值</returns>
        public static Int64 ToInt64(this string strVal, Int64 defaultValue = 0)
        {
            Int64 result;
            return Int64.TryParse(strVal, out result) ? result : defaultValue;
        }

        /// <summary>
        /// 对枚举器的每个元素执行指定的操作
        /// </summary>
        /// <typeparam name="T">枚举器类型参数</typeparam>
        /// <param name="source">枚举器</param>
        /// <param name="action">要对枚举器的每个元素执行的委托</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source.IsNullOrEmpty() || action == null)
            {
                return;
            }
            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// 按指定条件删除数据项
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="source">操作的数据源</param>
        /// <param name="predicate">移除的条件</param>
        /// <returns>移除后的数据项</returns>
        public static IList<T> Remove<T>(this IList<T> source, Func<T, bool> predicate = null)
        {
            if (source != null)
            {
                var removeItems = predicate != null
                                ? source.Where(predicate).ToArray()
                                : source.ToArray();

                foreach (var item in removeItems)
                {
                    source.Remove(item);
                }
            }
            return source;
        }

        /// <summary>
        /// 指示指定的枚举器是null还是没有任何元素
        /// </summary>
        /// <typeparam name="T">枚举器类型参数</typeparam>
        /// <param name="source">要测试的枚举器</param>
        /// <returns>true:枚举器是null或者没有任何元素 false:枚举器不为null并且包含至少一个元素</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// 获取类型的CustomAttribute
        /// </summary>
        /// <typeparam name="T">要获取的Attribute类型</typeparam>
        /// <param name="type">目标类型</param>
        /// <param name="inherit">是否采用继承方式查找</param>
        /// <returns>当前类型上的T类型的Attribute实例</returns>
        public static T GetCustomAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            if (type.IsDefined(typeof(T)))
            {
                return (T)type.GetCustomAttributes(typeof(T), inherit)[0];
            }
            return default(T);
        }
        /// <summary>
        /// 文件上会否存在
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }
        
    }
}
