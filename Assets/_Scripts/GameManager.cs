using System.Collections;
using System.Collections.Generic;
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

    [Header("Manage Monster")]
    public Spawner Spawner;
    public int CounterCreeps = 0;
    public int MonsterInGame = 0;

    [Header("Manage Shop, Stats, Sell & Upgrade")]
    public GameObject panelShop;
    public GameObject panelStats;
    public GameObject panelManage;
    private Tile tileSelected;

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
    }

    


    // Use this for initialization
    void Start()
    {
        Spawner = FindObjectOfType<Spawner>();
        Gold = startGold;
        Lives = startLives;        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
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
                        SelectTile(hit.transform.gameObject.GetComponent<Tile>());
                    }
                    else if(hit.transform.gameObject.tag == "Turret" || hit.transform.gameObject.tag == "Wall")
                    {
                        SelectTile(hit.transform.parent.gameObject.GetComponent<Tile>());
                    }
                }
            }            
        }
    }
       
    public void SelectTile(Tile tile)
    {
        tileSelected = tile;
        tileSelected.Activate();

        if(tileSelected.GetBuilding() != null)
        {
            panelManage.SetActive(true);
        }
        else
        {
            panelShop.SetActive(true);
        }
    }

    public void DeselectTile()
    {
        tileSelected.DeActivate();
        tileSelected = null;
        panelShop.SetActive(false);
        panelManage.SetActive(false);
    }

    public void AddMonster(int monster)
    {
        MonsterInGame += monster;
    }

    public void AddGold(int gold)
    {
        Gold += gold;
    }

    public void AddLives(int live)
    {
        Lives += live;
    }

    public int GetGolds() { return Gold; }
    public int GetLives() { return Lives; }
    public int GetMonsters() { return MonsterInGame; }

    public Tile GetTileSelected() { return tileSelected; }
}
