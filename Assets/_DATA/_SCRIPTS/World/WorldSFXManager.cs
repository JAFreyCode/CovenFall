using UnityEngine;

namespace NSG
{
    public class WorldSFXManager : MonoBehaviour
    {
        private static WorldSFXManager Singleton;
        public static WorldSFXManager _Singleton { get { return Singleton; } private set { Singleton = value; } }

        [Header("Sound Effects")]
        public AudioClip rollSFX;
        public AudioClip backStepSFX;

        private void Awake()
        {
            NSGUtils.SingletonCheck(ref Singleton, this);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
