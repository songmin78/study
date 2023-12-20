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
            Destroy(gameObject);//�κ��Ŵ����� ���� �� �������� ����Ҽ� �ִٸ� ����� ����
        }
        else
        {
            Debug.Log("������â�� ���� á���ϴ�");
        }
    }

}
