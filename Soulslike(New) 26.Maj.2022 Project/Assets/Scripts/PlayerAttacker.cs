using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        public string lastAttack;
        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();

        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if(inputHandler.comboFlag)
            {
             animatorHandler.anim.SetBool("canDoCombo",false);
             
             if(lastAttack == weapon.OH_Light_Attack_1)
             {
               animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2,true);
             }
             if(lastAttack == weapon.OH_Light_Attack_2)
             {
               animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_3,true);
             }
             if(lastAttack == weapon.OH_Heavy_Attack_1)
             {
               
               animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_2,true);
              Invoke("SetAnimatorWeight",2f);

             }
             else if(lastAttack == weapon.TH_Light_Attack_01)
             {
              
              animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_02,true);
              Invoke("SetAnimatorWeight",2f);
             }

            }
        }
        public void HandleLightAttack(WeaponItem weapon)
        {
          weaponSlotManager.attackingWeapon = weapon;

          if(inputHandler.twoHandFlag)
          {
            animatorHandler.anim.SetLayerWeight(4,0);
            animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_01,true);
            lastAttack = weapon.TH_Light_Attack_01;
          }
          else
          {

            animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1,true);
            lastAttack = weapon.OH_Light_Attack_1;
          }

        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
          weaponSlotManager.attackingWeapon = weapon;
          if(inputHandler.twoHandFlag)
          {

          }
          else
          {
            animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1,true);
            lastAttack = weapon.OH_Heavy_Attack_1;

          }
        }

        void SetAnimatorWeight()
        {
          animatorHandler.anim.SetLayerWeight(4,1);

        }
    }

}

