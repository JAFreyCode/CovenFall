using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace NSG
{
    public class TitleScreenManager : MonoBehaviour
    {
        [Header("Singleton")]
        private static TitleScreenManager Singleton;
        public static TitleScreenManager _Singleton { get { return Singleton; } private set { Singleton = value; } }

        SaveFileDataWriter saveFileDataWriter;

        [Header("TitleScreen")]
        [SerializeField] GameObject startButton;

        [Header("MainMenu")]
        [SerializeField] GameObject mainMenuObject;
        [SerializeField] GameObject titleBanner;
        [SerializeField] List<Button> menuButtons;

        [Header("LoadGameMenu")]
        public GameObject loadGameMenu;
        [SerializeField] List <Button> loadMenuSlots;

        [Header("SystemMenu")]
        [SerializeField] GameObject systemMenuObject;
        [SerializeField] GameObject graphicsMenu;
        [SerializeField] GameObject audioMenu;
        [SerializeField] GameObject networkMenu;
        [SerializeField] GameObject controlsMenu;
        [SerializeField] GameObject cameraMenu;

        [Header("SystemMenu Buttons")]
        [SerializeField] Button graphicsButton;
        [SerializeField] Button audioButton;
        [SerializeField] Button networkButton;

        [Header("SystemMenu Sub Buttons")]
        //--GRAPHICS
        [SerializeField] TMP_Dropdown resolutionOption;
        [SerializeField] TMP_Dropdown screenModeOption;
        [SerializeField] TMP_Dropdown qualityOption;
        //--AUDIO
        [SerializeField] Slider masterAudioOption;
        [SerializeField] Slider menuMusicOption;
        //--Network
        [SerializeField] Button goOfflineOption;
        [SerializeField] Button goOnlineOption;

        [Header("PopUp Menus")]
        public GameObject popupMenus;
        public GameObject CouldNotConnectPopup;
        public GameObject NoSaveSlotsAvaiablePopup;
        public GameObject deleteCharacterSlotPopup;

        [Header("PopUp Menu Buttons")]
        public Button couldNotConnectAcceptButton;
        public Button noSaveSlotsAvaiableAcceptButton;
        public Button deleteCharacterSlotAcceptButton;

        [Header("Menu Flags")]
        public bool isInSubMenu = false;

        [Header("AppData")]
        [SerializeField] TMP_Text onlineStatus;
        public bool isOnline;

        [Header("Save Slots")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

        private void Awake()
        {
            NSGUtils.SingletonCheck(ref Singleton, this);
        }

        #region Start Network

        public void StartNetworkAsHost()
        {
            isOnline = NetworkManager.Singleton.StartHost();

            SetOnlineStatus();
        }

        private void SetOnlineStatus()
        {
            SetOnlineStatusText();

            WorldGraphicsManager._Singleton.CheckNetworkConnect();
            
            if (!isOnline) OpenCouldNotConnectMenu();
        }

        public void SetOnlineStatusText()
        {
            if (isOnline)
            {
                onlineStatus.text = "ONLINE";
                OpenMainMenu();
                return;
            }

            onlineStatus.text = "OFFLINE";
        }

        public void OpenMainMenu()
        {
            startButton.SetActive(false);
            mainMenuObject.SetActive(true);
            titleBanner.SetActive(true);

            SelectMenuButton();
        }

        #endregion

        #region Menu Methods

        public void StartNewGame()
        {
            WorldSaveGameManager._Singleton.AttemptToCreateNewGame();
        }

        public void AttemptToCloseMenu()
        {
            if (popupMenus.activeSelf) return;

            if (isInSubMenu)
            {
                if (graphicsMenu.activeSelf) { graphicsButton.Select(); isInSubMenu = false; return; }

                if (audioMenu.activeSelf) { audioButton.Select(); isInSubMenu = false; return; }

                if (networkMenu.activeSelf) { networkButton.Select(); isInSubMenu = false; return; }
            }

            if (loadGameMenu.activeSelf) { CloseLoadGameMenu(); SelectMenuButton(); return; }

            if (systemMenuObject.activeSelf) { CloseSystemMenu(); SelectMenuButton(); return; }
        }

        public void OpenLoadGameMenu()
        {
            loadGameMenu.SetActive(true);

            foreach (Button slot in loadMenuSlots)
            {
                if (slot.gameObject.activeSelf) { slot.Select(); break; }
            }
        }

        public void CloseLoadGameMenu()
        {
            SelectNoSlot();

            loadGameMenu.SetActive(false);
        }

        public void OpenSystemMenu()
        {
            systemMenuObject.SetActive(true);
            graphicsButton.Select();
        }

        public void CloseSystemMenu()
        {
            systemMenuObject.SetActive(false);
            CloseOtherSystemMenus();
            graphicsMenu.SetActive(true);
        }

        public void CloseOtherSystemMenus()
        {
            graphicsMenu.SetActive(false);
            audioMenu.SetActive(false);
            networkMenu.SetActive(false);
            controlsMenu.SetActive(false);
            cameraMenu.SetActive(false);
        }

        public void ExitToDesktop()
        {
            Application.Quit();
        }

        #endregion

        #region PopUp Menu Methods

        public void OpenCouldNotConnectMenu()
        {
            popupMenus.SetActive(true);
            CouldNotConnectPopup.SetActive(true);
            couldNotConnectAcceptButton.Select();
        }

        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            popupMenus.SetActive(true);
            NoSaveSlotsAvaiablePopup.SetActive(true);
            noSaveSlotsAvaiableAcceptButton.Select();
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if (currentSelectedSlot == CharacterSlot.NO_SLOT)
                return;

            popupMenus.SetActive(true);
            deleteCharacterSlotPopup.SetActive(true);
            deleteCharacterSlotAcceptButton.Select();
        }

        #endregion

        #region Button Methods

        public void SelectMenuButton()
        {
            for (int i = 0; i < menuButtons.Count; i++)
            {
                if (!menuButtons[i].IsActive()) continue;

                if (!menuButtons[i].gameObject.activeSelf) continue;

                menuButtons[i].Select();

                return;
            }
        }

        public void LoadGameButton(bool enableButton)
        {
            menuButtons[1].gameObject.SetActive(enableButton);
        }

        public void ContinueGameButton(bool enableButton)
        {
            menuButtons[0].gameObject.SetActive(enableButton);
        }

        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }

        public void SelectLastSelectedCharacterSlot()
        {
            UI_CharacterSaveSlot[] characterSlots = Resources.FindObjectsOfTypeAll<UI_CharacterSaveSlot>();

            foreach (UI_CharacterSaveSlot characterslot in characterSlots)
            {
                if (characterslot.characterSlot == currentSelectedSlot)
                {
                    characterslot.gameObject.GetComponent<Button>().Select();
                    break;
                }

                continue;
            }
        }

        public void DeleteCharacterSlot()
        {
            WorldSaveGameManager._Singleton.DeleteGame(currentSelectedSlot);
            CloseLoadGameMenu();
            menuButtons[1].Select();
            SelectNoSlot();
        }

        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }

        public void SelectFirstGraphicsOption()
        {
            resolutionOption.Select();
            isInSubMenu = true;
        }

        public void SelectFirstAudioOption()
        {
            masterAudioOption.Select();
            isInSubMenu = true;
        }

        public void SelectFirstNetworkOption()
        {
            if (goOfflineOption.gameObject.activeSelf) { goOfflineOption.Select(); isInSubMenu = true; return; }

            if (goOnlineOption.gameObject.activeSelf) { goOnlineOption.Select(); isInSubMenu = true; return; }
        }

        #endregion
    }
}
