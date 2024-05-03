
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform launchPoint;
    public float missileSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireMissile();
        }
    }

    void FireMissile()
    {
        GameObject missile = Instantiate(missilePrefab, launchPoint.position, launchPoint.rotation);
        Rigidbody missileRb = missile.GetComponent<Rigidbody>();
        missileRb.velocity = launchPoint.forward * missileSpeed;
    }
}