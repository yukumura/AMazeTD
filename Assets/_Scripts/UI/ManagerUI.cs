using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{

    [Header("Menu")]
    public GameObject panelMenuShrink;
    public GameObject panelMenuExtended;
    public bool pauseFlag = false;
    public bool musicFlag = true;
    public Button musicOnButton;
    public Button musicOffButton;
    public bool effectFlag = true;
    public Button effectOnButton;
    public Button effectOffButton;

    [Header("Manage Win Condition")]
    public GameObject panelWinCondition;
    public GameObject panelLoseCondition;

    [Header("Level finished")]
    public GameObject panelLevelFinished;
    public Text textLevelFinished;

    [Header("Manage Spawner")]
    public GameObject panelSkip;
    public GameObject panelArrowSpawner;

    [Header("Manage Shop & Upgrade")]
    public GameObject panelShop;
    public Button managerShop;
    public GameObject panelUpgrade;
    public Button managerUpgrade;


    [Header("Manage Information")]
    public GameObject panelInformationExtended;
    public GameObject panelInformationShrink;


    private GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameManager.instance;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetPanelInformationText(string text)
    {
        panelLevelFinished.SetActive(true);
        textLevelFinished.text = text;
    }
    public void SetPanelInformationText(string text, float delay)
    {
        if (!panelLevelFinished.activeSelf)
        {
            panelLevelFinished.SetActive(true);
            StartCoroutine(WaitSecondsForHide(delay));
        }

        textLevelFinished.text = text;
    }

    IEnumerator WaitSecondsForHide(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HidePanelLevelFinished();
    }

    public void HidePanelLevelFinished()
    {
        panelLevelFinished.SetActive(false);
    }

    public void Pause()
    {
        pauseFlag = true;
        Time.timeScale = 0;

        Turret[] turrets = FindObjectsOfType<Turret>().Where(x => x.useLaser).ToArray();

        foreach (Turret turret in turrets)
        {
            turret.GetComponent<AudioSource>().volume = 0f;
        }
    }

    public void Resume()
    {
        pauseFlag = false;
        Time.timeScale = 1;
    }

    public void DisableMusic()
    {
        musicFlag = false;
        gameManager.GetAudioSource().volume = 0;
        musicOnButton.gameObject.SetActive(true);
        musicOffButton.gameObject.SetActive(false);
        PlayerPrefs.SetInt("Music", 0);
    }

    public void EnableMusic()
    {
        musicFlag = true;
        gameManager.GetAudioSource().volume = 1;
        musicOnButton.gameObject.SetActive(false);
        musicOffButton.gameObject.SetActive(true);
        PlayerPrefs.SetInt("Music", 1);
    }

    public void DisableEffects()
    {
        effectFlag = false;
        effectOnButton.gameObject.SetActive(true);
        effectOffButton.gameObject.SetActive(false);

        PlayerPrefs.SetInt("Effect", 0);

        Turret[] turrets = FindObjectsOfType<Turret>().Where(x => x.useLaser).ToArray();

        foreach (Turret turret in turrets)
        {
            turret.GetComponent<AudioSource>().volume = 0f;
        }
    }

    public void EnableEffects()
    {
        effectFlag = true;
        effectOnButton.gameObject.SetActive(false);
        effectOffButton.gameObject.SetActive(true);

        PlayerPrefs.SetInt("Effect", 1);

        Turret[] turrets = FindObjectsOfType<Turret>().Where(x => x.useLaser).ToArray();

        foreach (Turret turret in turrets)
        {
            turret.GetComponent<AudioSource>().volume = 1f;
        }
    }

    public void ShowMenu()
    {
        panelMenuExtended.SetActive(true);
        panelMenuShrink.SetActive(false);
        HidePanelInformation();

        if (gameManager.GetTileSelected() != null)
            gameManager.DeselectTile();

        Pause();
    }

    public void ShrinkMenu()
    {
        panelMenuExtended.SetActive(false);
        panelMenuShrink.SetActive(true);
        Resume();
    }

    public void HideMenu()
    {
        panelMenuExtended.SetActive(false);
        panelMenuShrink.SetActive(false);
    }

    public void HideShop()
    {
        panelShop.SetActive(false);
    }

    public void HideUpgrade()
    {
        panelUpgrade.SetActive(false);
    }

    public void SetWinDisplay()
    {
        panelWinCondition.SetActive(true);

        Turret[] turrets = FindObjectsOfType<Turret>().Where(x => x.useLaser).ToArray();

        foreach (Turret turret in turrets)
        {
            turret.GetComponent<LineRenderer>().enabled = false;
            turret.GetComponent<AudioSource>().volume = 0f;
        }
    }

    public void SetLoseDisplay()
    {
        panelLoseCondition.SetActive(true);

        Turret[] turrets = FindObjectsOfType<Turret>().Where(x => x.useLaser).ToArray();

        foreach (Turret turret in turrets)
        {
            turret.GetComponent<LineRenderer>().enabled = false;
            turret.GetComponent<AudioSource>().volume = 0f;
        }
    }

    public void ShowSkipButton()
    {
        panelSkip.SetActive(true);
    }

    public void HideSkipButton()
    {
        panelSkip.SetActive(false);
    }

    public void ShowArrowSpawn()
    {
        panelArrowSpawner.SetActive(true);
    }

    public void HideArrowSpawn()
    {
        panelArrowSpawner.SetActive(false);
    }

    public void ShowPanelInformation()
    {
        panelInformationExtended.SetActive(true);
        panelInformationShrink.SetActive(false);
        ShrinkMenu();
    }

    public void HidePanelInformation()
    {
        panelInformationShrink.SetActive(true);
        panelInformationExtended.SetActive(false);
    }

    public void HideInformationPanels()
    {
        panelInformationShrink.SetActive(false);
        panelInformationExtended.SetActive(false);
    }

    public void ShowPanelShop(Tile tile)
    {
        panelShop.SetActive(true);
        panelShop.transform.position = Camera.main.WorldToScreenPoint(tile.transform.position);
        managerShop.GetComponent<RadialMenuController>().OpenMenu();
    }

    public void HidePanelShop()
    {
        if (panelShop.activeSelf)
            managerShop.GetComponent<RadialMenuController>().OpenMenu();

        panelShop.SetActive(false);
    }

    public void ShowPanelUpgrade(Tile tile)
    {
        panelUpgrade.SetActive(true);
        panelUpgrade.transform.position = Camera.main.WorldToScreenPoint(tile.transform.position);
        managerUpgrade.GetComponent<RadialMenuController>().OpenMenu();
    }

    public void HidePanelUpgrade()
    {
        if (panelUpgrade.activeSelf)
            managerUpgrade.GetComponent<RadialMenuController>().OpenMenu();

        panelUpgrade.SetActive(false);
    }

}
