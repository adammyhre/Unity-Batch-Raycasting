using System;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public Vector3 direction;
    float maxDistance;
    Vector3 startPosition;

    public void Initialize(Vector3 position, Vector3 direction, float maxDistance) {
        transform.position = position;
        this.startPosition = position;
        this.direction = direction;
        this.maxDistance = maxDistance;
        
        transform.rotation = Quaternion.LookRotation(direction);
    }
    
    public bool HasTraveledMaxDistance() {
        float distanceTraveledSq = (startPosition - transform.position).sqrMagnitude;
        return distanceTraveledSq >= maxDistance * maxDistance;
    }
}