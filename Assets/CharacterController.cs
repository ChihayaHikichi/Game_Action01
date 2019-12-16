using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class Constants
{
    public const int LockOnAddDegree = 15;
    public const float DashSpeedMultiple = 2.0f;
    public const float StepSpeedMultiple = 3.5f;
    public const float StepTime = 0.2f;//0.2
    public const float StepRigorTime = 0.2f;
    public const int StaminaConsumptionDash = 3;
    public const int StaminaConsumptionStep = 80;
    public const int StaminaConsumptionAttack1 = 50;
    public const int StaminaConsumptionAttack3 = 60;
    public const int StaminaGain = 1;
    public const float ComboReception = 0.25f;
    

}

public class CharacterController : MonoBehaviour {

    // デバッグ用テキスト表示
    private GameObject DebugText;
    private int debugint = 0;

    // ロックオン対象
    private GameObject LockOnTarget;

    private GameObject Opponent;

    private GameObject MyInput;

    private Rigidbody myRigidbody;

    private Animator myAnimator;

    private GameObject Timer;

    // UI描画
    private GameObject UIController;

    private int i;

    // HP
    public int HPPoint;
    public int HP;
    public int HPMax;
    public int HP_Old;

    // SP
    public int SPPoint;
    public int SP;
    public int SPMax;
    public int SPGain;
    public int SP_Old;

    // スタミナ
    public int StaminaPoint;
    public int Stamina;
    public int StaminaMax;
    public int StaminaGain;
    public int Stamina_Old;

    public bool StaminaShortageFlag = false;

    // ボタンの長押し判定時間
    int NagaosiTime = 30;

    int SightTypeTime = 0;

    // キャラクター数
    static private int CharacterNum = 0;

    // キャラクターID
    private int ID;

    // チーム分け
    public int TeamID;

    // 左クリック
    private bool MouseL = false;
    private bool MouseLDown = false;

    // コンボ
    public bool AttackRigorFlag = false;
    private int Combo1 = 0;
    private float ComboReceptionCount = 0;
    private float Attack1Time = 0.5f;
    private float Attack3Time = 0.5f;
    private bool ComboNextFlag = false;
    //private float AttackTimeCount = 0;


    // 右手のコライダー
    private Collider RHandCollider;
    // 左手のコライダー
    private Collider LHandCollider;
    // 右足のコライダー
    private Collider RFootCollider;


    // 歩いているフラグ（F,B,R,L,FR,FL,BR,BL）
    private bool[] WalkFlag = new bool[8] { false, false, false, false, false, false, false, false };
    //private bool[] WalkFlag = new bool[8];
    private int WASD = 0;
    private int WASD_Old = 0;

    // 移動速度
    private float MoveSpeed;
    private float DashMultiple = 1;
    

    // ダッシュフラグ
    private bool DashFlag = false;
    private bool DashFlag_Old = false;

    // ステップフラグ
    private bool StepFlag = false;
    private int StepWASD;
    private float StepCount = 0;

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

        // タイマーのコンポーネントを取得
        this.Timer = GameObject.Find("Empty_Timer");

        // UI描画のコンポーネントを取得
        this.UIController = GameObject.Find("Empty_UIController");

        // 右手のコライダー
        this.RHandCollider = GameObject.Find("Character1_RightHand").GetComponent<CapsuleCollider>();
        // 左手のコライダー
        this.LHandCollider = GameObject.Find("Character1_LeftHand").GetComponent<CapsuleCollider>();
        // 右足のコライダー
        this.RFootCollider = GameObject.Find("Character1_RightToeBase").GetComponent<CapsuleCollider>();

        //this.LHandCollider.enabled = false;


        // HP
        this.HPPoint = 40;
        this.HP = this.HPPoint * 10;
        this.HPMax = this.HP;
        this.HP_Old = this.HP;


        // SP
        this.SPPoint = 40;
        this.SP = this.SPPoint * 10;
        this.SPMax = this.SP;
        this.SPGain = this.SPPoint;
        this.SP_Old = this.SP;

        // スタミナ
        this.StaminaPoint = 40;
        this.Stamina = this.StaminaPoint * 10;
        this.StaminaMax = this.Stamina;
        this.StaminaGain = this.StaminaPoint * Constants.StaminaGain;
        this.Stamina_Old = this.Stamina;


        // ID取得
        CharacterNum += 1;
        ID = CharacterNum;

        TeamID = ID;


        if (TeamID == 1)
        {
            this.Opponent = GameObject.Find("Character2");
        }
        else
        {
            this.Opponent = GameObject.Find("Character1");
        }


        //Debug.Log(ID);

        /*
        for (i = 0; i < 8; i++) 
        {
            WalkFlag[i] = false;
        }
        */


        // 移動速度
        MoveSpeed = 12000.0f;

        // 向き初期化
        HRotation = this.transform.localEulerAngles.y;
        VRotation = this.transform.localEulerAngles.x;

        SightType = false;


        




    }

    // Update is called once per frame
    void Update() {

        // 自分の操作を代入
        if (this.ID == 1)
        {
            // 左クリックを取得
            this.MouseL = MyInput.GetComponent<MyInput>().MouseL;
            this.MouseLDown = MyInput.GetComponent<MyInput>().MouseLDown;

            // WASDキーの入力を取得
            this.WASD = MyInput.GetComponent<MyInput>().WASD;

            // 視点タイプ切り替えを取得
            this.SightType = MyInput.GetComponent<MyInput>().SightType;

            // ダッシュフラグを取得

            this.DashFlag = MyInput.GetComponent<MyInput>().DashFlag;

            // ステップフラグを取得
            this.StepFlag = MyInput.GetComponent<MyInput>().StepFlag;



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




        }

        if (this.ID == 2)
        {
            debugint++;
            if (debugint > 30)
            {
                this.WASD = Random.Range(1, 9);
                debugint = 0;
            }

            //this.MouseLDown = true;
        }








        // 全キャラクターID共通




        // スタミナ自動回復
        if (this.Timer.GetComponent<Timer>().TimeCountOneSecoundGet() >= 1.0f)
        {
            if (this.Stamina < this.StaminaMax)
            {
                this.Stamina += this.StaminaGain;
                if (this.Stamina > this.StaminaMax)
                {
                    this.Stamina = this.StaminaMax;
                }
            }
        }

        // ダッシュでスタミナ消費
        if (this.DashFlag == true)
        {

            if (this.Stamina >= Constants.StaminaConsumptionDash)
            {
                if (this.Timer.GetComponent<Timer>().TimeCountZeroPOneSecoundGet() >= 0.1f)
                {
                    if (this.StepCount == 0 && this.AttackRigorFlag == false)
                    {
                        this.Stamina -= Constants.StaminaConsumptionDash;
                    }
                }
            }
            else
            {
                this.DashFlag = false;
                //this.myAnimator.SetBool("Dash_Start", false);
            }
        }

        // ダッシュフラグ
        // ダッシュフラグの切り替わり
        if (this.DashFlag_Old == false && this.DashFlag == true)
        {
            //this.MoveSpeed *= Constants.DashSpeedMultiple;
            this.DashMultiple = Constants.DashSpeedMultiple;
            this.myAnimator.SetBool("Dash_Start", true);

            if (WalkFlag[0] == true)
            {
                this.WalkFlagManager(-1);
            }
        }
        if (this.DashFlag_Old == true && this.DashFlag == false)
        {
            //this.MoveSpeed /= Constants.DashSpeedMultiple;
            this.DashMultiple = 1;
            this.myAnimator.SetBool("Dash_Start", false);

            if (this.WASD > 0)
            {
                this.myAnimator.SetBool("Walk_F_Start", true);
                //Debug.Log("OK");
            }
        }


        // ステップフラグ
        if (this.StepCount >= Constants.StepTime + Constants.StepRigorTime)
        {
            this.StepCount = 0;

            if (this.DashFlag == true)
            {
                this.myAnimator.SetBool("Dash_Start", true);
            }

            //WalkFlagManager(0);
        }
        if (this.StepCount > 0)
        {
            this.StepCount += this.Timer.GetComponent<Timer>().OneFlameTimeReturn();
        }
        if (this.StepFlag == true && this.StepCount == 0)
        {
            if (this.Stamina >= Constants.StaminaConsumptionStep && this.AttackRigorFlag == false)
            {
                // ステップ開始
                this.StepCount += this.Timer.GetComponent<Timer>().OneFlameTimeReturn();
                // スタミナ消費
                this.Stamina -= Constants.StaminaConsumptionStep;
                // 方向取得
                this.StepWASD = this.WASD;

                WalkFlagManager(-1);
                this.myAnimator.SetBool("Dash_Start", false);
            }
        }


        // キャラクターを指定角度に回転させる
        if (this.SightType == true && this.LockOnFlag == false)
        {
            if (this.StepCount == 0)
            {
                this.transform.rotation = Quaternion.Euler(0, HRotation, 0);
            }
        }

        // ロックオン対象を取得
        // 今回は敵が一体しかいないので省略する。
        this.LockOnTarget = GameObject.Find("Character2");

        // キャラクターをロックオン方向に向かす
        if (this.LockOnFlag == true)
        {


            this.transform.LookAt(LockOnTarget.transform.position);

            // H・VRotateを同期させる
            this.VRotation = this.transform.localEulerAngles.x + Constants.LockOnAddDegree;
            if (this.StepCount == 0)
            {
                this.HRotation = this.transform.localEulerAngles.y;
            }
            else
            {
                //Vector2(LockOnTarget.transform.position.x, LockOnTarget.transform.position.z)- Vector2(this.transform.position.x, this.transform.position.z)
                this.HRotation = 90 - Mathf.Atan2(LockOnTarget.transform.position.z - this.transform.position.z, LockOnTarget.transform.position.x - this.transform.position.x) / Mathf.PI * 180;


                //this.HRotation = this.transform.localEulerAngles.y;
            }
            //Debug.Log(this.transform.localEulerAngles.x);
        }



        // 基本コンボ
        if (this.ComboReceptionCount > 0)
        {
            this.ComboReceptionCount += this.Timer.GetComponent<Timer>().OneFlameTimeReturn();
        }
        if (this.StepCount == 0)
        {
            if ((this.MouseLDown == true && this.Combo1 == 0) && this.Stamina >= Constants.StaminaConsumptionAttack1)
            {
                this.myAnimator.SetBool("Attack1_Start", true);
                this.Combo1 = 1;
                this.ComboReceptionCount = this.Timer.GetComponent<Timer>().OneFlameTimeReturn();
                this.Stamina -= Constants.StaminaConsumptionAttack1;
                this.myRigidbody.AddForce(this.transform.forward * 8.0f, ForceMode.Impulse);
                this.AttackRigorFlag = true;
                //this.RHandCollider.enabled = true;
            }
            if (this.MouseLDown == true && (this.ComboReceptionCount >= this.Attack1Time - Constants.ComboReception && this.ComboReceptionCount <= this.Attack1Time + Constants.ComboReception))
            {
                this.ComboNextFlag = true;
            }
        }
        if(this.ComboReceptionCount >= this.Attack1Time && this.Combo1 == 1)
        {
            this.myAnimator.SetBool("Attack1_Start", false);
            this.AttackRigorFlag = false;
            //this.RHandCollider.enabled = false;
        }
        if (this.ComboReceptionCount >= this.Attack1Time + Constants.ComboReception && this.Combo1 == 1)
        {
            this.Combo1 = 0;
            this.ComboReceptionCount = 0;
        }
        if (this.StepCount == 0)
        {
            if ((this.ComboReceptionCount >= this.Attack1Time && this.Combo1 == 1) && this.ComboNextFlag == true && this.Stamina >= Constants.StaminaConsumptionAttack1)
            {
                
                this.myAnimator.SetBool("Attack2_Start", true);
                this.Combo1 = 2;
                this.ComboReceptionCount = this.Timer.GetComponent<Timer>().OneFlameTimeReturn();
                this.ComboNextFlag = false;
                this.Stamina -= Constants.StaminaConsumptionAttack1;
                this.myRigidbody.AddForce(this.transform.forward * 16.0f, ForceMode.Impulse);
                this.AttackRigorFlag = true;
            }
            if (this.MouseLDown == true && (this.ComboReceptionCount >= this.Attack1Time - Constants.ComboReception && this.ComboReceptionCount <= this.Attack1Time + Constants.ComboReception))
            {
                this.ComboNextFlag = true;
            }
        }
        if (this.ComboReceptionCount >= this.Attack1Time && this.Combo1 == 2)
        {
            this.myAnimator.SetBool("Attack2_Start", false);
            this.AttackRigorFlag = false;
        }
        if (this.ComboReceptionCount >= this.Attack1Time + Constants.ComboReception && this.Combo1 == 2)
        {
            this.Combo1 = 0;
            this.ComboReceptionCount = 0;
        }
        if (this.StepCount == 0)
        {
            if ((this.ComboReceptionCount >= this.Attack1Time && this.Combo1 == 2) && this.ComboNextFlag == true && this.Stamina >= Constants.StaminaConsumptionAttack3)
            {
                //this.myAnimator.SetBool("Attack2_Start", false);
                this.myAnimator.SetBool("Attack3_Start", true);
                this.Combo1 = 3;
                this.ComboReceptionCount = this.Timer.GetComponent<Timer>().OneFlameTimeReturn();
                this.ComboNextFlag = false;
                this.Stamina -= Constants.StaminaConsumptionAttack3;
                this.myRigidbody.AddForce(this.transform.forward * 10.0f, ForceMode.Impulse);
                this.AttackRigorFlag = true;
            }
        }
        if (this.ComboReceptionCount >= this.Attack3Time && this.Combo1 == 3)
        {
            this.myAnimator.SetBool("Attack3_Start", false);
            this.AttackRigorFlag = false;
            this.Combo1 = 0;
            this.ComboReceptionCount = 0;
        }


        if (this.AttackRigorFlag == true)
        {
            WalkFlagManager(-1);
            this.myAnimator.SetBool("Dash_Start", false);
        }
        else
        {
            if (this.DashFlag == true)
            {
                this.myAnimator.SetBool("Dash_Start", true);
            }
        }




        // 移動
        if (this.AttackRigorFlag == false)
        {
            if (this.StepCount == 0)
            //if (this.StepCount == 0 && this.AttackRigorFlag == false)
            {



                if (this.SightType == true || this.LockOnFlag == true)
                {
                    switch (this.WASD)
                    {
                        case 1:
                            // 前
                            this.myRigidbody.AddForce(this.transform.forward * MoveSpeed * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.WalkFlag[0] == false)
                            {
                                WalkFlagManager(0);
                            }
                            break;
                        case 2:
                            // 後ろ
                            this.myRigidbody.AddForce(this.transform.forward * (-1) * MoveSpeed * this.Timer.GetComponent<Timer>().OneFlameTimeReturn() * 0.7f);

                            // アニメーション
                            if (this.WalkFlag[1] == false)
                            {
                                WalkFlagManager(1);
                            }
                            break;
                        case 3:
                            // 右
                            this.myRigidbody.AddForce(this.transform.right * MoveSpeed * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.WalkFlag[2] == false)
                            {
                                WalkFlagManager(2);
                            }
                            break;
                        case 4:
                            // 左
                            this.myRigidbody.AddForce(this.transform.right * (-1) * MoveSpeed * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.WalkFlag[3] == false)
                            {
                                WalkFlagManager(3);
                            }
                            break;
                        case 5:
                            // 右前
                            this.myRigidbody.AddForce((this.transform.right + this.transform.forward) / Mathf.Sqrt(2) * MoveSpeed * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.WalkFlag[4] == false)
                            {
                                WalkFlagManager(4);
                            }
                            break;
                        case 6:
                            // 左前
                            this.myRigidbody.AddForce((this.transform.right * (-1) + this.transform.forward * 1) / Mathf.Sqrt(2) * MoveSpeed * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.WalkFlag[5] == false)
                            {
                                WalkFlagManager(5);
                            }
                            break;
                        case 7:
                            // 右後ろ
                            this.myRigidbody.AddForce((this.transform.right + this.transform.forward * (-1)) / Mathf.Sqrt(2) * MoveSpeed * this.Timer.GetComponent<Timer>().OneFlameTimeReturn() * 0.7f);

                            // アニメーション
                            if (this.WalkFlag[6] == false)
                            {
                                WalkFlagManager(6);
                            }
                            break;
                        case 8:
                            // 左後ろ
                            this.myRigidbody.AddForce((this.transform.right * (-1) + this.transform.forward * (-1)) / Mathf.Sqrt(2) * MoveSpeed * this.Timer.GetComponent<Timer>().OneFlameTimeReturn() * 0.7f);

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
                        this.myRigidbody.AddForce(this.transform.forward * this.MoveSpeed * this.DashMultiple * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                        // アニメーション
                        if (this.WalkFlag[0] == false)
                        {

                            WalkFlagManager(0);
                            //WalkFlag[0] = true;
                            //Debug.Log("OK");

                        }
                    }
                }
            }
            else if (this.StepCount <= Constants.StepTime)
            {
                // ステップ
                if (this.SightType == true || this.LockOnFlag == true)
                {
                    switch (this.StepWASD)
                    {
                        case 1:
                            // 前
                            this.myRigidbody.AddForce(this.transform.forward * MoveSpeed * Constants.StepSpeedMultiple * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.StepCount == this.Timer.GetComponent<Timer>().OneFlameTimeReturn())
                            {
                                this.myAnimator.SetTrigger("Step_F_Start");
                            }
                            break;
                        case 2:
                            // 後ろ
                            this.myRigidbody.AddForce(this.transform.forward * (-1) * MoveSpeed * Constants.StepSpeedMultiple * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.StepCount == this.Timer.GetComponent<Timer>().OneFlameTimeReturn())
                            {
                                this.myAnimator.SetTrigger("Step_B_Start");
                            }
                            break;
                        case 3:
                            // 右
                            this.myRigidbody.AddForce(this.transform.right * MoveSpeed * Constants.StepSpeedMultiple * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.StepCount == this.Timer.GetComponent<Timer>().OneFlameTimeReturn())
                            {
                                this.myAnimator.SetTrigger("Step_R_Start");
                            }
                            break;
                        case 4:
                            // 左
                            this.myRigidbody.AddForce(this.transform.right * (-1) * MoveSpeed * Constants.StepSpeedMultiple * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.StepCount == this.Timer.GetComponent<Timer>().OneFlameTimeReturn())
                            {
                                this.myAnimator.SetTrigger("Step_L_Start");
                            }
                            break;
                        case 5:
                            // 右前
                            this.myRigidbody.AddForce((this.transform.right + this.transform.forward) / Mathf.Sqrt(2) * MoveSpeed * Constants.StepSpeedMultiple * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.StepCount == this.Timer.GetComponent<Timer>().OneFlameTimeReturn())
                            {
                                this.myAnimator.SetTrigger("Step_FR_Start");
                            }
                            break;
                        case 6:
                            // 左前
                            this.myRigidbody.AddForce((this.transform.right * (-1) + this.transform.forward * 1) / Mathf.Sqrt(2) * MoveSpeed * Constants.StepSpeedMultiple * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.StepCount == this.Timer.GetComponent<Timer>().OneFlameTimeReturn())
                            {
                                this.myAnimator.SetTrigger("Step_FL_Start");
                            }
                            break;
                        case 7:
                            // 右後ろ
                            this.myRigidbody.AddForce((this.transform.right + this.transform.forward * (-1)) / Mathf.Sqrt(2) * MoveSpeed * Constants.StepSpeedMultiple * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.StepCount == this.Timer.GetComponent<Timer>().OneFlameTimeReturn())
                            {
                                this.myAnimator.SetTrigger("Step_BR_Start");
                            }
                            break;
                        case 8:
                            // 左後ろ
                            this.myRigidbody.AddForce((this.transform.right * (-1) + this.transform.forward * (-1)) / Mathf.Sqrt(2) * MoveSpeed * Constants.StepSpeedMultiple * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                            // アニメーション
                            if (this.StepCount == this.Timer.GetComponent<Timer>().OneFlameTimeReturn())
                            {
                                this.myAnimator.SetTrigger("Step_BL_Start");
                            }
                            break;
                    }
                }
                else
                {
                    this.myRigidbody.AddForce(this.transform.forward * this.MoveSpeed * Constants.StepSpeedMultiple * this.Timer.GetComponent<Timer>().OneFlameTimeReturn());

                    // アニメーション
                    if (this.StepCount == this.Timer.GetComponent<Timer>().OneFlameTimeReturn())
                    {
                        this.myAnimator.SetTrigger("Step_F_Start");

                    }

                }
            }
        }

        // 移動キーが押されていない
        if (this.WASD == 0 && this.WASD_Old > 0)
        {
            WalkFlagManager(-1);

            
        }



        if (this.ID == 1)
        {
            this.UIController.GetComponent<UIController>().DrawUI();

            
        }


        // 1フレーム前の情報を更新
        this.DashFlag_Old = this.DashFlag;
        this.HP_Old = this.HP;
        this.SP_Old = this.SP;
        this.Stamina_Old = this.Stamina;
        this.WASD_Old = this.WASD;





        // デバッグ
        if (this.ID == 1)
        {
            //this.DebugText.GetComponent<Text>().text = "" + this.WalkFlag[0];
            //this.DebugText.GetComponent<Text>().text = "" + myRigidbody.velocity.magnitude;
            // デバッグ
            if (Input.GetKey(KeyCode.Q) == true)
            {
                //Debug.Log("OK");
                //WalkFlag = false;
                //this.myAnimator.SetBool("Walk_F_Start", false);
                //this.DebugText.GetComponent<Text>().text = "F :" + this.WalkFlag[0] + "\nB :" + this.WalkFlag[1] + "\nR :" + this.WalkFlag[2] + "\nL :" + this.WalkFlag[3];
                //this.DebugText.GetComponent<Text>().text = "" + myRigidbody.velocity.magnitude;
                this.DebugText.GetComponent<Text>().text = "" + Opponent.GetComponent<CharacterController>().HP;
                //this.myAnimator.SetBool("Dash_Start", true);

                //this.Stamina = this.StaminaMax;

            }
            if (Input.GetKeyDown(KeyCode.P) == true)
            {
                //this.myRigidbody.AddForce(this.transform.forward * 100, ForceMode.Impulse);
            }

        }
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
                        this.WalkFlag[i] = true;
                        //Debug.Log("OK" + i);
                    }
                    else
                    {
                        this.WalkFlag[i] = false;
                        //Debug.Log(i +""+""+ this.WalkFlag[Direction]);
                        //Debug.Log("OK" + i);

                    }
                }
                
                if (this.DashFlag == false)
                {
                    this.myAnimator.SetBool("Walk_F_Start", true);
                }
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
                        this.WalkFlag[i] = true;
                    }
                    else
                    {
                        this.WalkFlag[i] = false;
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
                        this.WalkFlag[i] = true;
                    }
                    else
                    {
                        this.WalkFlag[i] = false;
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
                        this.WalkFlag[i] = true;
                    }
                    else
                    {
                        this.WalkFlag[i] = false;
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
                        this.WalkFlag[i] = true;
                    }
                    else
                    {
                        this.WalkFlag[i] = false;
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
                        this.WalkFlag[i] = true;
                    }
                    else
                    {
                        this.WalkFlag[i] = false;
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
                        this.WalkFlag[i] = true;
                    }
                    else
                    {
                        this.WalkFlag[i] = false;
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
                        this.WalkFlag[i] = true;
                    }
                    else
                    {
                        this.WalkFlag[i] = false;
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

    public void AddDamage(float Damage)
    {
        // ダメージ計算（要改善）


        
        if (Opponent.GetComponent<CharacterController>().AttackRigorFlag == true)
        {
            this.HP -= (int)Damage;

            if (this.HP < 0)
            {
                this.HP = 0;
            }
        }
       

        
    }
        
}
