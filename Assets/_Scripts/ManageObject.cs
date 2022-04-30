using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ManageObject : MonoBehaviour
{
    public GameObject buttonUpgrade;

    public Text textUpgrade;
    public Text textSell;

    private GameManager gameManager;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameManager = GameManager.instance;

        UpdateInformationButton();
    }

    private void OnEnable()
    {
       
    }

    //Call tile sell object method
    public void SellObject()
    {
        if (gameManager.GetTileSelected() == null) return;

        gameManager.GetTileSelected().SellObject();
        gameManager.DeselectTile();
    }

    //Call tile upgrade object method
    public void UpgradeObject()
    {
        if (gameManager.GetTileSelected() == null) return;
        if (gameManager.GetTileSelected().GetBuilding() == null) return;

        gameManager.GetTileSelected().UpgradeObject();
        gameManager.DeselectTile();
    }

    /// <summary>
    /// Check if can upgrade turret + update sell turret cost 
    /// </summary>
    public void UpdateInformationButton()
    {
        if (gameManager.GetTileSelected() == null || gameManager.GetTileSelected().GetBuilding() == null)
            gameObject.SetActive(false);

        ObjectBlueprint buildingBP = gameManager.GetTileSelected().GetBuilding();

        if (buildingBP.isUpgradeable)
        {
            ObjectBlueprint buildingUpgrade = buildingBP.upgrade;
            buttonUpgrade.SetActive(true);

            if (gameManager.GetGolds() < buildingUpgrade.cost)
            {
                textUpgrade.text = "Not enough money";                
            }
            else
            {
                textUpgrade.text = buildingBP.upgrade.cost.ToString();
            }
        }
        else
            buttonUpgrade.SetActive(false);

        textSell.text = buildingBP.valueToSell.ToString();
    }
}
