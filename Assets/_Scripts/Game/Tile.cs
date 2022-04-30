using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Material materialForHide;
    public Material selectedMaterial;
    private Renderer rend;
    private Material startMaterial;
    public int objectPlacedInRound = 0;

    private GameObject building;
    private ObjectBlueprint buildingBlueprint;

    public Vector3 positionOffset;

    public AudioClip error;
    public AudioClip placeObjectSound;
    public AudioClip upgradeObject;
    public AudioClip sellObject;
    public GameObject buildEffect;
    public GameObject upgradeEffect;
    public GameObject sellEffect;
    private AudioSource audioSource;

    GameManager gameManager;
    ManagerUI managerUI;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        startMaterial = rend.material;
        gameManager = GameManager.instance;
        audioSource = GetComponent<AudioSource>();
        managerUI = gameManager.GetComponent<ManagerUI>();
    }

    /// <summary>
    /// Retrieve the position for build objects
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Highlight tile when selected
    /// </summary>
    public void Activate()
    {
        rend.material = selectedMaterial;        
    }

    /// <summary>
    /// Unselect the tile
    /// </summary>
    public void DeActivate()
    {
        rend.material = startMaterial;        
    }

    /// <summary>
    /// Add Turret / Wall and set the tile as parent
    /// </summary>
    /// <param name="obj">Object to build</param>
    public void Build(ObjectBlueprint obj)
    {
        if (gameManager.GetGolds() < obj.cost)
        {
            if (managerUI.effectFlag)
            {
                audioSource.clip = error;
                audioSource.Play();
            }
            return;
        }

        buildingBlueprint = obj;
        gameManager.RemoveGold(buildingBlueprint.cost);
        GameObject bld = Instantiate(buildingBlueprint.prefab, GetBuildPosition(), Quaternion.identity);
        bld.transform.SetParent(transform);
        building = bld;

        if (managerUI.effectFlag)
        {
            audioSource.clip = placeObjectSound;
            audioSource.Play();
        }

        Instantiate(buildEffect, GetBuildPosition(), Quaternion.identity);

        objectPlacedInRound = gameManager.GetLevel();
        gameManager.BalanceTurretVolume();
        //gameManager.CalculatePathForAllEnemies();
    }

    /// <summary>
    /// Upgrade the tile object 
    /// </summary>
    public void UpgradeObject()
    {
        ObjectBlueprint buildingUpdate = buildingBlueprint.upgrade;

        if (gameManager.GetGolds() < buildingUpdate.cost)
        {
            if (managerUI.effectFlag)
            {
                audioSource.clip = error;
                audioSource.Play();
            }
            return;
        }

        gameManager.RemoveGold(buildingUpdate.cost);
        Destroy(building);
        GameObject bld = Instantiate(buildingUpdate.prefab, GetBuildPosition(), Quaternion.identity);
        bld.transform.SetParent(transform);
        building = bld;
        buildingBlueprint = buildingUpdate;

        if (managerUI.effectFlag)
        {
            audioSource.clip = upgradeObject;
            audioSource.Play();
        }

        Instantiate(upgradeEffect, GetBuildPosition(), Quaternion.identity);
    }

    /// <summary>
    /// Sell the tile object
    /// </summary>
    public void SellObject()
    {
        if (objectPlacedInRound == gameManager.GetLevel())
            gameManager.AddGold(buildingBlueprint.cost);
        else
            gameManager.AddGold(buildingBlueprint.valueToSell);

        Destroy(building);
        buildingBlueprint = null;

        if (managerUI.effectFlag)
        {
            audioSource.clip = sellObject;
            audioSource.Play();
        }

        Instantiate(sellEffect, GetBuildPosition(), Quaternion.identity);

    }

    /// <summary>
    /// Get the tile object
    /// </summary>
    /// <returns></returns>
    public ObjectBlueprint GetBuilding()
    {
        return buildingBlueprint;
    }

    public Turret GetChildTurret()
    {
        if (building != null && building.GetComponent<Turret>() != null)
            return building.GetComponent<Turret>();

        return null;
    }

    public void DestroyObject()
    {
        buildingBlueprint = null;
        building = null;
    }

    //public void Hide()
    //{
    //    //GetComponent<MeshRenderer>().material = materialForHide;
    //    anim.SetBool("Hide", true);
    //}

    //public void Show()
    //{
    //    //GetComponent<MeshRenderer>().material = startMaterial;
    //    anim.SetBool("Hide", false);
    //}
}
