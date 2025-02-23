using UnityEngine;
using UnityEngine.InputSystem;


public enum WeaponType { Melee, Ranged }

public class Weapon : MonoBehaviour
{
public Transform firePoint;
public GameObject bulletPrefab;

    void Update()
    {

    }

    void OnAttack() {
        // adjust for ranged later
        Shoot();
        Debug.Log("Shoot");
    }

    void Shoot() {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }


}
