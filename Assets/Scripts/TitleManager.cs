using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public string sceneName;

    public GameObject startButton;      //スタートボタンオブジェクト
    public GameObject continueButton;   //コンテニューボタンオブジェクト

    //public InputAction submitAction;

    //void OnEnable()
    //{
    //    submitAction.Enable();
    //}
    //void OnDisable()
    //{
    //    submitAction.Disable();
    //}
    void Start()
    {
        string jsonData = PlayerPrefs.GetString("SaveData");

        if(string.IsNullOrEmpty(jsonData))
        {
            continueButton.GetComponent<Button>().interactable = false;
        }
    }

    void OnSubmit(InputValue valuse)
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        //if (submitAction.WasPressedThisFrame())
        //{
        //    Load();
        //}
        /*Keyboard kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.enterKey.wasPressedThisFrame)
            {
                Load();
            }
        }*/
    }

    public void Load()
    {
        SaveDataManager.Initialize();
        //GameManager.totalScore = 0; //新しくゲームを始めるにあたってスコアをリセット
        SceneManager.LoadScene(sceneName);

    }

    public void ContinueLoad()
    {
        SaveDataManager.LoadGameData();
        SceneManager.LoadScene(sceneName);
    }
}
