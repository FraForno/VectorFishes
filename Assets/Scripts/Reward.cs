using UnityEngine;
using System.Collections;

public class Reward : MonoBehaviour {

    public Vector2 velocity = new Vector2(0, 2);
    public float UpdateFreq = 1.0f; //movement update frequency in seconds
    public float Intensity = 0.5f; //horizontal movement intensity

    public Sprite[] sprites = new Sprite[9];
    public int fishType = 0;

    private int signInverter = 1;

    // Use this for initialization
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = sprites[fishType];
        
        //size, position and movement
        this.GetComponent<Rigidbody2D>().velocity = velocity;
        InvokeRepeating("UpdateVel", UpdateFreq, UpdateFreq);

        Destroy(this.gameObject, 5.0f);
    }

    void UpdateVel()
    {
        Vector2 updVel = new Vector2(Random.Range(0.0f, Intensity) * signInverter, 0);
        this.GetComponent<Rigidbody2D>().velocity += updVel;
        signInverter = signInverter * -1;
    }
}
