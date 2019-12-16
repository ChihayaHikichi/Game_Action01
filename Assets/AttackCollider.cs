using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

    static private int Num = 0;
    private int TeamID;

    // Use this for initialization
    void Start () {
        
        this.TeamID = AttackCollider.Num/3+1;
        AttackCollider.Num++;

    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("OK)");
    }



    
    void OnTriggerEnter(Collider other)
    {

        //Debug.Log("OK)");
        if (other.gameObject.GetComponent<CharacterController>())
        {
            if (this.TeamID != other.gameObject.GetComponent<CharacterController>().TeamID)
            {
                other.gameObject.GetComponent<CharacterController>().AddDamage(80);
            }
        }
    }
}
