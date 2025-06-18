using UnityEngine;

namespace NSG
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] float timeUnitlDestroyed = 4;

        private void Awake()
        {
            Destroy(gameObject, timeUnitlDestroyed);
        }
    }
}
