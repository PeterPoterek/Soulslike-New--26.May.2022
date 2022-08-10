using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    public class EnemyStats : CharacterStats
    {
       

        Animator animator;




        private void Awake() {
            animator = GetComponentInChildren<Animator>();
        }


        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currHealth = maxHealth;

        }

        int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currHealth = currHealth - damage;

            animator.Play("TakeDamage");
            if(currHealth <= 0)
            {
                currHealth = 0;
                animator.Play("Death");
            }
        }


    }

}
