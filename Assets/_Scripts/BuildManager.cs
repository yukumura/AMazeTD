using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private ObjectBlueprint objectToBuild;

    public static BuildManager instance;

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
    }

    public void SelectTurretToBuild(ObjectBlueprint obj)
    {
        objectToBuild = obj;
    }

    public bool CanBuild { get { return objectToBuild != null; } }

    public void BuildObjectOn(Tile tile)
    {
        if (!CanBuild) return;

        if (tile.GetBuilding() != null)
        {
            Debug.Log("Already present same turret here.");
            return;
        }

        tile.Build(objectToBuild);
    }
}
