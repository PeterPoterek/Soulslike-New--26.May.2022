using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace L
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;
        NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidBody;

        public LayerMask detectionLayer;
        public CharacterStats currentTarget;

        public float distanceFromTarget;
        public float stoppingDistance = 1;
        public float rotationSpeed = 25f;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidBody = GetComponent<Rigidbody>();
        }

        private void Start() {
            navMeshAgent.enabled = false;
            enemyRigidBody.isKinematic = false; 
        }
        public void HandleDetection()
        {
           Collider[] colliders = Physics.OverlapSphere(transform.position,enemyManager.detectionRadius,detectionLayer);

           for (int i = 0; i < colliders.Length; i++)
           {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if(characterStats != null)
            {


                Vector3  targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection,  transform.forward);

                if(viewableAngle > enemyManager.minimumDetectionAngle
                && viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    currentTarget = characterStats;
                    Debug.Log(characterStats);
                }
            }
           }
        }

        public void HandleMoveToTarget()
        {
            if(enemyManager.isPerformingAction)
            return;
            
            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            float viewableAngle = Vector3.Angle(targetDirection,transform.forward);


            if(enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical",0, 0.1f, Time.deltaTime);
                navMeshAgent.enabled = false;
            }
            else
            {
                if(distanceFromTarget > stoppingDistance)
                {
                    enemyAnimatorManager.anim.SetFloat("Vertical",1,0.1f,Time.deltaTime);
                }
                else if(distanceFromTarget <= stoppingDistance)
                {
                    enemyAnimatorManager.anim.SetFloat("Vertical",0,0.1f,Time.deltaTime);
                }
            }
            HandleRotateTowardsTarget();
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        void HandleRotateTowardsTarget()
        {
            if(enemyManager.isPerformingAction)
            {
                Vector3 direction = currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if(direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
            }
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyRigidBody.velocity;

                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(currentTarget.transform.position);
                enemyRigidBody.velocity = targetVelocity;
                transform.rotation = Quaternion.Slerp
                (transform.rotation,navMeshAgent.transform.rotation , rotationSpeed / Time.deltaTime);
            }

            
        }
    }
}