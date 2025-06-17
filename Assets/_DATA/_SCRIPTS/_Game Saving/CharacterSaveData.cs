using UnityEngine;

namespace NSG
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int sceneIndex = 1;

        [Header("Player Name")]
        public string characterName = "Character";

        [Header("Player Stats")]
        public int vigorLevel;
        public int enduranceLevel;

        [Header("Current Player Stats")]
        public float currentHealth;
        public float currentStamina;

        [Header("Time Played")]
        public float secondsPlayed;

        [Header("Player Position In The World")]
        public float worldPositionX;
        public float worldPositionY;
        public float worldPositionZ;
    }
}
