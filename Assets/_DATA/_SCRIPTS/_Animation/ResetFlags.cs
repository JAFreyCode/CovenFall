using UnityEngine;

namespace NSG
{
    public class ResetFlags : StateMachineBehaviour
    {
        CharacterManager character;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null) { character = animator.GetComponent<CharacterManager>(); }

            character.isPerformingAction = false;
            character.characterAnimatorManager.applyRootMotion = false;
            character.canMove = true;
            character.canRotate = true;

            if (!character.IsOwner) return;

            character.characterNetworkManager.isJumping.Value = false;
        }
    }
}
