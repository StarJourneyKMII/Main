using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerBullet bulletPrefab;
    [SerializeField] private float shootTime = 2f;
    [SerializeField] private float bulletSpeed = 5;
    [SerializeField] private float maxDistance = 5;

    private float shootTimer;

    private void Start()
    {
        shootTimer = shootTime;
    }

    private void Update()
    {
        if(shootTimer < 0)
        {
            Shoot();
            shootTimer = shootTime;
        }
        else
            shootTimer -= Time.deltaTime;
    }

    private void Shoot()
    {
        for(int i = 0; i < 4; i++)
        {
            TowerBullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.SetBullet(bulletSpeed, GetDir(i), maxDistance);
        }
    }

    private Vector3 GetDir(int index)
    {
        if(index==0)
            return Vector3.up;
        else if(index==1)
            return Vector3.down;
        else if(index==2)
            return Vector3.left;
        else if (index==3)
            return Vector3.right;

        return Vector3.up;
    }

    private void OnDrawGizmos()
    {
        GizmosExtensions.DrawWireDisc(transform.position, Vector3.forward, maxDistance);

        //Gizmos.color = Color.red;
        //GizmosExtensions.DrawArrow(transform.position, Vector3.up * maxDistance , 1);
        //GizmosExtensions.DrawArrow(transform.position, Vector3.down * maxDistance, 1);
        //GizmosExtensions.DrawArrow(transform.position, Vector3.left * maxDistance, 1);
        //GizmosExtensions.DrawArrow(transform.position, Vector3.right * maxDistance, 1);
    }
}
