using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject impactVFX; // Impact effect prefab
    private bool collided; // Tracks if the projectile has collided

    void OnCollisionEnter(Collision co)
    {
        if (co.gameObject.tag != "Bullet" && co.gameObject.tag != "Player" && !collided)
        {
            collided = true;

            ContactPoint contact = co.contacts[0]; // Get the first contact point
            Vector3 impactPosition = contact.point + contact.normal * 0.1f; // Offset to avoid clipping
            Quaternion impactRotation = Quaternion.LookRotation(contact.normal); // Orient along the normal

            var impact = Instantiate(impactVFX, impactPosition, impactRotation); // Spawn effect properly

            // Ensure the impact has a particle system
            ParticleSystem ps = impact.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                // Destroy impact after the particle system's lifetime ends
                Destroy(impact.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                // Fallback in case there's no ParticleSystem (just destroy it after 1 second)
                Destroy(impact.gameObject, 1f);
            }

            Destroy(gameObject); // Destroy the projectile itself
        }
    }
}
