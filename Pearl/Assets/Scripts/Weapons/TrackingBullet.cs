using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBullet : MonoBehaviour
{
    private float damage;
    private float bulletSpeed;
    private LayerMask enemyLayer;

    private void OnEnable()
    {

    }
    private void Update()
    {
        
    }


    public void Init(float damage, float bulletSpeed, LayerMask enemyLayer)
    {
        this.damage = damage;
        this.bulletSpeed = bulletSpeed;
        this.enemyLayer = enemyLayer;
    }
}
