using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L
{
public class CameraHandler : MonoBehaviour
 {
     InputHandler inputHandler;
     PlayerManager playerManager;
     public Transform targetTransform;
     public Transform cameraTransform;
     public Transform cameraPivotTransform;
     private Transform myTransform;
     private Vector3 cameraTransformPosition;
     public LayerMask ignoreLayers;
     public LayerMask environmentLayer;
     private Vector3 cameraFollowVelocity = Vector3.zero;

     public static CameraHandler singelton;

     public float lookSpeed = 0.1f;
     public float followSpeed = 0.1f;
     public float pivotSpeed = 0.03f;
     private float targetPosition;

     private float defaultPosition;
     private float lookAngle;
     private float pivotAngle;
     public float minimumPivot = -35f;
     public float maximumPivot = 35f;

     public float cameraSphereRadius = 0.2f;
     public float cameraCollisionOffset = 0.2f;
     public float minimumCollisionOffset = 0.2f;


     public float lockedPivotPostion = 2.25f;
     public float unlockedPivotPostion = 1.65f;
     public CharacterManager currentLockOnTarget;
     List<CharacterManager> avalibleTargets = new List<CharacterManager>();
     public CharacterManager nearestLockOnTarget;
     public CharacterManager leftLockOnTarget;
     public CharacterManager rightLockOnTarget;
     public float maximumLockOnDistance = 30f;
     private void Start() 
     {
         environmentLayer = LayerMask.NameToLayer("Environment");
         
     }


     void Awake()
     {
         singelton = this;
         myTransform = transform;
         defaultPosition = cameraTransform.localPosition.z;
         //ignoreLayers = ~( 1 << 8 | 1 << 9 | 1 << 10);   
         targetTransform = FindObjectOfType<PlayerManager>().transform;
         inputHandler = FindObjectOfType<InputHandler>();
         playerManager = FindObjectOfType<PlayerManager>();
     }
     public void FollowTarget(float delta)
     {
            Vector3 targetPosition = Vector3.SmoothDamp
            (myTransform.position,targetTransform.position,ref cameraFollowVelocity,delta /followSpeed);
            myTransform.position = targetPosition;

            HandleCameraCollision(delta);
     }

     public void HandleCameraRotation(float delta,float mouseXinput,float mouseYInput)
     {
         if(inputHandler.lockOnFlag == false && currentLockOnTarget == null)
         {
         lookAngle += mouseXinput * lookSpeed * delta;
         pivotAngle -= mouseYInput * pivotSpeed * delta;
         pivotAngle = Mathf.Clamp(pivotAngle,minimumPivot,maximumPivot);

         Vector3 rotation = Vector3.zero;
         rotation.y = lookAngle;
         Quaternion targetRotation = Quaternion.Euler(rotation);
         myTransform.rotation = targetRotation;

         rotation = Vector3.zero;
         rotation.x = pivotAngle;
         
         targetRotation = Quaternion.Euler(rotation);
         cameraPivotTransform.localRotation = targetRotation;     
         }
         else
         {
             
             Vector3 dir = currentLockOnTarget.transform.position - transform.position;
             dir.Normalize();
             dir.y = 0;

             Quaternion targetRotation = Quaternion.LookRotation(dir);
             transform.rotation = targetRotation;

             dir = currentLockOnTarget.transform.position -cameraPivotTransform.position;
             dir.Normalize();
             targetRotation = Quaternion.LookRotation(dir);
             Vector3 eulerAngle = targetRotation.eulerAngles;
             eulerAngle.y = 0;
             cameraPivotTransform.localEulerAngles = eulerAngle;
         }

       
     }

     private void HandleCameraCollision(float delta)
     {
         targetPosition = defaultPosition;
         RaycastHit hit;
         Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
         direction.Normalize();

         if(Physics.SphereCast
         (cameraPivotTransform.position, cameraSphereRadius, direction, out hit , Mathf.Abs(targetPosition),ignoreLayers))
         {
             float dis = Vector3.Distance(cameraPivotTransform.position,hit.point);
             targetPosition = -(dis - cameraCollisionOffset);
         }
         if(Mathf.Abs(targetPosition)< minimumCollisionOffset)
         {
             targetPosition = -minimumCollisionOffset;
         }
         cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z,targetPosition,delta/ 0.2f);
         cameraTransform.localPosition = cameraTransformPosition;
     }
     public void HandleLockOn()
     {
         float shortestDistance = Mathf.Infinity;
         float shortestDistanceOfLeftTarget = -Mathf.Infinity;
         float shortestDistanceOfRightTarget = Mathf.Infinity; 

         Collider[] colliders = Physics.OverlapSphere(targetTransform.position,26);

         for(int i =0; i< colliders.Length; i++)
         {
             CharacterManager character = colliders[i].GetComponent<CharacterManager>();

             if(character != null)
             {
                 Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                 float distanceFromTarget = Vector3.Distance(targetTransform.position , character.transform.position);
                 float viewableAngle = Vector3.Angle(lockTargetDirection,cameraTransform.forward);
                 RaycastHit hit;

                 
                 

                 if(character.transform.root != targetTransform.transform.root && 
                 viewableAngle > -50 && viewableAngle< 50 && distanceFromTarget <= maximumLockOnDistance)
                 {
                     if(Physics.Linecast(playerManager.lockOnTransform.position,character.lockOnTransform.position, out hit))
                     {
                         Debug.DrawLine(playerManager.lockOnTransform.position,character.lockOnTransform.position);
                         if(hit.transform.gameObject.layer == environmentLayer)
                         {
                             //

                         }
                         else
                         {
                           avalibleTargets.Add(character);

                         }
                     }

                 }  
             }  

         }
         for(int k = 0; k < avalibleTargets.Count; k++)
         {
             float distanceFromTarget = Vector3.Distance
             (targetTransform.position,avalibleTargets[k].transform.position);

             if(distanceFromTarget < shortestDistance)
             {

                 shortestDistance = distanceFromTarget;
                 nearestLockOnTarget = avalibleTargets[k];
             }
             if(inputHandler.lockOnFlag)
             {
                 // Vector3 relativeEnemyPosition = currLockOnTarget.transform.InverseTransformPoint(avalibleTargets[k].transform.position);
                // var distanceFromLeftTarget = currLockOnTarget.transform.position.x - avalibleTargets[k].transform.position.x;
                // var distanceFromRightTarget = currLockOnTarget.transform.position.x + avalibleTargets[k].transform.position.x;
                Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(avalibleTargets[k].transform.position);
                var distanceFromLeftTarget = relativeEnemyPosition.x;
                var distanceFromRightTarget = relativeEnemyPosition.x;



                 if(relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget && avalibleTargets[k]!= currentLockOnTarget)
                 {
                     shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                     leftLockOnTarget = avalibleTargets[k];
                 }

                else if(relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget && avalibleTargets[k]!= currentLockOnTarget) 
                 {
                     shortestDistanceOfRightTarget = distanceFromRightTarget;
                     rightLockOnTarget = avalibleTargets[k];
                 }
             }
         }     
     }

     public void ClearLockOnTargets()
     {
        

         avalibleTargets.Clear();
         nearestLockOnTarget = null;
         currentLockOnTarget = null;
     }
     public void SetCameraHeight()
     {
         Vector3 velocity = Vector3.zero;
         Vector3 newLockedPostion = new Vector3(0,lockedPivotPostion);
         Vector3 newUnlockedPostion = new Vector3(0,unlockedPivotPostion);

         if(currentLockOnTarget != null)
         {
             cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp
             (cameraPivotTransform.transform.localPosition , newLockedPostion,ref velocity, Time.deltaTime);
         }
         else
         {
             cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp
             (cameraPivotTransform.transform.localPosition , newUnlockedPostion, ref velocity, Time.deltaTime);
         }

     }
    

 

















 }
}