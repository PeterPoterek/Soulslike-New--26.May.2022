using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    public class PlayerStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currHealth;

        public Healthbar healthbar;

        AnimatorHandler animatorHandler;


        private void Awake() {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }


        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currHealth = maxHealth;
            healthbar.SetMaxHealth(maxHealth);
        }

        int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currHealth = currHealth - damage;

            healthbar.SetCurrentHealth(currHealth);

            animatorHandler.PlayTargetAnimation("TakeDamage",true);

            if(currHealth <= 0)
            {
                currHealth = 0;
                animatorHandler.PlayTargetAnimation("Death",true);
            }
        }


    }

}
