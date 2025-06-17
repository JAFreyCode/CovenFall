using UnityEngine;

namespace NSG
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        public WeaponModelInstantiationLocation rightHandSlot;
        public WeaponModelInstantiationLocation leftHandSlot;

        [SerializeField] WeaponManager rightHandWeaponManager;
        [SerializeField] WeaponManager leftHandWeaponManager;

        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            InitializeWeaponSlots();
        }

        protected override void Start()
        {
            base.Start();

            LoadWeaponsOnBothHands();
        }

        private void InitializeWeaponSlots()
        {
            WeaponModelInstantiationLocation[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationLocation>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponsOnBothHands()
        {
            LoadRightHandWeapon();
            LoadLeftHandWeapon();
        }

        // RIGHT HAND

        public void LoadRightHandWeapon()
        {
            if (player.playerInventoryManager.currentRightHandWeapon != null)
            {
                rightHandSlot.UnloadWeaponModel();
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightHandWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        public void SwitchRightWeapon()
        {
            if (!player.IsOwner)
                return;

            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, true, true, true);

            WeaponItem selectedWeapon = null;

            // DISABLE TWO HANDING IF WE ARE TWO HANDING
            // CHECK OUR WEAPON INDEX (WE HAVE 3 SLOTS, SO THATS 3 POSSIBLE NUMBERS)

            // ADD ONE TO OUR INDEX TO GET TO THE NEXT WEAPON
            player.playerInventoryManager.rightHandWeaponIndex += 1;

            // IF OUR INDEX IS OUT OF BOUNDS RESET IT TO 0
            if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;

                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDataBase._Singleton.unarmedWeapn.itemID)
                    {
                        weaponCount += 1;

                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDataBase._Singleton.unarmedWeapn;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDataBase._Singleton.unarmedWeapn.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                    player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                    return;
                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }
        }

        // LEFT HAND

        public void LoadLeftHandWeapon()
        {
            if (player.playerInventoryManager.currentLeftHandWeapon != null)
            {
                leftHandSlot.UnloadWeaponModel();
                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                leftHandWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
            }
        }

        public void SwitchLeftWeapon()
        {
            if (!player.IsOwner)
                return;

            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, true, true, true);

            WeaponItem selectedWeapon = null;

            // DISABLE TWO HANDING IF WE ARE TWO HANDING
            // CHECK OUR WEAPON INDEX (WE HAVE 3 SLOTS, SO THATS 3 POSSIBLE NUMBERS)

            // ADD ONE TO OUR INDEX TO GET TO THE NEXT WEAPON
            player.playerInventoryManager.leftHandWeaponIndex += 1;

            // IF OUR INDEX IS OUT OF BOUNDS RESET IT TO 0
            if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
            {
                player.playerInventoryManager.leftHandWeaponIndex = 0;

                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDataBase._Singleton.unarmedWeapn.itemID)
                    {
                        weaponCount += 1;

                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.leftHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDataBase._Singleton.unarmedWeapn;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
            {
                if (player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDataBase._Singleton.unarmedWeapn.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID;
                    return;
                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
            {
                SwitchLeftWeapon();
            }
        }

        // DAMAGE COLLIDERS
        public void OpenDamageCollider()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightHandWeaponManager.meleeDamageCollider.EnableDamageCollider();
            }
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftHandWeaponManager.meleeDamageCollider.EnableDamageCollider();
            }

            // PLAY SWING / WOOSH SFX
        }

        public void CloseDamageCollider()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightHandWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftHandWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
        }
    }
}
