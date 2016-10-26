using CrimsonEngine.Physics;
using System;
using System.Collections.Generic;

namespace CrimsonEngine
{
    public static class Layer
    {
        private static Dictionary<string, PhysicsLayer> _layers = new Dictionary<string, PhysicsLayer>();
        private static PhysicsLayer nextAvailableEnum = PhysicsLayer.Layer1;

        public static PhysicsLayer GetLayer(string layerName)
        {
            if(!_layers.ContainsKey(layerName)) {
                
                _layers[layerName] = nextAvailableEnum;
                nextAvailableEnum = nextAvailableEnum.Next();
            }
            return _layers[layerName];
        }

        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            if (Arr.Length == j)
                throw new IndexOutOfRangeException("Maximum number of layers created.");
            return Arr[j];
        }
    }
}
