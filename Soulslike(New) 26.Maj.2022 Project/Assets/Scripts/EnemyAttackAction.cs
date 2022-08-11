using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    [CreateAssetMenu(menuName ="A.I Actions")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3;
        public float recoveryTime = 2;

        public float maximumAttackAngle = -70f;
        public float minimumAttackAngle = 70f;

        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;
    }
}