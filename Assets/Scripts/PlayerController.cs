using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text endText;
    public Text timerText;

    private System.Timers.Timer timer;
    private Rigidbody rb;
    private List<GameObject> pickUps;
    private int score;
    private int scoreToWin = 16;
    // Integer that holds the initial time
    private const int startTime = 30;
    // Integer to decrement and display to player
    private int timeTick;

    void Start()
    {
        pickUps = new List<GameObject>();
        rb = GetComponent<Rigidbody>();
        score = 0;
        timeTick = startTime;
        countText.text = "Count: " + score.ToString();
        endText.text = "";
        timerText.text = "Time: " + timeTick;

        PopulatePickUpsList();
        // Begin timer. Wait one second to start timer to ensure player is ready.
        InvokeRepeating("TimerTick", 1f, 1f);       
    }

    // Populate pickUps List with all PickUp prefabs
    private void PopulatePickUpsList()
    {
        var goList = FindObjectsOfType<GameObject>();
        foreach(var go in goList)
        {
            if (go.CompareTag("Pick Up") || go.CompareTag("Pick Up Special"))
            {
                pickUps.Add(go);
            }
        }
    }

    void Update()
    {
        CheckWin();        

        // Did user press 'N'?
        if (Input.GetKeyDown(KeyCode.N))
        {
            NewGame();
        }
    }

    private void CheckWin()
    {
        if (timeTick <= 0)
        {
            CancelInvoke("TimerTick");
            foreach (var go in pickUps)
            {
                go.SetActive(false);
            }
            endText.text = "Out of Time! You Lose.";
        }
        else if (score == scoreToWin)
        {
            CancelInvoke("TimerTick");
            endText.text = "You won! And with " + timeTick.ToString() + " seconds to spare. ";
            endText.text += timeTick > 5 ? "Impressive!" : "That was close!";
        }
    }

    private void NewGame()
    {
        CancelInvoke("TimerTick");
        foreach (var go in pickUps)
        {
            go.SetActive(true);
        }
        score = 0;
        timeTick = startTime;
        countText.text = "Count: " + score.ToString();
        timerText.text = "Time: " + timeTick.ToString();
        endText.text = "";
        InvokeRepeating("TimerTick", 1f, 1f);
    }

    // Called before any Physics Calculations are performed
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVerticalal = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVerticalal);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            ++score;
        }
        else if (other.gameObject.CompareTag("Pick Up Special"))
        {
            other.gameObject.SetActive(false);
            score += 5;
        }

        countText.text = "Count: " + score.ToString();
    }

    void SetCountText()
    {
        countText.text = "Count: " + score.ToString();
        if (score >= 16)
        {
            endText.text = "You Win!";
            GameObject.Find("Player").SendMessage("Finish");
        }
    }

    void TimerTick()
    {
        --timeTick;
        timerText.text = "Time: " + timeTick;
    }
}
