using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PeacefulMode
{
    public class PeacefulMode : MonoBehaviour
    {
        public static bool disablePeaceful = false;
        public bool overrideDisablePeaceful = false;
        public bool disablePeacefulOverride = true;
        
        public List<GameObjectSwapInfo> swaps = new List<GameObjectSwapInfo>();

        public static PeacefulMode singleton;
        
        void Awake()
        {
            if (singleton != null)
            {
                Debug.LogError("There can only be one peaceful instance in the scene!");
                Destroy(this);
                return;
            }
            
            singleton = this;
            
            if (disablePeaceful && !overrideDisablePeaceful || overrideDisablePeaceful && disablePeacefulOverride)
            {
                enabled = false;
                return;
            }
            
            Debug.Log("Enabled Peaceful Mode!");
        }

        public static GameObject NormalToPeacefulStatic(GameObject original)
        {
            return singleton.NormalToPeaceful(original);
        }
        
        public GameObject NormalToPeaceful(GameObject original)
        {
            // PeacefulMode is disabled
            if (!enabled)
                return original;

            var swapped = swaps.First(a => a.@from == original).to;
            return swapped ? swapped : original;
        }
    }
}
