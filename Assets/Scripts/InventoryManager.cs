using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("손데지 말것!")]
    [SerializeField, Tooltip("오브젝트를 껐다 켰다 할때 사용하는 오브젝트")] GameObject objInventory;//오브젝트 본체
    [SerializeField, Tooltip("프리팹 오브젝트")] GameObject objItem;// 인벤토리에 생성될 프리팹 오브젝트릐 오리지널
    [SerializeField] Transform trsInven;//인벤토리 초기화에 사용할 인벤토리들의 위치데이터

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

    private void initInven()//init 초기화
    {
        listInven.Clear();
        listInven.AddRange(trsInven.GetComponentsInChildren<Transform>());
        listInven.RemoveAt(0);//코드를 실행한 이 오브잭트의 Transform도 가져오기때문에 0번을 삭제//(0번은 이 오브젝트의 Transform)
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

                //Time.unscaledDeltaTime <= 플레이어는 시간에 간섭을 받지 않는다
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
            if(slot.childCount == 0)//위치에 자식이 없다면 비었있음
            {
                return iNum;
            }
        }
        return -1;
    }

    public bool GetItem(Sprite _spr)
    {
        int slotNum = getEmptyItemSlot();//비어있는, 곧 채울 아이템 슬롯의 번호
        if (slotNum == -1)//아이템을 더 넣을수 없음
        {
            return false;
        }

        InvenDragUi ui = Instantiate(objItem, listInven[slotNum]).GetComponent<InvenDragUi>();
        //ui스크립트에서 이 아이템 정보를 등록 시킬 예정
        ui.SetItem(_spr);

        return true;
    }

}
