using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fish : MonoBehaviour {

    public Vector2 velocity = new Vector2(2, 0);
    public float UpdateFreq = 1.0f; //movement update frequency in seconds
    public float Intensity = 0.5f; //vertical movement intensity
    public float turnSpeed = 1.0f; //rotation speed
    public float flipFreq = 3.0f;

    public AudioClip[] clips = new AudioClip[3];
    public Sprite[] sprites = new Sprite[3];
    public uint[] points = { 10, 20, 30 };

    public GameObject deadFish;
    public Object rewardPrefab = Resources.Load("Prefabs/Reward");

    int flip = 1;
    int fishType = 0;
    bool bDead = false;

    void Start()
    {
        //random fish type
        int perc = Random.Range(0, 100);

        if (perc <= 50) //0-50% spwans yellow (50%)
            fishType = 0;
        else if (perc <= 80) //51-80% spawns red (30%)
            fishType = 1;
        else //81-100% spawans green (20%)
            fishType = 2;

        this.GetComponent<SpriteRenderer>().sprite = sprites[fishType];

        // 50% chance of starting with rightwards motion
        if (Random.value > 0.5f)
        {
            transform.Rotate(new Vector3(0, 180, 0));
            flip = -1;
        }
        
        this.GetComponent<Rigidbody2D>().velocity = (velocity * flip);

        InvokeRepeating("UpdateFishVel", UpdateFreq, UpdateFreq);

        InvokeRepeating("FlipFish", flipFreq, flipFreq);
    }

    void UpdateFishVel()
    {
        //update vertical movement
        Vector2 updVel = new Vector2(0, Random.Range(Intensity*-1, Intensity));
        this.GetComponent<Rigidbody2D>().velocity += updVel;
    }

    void FlipFish()
    {
        if (Random.value > 0.5f)
        {
            flip = flip * -1;
            this.GetComponent<Rigidbody2D>().velocity = (velocity * flip);
        }
    }

    Reward InstantiateAndInitReward(int iType, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Instantiate(rewardPrefab, position, rotation) as GameObject;
        Reward rw = obj.GetComponent<Reward>();
        rw.fishType = iType;
        return rw;
    }

    void FixedUpdate ()
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
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle - ((flip == 1) ? 0 : 180), Vector3.forward) * ((flip==1) ? Quaternion.identity : Quaternion.AngleAxis(180.0f, Vector3.up)), Time.deltaTime * turnSpeed);
        }

        //check for touch
        int i = 0;
        List<Touch> aList = InputHelper.GetTouches();
        while (i < aList.Count)
        {
            if (aList[i].phase == TouchPhase.Began)
            {
                //int layerMask = 1 << LayerMask.NameToLayer("Fish");
                //RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(aList[i].position), Vector2.zero, Mathf.Infinity, layerMask);
                //if (hit.collider != null)
                Vector3 wp = Camera.main.ScreenToWorldPoint(aList[i].position);
                Vector2 touchPos = new Vector2(wp.x, wp.y);
                if (this.GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPos))
                {
                    if (!bDead)
                    {
                        bDead = true;
                            
                        //play audio
                        int j = Random.Range(0, 3);

                        this.GetComponent<AudioSource>().clip = clips[j];
                        this.GetComponent<AudioSource>().Play();

                        //hide
                        this.GetComponent<SpriteRenderer>().enabled = false;

                        //put dead fish in place
                        Instantiate(deadFish, this.transform.position, this.transform.rotation);

                        //put points in place
                        InstantiateAndInitReward(fishType, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);

                        //remove as soon as clip ends
                        Destroy(this.gameObject, clips[j].length);

                        //update score
                        GameObject spammy = GameObject.Find("MrSpammy");
                        Generator spammyscript = spammy.GetComponent<Generator>();
                        spammyscript.score += points[fishType];
                    }
                }
            }
            i++;
        }
    }
}