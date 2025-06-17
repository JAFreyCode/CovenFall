using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NSG
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public PlayerManager player;

        [Header("Singleton")]
        private static WorldSaveGameManager Singleton;
        public static WorldSaveGameManager _Singleton { get { return Singleton; } private set { Singleton = value; } }

        [Header("DEBUG")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("Save Data Writer")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("Curren Character Data")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("Character Slots")]
        public CharacterSaveData characterSlot01;
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot03;
        public CharacterSaveData characterSlot04;
        public CharacterSaveData characterSlot05;
        public CharacterSaveData characterSlot06;
        public CharacterSaveData characterSlot07;
        public CharacterSaveData characterSlot08;
        public CharacterSaveData characterSlot09;
        public CharacterSaveData characterSlot10;

        private void Awake()
        {
            NSGUtils.SingletonCheck(ref Singleton, this);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            LoadAllCharacterProfiles();
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";

            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "CF0001";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "CF0002";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "CF0003";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "CF0004";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "CF0005";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "CF0006";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "CF0007";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "CF0008";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "CF0009";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "CF0010";
                    break;
                default:
                    break;
            }

            return fileName;
        }

        public void AttemptToCreateNewGame()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_06;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_07;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_08;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_09;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_10;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            TitleScreenManager._Singleton.DisplayNoFreeCharacterSlotsPopUp();
        }

        private void NewGame()
        {
            player.playerNetworkManager.vigor.Value = 10;
            player.playerNetworkManager.endurance.Value = 10;

            SaveGame();
            StartCoroutine(LoadWorldScene());
        }

        public void ContinueGame()
        {
            if (!PlayerPrefs.HasKey("LastPlayedSave"))
            {
                Debug.LogWarning("No previous save found.");
                return;
            }

            int lastPlayedSlotIndex = PlayerPrefs.GetInt("LastPlayedSave");

            currentCharacterSlotBeingUsed = GetSlotByIndex(lastPlayedSlotIndex);

            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = saveFileName
            };

            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            if (currentCharacterData == null)
            {
                for (int i = 1; i < 10; i++)
                {
                    currentCharacterSlotBeingUsed = GetSlotByIndex(i);

                    saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

                    saveFileDataWriter = new SaveFileDataWriter
                    {
                        saveDataDirectoryPath = Application.persistentDataPath,
                        saveFileName = saveFileName
                    };

                    currentCharacterData = saveFileDataWriter.LoadSaveFile();

                    if (currentCharacterData != null)
                        break;
                }

                if (currentCharacterData == null)
                    return;
            }

            LoadGame();
        }

        public void LoadGame()
        {
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();

            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        private void LoadAllCharacterProfiles()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            characterSlot02 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            characterSlot03 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            characterSlot04 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            characterSlot05 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            characterSlot06 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            characterSlot07 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            characterSlot08 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            characterSlot09 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            characterSlot10 = saveFileDataWriter.LoadSaveFile();

            if (characterSlot01 != null) 
            { 
                TitleScreenManager._Singleton.LoadGameButton(true); 
                TitleScreenManager._Singleton.ContinueGameButton(true); 
                return; 
            }
            else if (characterSlot02 != null)
            {
                TitleScreenManager._Singleton.LoadGameButton(true);
                TitleScreenManager._Singleton.ContinueGameButton(true);
                return;
            }
            else if (characterSlot03 != null)
            {
                TitleScreenManager._Singleton.LoadGameButton(true);
                TitleScreenManager._Singleton.ContinueGameButton(true);
                return;
            }
            else if (characterSlot04 != null)
            {
                TitleScreenManager._Singleton.LoadGameButton(true);
                TitleScreenManager._Singleton.ContinueGameButton(true);
                return;
            }
            else if (characterSlot05 != null)
            {
                TitleScreenManager._Singleton.LoadGameButton(true);
                TitleScreenManager._Singleton.ContinueGameButton(true);
                return;
            }
            else if (characterSlot06 != null)
            {
                TitleScreenManager._Singleton.LoadGameButton(true);
                TitleScreenManager._Singleton.ContinueGameButton(true);
                return;
            }
            else if (characterSlot07 != null)
            {
                TitleScreenManager._Singleton.LoadGameButton(true);
                TitleScreenManager._Singleton.ContinueGameButton(true);
                return;
            }
            else if (characterSlot08 != null)
            {
                TitleScreenManager._Singleton.LoadGameButton(true);
                TitleScreenManager._Singleton.ContinueGameButton(true);
                return;
            }
            else if (characterSlot09 != null)
            {
                TitleScreenManager._Singleton.LoadGameButton(true);
                TitleScreenManager._Singleton.ContinueGameButton(true);
                return;
            }
            else if (characterSlot10 != null)
            {
                TitleScreenManager._Singleton.LoadGameButton(true);
                TitleScreenManager._Singleton.ContinueGameButton(true);
                return;
            }
            else
            {
                TitleScreenManager._Singleton.LoadGameButton(false);
                TitleScreenManager._Singleton.ContinueGameButton(false);
                return;
            }
        }

        public void SaveGame()
        {
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);

            AssignLastPlayedSlot();
        }

        public void DeleteGame(CharacterSlot characterSlot)
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            saveFileDataWriter.DeleteSaveFile();
        }

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentCharacterData.sceneIndex);

            player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);

            AssignLastPlayedSlot();

            yield return null;
        }

        public CharacterSlot GetSlotByIndex(int index)
        {
            switch (index)
            {
                case 1:
                    return CharacterSlot.CharacterSlot_01;
                case 2:
                    return CharacterSlot.CharacterSlot_02;
                case 3:
                    return CharacterSlot.CharacterSlot_03;
                case 4:
                    return CharacterSlot.CharacterSlot_04;
                case 5:
                    return CharacterSlot.CharacterSlot_05;
                case 6:
                    return CharacterSlot.CharacterSlot_06;
                case 7:
                    return CharacterSlot.CharacterSlot_07;
                case 8:
                    return CharacterSlot.CharacterSlot_08;
                case 9:
                    return CharacterSlot.CharacterSlot_09;
                case 10:
                    return CharacterSlot.CharacterSlot_10;
                default:
                    return CharacterSlot.CharacterSlot_01;
            }
        }

        private void AssignLastPlayedSlot()
        {
            switch (currentCharacterSlotBeingUsed)
            {
                case CharacterSlot.CharacterSlot_01:
                    PlayerPrefs.SetInt("LastPlayedSave", 1);
                    break;
                case CharacterSlot.CharacterSlot_02:
                    PlayerPrefs.SetInt("LastPlayedSave", 2);
                    break;
                case CharacterSlot.CharacterSlot_03:
                    PlayerPrefs.SetInt("LastPlayedSave", 3);
                    break;
                case CharacterSlot.CharacterSlot_04:
                    PlayerPrefs.SetInt("LastPlayedSave", 4);
                    break;
                case CharacterSlot.CharacterSlot_05:
                    PlayerPrefs.SetInt("LastPlayedSave", 5);
                    break;
                case CharacterSlot.CharacterSlot_06:
                    PlayerPrefs.SetInt("LastPlayedSave", 6);
                    break;
                case CharacterSlot.CharacterSlot_07:
                    PlayerPrefs.SetInt("LastPlayedSave", 7);
                    break;
                case CharacterSlot.CharacterSlot_08:
                    PlayerPrefs.SetInt("LastPlayedSave", 8);
                    break;
                case CharacterSlot.CharacterSlot_09:
                    PlayerPrefs.SetInt("LastPlayedSave", 9);
                    break;
                case CharacterSlot.CharacterSlot_10:
                    PlayerPrefs.SetInt("LastPlayedSave", 10);
                    break;
                default:
                    break;
            }
        }
    }
}
