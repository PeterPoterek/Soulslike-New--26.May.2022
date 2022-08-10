using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    public class PlayerStats : CharacterStats
    {



        public Healthbar healthbar;
        public Staminabar staminabar;

        AnimatorHandler animatorHandler;


        private void Awake() {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            healthbar = FindObjectOfType<Healthbar>();
            staminabar = FindObjectOfType<Staminabar>();
            
        }


        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currHealth = maxHealth;
            healthbar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currStamina = maxStamina;
            staminabar.SetMaxStamina(maxStamina);
        }

        int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }
        int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
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

        public void TakeStaminaDamage(int damage)
        {
            currStamina = currStamina - damage;
            staminabar.SetCurrentStamina(currStamina);
        }



    }

}
