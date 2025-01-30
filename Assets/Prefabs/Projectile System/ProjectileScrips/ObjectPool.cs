using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject projectilePrefab; // The projectile prefab
    public int poolSize = 10; // Number of projectiles to pool
    private Queue<GameObject> projectilePool; // The pool itself

    void Start()
    {
        // Initialize the pool and populate it with projectiles
        projectilePool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject proj = Instantiate(projectilePrefab);
            proj.SetActive(false); // Initially deactivate the projectile
            projectilePool.Enqueue(proj);
        }
    }

    public GameObject GetProjectile()
    {
        // If the pool is empty, we can optionally expand it
        if (projectilePool.Count > 0)
        {
            GameObject proj = projectilePool.Dequeue(); // Get a projectile from the pool
            proj.SetActive(true); // Activate it for use
            return proj;
        }
        else
        {
            // Optionally instantiate a new projectile if the pool is empty
            GameObject proj = Instantiate(projectilePrefab);
            return proj;
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false); // Deactivate it to return to the pool
        projectilePool.Enqueue(projectile); // Return it to the pool
    }
}
