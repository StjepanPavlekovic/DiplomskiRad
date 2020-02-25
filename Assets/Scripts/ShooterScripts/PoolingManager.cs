using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager instance;
    public int amountToPool;
    public GameObject bulletPrefab;
    public GameObject bulletHolder;

    private Stack<GameObject> bulletPool;

    void Awake()
    {
        instance = this;

        bulletPool = new Stack<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.parent = bulletHolder.transform;
            bullet.SetActive(false);
            bulletPool.Push(bullet);
        }
    }

    public void StoreBullet(GameObject bullet)
    {
        bulletPool.Push(bullet);
        bullet.SetActive(false);
    }

    public GameObject SpawnBullet()
    {
        GameObject bullet;

        if (bulletPool.Count > 0)
        {
            bullet = bulletPool.Pop();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            bullet = Instantiate(bulletPrefab);
            bullet.transform.parent = bulletHolder.transform;
            return bullet;
        }
    }
}
