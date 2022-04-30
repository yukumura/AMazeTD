using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carousel : MonoBehaviour
{

    public Text titleText;
    public Image newGame;
    public Image quit;


    public GameObject[] objects;
    public Color[] colors;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        int randomCell = Random.Range(0, objects.Length);

        objects[randomCell].SetActive(true);

        //if(colors.Length > randomCell)
        //{
        //    titleText.color = colors[randomCell];
        //    newGame.color = colors[randomCell];
        //    quit.color = colors[randomCell];
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
