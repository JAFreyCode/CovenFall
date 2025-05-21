using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Unity.Netcode;

namespace NSG
{
    public class WorldGraphicsManager : MonoBehaviour
    {
        [Header("Singleton")]
        private static WorldGraphicsManager Singleton;
        public static WorldGraphicsManager _Singleton { get { return Singleton; } private set { Singleton = value; } }

        [Header("Graphics Settings")]
        public TMP_Dropdown resolutionDropDown;
        public TMP_Dropdown screenModeDropdown;
        public TMP_Dropdown qualityDropdown;

        [Header("Audio Settings")]
        public Slider masterAudioSlider;
        public TMP_Text masterAudioValue;
        public Slider menuMusicSlider;
        public TMP_Text menuMusicValue;

        [Header("Network Settings")]
        public GameObject GoOnlineButton;
        public GameObject GoOfflineButton;

        [Header("Audio Data")]
        public AudioSource mainMenuMusicSource;

        [Header("Game Settings Data")]
        private Resolution[] resolutions;

        private string settingsFilePath;

        void Awake()
        {
            Application.targetFrameRate = 60;

            NSGUtils.SingletonCheck(ref Singleton, this);

            settingsFilePath = Path.Combine(Application.persistentDataPath, "gameSettings.json");
        }

        void Start()
        {
            GetResolutions();
            GetScreenModes();
            GetQualityOptions();

            GetCurrentMainAudio();
            GetCurrentMenuMusicAudio();

            screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
            qualityDropdown.onValueChanged.AddListener(SetQuality);
            resolutionDropDown.onValueChanged.AddListener(SetResolution);

            masterAudioSlider.onValueChanged.AddListener(SetMainAudio);
            menuMusicSlider.onValueChanged.AddListener(SetMenuMusicAudio);

            LoadGameSettings();
        }

        private void ApplyGameSettings()
        {
            SaveGameSettings(resolutionDropDown.value, screenModeDropdown.value, qualityDropdown.value, masterAudioSlider.value, menuMusicSlider.value);
        }

        #region JSON Save and Load

        private void SaveGameSettings(int resolutionIndex, int screenModeIndex, int qualityIndex, float masterAudio, float menuMusicAudio)
        {
            GameSettingsData settings = new GameSettingsData
            {
                resolutionIndex = resolutionIndex,
                screenModeIndex = screenModeIndex,
                qualityIndex = qualityIndex,
                masterAudio = masterAudio * 0.01f,  // Save in 0-1 range
                menuMusicAudio = menuMusicAudio * 0.01f  // Save in 0-1 range
            };

            string json = JsonUtility.ToJson(settings, true);
            File.WriteAllText(settingsFilePath, json);
        }

        private void LoadGameSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                string json = File.ReadAllText(settingsFilePath);
                GameSettingsData settings = JsonUtility.FromJson<GameSettingsData>(json);

                SetResolution(settings.resolutionIndex);
                SetScreenMode(settings.screenModeIndex);
                SetQuality(settings.qualityIndex);

                SetMainAudio(settings.masterAudio * 100); // Convert back to 0-100 range for the slider
                SetMenuMusicAudio(settings.menuMusicAudio * 100); // Convert back to 0-100 range for the slider
            }
            else
            {
                // If no settings file exists, use default values
                SetResolution(resolutionDropDown.value);
                SetScreenMode(GetCurrentScreenModeIndex());
                SetQuality(QualitySettings.GetQualityLevel());
                SetMainAudio(100);
                SetMenuMusicAudio(100);
            }
        }

        #endregion

        #region Graphics

        private void SetResolution(int index)
        {
            if (index < 0 || index >= resolutions.Length) index = 0;

            Resolution resolution = resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRateRatio);

            ApplyGameSettings();
        }

        private void SetScreenMode(int index)
        {
            switch (index)
            {
                case 0:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 2:
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                    break;
                case 3:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
            }

            ApplyGameSettings();
        }

        private void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index, true);

            ApplyGameSettings();
        }

        private void GetResolutions()
        {
            resolutions = Screen.resolutions;
            resolutionDropDown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "Hz";
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width
                    && resolutions[i].height == Screen.currentResolution.height
                    && resolutions[i].refreshRateRatio.value == Screen.currentResolution.refreshRateRatio.value)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropDown.AddOptions(options);
            resolutionDropDown.value = currentResolutionIndex;
            resolutionDropDown.RefreshShownValue();
        }

        private void GetQualityOptions()
        {
            string[] qualityLevels = QualitySettings.names;

            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(new List<string>(qualityLevels));

            qualityDropdown.value = QualitySettings.GetQualityLevel();
            qualityDropdown.RefreshShownValue();
        }

        private void GetScreenModes()
        {
            screenModeDropdown.ClearOptions();
            screenModeDropdown.AddOptions(new List<string> { "Fullscreen", "Windowed", "Maximized Window", "Borderless" });

            screenModeDropdown.value = GetCurrentScreenModeIndex();
            screenModeDropdown.RefreshShownValue();
        }

        int GetCurrentScreenModeIndex()
        {
            switch (Screen.fullScreenMode)
            {
                case FullScreenMode.ExclusiveFullScreen: return 0;
                case FullScreenMode.Windowed: return 1;
                case FullScreenMode.MaximizedWindow: return 2;
                case FullScreenMode.FullScreenWindow: return 3;
                default: return 0;
            }
        }

        #endregion

        #region Audio

        private void GetCurrentMainAudio()
        {
            float currentMasterAudio = AudioListener.volume;
            masterAudioSlider.maxValue = 100;
            masterAudioSlider.minValue = 0;
            masterAudioSlider.value = currentMasterAudio * 100;
            masterAudioValue.text = Mathf.Round(currentMasterAudio * 100).ToString();
        }

        private void SetMainAudio(float volume)
        {
            float normalizedVolume = volume * 0.01f;
            AudioListener.volume = normalizedVolume;
            masterAudioSlider.value = volume;
            masterAudioValue.text = Mathf.Round(volume).ToString();

            ApplyGameSettings();
        }

        private void GetCurrentMenuMusicAudio()
        {
            float currentMasterAudio = mainMenuMusicSource.volume;
            menuMusicSlider.maxValue = 100;
            menuMusicSlider.minValue = 0;
            menuMusicSlider.value = currentMasterAudio * 100;
            menuMusicValue.text = Mathf.Round(currentMasterAudio * 100).ToString();
        }

        private void SetMenuMusicAudio(float volume)
        {
            float normalizedVolume = volume * 0.01f;
            mainMenuMusicSource.volume = normalizedVolume;
            menuMusicSlider.value = volume;
            menuMusicValue.text = Mathf.Round(volume).ToString();

            ApplyGameSettings();
        }

        #endregion

        #region Network

        public void CheckNetworkConnect()
        {
            if (TitleScreenManager._Singleton.isOnline)
            {
                GoOnlineButton.SetActive(false);
                GoOfflineButton.SetActive(true);
            }
            else
            {
                GoOfflineButton.SetActive(false);
                GoOnlineButton.SetActive(true);
            }
        }

        public void AttemptToConnectToNetwork()
        {
            TitleScreenManager._Singleton.StartNetworkAsHost();
            CheckNetworkConnect();
        }

        public void DisconnectFromTheNetwork()
        {
            NetworkManager.Singleton.Shutdown();
            TitleScreenManager._Singleton.isOnline = false;
            TitleScreenManager._Singleton.SetOnlineStatusText();
            CheckNetworkConnect();
        }

        #endregion
    }
}
