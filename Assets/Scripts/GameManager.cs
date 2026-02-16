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

    void Start()
    {
        gameState = GameState.InGame;   //インゲームにする
        soundPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(gameState == GameState.GameClear)
        {
            soundPlayer.Stop();
            soundPlayer.PlayOneShot(meGameClear);
            gameState = GameState.GameEnd;
        }
        else if(gameState == GameState.GameOver)
        {
            soundPlayer.Stop();
            soundPlayer.PlayOneShot(meGameOver);
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
        SceneManager.LoadScene(nextSceneName);
    }
}
