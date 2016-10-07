using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine.Physics
{
    public static class Layers
    {
        private static Dictionary<String, uint> layers = new Dictionary<string, uint>();
        private static uint lastVal = 0x00000001;

        private static uint GetLayer(String layerName)
        {
            if (layers.ContainsKey(layerName))
                return layers[layerName];
            else
                return 0x00000000;
        }

        /// <summary>
        /// Retrieve a mask for the specified layer names.
        /// </summary>
        /// <param name="layers">The layers to include in the mask.</param>
        /// <returns>The mask for the specified layer names.</returns>
        public static uint GetLayers(params String[] layers)
        {
            uint total = 0x00000000;
            foreach(String l in layers)
            {
                total = total | GetLayer(l);
            }
            return total;
        }

        /// <summary>
        /// A layer mask containing all layers.
        /// </summary>
        public const uint AllLayers = 0xffffffff;

        /// <summary>
        /// Associates a layer name with the next layer field available.
        /// </summary>
        /// <param name="layerName"></param>
        public static void AddLayer(String layerName)
        {
            layers.Add(layerName, lastVal);
            lastVal = lastVal << 1;
        }

        /// <summary>
        /// Checks whether a specified layer name is contained within the layerMask.
        /// </summary>
        /// <param name="layerName">The layer to check if it's in the layerMask.</param>
        /// <param name="layers">The layerMask to check if the layerName is in it.</param>
        /// <returns>Whether a specified layer name is contained within the layerMask.</returns>
        public static bool isInLayer(String layerName, uint layers)
        {
            return (GetLayer(layerName) & layers) != 0;
        }
    }
}
