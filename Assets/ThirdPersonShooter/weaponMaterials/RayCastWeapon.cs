using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
    public bool isFiring = false;
    public float fireRate = 10f;
    public ParticleSystem muzzleFlash;
    public ParticleSystem muzzleSmoke;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;

   // public Transform raycastOrigin;
    public Transform raycastDestination;
    public Transform laserSightOrigin;

    Ray ray;
    RaycastHit hitInfo;
    float accumalatedTime;


    public void StartFiring()
    {
        isFiring = true;
        accumalatedTime = 0.0f;
        FireBullet();

       
    }

    public void UpdateFiring(float deltaTime)
    {
        accumalatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while (accumalatedTime >= 0.0f)
        {
            FireBullet();
            accumalatedTime -= fireInterval;
        }
    }

    private void FireBullet()
    {
        muzzleFlash.Emit(1);
        muzzleSmoke.Emit(1);

        ray.origin = laserSightOrigin.position;
        ray.direction = raycastDestination.position - laserSightOrigin.position;


        var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        tracer.AddPosition(ray.origin);


        if (Physics.Raycast(ray, out hitInfo))
        {
            //transform.position = hitInfo.point;
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            tracer.transform.position = hitInfo.point;
        }
        else
        {
            transform.position = ray.origin + ray.direction * 1000.0f;
        }
    }

    public void StopFiring()
    {
        isFiring = false;
    }

}
