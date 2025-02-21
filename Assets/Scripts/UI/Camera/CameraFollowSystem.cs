using System;
using UnityEngine;

namespace UI.Camera
{
    /**
     * Controls camera movement to follow a target while handling obstacle avoidance
     */
    public class CameraFollowSystem : MonoBehaviour
    {
        [Header("Target settings")]
        [SerializeField] public Transform target;
        [Tooltip("Smoothing factor for camera movement")]
        [SerializeField] private float smoothSpeed = 5f; /// Smoothing factor for camera movement
                                                         
        [Header("Camera position settings")]
        [Tooltip("Angle around the target (in degrees)")]
        [SerializeField, Range(-180f, 180f)] private float horizontalAngle = -90f; /// Angle around the target (in degrees)
        [Tooltip("Elevation angle around the target (in degrees)")]
        [SerializeField, Range(0f, 90f)] private float verticalAngle = 56f; /// Elevation angle around the target (in degrees)
        [Tooltip("Distance between the camera position and target position")]
        [SerializeField] private float distance = 25f; /// Distance between the camera position and target position
                                                      
        // Cache for optimization
        private float initialDistance;
        private float lastDistance;
        private Vector3 cachedDirection;
        private readonly float obstacleOffset = 0.5f;
        
        // Cache for RaycastNonAlloc
        private readonly RaycastHit[] raycastHits = new RaycastHit[64];

        private void Start()
        {
            initialDistance = distance;
            lastDistance = initialDistance;
            // Calculate and cache the direction vector since angles don't change that often
            UpdateDirectionCache();
        }

        /**
         * Updates the cached direction vector based on horizontal and vertical angles
         */
        private void UpdateDirectionCache()
        {
            float x = (float) Math.Cos(horizontalAngle * Mathf.Deg2Rad) * Mathf.Cos(verticalAngle * Mathf.Deg2Rad);
            float y = (float) Math.Sin(verticalAngle * Mathf.Deg2Rad);
            float z = (float) Math.Sin(horizontalAngle * Mathf.Deg2Rad) * Mathf.Cos(verticalAngle * Mathf.Deg2Rad);
            
            cachedDirection = new Vector3(x, y, z).normalized;
        }

        /**
         * Calculates the desired camera position at given distance from target
         */
        private Vector3 CalculateTargetPosition(float currentDistance)
        {
            return target.position + cachedDirection * currentDistance + 1.0f * Vector3.up;
        }

        private void LateUpdate()
        {
            if (!target) return;
        
            // Calculate ideal camera position a maximum distance
            distance = initialDistance;
            var idealPosition = CalculateTargetPosition(initialDistance);
            var targetPosition = idealPosition;

            // Check for obstacles between target and camera
            Ray ray = new Ray(target.position, cachedDirection);
            int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, initialDistance);
        
            // Find the closest building obstacle if any
            for (int i = 0; i < hitCount; i++)
            {
                if (!raycastHits[i].collider.CompareTag("Building")) continue;
                distance = Mathf.Max(raycastHits[i].distance - obstacleOffset, 0f); // Prevent negative distance
                targetPosition = CalculateTargetPosition(distance);
                break;
            }

            // Determine whether to use smooth or instant movement.
            bool shouldSmoothMove = Mathf.Approximately(distance, lastDistance) ||
                                    Vector3.Distance(targetPosition, idealPosition) < 0.01f;
            
            // Update camera position
            transform.position = shouldSmoothMove
                ? Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed)
                : targetPosition;
            
            lastDistance = distance;
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            // Update direction cache when values are changed in inspector
            UpdateDirectionCache();
            // Update new distance
            initialDistance = distance;
        }
        #endif
    }
}