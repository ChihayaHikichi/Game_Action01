using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerReseter : MonoBehaviour {


    private GameObject Timer;


    // Use this for initialization
    void Start () {
        this.Timer = GameObject.Find("Empty_Timer");
    }
	
	// Update is called once per frame
	void Update () {
        if (Timer.GetComponent<Timer>().TimeCountOneSecoundGet() >= 1.0f)
        {
            Timer.GetComponent<Timer>().TimeCountOneSecoundDecriment();
        }

        if (Timer.GetComponent<Timer>().TimeCountZeroPOneSecoundGet() >= 1.0f)
        {
            Timer.GetComponent<Timer>().TimeCountZeroPOneSecoundDecriment();
        }
    }
}
