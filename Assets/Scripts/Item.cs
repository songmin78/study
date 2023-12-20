using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    InventoryManager invenManager;
    SpriteRenderer sr;
    [SerializeField] Sprite sprite;

    private void Start()
    {
        invenManager = InventoryManager.Instance;
        sr = GetComponent<SpriteRenderer>();
        sprite = sr.sprite;
    }

    public void GetItem()
    {
        if (invenManager)
        {
            invenManager.GetItem(sprite);
            Destroy(gameObject);//인벤매니저로 부터 이 아이템을 등록할수 있다면 등록후 삭제
        }
        else
        {
            Debug.Log("아이템창이 가득 찼습니다");
        }
    }

}
