using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour {

    public GameObject bubble;
    public float BubbleRangeX = 6.5f;
    public float BubbleY = -5f;

    public GameObject fish;
    public GameObject badfish;
    public float FishRangeX = 6.5f;
    public float FishRangeY = 5f;
    public float FishGenDelay = 2.0f;
    public float BadFishGenDelay = 12.0f;

    public ulong score = 0;
    bool canLose = false;

    // Use this for initialization
    void Start ()
    {
        InvokeRepeating("CreateBubble", 1.0f, 1.0f);
        //InvokeRepeating("CreateFish", FishGenDelay, FishGenDelay);
        StartCoroutine("CreateFish");
        InvokeRepeating("CreateBadFish", BadFishGenDelay, BadFishGenDelay);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (score > 0)
            canLose = true;

        if(canLose && score == 0)
            SceneManager.LoadScene(3);
    }

    void CreateBubble()
    {
        Instantiate(bubble, new Vector3(Random.Range(BubbleRangeX * -1, BubbleRangeX), BubbleY, 0), Quaternion.identity);
    }

    IEnumerator CreateFish()
    {
        while (true)
        {
            Instantiate(fish, new Vector3(Random.Range(FishRangeX * -1, FishRangeX), Random.Range(FishRangeY * -1, FishRangeY), 0), Quaternion.identity);
            if (FishGenDelay > 0.2f)
                FishGenDelay -= 0.01f;
            yield return new WaitForSeconds(FishGenDelay);
        }
    }

    void CreateBadFish()
    {
        Instantiate(badfish, new Vector3(Random.Range(FishRangeX * -1, FishRangeX), Random.Range(FishRangeY * -1, FishRangeY), 0), Quaternion.identity);
    }

    void OnGUI()
    {
        GUI.color = Color.red;
        GUILayout.Label("<size=40> Score: " + score.ToString() + "</size>");
    }
}
