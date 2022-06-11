using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace L
{
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;

        public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weapon)
        {
            if(isLeft == false)
            {
                if(weapon.itemIcon != null)
                {
                 rightWeaponIcon.sprite = weapon.itemIcon;
                 rightWeaponIcon.transform.localScale = new Vector3(3,1.2f); // workaround
                 rightWeaponIcon.enabled = true;

                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }

            }
            else
            {
                if(weapon.itemIcon != null)
                {
                 leftWeaponIcon.sprite = weapon.itemIcon;
                 leftWeaponIcon.transform.localScale = new Vector3(3,1.2f); // workaround
                 leftWeaponIcon.enabled = true;

                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }
        }
    }

}

