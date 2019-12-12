using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    private GameObject MainCharacter;

    private GameObject HPGage;
    private GameObject SPGage;
    private GameObject StaminaGage;





    // Use this for initialization
    void Start () {
        this.MainCharacter = GameObject.Find("Character1");

        this.HPGage = GameObject.Find("HPGage");
        this.SPGage = GameObject.Find("SPGage");
        this.StaminaGage = GameObject.Find("StaminaGage");


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DrawUI() { 

        
        // HP
        if (MainCharacter.GetComponent<CharacterController>().HP != MainCharacter.GetComponent<CharacterController>().HP_Old)
        {
            HPGage.transform.localScale = new Vector3((float)MainCharacter.GetComponent<CharacterController>().HP / MainCharacter.GetComponent<CharacterController>().HPMax, 1, 1);
        }
        // SP
        if (MainCharacter.GetComponent<CharacterController>().SP != MainCharacter.GetComponent<CharacterController>().SP_Old)
        {
            SPGage.transform.localScale = new Vector3((float)MainCharacter.GetComponent<CharacterController>().SP / MainCharacter.GetComponent<CharacterController>().SPMax, 1, 1);
        }
        // Stamina
        if (MainCharacter.GetComponent<CharacterController>().Stamina != MainCharacter.GetComponent<CharacterController>().Stamina_Old)
        {
            StaminaGage.transform.localScale = new Vector3((float)MainCharacter.GetComponent<CharacterController>().Stamina / MainCharacter.GetComponent<CharacterController>().StaminaMax, 1, 1);
        }
        


    }
}
