using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public WaveDescriptor[] waves;
    public Text secondsToNextWaveText; 

    public Transform spawnPoint;

    public float timeBetweenWaves = 10.5f;
    public float startCountdown = 60f;

    private int waveIndex = 0;
    private GameManager gameManager;
    private bool isSpawning = false;


    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.instance;
    }

    /// <summary>
    /// When the wave monster are all killed, start new wave
    /// </summary>
    void Update()
    {
        if (gameManager.GetMonsters() > 0 || isSpawning) return;

        if(Mathf.Round(startCountdown) <= 0f)
        {
            StartCoroutine(SpawnWave());
            startCountdown = timeBetweenWaves;
        }

        startCountdown -= Time.deltaTime;

        if(Mathf.Round(startCountdown) <= 10 && Mathf.Round(startCountdown) > 0 && gameManager.GetMonsters() == 0)
        {
            secondsToNextWaveText.transform.gameObject.SetActive(true);
        }
        else
        {
            secondsToNextWaveText.transform.gameObject.SetActive(false);
        }

        secondsToNextWaveText.text = Mathf.Round(startCountdown).ToString();
    }

    IEnumerator SpawnWave()
    {
        isSpawning = true;
        WaveDescriptor waveDescriptor = waves[waveIndex];

        foreach (Wave wave in waveDescriptor.wave)
        {
            for (int i = 0; i < wave.count; i++)
            {
                SpawnEnemy(wave.monster);
                yield return new WaitForSeconds(1 / wave.rateForSpawn);
            }
        }
       
        waveIndex++;

        if(waveIndex >= waves.Length)
        {
            enabled = false;
            Debug.Log("No other waves. Show You won! message");
        }

        isSpawning = false;
    }

    void SpawnEnemy(GameObject monster)
    {
        if (monster == null)
        {
            Debug.Log("Enemy is not set. Don't spawn anything.");
            return;
        }

        EnemyMovement enemyMovement = monster.GetComponent<EnemyMovement>();
        if (enemyMovement.isFlyingCreature)
        {
            Vector3 spawnCreaturePosition = spawnPoint.position + enemyMovement.FlyPosition;
            Instantiate(monster, spawnCreaturePosition, spawnPoint.rotation);

        }
        else
            Instantiate(monster, spawnPoint.position, spawnPoint.rotation);

        gameManager.AddMonster(1);
    }
}
