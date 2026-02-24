using UnityEngine;
using UnityEngine.SceneManagement;

public class Advent_ItemBox : MonoBehaviour
{
    public Sprite openImage;        //開いた時の画像
    public GameObject itemPrefab;   //宝箱の中に格納するオブジェクト
    public bool isClosed = true;    //閉じているかのフラグ
    public AdventItemType Type = AdventItemType.None;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Type == AdventItemType.Key)
        {
            if (GameManager.keyGot[SceneManager.GetActiveScene().name])
            {
                //Close状態をfalse
                isClosed = false;
                //見た目をOpenの絵にする
                GetComponent<SpriteRenderer>().sprite = openImage;
            }
        }

            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(isClosed && collision.gameObject.tag == "Player")
        {
            //宝箱の絵をOpenの絵に変更
            GetComponent<SpriteRenderer>().sprite = openImage;

            //closeのフラグを解除
            isClosed = false;

            //その場に変数に指定したプレハブオブジェクトを生成
            //もし変数にプレハブオブジェクトが指定されていれば
            if(itemPrefab != null)
            {
                Instantiate(
                    itemPrefab,
                    transform.position,
                    Quaternion.identity);
            }

        }
    }
}
