using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    public class ResourceManager
    {
        private static Dictionary<Type, Dictionary<String, Object>> _resources = new Dictionary<Type, Dictionary<string, object>>();

        public static void StoreResource<T>(String name, T obj)
        {
            if(!_resources.ContainsKey(typeof(T)))
            {
                _resources[typeof(T)] = new Dictionary<string, object>();
            }

            _resources[typeof(T)][name] = obj;
        }

        public static T GetResource<T>(String name)
        { 
            if (!_resources.ContainsKey(typeof(T)))
            {
                return default(T);
            }

            return (T)_resources[typeof(T)][name];
        }
    }
}
