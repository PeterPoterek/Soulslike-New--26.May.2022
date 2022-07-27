using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 0;
        public string interactableText;
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);

        }

        public virtual void Interact(PlayerManager playerManager)
        {
            Debug.Log("You Interacted with Object");
        }
    }

}
