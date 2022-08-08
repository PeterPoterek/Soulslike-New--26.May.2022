using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;

        

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        public WeaponItem attackingWeapon;

        Animator animator;

        QuickSlotsUI quickSlotsUI;
        PlayerStats playerStats;
        InputHandler inputHandler;
        private void Awake()
        {

            animator = GetComponent<Animator>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if(weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if(weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }

            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem,bool isLeft)
        {
            if(isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true,weaponItem);

                #region Handle Left Weapon Idle Animations
                if(weaponItem != null)
                {
                    animator.CrossFade(weaponItem.left_Hand_Idle,0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty",0.2f);
                }
                #endregion
            }
        else
        {
                if(inputHandler.twoHandFlag)
                {
                    animator.CrossFade(weaponItem.th_Idle,0.2f);
                }
                else
            {
                animator.CrossFade("Both Arms Empty",0.2f);
                if(weaponItem != null)
                {
                    animator.CrossFade(weaponItem.right_Hand_Idle,0.2f);
                }
                else
                 {
                    animator.CrossFade("Right Hand Idle",0.2f);
                 }
                }
            
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false,weaponItem);

                }
        }

        #region Handle Weapon Damage Colliders
        

        void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

        }

        void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenRightDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void OpenLeftDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
        #endregion

        #region Handle Weapons Stamina Damage
        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage
            (Mathf.RoundToInt(attackingWeapon.baseStaminaCost * attackingWeapon.lightAttackMultiplier));
        }
        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage
            (Mathf.RoundToInt(attackingWeapon.baseStaminaCost * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion



    }

}

