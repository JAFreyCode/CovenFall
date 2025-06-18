using UnityEngine;

namespace NSG
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Ground Check & Jumping")]
        public Vector3 groundCheckSphereOffset;
        public Vector3 rayCastGroundCheckOffset;
        public float groundCheckSphereRadius = 1;
        public float gravityForce = -9.81f;
        public float groundDistance;
        [SerializeField] protected Vector3 yVelocity; // THE FORCE AT WHICH OUR CHARACTER IS PULLED UP OR DOWN
        [SerializeField] protected float groundVelocity = -20; // THE FORCE AT WHICH OUR CHARACTER IS STICKING TO THE GROUND WHILST THEY ARE GROUNDED
        [SerializeField] protected float fallStartYVelocity = -5; // THE FORCE AT WHICH OUR CHARACTER BEGINS TO FALLWHEN THEY BECOME UNGROUNDED
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;

        protected virtual void Awake()
        {
            GetReferences();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            if (character.isDead.Value) return;

            ApplyGravity();
            UpdateCharacterNetworkPosition();
        }

        protected virtual void FixedUpdate()
        {
            if (character.isDead.Value) return;

            HandleGroundCheck();
            HandleGroundRayCheck();
        }

        protected void HandleGroundCheck()
        {
            Vector3 checkOrigin = character.transform.position + groundCheckSphereOffset;

            if (!Physics.CheckSphere(character.transform.position + groundCheckSphereOffset, groundCheckSphereRadius, NSGUtils.GetGroundLayers().value))
            {
                character.isGrounded = character.characterController.isGrounded;
            }
            else
            {
                character.isGrounded = Physics.CheckSphere(character.transform.position + groundCheckSphereOffset, groundCheckSphereRadius, NSGUtils.GetGroundLayers().value);
            }
            
        }

        protected void HandleGroundRayCheck()
        {
            Ray ray = new Ray(transform.position + rayCastGroundCheckOffset, -transform.up);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, NSGUtils.GetGroundLayers().value))
            {
                groundDistance = Vector3.Distance(transform.position, hit.point);
            }
            else
            {
                groundDistance = 999;
            }

            Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.blue);
        }

        protected void ApplyGravity()
        {
            if (character.isGrounded)
            {
                groundDistance = 0;

                inAirTimer = 0;
                fallingVelocityHasBeenSet = false;
            }
            else
            {
                inAirTimer = inAirTimer + Time.deltaTime;
                yVelocity.y += gravityForce * Time.deltaTime;
                character.animator.SetFloat("GroundDistance", groundDistance);

                if (!character.characterNetworkManager.isJumping.Value && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }
            }

            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void UpdateCharacterNetworkPosition()
        {
            character.animator.SetBool("IsGrounded", character.isGrounded);

            if (character.IsOwner)
            {
                character.characterNetworkManager.networkPosition.Value = transform.position;
                character.characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    character.characterNetworkManager.networkPosition.Value,
                    ref character.characterNetworkManager.networkPositionVelocity,
                    character.characterNetworkManager.networkPositionSmoothTime
                    );

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    character.characterNetworkManager.networkRotation.Value,
                    character.characterNetworkManager.networkRotationSmoothTime
                    );
            }
        }

        protected virtual void GetReferences()
        {
            character = GetComponent<CharacterManager>();
        }

        protected void OnDrawGizmos()
        {
            if (character == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(character.transform.position + groundCheckSphereOffset, groundCheckSphereRadius);
        }

    }
}
