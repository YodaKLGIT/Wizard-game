using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSShooter : MonoBehaviour
{
    public Camera cam;

    private Vector3 destination;
    private bool leftHand; // Tracks whose turn it is to shoot
    private float timeToFire; // Time to fire the next projectile
    public float fireRate = 4; // Rate of fire
    public float arcRange = 1; // Range of the arc

    public GameObject projectile;
    public Transform LHFirePoint, RHFirePoint; // Left Hand and Right Hand Fire Points

    public float projectileSpeed = 30f; // Speed of the projectile



    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= timeToFire) // Left mouse button pressed
        {
            timeToFire = Time.time + 1 / fireRate; // Set the time to fire the next projectile
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        // Create a ray from the center of the camera
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Center of the screen
        RaycastHit hit;

        // Check for a hit
        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point; // Set destination to the hit point
        }
        else
        {
            destination = ray.GetPoint(1000); // Set destination far away if no hit
        }

        // Alternate between left hand and right hand
        if (leftHand)
        {
            leftHand = false; // Toggle the hand
            InstantiateProjectile(LHFirePoint);
        }
        else
        {
            leftHand = true; // Toggle the hand
            InstantiateProjectile(RHFirePoint);
        }
    }

    void InstantiateProjectile(Transform firePoint)
    {
        // Create the projectile
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;

        // Calculate the direction based on the camera's orientation
        Vector3 direction = (destination - firePoint.position).normalized;

        // Apply velocity to the projectile's Rigidbody
        Rigidbody rb = projectileObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed; // Move the projectile toward the target
        }

        // Add a random arc to the projectile
        iTween.PunchPosition(projectileObj, new Vector3(Random.Range(-arcRange, arcRange), Random.Range(-arcRange, arcRange), 0), Random.Range(0.5f, 2));

        // Destroy the projectile after 10 seconds if it doesn't collide
        Destroy(projectileObj, 5f);
    }
}
