using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CreditsScreen : MonoBehaviour {

    void Update()
    {
        int i = 0;
        List<Touch> aList = InputHelper.GetTouches();
        while (i < aList.Count)
        {
            if (aList[i].phase == TouchPhase.Ended)
            {
                SceneManager.LoadScene(0);
            }
            i++;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
