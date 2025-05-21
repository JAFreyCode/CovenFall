using UnityEngine;

namespace NSG
{
    public class CharacterSFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        [Header("Sound Effects Settings")]
        public float playerSoundEffectsVolume = 1;

        protected virtual void Awake()
        {
            GetReferences();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        public void PlaySFX(AudioClip soundEffect, float volume)
        {
            if (soundEffect == null) { Debug.LogWarning("There is no audio in place for this action!"); return; }

            audioSource.PlayOneShot(soundEffect, volume);
        }

        protected virtual void GetReferences()
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
}
