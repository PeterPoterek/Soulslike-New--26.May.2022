using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    [CreateAssetMenu(menuName ="Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;
        [Header("Idle Animations")]
        public string right_Hand_Idle;
        public string left_Hand_Idle;

        [Header("One Handed Attacks")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Light_Attack_3;
        public string OH_Heavy_Attack_1;
        public string OH_Heavy_Attack_2;
        
    }

}
