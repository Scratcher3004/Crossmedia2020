using System.Collections.Generic;
using UnityEngine;

namespace PeacefulMode
{
    public class PeacefulMode : MonoBehaviour
    {
        public static bool disablePeaceful = false;
        public bool overrideDisablePeaceful = false;
        public bool disablePeacefulOverride = true;
        
        public List<GameObjectSwapInfo> swap = new List<GameObjectSwapInfo>();

        public static PeacefulMode singleton;
        
        void Start()
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

        public static void NormalToPeaceful(GameObject original)
        {
            
            
            //var swapped = 
        }
    }
}
