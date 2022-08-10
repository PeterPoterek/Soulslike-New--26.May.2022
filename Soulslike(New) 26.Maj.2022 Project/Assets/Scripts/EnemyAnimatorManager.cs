using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  L
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        private void Awake() {
            anim = GetComponent<Animator>();

            enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
        }

        private void OnAnimatorMove() {
            float delta = Time.deltaTime;
            enemyLocomotionManager.enemyRigidBody.drag = 0;

            Vector3 deltaPostion = anim.deltaPosition;
            deltaPostion.y = 0;

            Vector3 velocity = deltaPostion / delta;
            enemyLocomotionManager.enemyRigidBody.velocity = velocity; // * movespeed

        }
    }
}