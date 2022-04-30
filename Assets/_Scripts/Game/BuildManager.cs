using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{

    public GameObject range;
    private ObjectBlueprint objectToBuild;
    public static BuildManager instance;

    private GameManager gameManager;
    //private ManagerUI managerUI;

    private bool showRange = false;

    private GameObject rangeInstantiated;

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


        gameManager = GameManager.instance;
        //managerUI = gameManager.GetComponent<ManagerUI>();
    }

    public void SelectTurretToBuild(ObjectBlueprint obj)
    {
        if(objectToBuild != null && objectToBuild != obj)
        {
            DestroyRange();
        }

        objectToBuild = obj;
        //managerUI.SetObjectsDescriptorText(obj.description);
        //managerUI.ShowObjectsDescriptor();
        if(!showRange)
            ShowRange();
        else
            BuildObjectOn();
    }

    private void ShowRange()
    {
        showRange = true;
        Turret turret = objectToBuild.prefab.GetComponent<Turret>();

        if (turret == null)
        {
            BuildObjectOn();
            return;
        }

        Vector3 tilePos = gameManager.GetTileSelected().transform.position;

        rangeInstantiated = Instantiate(range, new Vector3(tilePos.x, 0.026f, tilePos.z), range.transform.rotation);
        Image rangeImage = rangeInstantiated.GetComponentInChildren<Image>();
        rangeImage.transform.localScale = new Vector3(turret.range * 4, turret.range * 4, turret.range * 4);
        rangeImage.transform.localPosition = Vector3.zero;
    }

    public bool CanBuild { get { return objectToBuild != null; } }

    public void BuildObjectOn()
    {
        if (!CanBuild) return;

        if (gameManager.GetTileSelected().GetBuilding() != null)
        {
            return;
        }

        gameManager.GetTileSelected().Build(objectToBuild);
        gameManager.DeselectTile();
        DestroyRange();
    }

    public void DestroyRange()
    {
        if(rangeInstantiated != null)
        {
            Destroy(rangeInstantiated);
            rangeInstantiated = null;
        }
        
        showRange = false;
    }
}
