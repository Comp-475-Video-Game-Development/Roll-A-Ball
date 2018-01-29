using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speedyHurryUpZoom;
    public Text countText;
    public Text endText;

    private Rigidbody rb;
    private int count;
    private float elapsedTime;
    private bool gameOver;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        endText.text = "";
        elapsedTime = 0.0f;
        gameOver = false;
    }

    void Update()
    {
        if (!gameOver)
        {

        }

        // Did user press 'N'?
        if (Input.GetKeyDown(KeyCode.N))
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        print("NewGame called.");
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (var go in allObjects)
        {
            if (go.CompareTag("Pick Up") || go.CompareTag("Pick Up Special"))
            {
                go.SetActive(true);
            }
        }
        endText.text = "";
        GameObject.Find("Player").SendMessage("Finish");
    }

    // Called before any Physics Calculations are performed
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVerticalal = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVerticalal);
        rb.AddForce(movement * speedyHurryUpZoom);
    }

    void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            ++count;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Pick Up Special"))
        {
            other.gameObject.SetActive(false);
            count += 5;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 16)
        {
            endText.text = "You Win!";
            GameObject.Find("Player").SendMessage("Finish");
        }
    }
}
