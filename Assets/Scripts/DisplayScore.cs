using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine;

public class DisplayScore : MonoBehaviour {

    [SerializeField] private Text scoreMessage;
    [SerializeField] private Text elapsedTime;
    private string player;

    // Use this for initialization
    void Start() {
        scoreMessage.text = "";
        elapsedTime.text = "";
    }


    public void Display()
    {
        scoreMessage.text = "Correct: " + Results.score.ToString();
        elapsedTime.text = "Time: " + Results.time.ToString();
    }

}

