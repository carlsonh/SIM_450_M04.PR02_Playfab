using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;

    private float startTime;
    private float timeTaken;

    private int collectablesCollected;
    public int maxCollectables = 9;

    private bool bIsPlaying;


    public GameObject playButton;
    public TextMeshProUGUI curTimeText;

    public int keyPresses = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bIsPlaying)
        {
            return;
        }

        float _x = Input.GetAxis("Horizontal") * speed;
        float _z = Input.GetAxis("Vertical") * speed;

        rb.velocity = new Vector3(_x, rb.velocity.y, _z);

        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D))
        {
            keyPresses++;
        }


    curTimeText.text = (Time.time - startTime).ToString("F3");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collectablesCollected++;
            Destroy(other.gameObject);

            if (collectablesCollected == maxCollectables)
            {
                End();
            }
        }
    }

    public void Begin()
    {
        startTime = Time.time;
        bIsPlaying = true;
        playButton.SetActive(false);
    }

    void End()
    {
        timeTaken = Time.time - startTime;
        bIsPlaying = false;
        playButton.SetActive(true);

        Leaderboard.instance.SetLeaderboardEntryInputs(-keyPresses);
        Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
    }
}
