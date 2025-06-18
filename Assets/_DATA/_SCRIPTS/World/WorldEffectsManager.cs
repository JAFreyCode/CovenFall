using System.Collections.Generic;
using UnityEngine;

namespace NSG
{
    public class WorldEffectsManager : MonoBehaviour
    {
        [Header("Singleton")]
        private static WorldEffectsManager Singleton;
        public static WorldEffectsManager _Singleton { get { return Singleton; } private set { Singleton = value; } }

        [Header("VFX")]
        public List<GameObject> bloodSplatterVFX = new List<GameObject>();

        [Header("Damage Effect")]
        public InstantDamageEffect instantDamageEffect;

        [Header("Character Effects")]
        public List<InstantCharacterEffect> instantEffects;

        private void Awake()
        {
            NSGUtils.SingletonCheck(ref Singleton, this);
        }

        private void Start()
        {
            GenerateEffectIDs();
        }

        private void GenerateEffectIDs()
        {
            for (int i = 0; i < instantEffects.Count; i++)
            {
                instantEffects[i].instantEffectID = i;
            }
        }
    }
}
