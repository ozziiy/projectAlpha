using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLaser : MonoBehaviour
{
    public TrailRenderer tracerEffect;
    public Transform laserSightOrigin;
    public Transform raycastDestination;
    Ray ray;
    RaycastHit hitInfo;

    // Start is called before the first frame update
    public void ActivateLaserTrail()
    {
        // LASER TRAIL
        ray.origin = laserSightOrigin.position;
        ray.direction = raycastDestination.position - laserSightOrigin.position;

        var movedLaserPos = new Vector3(ray.origin.x, ray.origin.y - 0.01f, ray.origin.z + 0.05f);

        var tracer = Instantiate(tracerEffect, movedLaserPos, Quaternion.identity);
        tracer.AddPosition(movedLaserPos);

        if (Physics.Raycast(ray, out hitInfo))
        {
            tracer.transform.position = hitInfo.point;
        }
        else
        {
            tracer.transform.position = ray.GetPoint(20f);
            //transform.position = ray.origin + ray.direction * 1000.0f;
        }
        
    }


}
