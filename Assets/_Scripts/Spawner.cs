using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Text secondsToNextWaveText; 

    public Transform spawnPoint;

    public float timeBetweenWaves = 10.5f;
    public float startCountdown = 60f;

    private int waveIndex = 0;
    private GameManager gameManager;


    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetMonsters() > 0) return;

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
        Wave wave = waves[waveIndex];

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.monster);
            yield return new WaitForSeconds(1 / wave.rateForSpawn);
        }

        waveIndex++;

        if(waveIndex >= waves.Length)
        {
            enabled = false;
        }
    }

    void SpawnEnemy(GameObject monster)
    {
        if (monster == null)
        {
            Debug.Log("Enemy is not set. Don't spawn anything.");
            return;
        }

        Instantiate(monster, spawnPoint.position, spawnPoint.rotation);
        gameManager.AddMonster(1);
    }
}
