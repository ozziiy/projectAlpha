using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform laserDot;
    [SerializeField] private Transform upperChest;

    public float aimSpeed = 0.3f;
    public Rig aimLayer;
    public Rig bodyAimLayer;
    public Rig bodyAimStaticLayer;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    public RayCastWeapon weapon;
    public ActivateLaser laser;


    private void Awake() {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        weapon = weapon.GetComponentInChildren<RayCastWeapon>();
        laser = laser.GetComponentInChildren<ActivateLaser>();
    }

    private void Update()
    {


        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            laserDot.gameObject.SetActive(true);
            thirdPersonController.SetRotateOnMove(false);
            laser.ActivateLaserTrail();

            //set animation layers

            aimLayer.weight += Time.deltaTime / aimSpeed;
            bodyAimLayer.weight += Time.deltaTime / aimSpeed;
            bodyAimStaticLayer.weight -= Time.deltaTime / (aimSpeed);

            //STARTER ASSET LOOK FORWARD

            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                mouseWorldPosition = raycastHit.point;
            }
            else
            {
                mouseWorldPosition = ray.GetPoint(20f);
            }

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            
            
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.StartFiring();

            }
            if (weapon.isFiring)
            {
                weapon.UpdateFiring(Time.deltaTime);
            }

            if (Input.GetButtonUp("Fire1"))
            {
                weapon.StopFiring();
            }          

        }
        else
        {
            weapon.StopFiring();
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            laserDot.gameObject.SetActive(false);
            thirdPersonController.SetRotateOnMove(true);
            

            aimLayer.weight -= Time.deltaTime / aimSpeed;
            bodyAimLayer.weight -= Time.deltaTime / aimSpeed;
            bodyAimStaticLayer.weight += Time.deltaTime / (aimSpeed);
            

            //TRIED FIXING HEAD CLIPPING WITH GUN 
            //Debug.Log(upperChest.transform.rotation.eulerAngles);
            //if (upperChest.transform.rotation.eulerAngles.x > 46f)
            //{
            //    bodyAimStaticLayer.GetComponentInChildren<MultiRotationConstraint>().weight -= Time.deltaTime / (aimSpeed);
                
            //}
            //else if(upperChest.transform.rotation.eulerAngles.x < 34f)
            //{              
            //    if (bodyAimStaticLayer.GetComponentInChildren<MultiRotationConstraint>().weight < 0.25f)
            //    {
            //        bodyAimStaticLayer.GetComponentInChildren<MultiRotationConstraint>().weight += Time.deltaTime / (aimSpeed);
            //    }            
            //}

        }
    }
}
