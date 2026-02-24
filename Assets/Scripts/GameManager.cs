using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState   //ゲームの状態
{
    InGame,             //ゲーム中
    GameClear,          //ゲームクリア
    GameOver,           //ゲームオーバー
    GameEnd,            //ゲーム終了
}




public class GameManager : MonoBehaviour
{
    //ゲームの状態
    public static GameState gameState;
    public string nextSceneName;            // 次のシーン名

    //スコア追加
    public static int totalScore;           //合計スコア

    //サウンド関連
    public AudioClip meGameClear;
    public AudioClip meGameOver;
    AudioSource soundPlayer;

    public bool isGameClear = false;
    public bool isGameOver = false;

    public static int currentDoorNumber = 0;
    public static int keys = 1;
    public static Dictionary<string, bool> keyGot;
    public static int arrows = 10;

    void Awake()
    {
        gameState = GameState.InGame;   //インゲームにする
        soundPlayer = GetComponent<AudioSource>();

        if(keyGot == null)
        {
            keyGot = new Dictionary<string, bool>();
        }
        if (!(keyGot.ContainsKey(SceneManager.GetActiveScene().name)))
        {
            keyGot.Add(SceneManager.GetActiveScene().name, false);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(gameState == GameState.GameClear)
        {
            soundPlayer.Stop();
            soundPlayer.PlayOneShot(meGameClear);
            isGameClear = true;
            gameState = GameState.GameEnd;
        }
        else if(gameState == GameState.GameOver)
        {
            soundPlayer.Stop();
            soundPlayer.PlayOneShot(meGameOver);
            isGameOver = true;
            gameState = GameState.GameEnd;
        }
    }

    //リスタート
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //次へ
    public void Next()
    {
        SaveDataManager.SaveGamedata();
        SceneManager.LoadScene(nextSceneName);
    }

    public void GameEnd()
    {
        //UI表示が終わって最後の状態であれば
        if(gameState == GameState.GameEnd)
        {
            //ゲームクリア状態なら
            if (isGameClear) { Next(); }
            //ゲームオーバー状態なら
            else if (isGameOver) { Restart(); }
        }
    }
}
