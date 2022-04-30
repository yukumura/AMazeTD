using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Manage Gold")]
    public static int Gold;
    public int startGold = 400;

    [Header("Manage Lives")]
    public static int Lives;
    public int startLives = 20;

    [Header("Manage Level")]
    public static int Level;
    public int startLevel = 1;

    [Header("Manage Monster")]
    private Spawner spawner;
    public int CounterCreeps = 0;
    public int MonsterInGame = 0;

    [Header("Manage Win Condition")]
    public bool WinFlag = false;
    public bool LoseFlag = false;    
    
    [Header("Audio")]
    public AudioClip victory;
    public AudioClip lose;
    public AudioClip killmonster;
    private AudioSource audioSource;

    [Header("Manage Tile")]
    private Tile tileSelected;


    private ManagerUI managerUI;
    private BuildManager buildManager;

    private bool isStartGame = false;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        // DontDestroyOnLoad(gameObject);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
       
    }




    // Use this for initialization
    void Start()
    {
        Level = startLevel;
        Gold = startGold;
        Lives = startLives;
        spawner = FindObjectOfType<Spawner>();
        audioSource = GetComponent<AudioSource>();
        buildManager = GetComponent<BuildManager>();
        managerUI = GetComponent<ManagerUI>();
        managerUI.Pause();

        if (PlayerPrefs.GetInt("Music", 99) == 99 || PlayerPrefs.GetInt("Music", 99) == 1)
            managerUI.EnableMusic();
        else
            managerUI.DisableMusic();

        if (PlayerPrefs.GetInt("Effect", 99) == 99 || PlayerPrefs.GetInt("Effect", 99) == 1)
            managerUI.EnableEffects();
        else
            managerUI.DisableEffects();

    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {            
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                if (!isStartGame)
                {
                    return;
                }

                if (managerUI.pauseFlag)
                    managerUI.ShrinkMenu();

                if (spawner.InProgress() || MonsterInGame > 0 || WinFlag || LoseFlag)
                {
                    return;
                }

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (tileSelected != null)
                {                    
                    DeselectTile();
                }

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "Tile")
                    {
                        Tile tile = hit.transform.gameObject.GetComponent<Tile>();
                        SelectTile(tile);
                    }
                    else if (hit.transform.gameObject.tag == "Turret" || hit.transform.gameObject.tag == "Wall")
                    {
                        Tile tile = hit.transform.gameObject.GetComponentInParent<Tile>();
                        SelectTile(tile);
                    }
                }
            }
            else
            {
            }
        }

        if (!spawner.isActiveAndEnabled && GetMonsters() == 0)
        {
            if (!WinFlag && !LoseFlag)
            {
                WinFlag = true;
                LoseFlag = false;
                Win();
            }
        }
    }

    public void SelectTile(Tile tile)
    {
        tileSelected = tile;
        tileSelected.Activate();

        if (tileSelected.GetBuilding() != null)
        {
            managerUI.ShowPanelUpgrade(tileSelected);
        }
        else
        {
            managerUI.ShowPanelShop(tileSelected);
        }
    }

    public void DeselectTile()
    {
        tileSelected.DeActivate();
        tileSelected = null;
        managerUI.HidePanelShop();
        managerUI.HidePanelUpgrade();
        buildManager.DestroyRange();
    }

    /// <summary>
    /// Add monster in counter
    /// </summary>
    /// <param name="monster"></param>
    public void AddMonster(int monster)
    {
        MonsterInGame += monster;
    }

    public void RemoveMonster(int monster)
    {
        MonsterInGame -= monster;
    }

    public void KillMonster(int monster)
    {
        MonsterInGame -= monster;

        if (managerUI.effectFlag)
            AudioSource.PlayClipAtPoint(killmonster, Camera.main.transform.position);
    }

    /// <summary>
    /// Add gold in counter
    /// </summary>
    /// <param name="gold"></param>
    public void AddGold(int gold)
    {
        Gold += gold;
    }

    public void RemoveGold(int gold)
    {
        Gold -= gold;
    }

    /// <summary>
    /// Add level in counter
    /// </summary>
    /// <param name="gold"></param>
    public void AddLevel(int level)
    {
        Level += level;
    }

    /// <summary>
    /// Add live in counter
    /// </summary>
    /// <param name="live"></param>
    public void RemoveLives(int live)
    {
        Lives -= live;
        if (Lives <= 0 && !WinFlag && !LoseFlag)
        {
            LoseFlag = true;
            spawner.enabled = false;
            Lose();
        }
    }

    public int GetGolds() { return Gold; }
    public int GetLives() { return Lives; }
    public int GetMonsters() { return MonsterInGame; }
    public int GetLevel() { return Level; }

    public Tile GetTileSelected() { return tileSelected; }

    public void Win()
    {
        if (managerUI.effectFlag)
        {
            audioSource.volume = 1;
            audioSource.clip = victory;
            audioSource.loop = false;
            audioSource.Play();
        }

        managerUI.HideMenu();
        managerUI.HideInformationPanels();
        managerUI.SetWinDisplay();        
    }

    public void Lose()
    {
        if (managerUI.effectFlag)
        {
            audioSource.volume = 1;
            audioSource.clip = lose;
            audioSource.loop = false;
            audioSource.Play();
        }

        managerUI.HideMenu();
        managerUI.HideInformationPanels();
        managerUI.SetLoseDisplay();

        EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();

        foreach (EnemyMovement enemy in enemies)
        {
            enemy.StopMoving();
        }
    }

    //public void CalculatePathForAllEnemies()
    //{
    //    EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();

    //    foreach (EnemyMovement enemy in enemies)
    //    {
    //        enemy.CheckIfCanReachGoal();
    //    }
    //}

    public AudioSource GetAudioSource() { return audioSource; }

    public void BalanceTurretVolume()
    {
        Turret[] turrets = FindObjectsOfType<Turret>();
        
        foreach (var turret in turrets)
        {
            turret.GetComponent<AudioSource>().volume = turret.useLaser ? 0f : 1f / turrets.Where(x => !x.useLaser).ToArray().Length;
        }
    }
    public void SetGameStart()
    {
        isStartGame = true;
    }
}
