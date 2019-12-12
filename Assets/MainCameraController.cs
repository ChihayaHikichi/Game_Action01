using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour {

    //private float HRotation = 0.0f;
    //private float VRotation = 0.0f;

    // キャラクターからどれだけ引くか
    float CameraDistanceZ = - 3.2f;
    // カメラの高さ（キャラクター基準）
    float CameraDistanceY = 1.6f;

    // 自分が操作するキャラのオブジェクトを取得
    private GameObject MainCharacter;
    //public GameObject MainCharacter;


    // Use this for initialization
    void Start () {
        this.MainCharacter = GameObject.Find("Character1");
    }
	
	// Update is called once per frame
	void Update () {




        //MainCharacter.GetComponent<CharacterController>().HRotation;


        this.transform.rotation = Quaternion.Euler(
            MainCharacter.GetComponent<CharacterController>().VRotation,
            MainCharacter.GetComponent<CharacterController>().HRotation,
            0);
        /*
        this.transform.rotation=Quaternion.Euler(
            0, 
            MainCharacter.transform.localEulerAngles.y, 
            MainCharacter.transform.localEulerAngles.z);
        */



        this.transform.position = new Vector3(
            MainCharacter.transform.position.x + CameraDistanceZ * Mathf.Sin(MainCharacter.GetComponent<CharacterController>().HRotation * Mathf.PI / 180) * Mathf.Cos(MainCharacter.GetComponent<CharacterController>().VRotation * Mathf.PI / 180), 
            MainCharacter.transform.position.y - CameraDistanceZ * Mathf.Sin(MainCharacter.GetComponent<CharacterController>().VRotation * Mathf.PI / 180) + CameraDistanceY, 
            MainCharacter.transform.position.z + CameraDistanceZ * Mathf.Cos(MainCharacter.GetComponent<CharacterController>().HRotation * Mathf.PI / 180) * Mathf.Cos(MainCharacter.GetComponent<CharacterController>().VRotation * Mathf.PI / 180));

        //Debug.Log(MainCharacter.transform.localEulerAngles.y);


        


    }
}
