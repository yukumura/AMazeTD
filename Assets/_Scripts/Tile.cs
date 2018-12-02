using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public Color selectedColor;
    private Renderer rend;
    private Color startColor;

    private GameObject building;
    private ObjectBlueprint buildingBlueprint;

    public Vector3 positionOffset;

    GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        gameManager = GameManager.instance;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        rend.material.color = selectedColor;
    }

    public void DeActivate()
    {
        rend.material.color = startColor;
    }

    /// <summary>
    /// Add Turret / Wall and set the tile as parent
    /// </summary>
    public void Build(ObjectBlueprint obj)
    {
        if (gameManager.GetGolds() < obj.cost)
        {
            Debug.Log("Not enough money to build that!");
            return;
        }

        buildingBlueprint = obj;
        gameManager.AddGold(-buildingBlueprint.cost);
        GameObject bld = Instantiate(buildingBlueprint.prefab, GetBuildPosition(), Quaternion.identity);
        bld.transform.SetParent(transform);
        building = bld;
    }

    public void UpgradeObject()
    {
        ObjectBlueprint buildingUpdate = buildingBlueprint.upgrade;

        if (gameManager.GetGolds() < buildingUpdate.cost)
        {
            Debug.Log("Not enough money to build that!");
            return;
        }

        gameManager.AddGold(-buildingUpdate.cost);
        Destroy(building);
        GameObject bld = Instantiate(buildingUpdate.prefab, GetBuildPosition(), Quaternion.identity);
        bld.transform.SetParent(transform);
        building = bld;
        buildingBlueprint = buildingUpdate;
    }

    public void SellObject()
    {
        gameManager.AddGold(buildingBlueprint.valueToSell);
        Destroy(building);
        Debug.Log("object sell for " + buildingBlueprint.valueToSell);
        buildingBlueprint = null;
    }

    public ObjectBlueprint GetBuilding()
    {
        return buildingBlueprint;
    }
}
