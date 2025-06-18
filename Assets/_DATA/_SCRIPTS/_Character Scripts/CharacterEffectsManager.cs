using UnityEngine;
using System.Collections.Generic;

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

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            if (WorldEffectsManager._Singleton.bloodSplatterVFX.Count != 0)
            {
                GameObject bloodSplatter = Instantiate(SelectRandomVFXFromArray(WorldEffectsManager._Singleton.bloodSplatterVFX), contactPoint, Quaternion.identity);
            }
        }

        public GameObject SelectRandomVFXFromArray(List<GameObject> vfx)
        {
            int arraySize = vfx.Count;
            int randomIndex = Random.Range(0, arraySize - 1);

            return WorldEffectsManager._Singleton.bloodSplatterVFX[randomIndex];
        }
    }
}
