using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    public class EnemyManager : CharacterManager
    {
        public State currentState;
        public CharacterStats currentTarget;

        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStats enemyStats;
        public bool isPerformingAction;


        [Header("A.I Settings")]
        public float detectionRadius = 20f;
        public float minimumDetectionAngle = 50;
        public float maximumDetectionAngle = -50;

        public float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
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

        void GetNewAttack()
        {
            // Vector3 targetsDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
            // float viewableAngle = Vector3.Angle(targetsDirection,transform.forward);
            // enemyLocomotionManager.distanceFromTarget = Vector3.Distance
            // (enemyLocomotionManager.currentTarget.transform.position,transform.position);

            // int maxScore = 0;

            // for (int i = 0; i < enemyAttacks.Length; i++)
            // {
            //     EnemyAttackAction enemyAttackAction = enemyAttacks[i];
            //     if(enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
            //     && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            //     {
            //         if(viewableAngle <= enemyAttackAction.maximumAttackAngle 
            //         && viewableAngle >= enemyAttackAction.minimumAttackAngle)
            //         {
            //             maxScore += enemyAttackAction.attackScore;
            //         }
            //     }
            // }

            // int randomValue = Random.Range(0, maxScore);
            // int temporaryScore = 0;

            // for (int i = 0; i < enemyAttacks.Length; i++)
            // {
            //        EnemyAttackAction enemyAttackAction = enemyAttacks[i];
            //     if(enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
            //     && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            //     {
            //         if(viewableAngle <= enemyAttackAction.maximumAttackAngle 
            //         && viewableAngle >= enemyAttackAction.minimumAttackAngle)
            //         {
            //             if(currentAttack != null)
            //             return;

            //             temporaryScore += enemyAttackAction.attackScore;

            //             if(temporaryScore > randomValue)
            //             {
            //                 currentAttack = enemyAttackAction;
            //             }
            //         }
            //     }
            // }
        }

        void AttackTarget()
        {
            // if(isPerformingAction)
            // return;

            // if(currentAttack == null)
            // {
            //     GetNewAttack();
            // }
            // else
            // {
            //     isPerformingAction = true;
            //     currentRecoveryTime = currentAttack.recoveryTime;
            //     enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation,true);
            //     currentAttack = null;
            // }
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
