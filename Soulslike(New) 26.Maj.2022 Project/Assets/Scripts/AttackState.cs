using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    public class AttackState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats,EnemyAnimatorManager enemyAnimatorManager)
        {
            return this;
        }
    }

}