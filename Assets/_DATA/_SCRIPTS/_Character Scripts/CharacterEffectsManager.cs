using System.Security.Cryptography;
using UnityEngine;

namespace NSG
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterManager character;

        // PROCESS INSTANT EFFECTS (TAKE DAMAGE, HEAL)
        // PROCESS TIMED EFFECTS (POISON, BUILD UPS)
        // PROCESS STATIC EFFECTS (ADDING / REMOVING BUFFS)

        protected virtual void Awake()
        {
            GetReferences();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }

        public virtual void GetReferences()
        {
            character = GetComponent<CharacterManager>();
        }
    }
}
