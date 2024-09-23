using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnInterval = 10f;
    public Transform playerTransform;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnBulletRoutine());
    }



    private IEnumerator SpawnBulletRoutine()
    {
        SpawnBullet();
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnBullet();
        }
    }

    private void SpawnBullet()
    {
        Vector2 spawnPosition = transform.position;
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();

        if (bulletController != null)
        {
            //bulletController.SetTarget(playerTransform);
        }
    }
}
