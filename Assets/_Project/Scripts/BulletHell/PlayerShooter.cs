using System;
using PrimeTween;
using UnityEngine;

public class PlayerShooter : MonoBehaviour {
    [SerializeField] float fireRate = 1f;
    float fireCooldown;

    [SerializeField] Transform guns;

    [SerializeField] KeyCode radialPatternKey = KeyCode.Alpha1;
    [SerializeField] KeyCode wavePatternKey = KeyCode.Alpha2;
    [SerializeField] KeyCode spiralPatternKey = KeyCode.Alpha3;
    
    Type pattern = typeof(RadialPattern);

    void Update() {
        HandlePatternSwitching();
        HandleFiring();
    }

    void HandlePatternSwitching() {
        if (Input.GetKeyDown(radialPatternKey)) {
            BulletHellManager.Instance.SetPattern(new RadialPattern());
            pattern = typeof(RadialPattern);
            Debug.Log("Switched to Radial Pattern");
        } else if (Input.GetKeyDown(wavePatternKey)) {
            BulletHellManager.Instance.SetPattern(new WavePattern());
            pattern = typeof(WavePattern);
            Debug.Log("Switched to Wave Pattern");
        } else if (Input.GetKeyDown(spiralPatternKey)) {
            BulletHellManager.Instance.SetPattern(new SpiralPattern());
            pattern = typeof(SpiralPattern);
            Debug.Log("Switched to Spiral Pattern");
        }
    }

    void HandleFiring() {
        fireCooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && fireCooldown <= 0f) {
            BulletHellManager.Instance.SpawnBulletPattern();
            fireCooldown = fireRate;
        }
    }
}