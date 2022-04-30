using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RadialMenuController : MonoBehaviour
{
    List<Button> childButtons = new List<Button>();
    Vector2[] buttonGoalPos;
    bool open = false;
    int buttonDistance = 100;
    float speed = 4f;

    // Start is called before the first frame update
    void OnEnable()
    {
        childButtons = GetComponentsInChildren<Button>(true).Where(x => x.gameObject.transform.parent != transform.parent).ToList();
        buttonGoalPos = new Vector2[childButtons.Count];
        //GetComponent<Button>().onClick.AddListener(() => { OpenMenu(); });
        GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

        foreach (Button b in childButtons)
        {
            b.gameObject.transform.position = transform.position;
            b.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            b.gameObject.SetActive(false);

            Text[] childs = b.gameObject.transform.GetComponentsInChildren<Text>();
            foreach (Text child in childs)
            {
                child.color = new Color(1, 1, 1, 0);
                child.gameObject.SetActive(false);
            }
        }
    }

    public void OpenMenu()
    {
        float angle;
        open = !open;

        if (childButtons.Count < 3)
            angle = 180 / (childButtons.Count - 1) * Mathf.Deg2Rad;
        else
            angle = 290 / (childButtons.Count - 1) * Mathf.Deg2Rad;

        for (int i = 0; i < childButtons.Count; i++)
        {
            if (open)
            {
                float xpos = Mathf.Cos(angle * i) * (buttonDistance * Screen.width / 800);
                float ypos = Mathf.Sin(angle * i) * (buttonDistance * Screen.width / 800);

                buttonGoalPos[i] = new Vector2(transform.position.x + xpos, transform.position.y + ypos);
                //childButtons[i].gameObject.transform.position = new Vector2(transform.position.x + xpos, transform.position.y + ypos);
            }
            else
            {
                buttonGoalPos[i] = transform.position;
                //childButtons[i].gameObject.transform.position = transform.position;
            }
        }

        StartCoroutine(MoveButtons());
    }

    private IEnumerator MoveButtons()
    {
        foreach (Button b in childButtons)
        {
            b.gameObject.SetActive(true);
            b.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        int loops = 0;
        while (loops <= buttonDistance / speed)
        {
            yield return new WaitForSeconds(0.008f);
            for (int i = 0; i < childButtons.Count; i++)
            {
                Color c = childButtons[i].gameObject.GetComponent<Image>().color;
                if (open)
                    //c.a = Mathf.Lerp(c.a, 1, speed * Time.deltaTime);
                    c.a = 1;
                else
                    c.a = Mathf.Lerp(c.a, 0, speed * Time.deltaTime);

                childButtons[i].gameObject.GetComponent<Image>().color = c;
                childButtons[i].gameObject.transform.position = Vector2.Lerp(childButtons[i].gameObject.transform.position, buttonGoalPos[i], speed * Time.deltaTime);

                Text[] childs = childButtons[i].gameObject.transform.GetComponentsInChildren<Text>();
                foreach (Text child in childs)
                {
                    child.color = c;
                }

            }
            loops++;
        }
        if (!open)
        {
            foreach (Button b in childButtons)
            {
                b.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
