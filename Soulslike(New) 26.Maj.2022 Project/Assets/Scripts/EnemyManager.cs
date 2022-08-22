using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace L
{
    public class EnemyManager : CharacterManager
    {
        public State currentState;
        public CharacterStats currentTarget;

        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStats enemyStats;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidBody;

        public bool isPerformingAction;
        
        public float distanceFromTarget;
        public float rotationSpeed = 25f;
        public float maximumAttackRange = 1.5f;



        [Header("A.I Settings")]
        public float movespeed = 1f;
        public float detectionRadius = 20f;
        public float minimumDetectionAngle = 50;
        public float maximumDetectionAngle = -50;

        public float viewableAngle;

        public float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            enemyRigidBody = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;


        }
        private void Start()
        {
            enemyRigidBody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTimer();
        }
        private void FixedUpdate()
        {
            HandleStateMachine();

            
        }

        void HandleStateMachine()
        {
            if(currentState != null)
            {
                State nextState = currentState.Tick(this,enemyStats,enemyAnimatorManager);

                if(nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        void SwitchToNextState(State state)
        {
            currentState = state;
        }

        
        
        void HandleRecoveryTimer()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if(isPerformingAction)
            {
                if(currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
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
