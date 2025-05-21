using UnityEngine;

namespace NSG
{
    public class TitleScreenLoadMenuInputManager : MonoBehaviour
    {
        [Header("Singleton")]
        private static TitleScreenLoadMenuInputManager Singleton;
        public static TitleScreenLoadMenuInputManager _Singleton { get { return Singleton; } private set { Singleton = value; } }

        PlayerControls playerControls;

        [Header("Title Screen Inputs")]
        [SerializeField] bool deleteCharacterSlot = false;
        [SerializeField] bool closeOpenedMenu = false;
        public Vector2 scrollValue;

        private void Awake()
        {
            NSGUtils.SingletonCheck(ref Singleton, this);
        }

        private void Update()
        {
            if (deleteCharacterSlot)
            {
                deleteCharacterSlot = false;
                TitleScreenManager._Singleton.AttemptToDeleteCharacterSlot();
            }

            if (closeOpenedMenu)
            {
                closeOpenedMenu = false;
                TitleScreenManager._Singleton.AttemptToCloseMenu();
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
            }

            playerControls.UI.DeleteSlot.performed += i => deleteCharacterSlot = true;
            playerControls.UI.CloseMenu.performed += i => closeOpenedMenu = true;

            playerControls.UI.Scroll.performed += i => scrollValue = i.ReadValue<Vector2>();
            playerControls.UI.Scroll.canceled += i => scrollValue = i.ReadValue<Vector2>();

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}
