using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("�յ��� ����!")]
    [SerializeField, Tooltip("������Ʈ�� ���� �״� �Ҷ� ����ϴ� ������Ʈ")] GameObject objInventory;//������Ʈ ��ü
    [SerializeField, Tooltip("������ ������Ʈ")] GameObject objItem;// �κ��丮�� ������ ������ ������Ʈ�l ��������
    [SerializeField] Transform trsInven;//�κ��丮 �ʱ�ȭ�� ����� �κ��丮���� ��ġ������

    private List<Transform> listInven = new List<Transform>();

    [SerializeField] KeyCode openInvenkey;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        initInven();
    }

    private void initInven()//init �ʱ�ȭ
    {
        listInven.Clear();
        listInven.AddRange(trsInven.GetComponentsInChildren<Transform>());
        listInven.RemoveAt(0);//�ڵ带 ������ �� ������Ʈ�� Transform�� �������⶧���� 0���� ����//(0���� �� ������Ʈ�� Transform)
    }

    // Update is called once per frame
    void Update()
    {
        onenInventory();
    }

    private void onenInventory()
    {
        if(Input.GetKeyDown(openInvenkey))
        {
            if(objInventory.activeSelf == true)
            {
                objInventory.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else
            {
                objInventory.SetActive(true);
                Time.timeScale = 0.1f;

                //Time.unscaledDeltaTime <= �÷��̾�� �ð��� ������ ���� �ʴ´�
            }
            //objInventory.SetActive(!objInventory.activeSelf);
        }
    }

    private int getEmptyItemSlot()
    {
        int count = listInven.Count;
        for(int iNum = 0; iNum < count; iNum++)
        {
            Transform slot = listInven[iNum];
            if(slot.childCount == 0)//��ġ�� �ڽ��� ���ٸ� �������
            {
                return iNum;
            }
        }
        return -1;
    }

    public bool GetItem(Sprite _spr)
    {
        int slotNum = getEmptyItemSlot();//����ִ�, �� ä�� ������ ������ ��ȣ
        if (slotNum == -1)//�������� �� ������ ����
        {
            return false;
        }

        InvenDragUi ui = Instantiate(objItem, listInven[slotNum]).GetComponent<InvenDragUi>();
        //ui��ũ��Ʈ���� �� ������ ������ ��� ��ų ����
        ui.SetItem(_spr);

        return true;
    }

}
