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
        public float ledgeRayLength = 1f;
        public float pullSpeed = 2.5f;
        public float gravityForce = -9.81f;
        public float groundDistance;
        [SerializeField] protected Vector3 yVelocity; // THE FORCE AT WHICH OUR CHARACTER IS PULLED UP OR DOWN
        [SerializeField] protected float groundVelocity = -20; // THE FORCE AT WHICH OUR CHARACTER IS STICKING TO THE GROUND WHILST THEY ARE GROUNDED
        [SerializeField] protected float fallStartYVelocity = -5; // THE FORCE AT WHICH OUR CHARACTER BEGINS TO FALLWHEN THEY BECOME UNGROUNDED
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;

        [Header("Ledge Detection")]
        public float ledgeCheckDistance = 0.5f;
        public float ledgePullForce = 2.5f;
        public float ledgePushForce = 2.5f;
        public float ledgeAngleThreshold = 45f;
        public float ledgeSmoothingFactor = 0.1f;
        private Vector3[] ledgeRayDirections = new Vector3[4];
        private RaycastHit[] ledgeHits = new RaycastHit[4];
        private bool[] ledgeHitsFound = new bool[4];
        private Vector3 ledgeVelocity = Vector3.zero;

        protected virtual void Awake()
        {
            GetReferences();
        }

        protected virtual void Start()
        {
            ledgeRayDirections[0] = Vector3.forward;
            ledgeRayDirections[1] = Vector3.right;
            ledgeRayDirections[2] = Vector3.back;
            ledgeRayDirections[3] = Vector3.left;
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
            character.isGrounded = Physics.CheckSphere(character.transform.position + groundCheckSphereOffset, groundCheckSphereRadius, NSGUtils.GetGroundLayers().value);

            if (!character.isGrounded)
            {
                CheckLedges(checkOrigin);
            }
        }

        private void CheckLedges(Vector3 origin)
        {
            bool foundLedge = false;
            Vector3 ledgePullDirection = Vector3.zero;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < 4; i++)
            {
                Vector3 rayDirection = transform.TransformDirection(ledgeRayDirections[i]);
                ledgeHitsFound[i] = Physics.Raycast(origin, rayDirection, out ledgeHits[i], ledgeCheckDistance, NSGUtils.GetGroundLayers().value);

                if (ledgeHitsFound[i])
                {
                    float angle = Vector3.Angle(ledgeHits[i].normal, Vector3.up);
                    if (angle > ledgeAngleThreshold)
                    {
                        foundLedge = true;
                        float distance = ledgeHits[i].distance;
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            ledgePullDirection = -rayDirection;
                        }
                    }
                }

                Debug.DrawRay(origin, rayDirection * ledgeCheckDistance, ledgeHitsFound[i] ? Color.green : Color.red);
            }

            if (foundLedge)
            {
                bool isMoving = Mathf.Abs(character.characterController.velocity.x) > 0.1f || 
                               Mathf.Abs(character.characterController.velocity.z) > 0.1f;

                if (!isMoving)
                {
                    Vector3 targetVelocity = ledgePullDirection * ledgePullForce;

                    ledgeVelocity = Vector3.Lerp(ledgeVelocity, targetVelocity, ledgeSmoothingFactor);

                    character.characterController.Move(ledgeVelocity * Time.deltaTime);
                }
                else
                {
                    ledgeVelocity = Vector3.zero;
                }
            }
            else
            {
                ledgeVelocity = Vector3.zero;
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
                yVelocity.y = groundVelocity;
            }
            else
            {
                inAirTimer = inAirTimer + Time.deltaTime;
                yVelocity.y += gravityForce * Time.deltaTime;
                character.animator.SetFloat("GroundDistance", groundDistance);

                if (!character.isJumping && !fallingVelocityHasBeenSet)
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

            // Draw ledge check rays
            Vector3 origin = character.transform.position + groundCheckSphereOffset;
            for (int i = 0; i < 4; i++)
            {
                Vector3 direction = character.transform.TransformDirection(ledgeRayDirections[i]);
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(origin, direction * ledgeCheckDistance);
            }
        }

    }
}
