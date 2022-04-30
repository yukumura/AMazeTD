using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public WaveDescriptor[] waves;

    public float timeBetweenWaves = 10.5f;
    public float countdown = 60f;
    public GameObject TilesController;

    private int waveIndex = 0;
    private GameManager gameManager;
    private ManagerUI managerUI;
    private bool isSpawning = false;
    private bool cleanLevelFlag = true;

    public Vector3 spawnPositionOffset;
    public AudioClip levelCleanSound;
    private AudioSource audioSource;

    private WaveDescriptor waveDescriptor;

    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.instance;
        managerUI = gameManager.GetComponent<ManagerUI>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// When the wave monster are all killed, start new wave
    /// </summary>
    void Update()
    {
        if (gameManager.GetMonsters() > 0 || isSpawning) return;

        if (gameManager.GetMonsters() == 0 && !isSpawning && !cleanLevelFlag)
        {
            cleanLevelFlag = true;
            LevelClean();
        }

        //if (Mathf.Round(countdown) <= 0f)
        //{
        //    StartCoroutine(SpawnWave());
        //    countdown = timeBetweenWaves;
        //    gameManager.AddLevel(1);
        //}

        //countdown -= Time.deltaTime;

        //if (gameManager.GetMonsters() != 0 || isSpawning)
        //{
        //    managerUI.HidePanelInformation();
        //    managerUI.HideSkipButton();
        //}
        //else
        //{
        //    managerUI.SetPanelInformationText(string.Format("Congratulation! You have finished the level!"));
        //    managerUI.ShowSkipButton();
        //}
    }

    public Vector3 GetSpawnPosition()
    {
        return transform.position + spawnPositionOffset;
    }

    IEnumerator SpawnWave()
    {
        if (gameManager.GetTileSelected() != null)
            gameManager.DeselectTile();

        managerUI.HideShop();
        managerUI.HideUpgrade();
        managerUI.HideArrowSpawn();
        managerUI.HidePanelInformation();
        //gameManager.HideTiles();
        managerUI.HidePanelLevelFinished();
        managerUI.HideSkipButton();
        TilesController.GetComponent<Animator>().SetBool("Hide", true);

        cleanLevelFlag = false;
        isSpawning = true;
        waveDescriptor = waves[waveIndex];

        foreach (Wave wave in waveDescriptor.wave)
        {
            for (int i = 0; i < wave.count; i++)
            {
                if (!gameManager.LoseFlag)
                    SpawnEnemy(wave.monster);

                yield return new WaitForSeconds(1 / wave.rateForSpawn);
            }
        }

        waveIndex++;

        if (waveIndex >= waves.Length)
        {
            enabled = false;
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
        Enemy enemy = monster.GetComponent<Enemy>();

        if (enemyMovement.isFlyingCreature)
        {
            Vector3 spawnCreaturePosition = transform.position + enemyMovement.FlyPosition;
            Instantiate(monster, spawnCreaturePosition, transform.rotation);

            if(enemy.spawnEffect != null)
                Instantiate(enemy.spawnEffect, spawnCreaturePosition, transform.rotation);


        }
        else
            Instantiate(monster, GetSpawnPosition(), transform.rotation);

        if (enemy.spawnEffect != null)
            Instantiate(enemy.spawnEffect, GetSpawnPosition(), transform.rotation);

        gameManager.AddMonster(1);
    }

    void LevelClean()
    {
        if (managerUI.effectFlag)
        {
            audioSource.clip = levelCleanSound;
            audioSource.Play();
        }

        gameManager.AddGold(waveDescriptor.goldBonus);
        TilesController.GetComponent<Animator>().SetBool("Hide", false);
        managerUI.ShowSkipButton();
    }

    public void SkipWaiting()
    {
        //countdown = 0;
        StartCoroutine(SpawnWave());
        //countdown = timeBetweenWaves;
        gameManager.AddLevel(1);
        managerUI.HideSkipButton();
    }

    public bool InProgress() { return isSpawning; }
}
