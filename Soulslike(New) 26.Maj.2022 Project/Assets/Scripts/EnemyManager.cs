using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        public bool isPerformingAction;
        [Header("A.I Settings")]
        public float detectionRadius = 20f;
        public float minimumDetectionAngle = 50;
        public float maximumDetectionAngle = -50;
        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        }
        private void Update()
        {
        }
        private void FixedUpdate()
        {
            HandleCurrentAction();

            
        }

        void HandleCurrentAction()
        {
            if(enemyLocomotionManager.currentTarget == null)
            {
                enemyLocomotionManager.HandleDetection();
            }
            else
            {
                enemyLocomotionManager.HandleMoveToTarget();
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,detectionRadius);
            
        Vector3 fovLine1 = Quaternion.AngleAxis(maximumDetectionAngle, transform.up) * transform.forward * detectionRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(minimumDetectionAngle, transform.up) * transform.forward * detectionRadius;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);
        }
    }

}
