using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInput : MonoBehaviour {


    //public bool[] WASD = new bool[8] { false, false, false, false, false, false, false, false };

    private int i;

    


    // 前後左右移動入力
    public int WASD = 0;

    // ボタンの長押し判定時間
    int NagaosiTime = 30;

    // マウス3ボタン
    public int Mouse3Time = 0;

    // 視点タイプ
    public bool SightType = false;

    // ロックオンフラグ
    public bool LockOnFlag = false;

    // マウス移動量
    public float HRotation = 0;
    public float VRotation = 0;

    // ダッシュ判定
    private int[] DashTime = new int[4] { 0, 0, 0, 0 };
    public bool DashFlag = false;

    // ステップ判定
    public bool StepFlag = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        



        // WASD
        if ((Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == true) && (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == true))
        {
            // 右後ろ
            this.WASD = 7;
        }
        else if ((Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == true) && Input.GetKey(KeyCode.A) == true)
        {
            // 左後ろ
            this.WASD = 8;
        }
        else if ((Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == true) && (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false))
        {
            // 後ろ
            this.WASD = 2;
        }
        else if (Input.GetKey(KeyCode.W) == true && (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == true))
        {
            // 右前
            this.WASD = 5;
        }
        else if (Input.GetKey(KeyCode.W) == true && Input.GetKey(KeyCode.A) == true)
        {
            // 左前
            this.WASD = 6;
        }
        else if (Input.GetKey(KeyCode.W) == true && (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false))
        {
            // 前
            this.WASD = 1;
        }
        else if ((Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false) && (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == true))
        {
            // 右
            this.WASD = 3;
        }
        else if ((Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false) && Input.GetKey(KeyCode.A) == true)
        {
            // 左
            this.WASD = 4;
        }

        if ((Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false) && (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false))
        {
            this.WASD = 0;
        }


        // マウス3
        // 視点タイプ切り替え
        if (Input.GetMouseButton(2) == true && this.Mouse3Time < NagaosiTime)
        {
            this.Mouse3Time++;
        }
        if (Input.GetMouseButton(2) == false && this.Mouse3Time > 0)
        {
            if (this.Mouse3Time < NagaosiTime)
            {
                if (this.LockOnFlag == false)
                {
                    this.LockOnFlag = true;

                    // ダッシュを止める
                    DashFlag = false;
                }
                else
                {
                    this.LockOnFlag = false;
                }
            }

            this.Mouse3Time = 0;
            
        }

        if (this.Mouse3Time == NagaosiTime)
        {
            if (this.LockOnFlag == false)
            {
                if (this.SightType == false)
                {
                    this.SightType = true;

                    // ダッシュを止める
                    DashFlag = false;
                }
                else
                {
                    this.SightType = false;
                }
            }
            //this.Mouse3Time = 0;
            this.Mouse3Time++;
        }

        // マウス移動量
        this.HRotation += Input.GetAxis("Mouse X");
        this.VRotation -= Input.GetAxis("Mouse Y");

        // HRotationが一周したら360引く
        if (this.HRotation > 360)
        {
            this.HRotation -= 360;
        }
        if (this.HRotation < -360)
        {
            this.HRotation += 360;
        }

        // VRotationに制限を設ける
        if (this.VRotation > 90)
        {
            this.VRotation = 90;
        }
        if (this.VRotation < -60)
        {
            this.VRotation = -60;
        }

        // ステップ開始
        if (this.StepFlag == true)
        {
            this.StepFlag = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) == true && this.WASD > 0)
        {
            this.StepFlag = true;
        }
        







        // ダッシュ判定 ----------------------------------------------------------------------------------------------------------- //
        // ダッシュ処理へ
        if ((DashTime[0] > 0 && DashTime[0] < NagaosiTime) && Input.GetKeyDown(KeyCode.W) == true)
        {
            DashTime[0] = 0;

            if (SightType == false && LockOnFlag == false)
            {
                DashFlag = true;
            }
            
        }
        if ((DashTime[1] > 0 && DashTime[1] < NagaosiTime) && Input.GetKeyDown(KeyCode.S) == true)
        {
            DashTime[1] = 0;

            if (SightType == false && LockOnFlag == false)
            {
                DashFlag = true;
            }

        }
        if ((DashTime[2] > 0 && DashTime[2] < NagaosiTime) && Input.GetKeyDown(KeyCode.D) == true)
        {
            DashTime[2] = 0;

            if (SightType == false && LockOnFlag == false)
            {
                DashFlag = true;
            }

        }
        if ((DashTime[3] > 0 && DashTime[3] < NagaosiTime) && Input.GetKeyDown(KeyCode.A) == true)
        {
            DashTime[3] = 0;

            if (SightType == false && LockOnFlag == false)
            {
                DashFlag = true;
            }

        }

        for (i = 0; i < 4; i++)
        {
            // 受付時間計算
            if (DashTime[i] > 0)
            {
                DashTime[i]++;
            }

            // ダッシュ判定受付終了
            if (DashTime[i] >= NagaosiTime)
            {
                DashTime[i] = 0;
            }
        }

        // ダッシュ判定受付開始
        if (Input.GetKeyDown(KeyCode.W) == true && this.DashFlag == false)
        {
            DashTime[0]++;
        }
        if (Input.GetKeyDown(KeyCode.S) == true && this.DashFlag == false)
        {
            DashTime[1]++;
        }
        if (Input.GetKeyDown(KeyCode.D) == true && this.DashFlag == false)
        {
            DashTime[2]++;
        }
        if (Input.GetKeyDown(KeyCode.A) == true && this.DashFlag == false)
        {
            DashTime[3]++;
        }

        // ダッシュの終了
        if (DashFlag == true)
        {
            if(this.WASD == 0)
            {
                DashFlag = false;
            }
        }
        // ------------------------------------------------------------------------------------------------------------------------ //

        

    }
}
