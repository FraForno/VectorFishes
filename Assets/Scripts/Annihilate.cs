using UnityEngine;
using System.Collections;

public class Annihilate : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Fish", System.StringComparison.Ordinal))
        {
            //deactivate collision and hide sprite
            other.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            //play escape sound
            other.gameObject.GetComponent<Fish>().PlayEscapeSound();

            //update score
            GameObject spammy = GameObject.Find("MrSpammy");
            Generator spammyscript = spammy.GetComponent<Generator>();

            if (spammyscript.score < 10)
                spammyscript.score = 0;
            else if(spammyscript.score <= 50)
                spammyscript.score -= 10;
            else if (spammyscript.score <= 100)
                spammyscript.score -= 20;
            else if (spammyscript.score <= 200)
                spammyscript.score -= 40;
            else
                spammyscript.score -= 50;
        }

        Destroy(other.gameObject, 1.0f); //delay of 1 sec to let the escape sound finish
    }
}
