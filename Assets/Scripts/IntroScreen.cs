using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour {

    public GameObject creditsButton;
	
	// Update is called once per frame
	void Update ()
    {
        //check for touch
        int i = 0;
        List<Touch> aList = InputHelper.GetTouches();
        while (i < aList.Count)
        {
            if (aList[i].phase == TouchPhase.Began)
            {
                Vector3 wp = Camera.main.ScreenToWorldPoint(aList[i].position);
                Vector2 touchPos = new Vector2(wp.x, wp.y);
                if (creditsButton.GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPos))
                {
                    SceneManager.LoadScene(2);
                }
                else
                {
                    SceneManager.LoadScene(1);
                }
            }
            i++;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
