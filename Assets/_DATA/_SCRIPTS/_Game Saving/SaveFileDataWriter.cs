using UnityEngine;
using System;
using System.IO;

namespace NSG
{
    public class SaveFileDataWriter
    {
        [Header("Save Data")]
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";

        public bool CheckToSeeIfFileExists()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName))) return true;

            return false;
        }

        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        public void CreateNewCharacterSaveFile(CharacterSaveData characteData)
        {
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log($"CREATING SAVE FILE, AT SAVE PATH: {savePath} with the name: {saveFileName}");

                string dataToStore = JsonUtility.ToJson(characteData, true);

                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;

            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader fileReader = new StreamReader(stream))
                        {
                            dataToLoad = fileReader.ReadToEnd();
                        }
                    }

                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            return characterData;
        }
    }
}
