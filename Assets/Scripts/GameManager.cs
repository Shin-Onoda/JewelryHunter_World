using UnityEngine;

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
    void Start()
    {
        gameState = GameState.InGame;   //インゲームにする
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
