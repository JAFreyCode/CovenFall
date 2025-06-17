using UnityEngine;
using System.Linq;

namespace NSG
{
    public class WorldActionManager : MonoBehaviour
    {
        public static WorldActionManager _Singleton;

        [Header("Weapon Item Actions")]
        public WeaponItemAction[] weaponItemActions;

        private void Awake()
        {
            if (_Singleton == null)
                _Singleton = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            for (int i = 0; i < weaponItemActions.Length; i++)
            {
                weaponItemActions[i].actionID = i;
            }
        }

        public WeaponItemAction GetWeaponItemActionByID(int ID)
        {
            return weaponItemActions.FirstOrDefault(action => action.actionID == ID);
        }
    }
}
