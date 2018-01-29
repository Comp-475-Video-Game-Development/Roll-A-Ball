using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public Text endText;
    private float timer;
    private bool finished = false;

	// Use this for initialization
	void Start ()
    {
        timer = 30;
        timerText.text = "Time: 30";
        InvokeRepeating("UpdateTimer", 1.0f, 1.0f);
	}
	
	void UpdateTimer ()
    {
        if (finished)
        {
            return;
        }

        float t = timer - Time.time;
        string seconds = (t % 60).ToString("f0");
        timerText.text = "Time: " + seconds;

        if (t <= 0.0f)
        {
            finished = true;
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (var go in allObjects)
            {
                if (go.CompareTag("Pick Up") || go.CompareTag("Pick Up Special"))
                {
                    go.SetActive(false);
                }
            }
            endText.text = "Out of Time! You Lose.";
        }
	}

    public void Finish()
    {
        finished = true;
        timerText.color = Color.yellow;
    }
}
