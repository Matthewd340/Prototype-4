using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    private int enemyIndex;
    public GameObject powerupPrefab;
    public GameObject shootingPowerupPrefab;
    public GameObject jumpPowerupPrefab;
    private float spawnRange = 9;
    public int enemyCount = 2;
    public int waveNumber = 0;
    public int deathCount;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        Instantiate(shootingPowerupPrefab, GenerateSpawnPosition(), shootingPowerupPrefab.transform.rotation);
        Instantiate(jumpPowerupPrefab, GenerateSpawnPosition(), shootingPowerupPrefab.transform.rotation);
    }

    void Update()
    {
        Debug.Log("L's Taken: " + deathCount);
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            Instantiate(shootingPowerupPrefab, GenerateSpawnPosition(), shootingPowerupPrefab.transform.rotation);
            Instantiate(jumpPowerupPrefab, GenerateSpawnPosition(), shootingPowerupPrefab.transform.rotation);
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int enemyIndex = Random.Range(0, enemyPrefab.Length);
            Instantiate(enemyPrefab[enemyIndex], GenerateSpawnPosition(), enemyPrefab[enemyIndex].transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
}
