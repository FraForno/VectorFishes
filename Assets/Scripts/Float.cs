using UnityEngine;
using System.Collections;

public class Float : MonoBehaviour {

    public Vector2 velocity = new Vector2(0, 2);
    public float UpdateFreq = 1.0f; //movement update frequency in seconds
    public float Intensity = 0.5f; //horizontal movement intensity

    private int signInverter = 1;

    public AudioClip[] clips = new AudioClip[3];

    // Use this for initialization
    void Start ()
    {
        //size, position and movement
        float scale = Random.Range(0.0f, 1.0f); //scale the object by a factor ranging from 0% to 100%
        transform.localScale += new Vector3(scale, scale, 0);
        this.GetComponent<Rigidbody2D>().velocity = velocity;
        InvokeRepeating("UpdateBubbleVel", UpdateFreq, UpdateFreq);

        //audio
        int i = Random.Range(0, 6);

        if (i < clips.Length)
        {
            this.GetComponent<AudioSource>().clip = clips[i];
            this.GetComponent<AudioSource>().Play();
        }
    }
	
	void UpdateBubbleVel()
    {
        Vector2 updVel = new Vector2(Random.Range(0.0f, Intensity) * signInverter, 0);
        this.GetComponent<Rigidbody2D>().velocity += updVel;
        signInverter = signInverter * -1;
    }
}
