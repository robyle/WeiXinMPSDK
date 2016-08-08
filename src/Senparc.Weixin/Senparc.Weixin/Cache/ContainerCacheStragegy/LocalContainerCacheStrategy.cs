﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using Senparc.Weixin.Containers;

namespace Senparc.Weixin.Cache
{
    /// <summary>
    /// 全局静态数据源帮助类
    /// </summary>
    public static class LocalCacheHelper
    {
        /// <summary>
        /// 所有数据集合的列表
        /// </summary>
        internal static IDictionary<string, IBaseContainerBag> LocalCache { get; set; }

        static LocalCacheHelper()
        {
            LocalCache = new Dictionary<string, IBaseContainerBag>(StringComparer.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// 本地容器缓存策略
    /// </summary>
    public sealed class LocalContainerCacheStrategy : IContainerCacheStragegy
    //where TContainerBag : class, IBaseContainerBag, new()
    {
        #region 数据源

        private IDictionary<string, IBaseContainerBag> _cache = LocalCacheHelper.LocalCache;

        #endregion

        #region 单例

        /// <summary>
        /// LocalCacheStrategy的构造函数
        /// </summary>
        LocalContainerCacheStrategy()
        {
        }

        //静态LocalCacheStrategy
        public static IContainerCacheStragegy Instance
        {
            get
            {
                return Nested.instance;//返回Nested类中的静态成员instance
            }
        }

        class Nested
        {
            static Nested()
            {
            }
            //将instance设为一个初始化的LocalCacheStrategy新实例
            internal static readonly LocalContainerCacheStrategy instance = new LocalContainerCacheStrategy();
        }


        #endregion

        #region ILocalCacheStrategy 成员

        public string CacheSetKey { get; set; }


        public void InsertToCache(string key, IBaseContainerBag value)
        {
            if (key == null || value == null)
            {
                return;
            }
            _cache[key] = value;
        }

        public void RemoveFromCache(string key)
        {
            _cache.Remove(key);
        }

        public IBaseContainerBag Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            if (!CheckExisted(key))
            {
                return null;
                //InsertToCache(key, new ContainerItemCollection());
            }

            return _cache[key];
        }

        public IDictionary<string, IBaseContainerBag> GetAll()
        {
            return _cache;
        }

        public bool CheckExisted(string key)
        {
            return _cache.ContainsKey(key);
        }

        public long GetCount()
        {
            return _cache.Count;
        }

        public void Update(string key, IBaseContainerBag value)
        {
            _cache[key] = value;
        }

        public void UpdateContainerBag(string key, IBaseContainerBag bag)
        {
            Update(key, bag);
        }

        #endregion
    }
}
