using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public string sceneName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load()
    {
        GameManager.totalScore = 0; //新しくゲームを始めるにあたってスコアをリセット
        SceneManager.LoadScene(sceneName);
    }
}
