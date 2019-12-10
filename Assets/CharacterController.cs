using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class Constants
{
    public const int LockOnAddDegree = 15;
    public const float DashSpeedMultiple = 1.5f;
}

public class CharacterController : MonoBehaviour {

    // デバッグ用テキスト表示
    private GameObject DebugText;

    // ロックオン対象
    private GameObject LockOnTarget;

    private GameObject MyInput;

    private Rigidbody myRigidbody;

    private Animator myAnimator;

    private int i;

    // ボタンの長押し判定時間
    int NagaosiTime = 30;

    int SightTypeTime = 0;

    // キャラクター数
    static private int CharacterNum = 0;

    // キャラクターID
    private int ID;

    // チーム分け
    public int TeamID;

    // 歩いているフラグ（F,B,R,L,FR,FL,BR,BL）
    private bool[] WalkFlag = new bool[8] { false, false, false, false, false, false, false, false };
    //private bool[] WalkFlag = new bool[8];
    private int WASD = 0;


    // 移動速度
    private float MoveSpeed;
    

    // ダッシュフラグ
    private bool DashFlag = false;

    // 向き
    public float HRotation;
    public float VRotation;

    // ロックオンフラグ
    private bool LockOnFlag = false;

    // 視点タイプ（F.キャラとカメラの向き不一致, T.キャラとカメラの向き一致）
    public bool SightType;

    // Use this for initialization
    void Start () {

        // デバッグ用テキスト表示
        this.DebugText = GameObject.Find("debug");



        this.MyInput = GameObject.Find("Empty_MyInput");

        //Rigidbodyコンポーネントを取得
        this.myRigidbody = GetComponent<Rigidbody>();

        //Animatorコンポーネントを取得
        this.myAnimator = GetComponent<Animator>();

        

        // ID取得
        CharacterNum += 1;
        ID = CharacterNum;

        //Debug.Log(ID);

        /*
        for (i = 0; i < 8; i++) 
        {
            WalkFlag[i] = false;
        }
        */


        // 移動速度
        MoveSpeed = 200.0f;

        // 向き初期化
        HRotation = this.transform.localEulerAngles.y;
        VRotation = this.transform.localEulerAngles.x;

        SightType = false;


        




    }
	
	// Update is called once per frame
	void Update () {

        // 自分の操作を代入
        if (this.ID == 1) 
        {
            // WASDキーの入力を取得
            this.WASD = MyInput.GetComponent<MyInput>().WASD;

            // 視点タイプ切り替えを取得
            this.SightType = MyInput.GetComponent<MyInput>().SightType;

            // ダッシュフラグを取得
            // ダッシュフラグの切り替わり
            if (this.DashFlag == false && MyInput.GetComponent<MyInput>().DashFlag == true)
            {
                
            }
            this.DashFlag = MyInput.GetComponent<MyInput>().DashFlag;




            // ロックオンフラグを取得
            // ロックオンの切り替わり
            if (this.LockOnFlag == true && MyInput.GetComponent<MyInput>().LockOnFlag == false)
            {
                MyInput.GetComponent<MyInput>().HRotation = this.HRotation;
                MyInput.GetComponent<MyInput>().VRotation = this.VRotation;
            }
            this.LockOnFlag = MyInput.GetComponent<MyInput>().LockOnFlag;

            // 視点移動
            // マウスの移動分を回転方向に加算
            this.HRotation = MyInput.GetComponent<MyInput>().HRotation;
            this.VRotation = MyInput.GetComponent<MyInput>().VRotation;


            // デバッグ
            if (Input.GetKey(KeyCode.Q) == true)
            {
                //Debug.Log("OK");
                //WalkFlag = false;
                //this.myAnimator.SetBool("Walk_F_Start", false);
                //this.DebugText.GetComponent<Text>().text = "F :" + this.WalkFlag[0] + "\nB :" + this.WalkFlag[1] + "\nR :" + this.WalkFlag[2] + "\nL :" + this.WalkFlag[3];

                //this.myAnimator.SetBool("Dash_Start", true);
            }

        }









        // 全キャラクターID共通

        // キャラクターを指定角度に回転させる
        if (this.SightType == true && this.LockOnFlag == false)
        {
            this.transform.rotation = Quaternion.Euler(0, HRotation, 0);
        }

        // ロックオン対象を取得
        // 今回は敵が一体しかいないので省略する。悪く思うなよ。
        this.LockOnTarget = GameObject.Find("Character2");

        // キャラクターをロックオン方向に向かす
        if (this.LockOnFlag == true)
        {
            this.transform.LookAt(LockOnTarget.transform.position);
            // H・VRotateを同期させる
            this.HRotation = this.transform.localEulerAngles.y;
            this.VRotation = this.transform.localEulerAngles.x + Constants.LockOnAddDegree; 
            //Debug.Log(this.transform.localEulerAngles.x);
        }



        // 移動
        if (this.SightType == true || this.LockOnFlag == true)
        {
            switch (this.WASD)
            {
                case 1:
                    // 前
                    this.myRigidbody.AddForce(this.transform.forward * MoveSpeed);

                    // アニメーション
                    if (this.WalkFlag[0] == false)
                    {
                        WalkFlagManager(0);
                    }
                    break;
                case 2:
                    // 後ろ
                    this.myRigidbody.AddForce(this.transform.forward * (-1) * MoveSpeed * 0.4f);

                    // アニメーション
                    if (this.WalkFlag[1] == false)
                    {
                        WalkFlagManager(1);
                    }
                    break;
                case 3:
                    // 右
                    this.myRigidbody.AddForce(this.transform.right * MoveSpeed);

                    // アニメーション
                    if (this.WalkFlag[2] == false)
                    {
                        WalkFlagManager(2);
                    }
                    break;
                case 4:
                    // 左
                    this.myRigidbody.AddForce(this.transform.right * (-1) * MoveSpeed);

                    // アニメーション
                    if (this.WalkFlag[3] == false)
                    {
                        WalkFlagManager(3);
                    }
                    break;
                case 5:
                    // 右前
                    this.myRigidbody.AddForce((this.transform.right + this.transform.forward) * MoveSpeed);

                    // アニメーション
                    if (this.WalkFlag[4] == false)
                    {
                        WalkFlagManager(4);
                    }
                    break;
                case 6:
                    // 左前
                    this.myRigidbody.AddForce((this.transform.right * (-1) + this.transform.forward * 1) * MoveSpeed);

                    // アニメーション
                    if (this.WalkFlag[5] == false)
                    {
                        WalkFlagManager(5);
                    }
                    break;
                case 7:
                    // 右後ろ
                    this.myRigidbody.AddForce((this.transform.right + this.transform.forward * (-1)) * MoveSpeed * 0.4f);

                    // アニメーション
                    if (this.WalkFlag[6] == false)
                    {
                        WalkFlagManager(6);
                    }
                    break;
                case 8:
                    // 左後ろ
                    this.myRigidbody.AddForce((this.transform.right * (-1) + this.transform.forward * (-1)) * MoveSpeed * 0.4f);

                    // アニメーション
                    if (this.WalkFlag[7] == false)
                    {
                        WalkFlagManager(7);
                    }
                    break;
            }
        }
        else
        {
            switch (this.WASD)
            {
                case 1:
                    // 前
                    this.transform.rotation = Quaternion.Euler(0, HRotation, 0);
                    break;
                case 2:
                    // 後ろ
                    this.transform.rotation = Quaternion.Euler(0, HRotation + 180, 0);
                    break;
                case 3:
                    // 右
                    this.transform.rotation = Quaternion.Euler(0, HRotation + 90, 0);
                    break;
                case 4:
                    // 左
                    this.transform.rotation = Quaternion.Euler(0, HRotation - 90, 0);
                    break;
                case 5:
                    // 右前
                    this.transform.rotation = Quaternion.Euler(0, HRotation + 45, 0);
                    break;
                case 6:
                    // 左前
                    this.transform.rotation = Quaternion.Euler(0, HRotation - 45, 0);
                    break;
                case 7:
                    // 右後ろ
                    this.transform.rotation = Quaternion.Euler(0, HRotation + 135, 0);
                    break;
                case 8:
                    // 左後ろ
                    this.transform.rotation = Quaternion.Euler(0, HRotation - 135, 0);
                    break;
            }

            if (this.WASD > 0)
            {
                this.myRigidbody.AddForce(this.transform.forward * MoveSpeed);

                // アニメーション
                if (this.WalkFlag[0] == false)
                {
                    //WalkFlag[0] = true;
                    WalkFlagManager(0);
                    //WalkFlag[0] = true;
                }
            }
        }

        // 移動キーが押されていない
        if (this.WASD == 0)
        {
            WalkFlagManager(-1);
        }
















        // デバッグ
        //this.DebugText.GetComponent<Text>().text = "" + this.WalkFlag[0];


    }

    private void WalkFlagManager(int Direction)
    {
       

        switch (Direction)
        {
            case 0:
                for (i = 0; i < 8; i++) 
                {
                    if (Direction == i)
                    {
                        this.WalkFlag[Direction] = true;
                    }
                    else
                    {
                        this.WalkFlag[Direction] = false;
                    }
                }

                this.myAnimator.SetBool("Walk_F_Start", true);
                this.myAnimator.SetBool("Walk_B_Start", false);
                this.myAnimator.SetBool("Walk_R_Start", false);
                this.myAnimator.SetBool("Walk_L_Start", false);
                this.myAnimator.SetBool("Walk_FR_Start", false);
                this.myAnimator.SetBool("Walk_FL_Start", false);
                this.myAnimator.SetBool("Walk_BR_Start", false);
                this.myAnimator.SetBool("Walk_BL_Start", false);
                break;
            case 1:
                for (i = 0; i < 8; i++)
                {
                    if (Direction == i)
                    {
                        this.WalkFlag[Direction] = true;
                    }
                    else
                    {
                        this.WalkFlag[Direction] = false;
                    }
                }

                this.myAnimator.SetBool("Walk_F_Start", false);
                this.myAnimator.SetBool("Walk_B_Start", true);
                this.myAnimator.SetBool("Walk_R_Start", false);
                this.myAnimator.SetBool("Walk_L_Start", false);
                this.myAnimator.SetBool("Walk_FR_Start", false);
                this.myAnimator.SetBool("Walk_FL_Start", false);
                this.myAnimator.SetBool("Walk_BR_Start", false);
                this.myAnimator.SetBool("Walk_BL_Start", false);
                break;
            case 2:
                for (i = 0; i < 8; i++)
                {
                    if (Direction == i)
                    {
                        this.WalkFlag[Direction] = true;
                    }
                    else
                    {
                        this.WalkFlag[Direction] = false;
                    }
                }

                this.myAnimator.SetBool("Walk_F_Start", false);
                this.myAnimator.SetBool("Walk_B_Start", false);
                this.myAnimator.SetBool("Walk_R_Start", true);
                this.myAnimator.SetBool("Walk_L_Start", false);
                this.myAnimator.SetBool("Walk_FR_Start", false);
                this.myAnimator.SetBool("Walk_FL_Start", false);
                this.myAnimator.SetBool("Walk_BR_Start", false);
                this.myAnimator.SetBool("Walk_BL_Start", false);
                break;
            case 3:
                for (i = 0; i < 8; i++)
                {
                    if (Direction == i)
                    {
                        this.WalkFlag[Direction] = true;
                    }
                    else
                    {
                        this.WalkFlag[Direction] = false;
                    }
                }

                this.myAnimator.SetBool("Walk_F_Start", false);
                this.myAnimator.SetBool("Walk_B_Start", false);
                this.myAnimator.SetBool("Walk_R_Start", false);
                this.myAnimator.SetBool("Walk_L_Start", true);
                this.myAnimator.SetBool("Walk_FR_Start", false);
                this.myAnimator.SetBool("Walk_FL_Start", false);
                this.myAnimator.SetBool("Walk_BR_Start", false);
                this.myAnimator.SetBool("Walk_BL_Start", false);
                break;
            case 4:
                for (i = 0; i < 8; i++)
                {
                    if (Direction == i)
                    {
                        this.WalkFlag[Direction] = true;
                    }
                    else
                    {
                        this.WalkFlag[Direction] = false;
                    }
                }

                this.myAnimator.SetBool("Walk_F_Start", false);
                this.myAnimator.SetBool("Walk_B_Start", false);
                this.myAnimator.SetBool("Walk_R_Start", false);
                this.myAnimator.SetBool("Walk_L_Start", false);
                this.myAnimator.SetBool("Walk_FR_Start", true);
                this.myAnimator.SetBool("Walk_FL_Start", false);
                this.myAnimator.SetBool("Walk_BR_Start", false);
                this.myAnimator.SetBool("Walk_BL_Start", false);
                break;
            case 5:
                for (i = 0; i < 8; i++)
                {
                    if (Direction == i)
                    {
                        this.WalkFlag[Direction] = true;
                    }
                    else
                    {
                        this.WalkFlag[Direction] = false;
                    }
                }

                this.myAnimator.SetBool("Walk_F_Start", false);
                this.myAnimator.SetBool("Walk_B_Start", false);
                this.myAnimator.SetBool("Walk_R_Start", false);
                this.myAnimator.SetBool("Walk_L_Start", false);
                this.myAnimator.SetBool("Walk_FR_Start", false);
                this.myAnimator.SetBool("Walk_FL_Start", true);
                this.myAnimator.SetBool("Walk_BR_Start", false);
                this.myAnimator.SetBool("Walk_BL_Start", false);
                break;
            case 6:
                for (i = 0; i < 8; i++)
                {
                    if (Direction == i)
                    {
                        this.WalkFlag[Direction] = true;
                    }
                    else
                    {
                        this.WalkFlag[Direction] = false;
                    }
                }

                this.myAnimator.SetBool("Walk_F_Start", false);
                this.myAnimator.SetBool("Walk_B_Start", false);
                this.myAnimator.SetBool("Walk_R_Start", false);
                this.myAnimator.SetBool("Walk_L_Start", false);
                this.myAnimator.SetBool("Walk_FR_Start", false);
                this.myAnimator.SetBool("Walk_FL_Start", false);
                this.myAnimator.SetBool("Walk_BR_Start", true);
                this.myAnimator.SetBool("Walk_BL_Start", false);
                break;
            case 7:
                for (i = 0; i < 8; i++)
                {
                    if (Direction == i)
                    {
                        this.WalkFlag[Direction] = true;
                    }
                    else
                    {
                        this.WalkFlag[Direction] = false;
                    }
                }

                this.myAnimator.SetBool("Walk_F_Start", false);
                this.myAnimator.SetBool("Walk_B_Start", false);
                this.myAnimator.SetBool("Walk_R_Start", false);
                this.myAnimator.SetBool("Walk_L_Start", false);
                this.myAnimator.SetBool("Walk_FR_Start", false);
                this.myAnimator.SetBool("Walk_FL_Start", false);
                this.myAnimator.SetBool("Walk_BR_Start", false);
                this.myAnimator.SetBool("Walk_BL_Start", true);
                break;
            default:
                for (i = 0; i < 8; i++) 
                {
                    this.WalkFlag[i] = false;
                }

                this.myAnimator.SetBool("Walk_F_Start", false);
                this.myAnimator.SetBool("Walk_B_Start", false);
                this.myAnimator.SetBool("Walk_R_Start", false);
                this.myAnimator.SetBool("Walk_L_Start", false);
                this.myAnimator.SetBool("Walk_FR_Start", false);
                this.myAnimator.SetBool("Walk_FL_Start", false);
                this.myAnimator.SetBool("Walk_BR_Start", false);
                this.myAnimator.SetBool("Walk_BL_Start", false);
                break;
        }
    }
        
}
