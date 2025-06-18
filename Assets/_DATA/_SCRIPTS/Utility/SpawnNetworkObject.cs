using UnityEngine;
using Unity.Netcode;

namespace NSG
{
    public class SpawnNetworkObject : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<NetworkObject>().Spawn();
        }
    }
}
