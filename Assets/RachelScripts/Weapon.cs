using UnityEngine;
using UnityEngine.InputSystem;


public enum WeaponType { Melee, Ranged }

public class Weapon : MonoBehaviour
{
public Transform firePoint;
public GameObject bulletPrefab;

  AudioManager audioManager;
    public float minPitch = 0.9f; // Lower pitch limit
    public float maxPitch = 2.0f; // Upper pitch limit

    void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {

    }

    void OnAttack() {
        // adjust for ranged later
        Shoot();
        // Debug.Log("Shoot");
        audioManager.SFXSource.pitch = Random.Range(minPitch, maxPitch);
        audioManager.PlaySFX(audioManager.shoot);
    }

    void Shoot() {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }


}
