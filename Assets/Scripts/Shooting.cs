using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    // Shooting variables
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletForce = 10f;
    public float fireRate = 0.5f;
    private float nextFireTime;
    public bool hasHeadshotPowerup = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }



    void Shoot()
    {
        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Apply force to bullet
        bullet.GetComponent<Rigidbody>().AddForce(firePoint.forward * bulletSpeed * bulletForce);
    }



    void Start()
    {
        // Give the player the headshot powerup on their last turn
        if (currentTurn == maxTurns - 1)
        {
            hasHeadshotPowerup = true;
        }
    }


// Bullet.cs
    public class Bullet : MonoBehaviour
    {

        // ... (Existing code remains the same) ...

        public int damage = 5; // Default damage

        void OnTriggerEnter(Collider other)
        {
            // Check if the bullet hits a player
            if (other.CompareTag("Player"))
            {
                // Deal damage to the player
                other.GetComponent<PlayerMovement>().TakeDamage(damage, hit);
                Destroy(gameObject); // Destroy the bullet on impact
            }
        }
    }
}