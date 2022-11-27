using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]   // ����ȭ
public class Craft      // ���๰ Ŭ����
{
    public string craftName;              // ���๰ �̸�
    public Sprite craftImage;             // ���๰ �̹���
    public string craftDesc;              // ���๰ ����
    public string[] craftNeedItem;        // ����� �ʿ��� ������
    public int[] craftNeedItemCount;      // ����� �ʿ��� �������� ����
    public GameObject go_Prefab;          // ���� ��ġ�� ������
    public GameObject go_PreviewPrefab;   // �̸����� ������
}

public class CraftManual : MonoBehaviour
{
    // ���º���
    private bool isActiavted = false;           // ���� �޴��� Ȱ��ȭ ����
    private bool isPreviewActivated = false;    // �̸����� Ȱ��ȭ ����

    [SerializeField]
    private GameObject go_BaseUI;           // �⺻ ���̽� UI

    private int tabNumber = 0;              // �� ��ȣ
    private int page = 1;                   // ù��° �� ������ ����
    private int selectedSlotNumber;         // ������ ���� ��ȣ
    private Craft[] craft_SelectedTab;      // ������ ���� �з� ��


    [SerializeField]
    private Craft[] craft_fire;             // ��ںҿ� ��
    [SerializeField]
    private Craft[] craft_build;            // ����� ��

    private GameObject go_Preview;          // �̸����� �������� ���� ����
    private GameObject go_Prefab;           // ���� ������ �������� ���� ����

    [SerializeField]
    private Transform tf_Player;            // �÷��̾� ��ġ�� �޾ƿ� ����

    // Raycast �ʿ� ���� ����
    private RaycastHit hitInfo;         // �������� �´� ��ü ������ �ٷ�� ���� ����
    [SerializeField]
    private LayerMask layerMask;        // �������� ���� ���̾� ����
    [SerializeField]
    private float range;                // ������ �����Ÿ�

    // �ʿ��� UI Slot ���
    [SerializeField]
    private GameObject[] go_Slots;          // ������ ���� ����
    [SerializeField]
    private Image[] image_Slot;             // ���� �̹���
    [SerializeField]
    private Text[] text_SlotName;           // ���� �̸�
    [SerializeField]
    private Text[] text_SlotDesc;           // ���� ����
    [SerializeField]
    private Text[] text_SlotNeedItem;       // ����� �ʿ��� ������ �̸�

    // �ʿ��� ������Ʈ
    private Inventory theInventory;         // ��� ���� ���������� ���� ����


    void Start()
    {
        theInventory = FindObjectOfType<Inventory>();
        tabNumber = 0;
        page = 1;
        TabSlotSetting(craft_fire);     // �����Ҷ� ��ں� ���� ������ ����, ���ҽ� ó�����۽� ���� �߻�
    }

    // ��ưŬ���� pulbic ���� ����
    public void TabSetting(int _tabNumber)
    {
        tabNumber = _tabNumber;
        page = 1;

        switch(tabNumber)       // �� ���ڿ� ���� ���๰ ���� ����
        {
            case 0:
                TabSlotSetting(craft_fire);   // �� ����
                break;
            case 1:
                TabSlotSetting(craft_build);   // �ǹ� ����
                break;
        }
    }

    private void ClearSlot()        // ���� ���� ���� ��Ȱ��ȭ, ������ �ٲ𶧸��� ���־�� �Ѵ�
    {
        for (int i = 0; i < go_Slots.Length; i++)
        {
            image_Slot[i].sprite = null;
            text_SlotName[i].text = "";
            text_SlotDesc[i].text = "";
            text_SlotNeedItem[i].text = "";
            go_Slots[i].SetActive(false);
        }
    }

    public void RightPageSetting()      // ���� ȭ��ǥ Ŭ���� ������ �����Լ�
    {
        // �ִ� �������� ���� �ʵ��� (���๰���� / ���Ա��� = ������, +1 �� page �ʱⰪ�� 1�̿���)
        if (page < (craft_SelectedTab.Length / go_Slots.Length) + 1)
            page++;
        else        // �ִ� ������ ������
            page = 1;       // ù �������� �̵�

        TabSlotSetting(craft_SelectedTab);      // �ǽ��� ����
    }
    public void LeftPageSetting()       // ���� ȭ��ǥ Ŭ���� ������ �����Լ�
    {
        // ù��° ���̰� �ƴҰ�� 
        if (page != 1)
            page--;
        else        // ù��° �������� ���  , �ִ� ������(���๰���� / ���Ա��� = ������, +1 �� page �ʱⰪ�� 1�̿���) �� ����
            page = (craft_SelectedTab.Length / go_Slots.Length) + 1;       // �ִ� �������� �̵�

        TabSlotSetting(craft_SelectedTab);      // �ǽ��� ����
    }

    private void TabSlotSetting(Craft[] _craft_tab)     // �ǽ��� ����, �� Ŭ���� �ش� ���� �з� �������� ����
    {
        ClearSlot();            // ���� ���� ���� ��Ȱ��ȭ
        craft_SelectedTab = _craft_tab;     // ������ ���� �з� ��
        // ������ ���� ù��°�� ���� ��ȣ�� ��Ÿ�� , 4�� ���
        int startSlotNumber = (page - 1) * go_Slots.Length;

        // ���๰ ������ŭ �ݺ�
        for (int i = startSlotNumber; i < craft_SelectedTab.Length; i++)
        {
            if (i == page * go_Slots.Length)    // ������������ �ٲ�(���� ������ ������ Ž���Ҷ�)
                break;
            // �ս��Ժ��� ���������� Ȱ��ȭ
            go_Slots[i - startSlotNumber].SetActive(true);

            // �ս��Ժ��� ���������� �ش� ���๰ �̹��� ����, �̸� ����, ����, �ʿ��� ������ ����
            image_Slot[i - startSlotNumber].sprite = craft_SelectedTab[i].craftImage;
            text_SlotName[i - startSlotNumber].text = craft_SelectedTab[i].craftName;
            text_SlotDesc[i - startSlotNumber].text = craft_SelectedTab[i].craftDesc;
            // �ʿ��� �������� �������� ���� �־ for������ ������ �� �־��ش�
            for (int x = 0; x < craft_SelectedTab[i].craftNeedItem.Length; x++)
            {
                // �������� x�� �̻��϶��� ���ؼ� �߰����ش�
                // ������ �̸� x ������ ���� ���·� �ؽ�Ʈ�� ���´�
                text_SlotNeedItem[i - startSlotNumber].text += craft_SelectedTab[i].craftNeedItem[x];
                text_SlotNeedItem[i - startSlotNumber].text += " x " + craft_SelectedTab[i].craftNeedItemCount[x] + "\n";
            }
        }
    }

    public void SlotClick(int _slotNumber)      // ������ Ŭ���ϸ� �߻��ϴ� �Լ�
    {
        // ������ ���� Ŭ���� ������ ��ȣ�� ��Ÿ��
        selectedSlotNumber = _slotNumber + (page - 1) * go_Slots.Length;

        if (!CheckIngredient())     // ��ᰡ ���ٸ�
            return;                 // �Լ� ����

        // �̸����� ������ �÷��̾���� ���鿡 ������,  ������ �ϱ� ���� ������ ����
        go_Preview = Instantiate(craft_SelectedTab[selectedSlotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        // ���� �޴��󿡼� Ŭ���� ���Կ� ���� ���� �����տ� ���� ������ ����
        go_Prefab = craft_SelectedTab[selectedSlotNumber].go_Prefab;
        isPreviewActivated = true;          // �̸����� Ȱ��ȭ
        go_BaseUI.SetActive(false);         // �⺻ ���̽� UI ��Ȱ��ȭ
    }

    private bool CheckIngredient()      // ��� ���� Ȯ�� �Լ�
    {
        // ������ ������ ������ ������ŭ �ݺ�
        for (int i = 0; i < craft_SelectedTab[selectedSlotNumber].craftNeedItem.Length; i++)
        {
            // ���õ� ������ ������ �ʿ��� �������� ������ ���� �κ��丮 or �����Կ� �ִ� �������� �������� ���ٸ�
            if(theInventory.GetItemCount(craft_SelectedTab[selectedSlotNumber].craftNeedItem[i])  
                < craft_SelectedTab[selectedSlotNumber].craftNeedItemCount[i])
            return false;
        }
        return true;
    }

    private void UseIngredient()
    {
        // ������ ������ ������ ������ŭ �ݺ�
        for (int i = 0; i < craft_SelectedTab[selectedSlotNumber].craftNeedItem.Length; i++)
        {
            // ���õ� ������ ������ �ʿ��� �������� ������ŭ �κ��丮 or �������� ����������� ������ ����
            theInventory.SetItemCount(craft_SelectedTab[selectedSlotNumber].craftNeedItem[i], 
                craft_SelectedTab[selectedSlotNumber].craftNeedItemCount[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)        // ��Ű�� �������� && �̸����� ��Ȱ���϶�
            Window();           // â ���� �Լ�(����, �ݱ�)

        if (isPreviewActivated)             // �̸����� Ȱ��ȭ��
            PreviewPositionUpdate();        // �̸����� ������ ��ġ ������Ʈ

        if (Input.GetButtonDown("Fire1"))    // ���콺 �¹�ư Ŭ����
            Build();        // ���� ������ ����

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();           // ��� �Լ�
    }

    private void Build()            // ���� ������ ����
    {
        // �̸����� Ȱ��ȭ�� && �ǹ��� ���� �� ������
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().IsBuildable())      
        {
            UseIngredient();
            // �������� ��� ��ǥ�� ���� ������ ����, �̸����� �������� ȸ������ ����
            Instantiate(go_Prefab, go_Preview.transform.position, go_Preview.transform.rotation);
            Destroy(go_Preview);        // �̸����� ������ ����
            // ���º���, ������, UI ��Ȱ��ȭ
            isActiavted = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }

    private void PreviewPositionUpdate()        // �̸����� ������ ��ġ ������Ʈ �Լ�
    {
        // �� ������ ������ ����
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform != null)       // �浹��ü�� ������
            {
                // hitInfo.point => �������� ���� ���� ���� ��ǥ�� ��ȯ
                Vector3 _location = hitInfo.point;

                if(Input.GetKeyDown(KeyCode.Q))
                    go_Preview.transform.Rotate(0f, -90f, 0f);      // �̸����� ������ �ݽð� �������� ������
                else if(Input.GetKeyDown(KeyCode.E))
                    go_Preview.transform.Rotate(0f, +90f, 0f);      // �̸����� ������ �ð� �������� ������

                // x, z ��ǥ�� �ݿø��Ͽ� �����ϰ�, y ��ǥ�� ���� ����ȭ�ؼ� �����ֵ��� ����
                _location.Set(Mathf.Round(_location.x), Mathf.Round(_location.y / 0.1f) * 0.1f, Mathf.Round(_location.z));
                go_Preview.transform.position = _location;      // �������� ��ǥ�� ���� �̸����� ������ ������ => ���콺�� ���󰡴� ��ó�� ����
            }
        }
    }

    private void Cancel()               // ��� �Լ�
    {
        if(isPreviewActivated)              // �̸����� Ȱ��ȭ��
            Destroy(go_Preview);            // �̸����� ������ ����

        // ���º���, ������, UI ��Ȱ��ȭ
        isActiavted = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
        go_BaseUI.SetActive(false);

    }

    private void Window()           // â ���� �Լ�(����, �ݱ�)
    {
        if (!isActiavted)               // ũ����Ʈ �޴����� ��Ȱ��ȭ�϶�
            OpenWindow();           // â�� ���� �Լ�
        else                            // ũ����Ʈ �޴����� Ȱ��ȭ�϶�
            CloseWindow();          // â�� �ݴ� �Լ�
    }

    private void OpenWindow()                    // â�� ���� �Լ�
    {
        isActiavted = true;                      // ���º��� Ȱ��ȭ
        go_BaseUI.SetActive(true);               // ������Ʈ Ȱ��ȭ
    }

    private void CloseWindow()                  // â�� �ݴ� �Լ�
    {
        isActiavted = false;                    // ���º��� ��Ȱ��ȭ
        go_BaseUI.SetActive(false);             // ������Ʈ ��Ȱ��ȭ
    }
}
