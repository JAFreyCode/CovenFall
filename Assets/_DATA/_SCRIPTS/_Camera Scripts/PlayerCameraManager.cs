using UnityEngine;

namespace NSG
{
    public class PlayerCameraManager : MonoBehaviour
    {
        [Header("Singleton")]
        private static PlayerCameraManager Singleton;
        public static PlayerCameraManager _Singleton { get { return Singleton; } private set { Singleton = value; } }

        [Header("References")]
        public PlayerManager player;
        public Transform pivotLeftAndRight;
        public Transform pivotUpAndDown;

        [Header("Camera Settings")]
        [SerializeField] float cameraFallowSmoothSpeed = 1;
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float leftAndRightRotationSpeed = 220;
        [SerializeField] float minimumPivot = -25;
        [SerializeField] float maximumPivot = 75;
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] float cameraCollisionSmoothing = 0.2f;

        [Header("Player Camera Data")]
        public Camera cameraObject;
        [SerializeField] Vector3 cameraVelocity;
        [SerializeField] Vector3 cameraObjectPosition;
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        [SerializeField] float cameraZPosition;
        [SerializeField] float cameraTargetZPosition;

        private void Awake()
        {
            NSGUtils.SingletonCheck(ref Singleton, this);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            AssignDefaultValues();
        }

        public void HandleAllCameraActions()
        {
            if (player == null)
                return;

            HandleFallowTarget();
            HandleCameraRotation();
            HandleCameraCollision();
        }

        private void HandleFallowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraFallowSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleCameraRotation()
        {
            float cameraHorizontal = WorldInputManager._Singleton.cameraHorizontal_Input;
            float cameraVertical = WorldInputManager._Singleton.cameraVertical_Input;

            leftAndRightLookAngle += (cameraHorizontal * leftAndRightRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle -= (cameraVertical * upAndDownRotationSpeed) * Time.deltaTime;

            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;
            
            cameraRotation.y = leftAndRightLookAngle;

            targetRotation = Quaternion.Euler(cameraRotation);
            pivotLeftAndRight.rotation = targetRotation;

            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;

            targetRotation = Quaternion.Euler(cameraRotation);
            pivotUpAndDown.localRotation = targetRotation;
        }

        public void HandleCameraCollision()
        {
            cameraTargetZPosition = cameraZPosition;

            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - pivotLeftAndRight.position;
            direction.Normalize();

            float targetDistance = Mathf.Abs(cameraTargetZPosition);
            float extendedDistance = targetDistance + cameraCollisionRadius;

            if (Physics.SphereCast(pivotLeftAndRight.position, cameraCollisionRadius, direction, out hit, extendedDistance, NSGUtils.GetGroundLayers()))
            {
                float distanceFromHitObject = Vector3.Distance(pivotLeftAndRight.position, hit.point);
                cameraTargetZPosition = -(distanceFromHitObject - cameraCollisionRadius);

                if (Mathf.Abs(cameraTargetZPosition) < cameraCollisionRadius)
                {
                    cameraTargetZPosition = -cameraCollisionRadius;
                }
            }

            if (Physics.SphereCast(pivotLeftAndRight.position, cameraCollisionRadius, -direction, out hit, cameraCollisionRadius, NSGUtils.GetGroundLayers()))
            {
                Vector3 pushOutDirection = hit.normal;
                pivotLeftAndRight.position += pushOutDirection * (cameraCollisionRadius - hit.distance);
            }

            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, cameraTargetZPosition, cameraCollisionSmoothing);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }

        private void AssignDefaultValues()
        {
            cameraZPosition = cameraObject.transform.position.z;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(cameraObject.transform.position, cameraCollisionRadius);

            if (pivotLeftAndRight == null || cameraObject == null) return;

            Vector3 direction = cameraObject.transform.position - pivotLeftAndRight.position;
            direction.Normalize();

            // SphereCast to check forward collision
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pivotLeftAndRight.position, pivotLeftAndRight.position + direction * Mathf.Abs(cameraZPosition));

            // Check proximity of the pivot to walls
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pivotLeftAndRight.position, pivotLeftAndRight.position - direction * cameraCollisionRadius);
            Gizmos.DrawWireSphere(pivotLeftAndRight.position - direction * cameraCollisionRadius, cameraCollisionRadius);
        }
    }
}
