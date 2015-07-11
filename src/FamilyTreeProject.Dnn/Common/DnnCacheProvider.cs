//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using DotNetNuke.Common.Utilities;
using Naif.Core.Caching;

namespace FamilyTreeProject.Dnn.Common
{
    public class DnnCacheProvider : ICacheProvider
    {
        public object Get(string key)
        {
            return DataCache.GetCache(key);
        }

        public void Insert(string key, object value, DateTime absoluteExpiration)
        {
            DataCache.SetCache(key, value, absoluteExpiration);
        }

        public void Insert(string key, object value)
        {
            DataCache.SetCache(key, value);
        }

        public void Remove(string key)
        {
            DataCache.RemoveCache(key);
        }

        public object this[string key]
        {
            get { return DataCache.GetCache(key); }
            set { DataCache.SetCache(key, value); }
        }
    }
}