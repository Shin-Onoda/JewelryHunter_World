using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    public string sceneName;

    //public InputAction submitAction;

    //void OnEnable()
    //{
    //    submitAction.Enable();
    //}
    //void OnDisable()
    //{
    //    submitAction.Disable();
    //}
    //void Start()
    //{
    //    
    //}

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
        GameManager.totalScore = 0; //新しくゲームを始めるにあたってスコアをリセット
        SceneManager.LoadScene(sceneName);
    }
}
