namespace Animate.Data.SliderCaptha
{
    public class CacheHelper
    {
        //快取容器 
        private static Dictionary<string, object> CacheDictionary = new Dictionary<string, object>();
        /// <summary>
        /// 新增快取
        /// </summary>
        public static void Add(string key, object value)
        {
            CacheDictionary.Add(key, value);
        }

        /// <summary>
        /// 移除快取
        /// </summary>
        public static void remove(string key)
        {
            CacheDictionary.Remove(key);
        }


        /// <summary>
        /// 獲取快取
        /// </summary>
        public static T Get<T>(string key)
        {
            return (T)CacheDictionary[key];
        }


        /// <summary>
        /// 快取獲取方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">快取字典容器對應key</param>
        /// <param name="func">委託方法 傳入操作物件</param>
        /// <returns></returns>
        public static T GetCache<T>(string key, Func<T> func)
        {
            T t = default(T);
            if (CacheHelper.Exsits(key))
            {
                //快取存在，直接獲取原資料
                t = CacheHelper.Get<T>(key);
            }
            else
            {
                //快取不存在，去生成快取，並加入容器
                t = func.Invoke();
                CacheHelper.Add(key, t);
            }
            return t;
        }

        /// <summary>
        /// 判斷快取是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exsits(string key)
        {
            return CacheDictionary.ContainsKey(key);
        }
    }
}
