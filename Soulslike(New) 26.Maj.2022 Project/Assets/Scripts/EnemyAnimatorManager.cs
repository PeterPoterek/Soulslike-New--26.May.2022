using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  L
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;
        private void Awake() {
            anim = GetComponent<Animator>();

            enemyManager = GetComponentInParent<EnemyManager>();
        }

        private void OnAnimatorMove() {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidBody.drag = 0;

            Vector3 deltaPostion = anim.deltaPosition;
            deltaPostion.y = 0;

            Vector3 velocity = deltaPostion / delta;
            enemyManager.enemyRigidBody.velocity = velocity * enemyManager.movespeed;

        }
    }
}