using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public float OneFlameTime = 0;
    private float TimeCountOneSecound = 0;
    private float TimeCountZeroPOneSecound = 0;




    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        OneFlameTime = Time.deltaTime;
        TimeCountOneSecound += OneFlameTime;
        TimeCountZeroPOneSecound += OneFlameTime;





    }


    public float TimeCountOneSecoundGet()
    {
        return TimeCountOneSecound;
    }

    public void TimeCountOneSecoundDecriment()
    {
        TimeCountOneSecound -= 1.0f;
    }

    public float TimeCountZeroPOneSecoundGet()
    {
        return TimeCountZeroPOneSecound;
    }

    public void TimeCountZeroPOneSecoundDecriment()
    {
        TimeCountZeroPOneSecound -= 0.1f;
    }
}
