using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
// Start is called once before the first execution of Update after the MonoBehaviour is created
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;              //Rigidbody2D型の変数
    float axisH = 0.0f;             //入力
    public float speed = 3.0f;      //移動スピード用変数
    public float jump = 9.0f;       //ジャンプ力用変数
    public LayerMask groundLayer;   //着地できるレイヤー
    bool goJump = false;            //ジャンプ開始フラグ
    bool onGround = false;          //地面フラグ

    // アニメーション対応
    Animator animator; // アニメーター

    //値はあくまでアニメーションクリップ名
    public string stopAnime = "Idle";
    public string moveAnime = "Run";
    public string jumpAnime = "Jump";
    public string goalAnime = "Goal";
    public string deadAnime = "Dead";
    string nowAnime = "";
    string oldAnime = "";

    public int score = 0; //スコア

    InputAction moveAction; //Moveアクション
    InputAction jumpAction; //Jumpアクション
    PlayerInput input;      //PlayerInputコンポーネント

    GameManager gm;

    public static int playerLife = 10;
    bool inDamage;          //ダメージ管理フラグ

    public float shootSpeed = 12.0f;
    public float shootDelay = 0.25f;
    public GameObject arrowPrefab;
    public GameObject gate;

    public static void PlayerRecovery(int life)
    {
        playerLife += life;
        if (playerLife > 10) playerLife = 10;
    }

    void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();
        axisH = moveInput.x;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            goJump = true;
        }
    }

    //UI表示にSubmitボタンが押されたら
    void OnSubmit(InputValue value)
    {
        if (GameManager.gameState != GameState.InGame)
        {
            gm.GameEnd();
        }
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.arrows > 0)
        {
            ShootArrow();
        }
    }

    void ShootArrow()
    {
        SoundManager.currentSoundManager.PlaySE(SEType.Shoot);

        GameManager.arrows--;
        Quaternion r;
        if(transform.localScale.x > 0)
        {
            r = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            r = Quaternion.Euler(0, 0, 180);
        }

        GameObject arrowObj = Instantiate(
            arrowPrefab,
            gate.transform.position,
            r);
        Rigidbody2D arrowRbody =
            arrowObj.GetComponent<Rigidbody2D>();
        arrowRbody.AddForce(new Vector2
            (transform.localScale.x, 0) * shootSpeed,
            ForceMode2D.Impulse);

    }
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();   //Rigidbody2Dを取ってくる
        animator = GetComponent<Animator>();        // Animator を取ってくる
        nowAnime = stopAnime;                       // 停止から開始する
        oldAnime = stopAnime;                       // 停止から開始する

        input = GetComponent<PlayerInput>();
        moveAction = input.currentActionMap.FindAction("Move");
        jumpAction = input.currentActionMap.FindAction("Jump");
        InputActionMap uiMap = input.actions.FindActionMap("UI");
        uiMap.Disable();

        gm = GameObject.FindFirstObjectByType<GameManager>();

        playerLife = 10;    //体力を初期値にリセット
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameState != GameState.InGame || inDamage)
        {
            //もしダメージ管理フラグが立っていたら点滅処理
            if (inDamage)
            {
                float val = Mathf.Sin(Time.time * 50);
                if(val > 0)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            return;
        }

        // 地上判定
        onGround = Physics2D.CircleCast(transform.position,    // 発射位置
                                        0.2f,                  // 円の半径
                                        Vector2.down,          // 発射方向
                                        0.0f,                  // 発射距離
                                        groundLayer);          // 検出するレイヤー
        //if (Input.GetButtonDown("Jump"))                         //キャラクターをジャンプさせる
        if(jumpAction.WasPressedThisFrame())
        {
            goJump = true;  //ジャンプフラグを立てる
        }

        //水平方向の入力をチェックする
        //axisH = Input.GetAxisRaw("Horizontal");
        axisH = moveAction.ReadValue<Vector2>().x;

        if (axisH > 0.0f)                           // 向きの調整
        {
            //Debug.Log("右移動");
            transform.localScale = new Vector2(1, 1);   // 右移動
        }
        else if (axisH < 0.0f)
        {
            //Debug.Log("左移動");
            transform.localScale = new Vector2(-1, 1); // 左右反転させる
        }

        // アニメーション更新
        if (onGround)       // 地面の上
        {
            if (axisH == 0)
            {
                nowAnime = stopAnime; // 停止中
            }
            else
            {
                nowAnime = moveAnime; // 移動
            }
        }
        else                // 空中
        {
            nowAnime = jumpAnime;
        }
        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); // アニメーション再生
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.gameState != GameState.InGame || inDamage)
        {
            return; //Updateを中断
        }

        if (onGround || axisH != 0) //地面の上 or 速度が0ではない
        {
            //速度を更新する
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
        }
        if (onGround && goJump)      //地面の上でジャンプキーが押された
        {
            //ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;
        }
        //速度を更新する
        rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
    }

    //接触開始
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Goal")
        {
            Goal();
        }
        else if(collision.gameObject.tag == "Dead")
        {
            GameOver();
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            SoundManager.currentSoundManager.PlaySE(SEType.ItemGet);
            // スコアアイテム
            ScoreItem item = collision.gameObject.GetComponent<ScoreItem>();  // ScoreItemを得る			
            score = item.itemdata.value;                // スコアを得る
            UIController ui = Object.FindFirstObjectByType<UIController>();      // UIControllerを探す
            if (ui != null)
            {
                ui.UpdateScore(score);                  // スコア表示を更新する
            }
            score = 0; //次に備えてスコアをリセット
            Destroy(collision.gameObject);              // アイテム削除する
        }
        else if(collision.gameObject.tag == "Enemy")
        {
            if (!inDamage)  //ダメージ中でなければ
            {
                GetDamage(collision.gameObject);
            }
        }
    }
    //ゴール
    public void Goal()
    {
        animator.Play(goalAnime);
        GameManager.gameState = GameState.GameClear;
        GameStop();
    }
    //ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);
        GameManager.gameState = GameState.GameOver;
        GameStop();

        //ゲームオーバー演出
        GetComponent<CapsuleCollider2D>().enabled = false;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

        Destroy(gameObject, 2.0f);      //2秒後にヒエラルキーからオブジェクトを抹消
    }
    //ゲーム停止
    void GameStop()
    {
        rbody.linearVelocity = new Vector2(0, 0);   //ゲーム停止時速度を0にする

        input.currentActionMap.Disable();
        input.SwitchCurrentActionMap("UI");
        input.currentActionMap.Enable();
    }

    //プレイヤーのaxisH()の値を取得
    public float GetAxisH()
    {
        return axisH;
    }

    void GetDamage(GameObject target)
    {
        if (GameManager.gameState == GameState.InGame)
        {
            SoundManager.currentSoundManager.PlaySE(SEType.GetDamage);

            playerLife -= 1;
            if (playerLife > 0)
            {
                rbody.linearVelocity = new Vector2(0, 0);
                Vector3 v = (transform.position - target.transform.position).normalized;
                rbody.AddForce(new Vector2(v.x * 4, v.y * 4), ForceMode2D.Impulse);
                inDamage = true;
                Invoke("DamageEnd", 0.25f);
            }
            else
            {
                GameOver();
            }
        }
    }
    void DamageEnd()
    {
        inDamage = false;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
