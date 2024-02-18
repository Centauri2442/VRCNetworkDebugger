
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Centauri.NetDebug
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TakeOwnerOfRandom : UdonSharpBehaviour
    {
        public GameObject[] OwnershipObjects;


        public void Start()
        {
            SendCustomEventDelayedSeconds(nameof(DelayedStart), 2f);
        }

        public void DelayedStart()
        {
            foreach (var item in OwnershipObjects)
            {
                if (Random.Range(0f, 100f) < 50f)
                {
                    Networking.SetOwner(Networking.LocalPlayer, item);
                }
            }
        }
    }
}
