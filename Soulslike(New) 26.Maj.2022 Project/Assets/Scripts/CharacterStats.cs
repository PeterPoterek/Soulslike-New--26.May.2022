using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    public class CharacterStats : MonoBehaviour
    {
        
        [Header("Health")]
        public int healthLevel = 10;
        public int maxHealth;
        public int currHealth;
        [Header("Stamina")]
        public int staminaLevel = 10;
        public int maxStamina;
        public int currStamina;
    }
}