using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] waypoints;
    public EnemyAIType type;
    public Transform enemyParenet;
    public GameObject enemyPrefab;
    public RespawnPoint spawnerForRespawn;
    public GameObject keyItem;
    public bool noFlashObjects = false;
    public bool inversePatroll = true;

    private void Start()
    {
        GameManager.instance.enemySpawners.Add(this);
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity, enemyParenet);
        if (noFlashObjects)
        {
            for (int i = 0; i < enemy.GetComponent<EnemyScript>().flashObjects.Length; i++)
            {
                enemy.GetComponent<EnemyScript>().flashObjects[i].SetActive(false);
            }
        }
        enemy.GetComponent<EnemyScript>().keyItemDrop = keyItem;
        enemy.GetComponent<EnemyAI>().InitAI(waypoints, type, this, inversePatroll);
    }
}
