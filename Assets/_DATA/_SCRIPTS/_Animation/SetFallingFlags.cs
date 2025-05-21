using UnityEngine;

namespace NSG
{
    public class SetFallingFlags : StateMachineBehaviour
    {
        CharacterManager character;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null) { character = animator.GetComponent<CharacterManager>(); }

            if (!character.IsOwner) return;

            character.canMove = false;
            character.canRotate = false;
        }
    }
}
