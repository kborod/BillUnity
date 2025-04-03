using System.Collections.Generic;
using System;
using UnityEngine;

namespace Kborod.Utils
{
    public class ObjectsCache<TElementBase>
    {
        private Dictionary<Type, object> _map;

        public ObjectsCache()
        {
            _map = new Dictionary<Type, object>();
        }


        public T Get<T>() where T : TElementBase
        {
            Type key = typeof(T);
            if (_map.ContainsKey(key))
            {
                return (T)_map[key];
            }
            else
            {
                Debug.LogError($"Attempt to get non-existing item : {key.FullName}");
                return default;
            }
        }

        public void Set<T>(T item) where T : TElementBase
        {
            Type key = typeof(T);
            if (_map.ContainsKey(key))
            {
                if (item == null)
                    _map.Remove(key);
                else
                    _map[key] = item;
            }
            else
            {
                if (item != null)
                    _map.Add(key, item);
            }
        }

        public bool HasValue<T>() where T : TElementBase
        {
            return _map.ContainsKey(typeof(T));
        }

        public void Clear()
        {
            _map.Clear();
        }
    }
}

/* Copyright: Made by Appfox */