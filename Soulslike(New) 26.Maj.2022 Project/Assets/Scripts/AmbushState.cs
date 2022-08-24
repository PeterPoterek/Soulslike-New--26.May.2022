using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    public class AmbushState : State
    {
        public bool isSleeping;
        public float detectionRadius = 2f;
        public string sleepAnimation;
        public string wakeAnimation;
        public LayerMask detectionLayer;

        public PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats,EnemyAnimatorManager enemyAnimatorManager)
        {
            if(isSleeping && enemyManager.isInteracting == false)
            {
                enemyAnimatorManager.PlayTargetAnimation(sleepAnimation,true);
            }


            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position,detectionRadius,detectionLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
                
                Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection,transform.forward);

                if(characterStats != null)
                {
                    Vector3 targetsDirection = characterStats.transform.position - enemyManager.transform.position;
                    viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

                    if(viewableAngle > enemyManager.minimumDetectionAngle 
                    && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                        isSleeping = false;
                        enemyAnimatorManager.PlayTargetAnimation(wakeAnimation,true);
                    }
                }
            }

            if(enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }


        }

    }
}
