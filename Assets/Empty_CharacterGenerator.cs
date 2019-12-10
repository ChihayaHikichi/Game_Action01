using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty_CharacterGenerator : MonoBehaviour {

    public GameObject CharacterPrehab;


    // Use this for initialization
    void Start () {

        // じぶんを作成
        GameObject Character1 = Instantiate(CharacterPrehab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
        Character1.name = "Character1";
        Character1.GetComponent<CharacterController>().TeamID = 0;

        GameObject Character2 = Instantiate(CharacterPrehab, new Vector3(0, 3, 2), new Quaternion(0, 0, 0, 0)) as GameObject;
        Character2.name = "Character2";
        Character2.GetComponent<CharacterController>().TeamID = 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
