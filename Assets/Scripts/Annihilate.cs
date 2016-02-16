using UnityEngine;
using System.Collections;

public class Annihilate : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Fish", System.StringComparison.Ordinal))
        {
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

        Destroy(other.gameObject);
    }
}
