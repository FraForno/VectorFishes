using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BadFish : MonoBehaviour
{

    public Vector2 velocity = new Vector2(2, 0);
    public float UpdateFreq = 1.0f; //movement update frequency in seconds
    public float Intensity = 0.5f; //vertical movement intensity
    public float turnSpeed = 1.0f; //rotation speed

    public float suicideDelay = 4.0f;

    public uint points = 20;

    public GameObject deadFish;
    public Object rewardPrefab;

    int flip = 1;
    int fishType = 4;
    bool bDead = false;

    void Start()
    {
        rewardPrefab = Resources.Load("Prefabs/Reward");

        // 50% chance of starting with rightwards motion
        if (Random.value > 0.5f)
        {
            transform.Rotate(new Vector3(0, 180, 0));
            flip = -1;
        }

        this.GetComponent<Rigidbody2D>().velocity = (velocity * flip);

        InvokeRepeating("UpdateFishVel", UpdateFreq, UpdateFreq);

        Invoke("Suicide", suicideDelay);
    }

    void Suicide()
    {
        if(!bDead)
            Destroy(this.gameObject);
    }

    void UpdateFishVel()
    {
        //update vertical movement
        Vector2 updVel = new Vector2(0, Random.Range(Intensity * -1, Intensity));
        this.GetComponent<Rigidbody2D>().velocity += updVel;
    }

    Reward InstantiateAndInitReward(int iType, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Instantiate(rewardPrefab, position, rotation) as GameObject;
        Reward rw = obj.GetComponent<Reward>();
        rw.fishType = 3;
        return rw;
    }

    void FixedUpdate()
    {
        //rotate fish along movement vector
        if (this.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            //calculate the angle in degrees between the X axis and the velocity vector
            float angle = Mathf.Atan2(this.GetComponent<Rigidbody2D>().velocity.y, this.GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;

            //rotate the fish arounz the Z axis by that angle
            //note that if the fish is moving leftwards, then it is already rotated by 180 degrees
            //around the Y axis so that rotation must be taken into account when lerping
            //also the angle between the X axis and a "left-pointing" velocity vector must be 
            //corrected by subtracting 180 degrees
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle - ((flip == 1) ? 0 : 180), Vector3.forward) * ((flip == 1) ? Quaternion.identity : Quaternion.AngleAxis(180.0f, Vector3.up)), Time.deltaTime * turnSpeed);
        }

        //check for touch
        int i = 0;
        List<Touch> aList = InputHelper.GetTouches();
        while (i < aList.Count)
        {
            if (aList[i].phase == TouchPhase.Began)
            {
                Vector3 wp = Camera.main.ScreenToWorldPoint(aList[i].position);
                Vector2 touchPos = new Vector2(wp.x, wp.y);
                if (this.GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPos))
                {
                    if (!bDead)
                    {
                        bDead = true;

                        //play audio
                        this.GetComponent<AudioSource>().Play();

                        //hide
                        this.GetComponent<SpriteRenderer>().enabled = false;

                        //put dead fish in place
                        Instantiate(deadFish, this.transform.position, this.transform.rotation);

                        //put points in place
                        InstantiateAndInitReward(fishType, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);

                        //remove as soon as clip ends
                        Destroy(this.gameObject, this.GetComponent<AudioSource>().clip.length);

                        //update score
                        GameObject spammy = GameObject.Find("MrSpammy");
                        Generator spammyscript = spammy.GetComponent<Generator>();
                        if (spammyscript.score > 20)
                            spammyscript.score -= points;
                        else
                            spammyscript.score = 0;
                    }
                }
            }
            i++;
        }
    }
}