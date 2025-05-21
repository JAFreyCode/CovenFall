using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NSG
{
    public class UI_CharacterSaveSlot : MonoBehaviour
    {
        SaveFileDataWriter saveFileDataWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI characterLevel;
        public TextMeshProUGUI characterLocation;
        public TextMeshProUGUI characterTimePlayed;
        public Image characterSlotScreenshot;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            if (characterSlot == CharacterSlot.CharacterSlot_01)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager._Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager._Singleton.characterSlot01.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager._Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager._Singleton.characterSlot02.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager._Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager._Singleton.characterSlot03.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_04)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager._Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager._Singleton.characterSlot04.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_05)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager._Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager._Singleton.characterSlot05.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_06)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager._Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager._Singleton.characterSlot06.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_07)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager._Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager._Singleton.characterSlot07.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_08)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager._Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager._Singleton.characterSlot08.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_09)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager._Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager._Singleton.characterSlot09.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_10)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager._Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager._Singleton.characterSlot10.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager._Singleton.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager._Singleton.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreenManager._Singleton.SelectCharacterSlot(characterSlot);
        }
    }
}
