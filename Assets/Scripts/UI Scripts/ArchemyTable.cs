using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ArchemyItem            // ���� ������
{
    public string itemName;         // ���� ������ �̸�
    public string itemDesc;         // ���� ������ ����
    public Sprite itemImage;        // ���� ������ �̹���

    public string[] needItemName;       // �ʿ��� ������ �̸���
    public int[] needItemNumber;        // �ʿ��� ������ ������

    public float itemCraftingTime;      // ���� ������ �ɸ��� �ð� (5��, 10��, 100��)

    public GameObject go_ItemPrefab;    // ���� ������ ������
}

public class ArchemyTable : MonoBehaviour
{
    public bool GetIsOpen()                 // �ܺο��� ������ ����â Ȱ��ȭ ���θ� ��� ���ִ� �Լ�
    {
        return isOpen;
    }
    private bool isOpen = false;            // ������ ����â Ȱ��ȭ ����(�ߺ� ����)
    private bool isCrafting = false;        // �������� ���� ���� ����


    [SerializeField]
    private ArchemyItem[] archemyItems;         // ������ �� �ִ� ���� ������ ����
    private Queue<ArchemyItem> archemyItemQueue = new Queue<ArchemyItem>();     // ���� ������ ���� ��⿭(ť)
    private ArchemyItem currentCraftingItem;        // ���� �������� ���� ������

    private float craftingTime;                 // ���� ������ ���� �ð�
    private float currentCraftingTime;          // ���� ���� �ð�
    private int page = 1;                       // ���� ���� ���̺��� ������ ��ȣ
    [SerializeField]
    private int theNumberOfSlot;                // �� �������� ������ �ִ� ����(4��)   

    [SerializeField]
    private Image[] image_ArchemyItems;     // �������� ���� ������ �̹�����
    [SerializeField]
    private Text[] text_ArchemyItems;       // �������� ���� ������ �ؽ�Ʈ��
    [SerializeField]
    private Button[] btn_ArchemyItems;      // �������� ���� ����� ��ư��
    [SerializeField]
    private Slider slider_Gauge;            // �����̴� ������
    [SerializeField]
    private Transform tf_BaseUI;            // ���̽� UI
    [SerializeField]
    private Transform tf_PotionAppearPos;   // ���� ���� ��ġ
    [SerializeField]
    private GameObject go_Liquid;           // ���۽�Ű�� ��ü ����
    [SerializeField]
    private Image[] image_CraftingItems;    // ��⿭ ���Կ� �ִ� ������ �̹�����


    // �ʿ��� ������Ʈ
    [SerializeField]
    private ArchemyToolTip theToolTip;      // UI ����â�� �ٷ�� ���� ������Ʈ
    private AudioSource theAudio;       // ȿ���� ����� ���� ������Ʈ
    private Inventory theInven;         // �ʿ��� ������ ����(���)�� ���� ������ ���� ������Ʈ
    [SerializeField]
    private AudioClip sound_ButtonClick;        // ��ư Ŭ�� ȿ����
    [SerializeField]
    private AudioClip sound_Beep;               // ���� ����(��� ����) ȿ����
    [SerializeField]
    private AudioClip sound_Activate;           // ���� ȿ����
    [SerializeField]
    private AudioClip sound_ExitItem;           // ������ ���� ȿ����


    private void PlaySE(AudioClip _clip)        // ȿ���� ��� �Լ�
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

    void Start()
    {
        theInven = FindObjectOfType<Inventory>();
        theAudio = GetComponent<AudioSource>();
        ClaerSlot();        // ������ ���� �̹��� �ʱ�ȭ
        PageSetting();      // ������ ���� �̹��� ����
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsFinish())     // �������϶�
        {
            Crafting();
        }

        if (isOpen)     // ������ ����â Ȱ��ȭ��, �ߺ�����
            if (Input.GetKeyDown(KeyCode.Escape))       // ESCŰ�� �����ٸ�
                CloseWindow();      // â �ݱ�
    }

    private bool IsFinish()     // ������ ���������� ���� ����
    {
        // ��⿭�� ������ && �������� �ƴҶ�
        if(archemyItemQueue.Count == 0 && !isCrafting)
        {
            go_Liquid.SetActive(false);
            slider_Gauge.gameObject.SetActive(false);
            return true;
        }
        else
        {
            go_Liquid.SetActive(true);
            slider_Gauge.gameObject.SetActive(true);
            return false;
        }
    }

    private void Crafting()     // ���� ������ ���� �Լ�
    {
        // �������� �ƴҶ� && ��⿭�� �ִٸ�
        if (!isCrafting && archemyItemQueue.Count != 0)
            DequeueItem();      // ��⿭���� ���� ������

        if(isCrafting)      // ���� ���϶�
        {
            currentCraftingTime += Time.deltaTime;      // ���� ���۰�� �ð��� 1�ʾ� ����
            slider_Gauge.value = currentCraftingTime;   // ������ ���� ���� ���۰�� �ð��� ����(�������� ������ ���� ����)

            if(currentCraftingTime >= craftingTime)     // ���� �ð��� �Ǹ�
                ProductionComplete();         // ���� ������ ����Ϸ�

        }
    }

    private void DequeueItem()      // ��⿭���� ���� ������ �Լ�
    {
        PlaySE(sound_Activate);      // ���� ȿ���� ���
        isCrafting = true;        // ������
        // ���� �������� ���� �����ۿ� ��⿭���� ���� �������� ����
        currentCraftingItem = archemyItemQueue.Dequeue();       

        // ���� ������ ���� �ð��� ��⿭���� ���� �������� ���� �ð��� ����
        craftingTime = currentCraftingItem.itemCraftingTime;
        currentCraftingTime = 0;        // ���� ���۽ð� �ʱ�ȭ
        slider_Gauge.maxValue = craftingTime;   // �����̴� ������ �ִ밪 ����

        CraftingImageChange();      // ��⿭�� ���� �̹��� ��ü
    }

    private void CraftingImageChange()      // ��⿭�� ���� �̹��� ��ü
    {
        image_CraftingItems[0].gameObject.SetActive(true);  // ù��° ��⿭ �̹��� Ȱ��ȭ

        // �̹����� �״�� �����ִµ� DequeueItem()�Լ����� Dequeue�� ���༭ +1�� ���ش�
        for (int i = 0; i < archemyItemQueue.Count + 1; i++)
        {
            // ��⿭ ������ ������ �̹��� ��ĭ�� ��ܿ���
            image_CraftingItems[i].sprite = image_CraftingItems[i + 1].sprite;
            // ������° ���� ��⿭�� ���̺��� Ŀ�� ���
            if (i + 1 == archemyItemQueue.Count + 1)
                image_CraftingItems[i + 1].gameObject.SetActive(false); // �ش� �̹��� ��Ȱ��ȭ
        }
    }

    public void Window()        // ������ ����â Ȱ��ȭ, ��Ȱ��ȭ
    {
        isOpen = !isOpen;       // ��ư�� ó�� true -> false, false -> true
        if (isOpen)             // ������ ����â Ȱ��ȭ��, �ߺ�����
            OpenWindow();
        else
            CloseWindow();
    }

    private void OpenWindow()       // â ���� �Լ�
    {
        isOpen = true;          // ������ ����â Ȱ��ȭ, �ߺ�����
        GameManager.isOpenArchemyTable = true;      // Ŀ�� Ȱ��ȭ
        tf_BaseUI.localScale = new Vector3(1f, 1f, 1f);     // ũŰ�� 1�� �Ǿ Ȱ��ȭ�� ��ó�� �����
    }

    private void CloseWindow()      // â �ݱ� �Լ�
    {
        isOpen = false;         // ������ ����â ��Ȱ��ȭ, �ߺ�����
        GameManager.isOpenArchemyTable = false;     // Ŀ�� ��Ȱ��ȭ
        tf_BaseUI.localScale = new Vector3(0f, 0f, 0f);     // ũŰ�� 0�� �Ǿ ��Ȱ��ȭ�� ��ó�� �����
    }

    public void ButtonClick(int _buttonNum)         // ����â ��ư Ŭ���� �߻��ϴ� �Լ�
    {
        PlaySE(sound_ButtonClick);      // ��ư Ŭ�� ȿ���� ���

        if (archemyItemQueue.Count < 3)         // ��⿭ ���̾��̸�
        {
            // �������� �ش��ϴ� ���� ��ư�� ������ ���� ����
            // �ִ������� + ��ư��ȣ = ���� �������� �ش��ϴ� ���Թ�ư ��ȣ
            int archemyItemArrayNumber = _buttonNum + ((page - 1) * theNumberOfSlot);

            // �κ��丮���� ��� �˻�
            for (int i = 0; i < archemyItems[archemyItemArrayNumber].needItemName.Length; i++)
            {
                // �ش� ���� ������ ���� �ʿ��� �κ��丮�� �ִ� �������� ������
                // ���ۿ� �ʿ��� �������� ���� ���� ������
                // ��ᰡ ������ ��
                if(theInven.GetItemCount(archemyItems[archemyItemArrayNumber].needItemName[i]) 
                    < archemyItems[archemyItemArrayNumber].needItemNumber[i])
                {
                    PlaySE(sound_Beep);     // ���� ����(��� ����) ȿ���� ���
                    return;
                }
            }

            // �κ��丮 ��� ����
            for (int i = 0; i < archemyItems[archemyItemArrayNumber].needItemName.Length; i++)
            {
                theInven.SetItemCount(archemyItems[archemyItemArrayNumber].needItemName[i], archemyItems[archemyItemArrayNumber].needItemNumber[i]);
            }

            // ���� ����
            // ��⿭�� ��ư�� ���� �������� �շ�
            archemyItemQueue.Enqueue(archemyItems[archemyItemArrayNumber]);

            // ��⿭ ������ Ȱ��ȭ, �̹��� ����
            image_CraftingItems[archemyItemQueue.Count].gameObject.SetActive(true);
            image_CraftingItems[archemyItemQueue.Count].sprite = archemyItems[archemyItemArrayNumber].itemImage;
        }
        else
        {
            PlaySE(sound_Beep);     // ���� ����(��� ����) ȿ���� ���
        }
    }

    private void ProductionComplete()     // ���� ������ ����Ϸ� �Լ�
    {
        isCrafting = false;     // ������ ����
        image_CraftingItems[0].gameObject.SetActive(false);     // (�������� ����Ϸ�)������ �����鼭 0��° �̹��� ��Ȱ��ȭ

        PlaySE(sound_ExitItem);     // ������ ���� ȿ����

        // ���� ���̺� ���� ��ġ�� �ش� ���� ������ ����
        Instantiate(currentCraftingItem.go_ItemPrefab, tf_PotionAppearPos.position, Quaternion.identity);
    }

    public void UpButton()      // ������ ����ư �Լ�
    {
        PlaySE(sound_ButtonClick);      // ��ư Ŭ�� ȿ���� ���

        if (page != 1)   // �������� ù��° ���� �ƴ϶��
            page--;     // ������ ��ȣ -1
        else            // ������ ���� / ���� �ִ� ���� = ������ ���� + 1 => �ִ� ������
            page = 1 + (archemyItems.Length / theNumberOfSlot);

        ClaerSlot();        // ������ ���� �̹��� �ʱ�ȭ
        PageSetting();      // ������ ���� �̹��� ����
    }
    public void DownButton()    // ������ �ٿ��ư �Լ�
    {
        PlaySE(sound_ButtonClick);      // ��ư Ŭ�� ȿ���� ���

        if (page < 1 + (archemyItems.Length / theNumberOfSlot))  // �ִ� ������ ���� ������
            page++;     // ������ ��ȣ +1
        else        // �ִ� �������� ������ ù��° ������ �̵�
            page = 1;

        ClaerSlot();        // ������ ���� �̹��� �ʱ�ȭ
        PageSetting();      // ������ ���� �̹��� ����
    }

    private void ClaerSlot()    // ������ ���� �̹��� �ʱ�ȭ �Լ�
    {
        // ���� ������â ����
        for (int i = 0; i < theNumberOfSlot; i++)
        {   // ���� ������ ���� ��Ȱ��ȭ, �̹���,��ư, �ؽ�Ʈ �ʱ�ȭ �۾�
            image_ArchemyItems[i].sprite = null;
            image_ArchemyItems[i].gameObject.SetActive(false) ;
            btn_ArchemyItems[i].gameObject.SetActive(false);
            text_ArchemyItems[i].text = "";
        }
    }

    private void PageSetting()      // ������ ���� �̹��� ���� �Լ�
    {
        // �������� ù��° ������ ��ȣ
        int pageArrayStartNumber = (page - 1) * theNumberOfSlot;    // 0, 4, 8, 12

        for (int i = pageArrayStartNumber; i < archemyItems.Length; i++)
        {
            // ����ȣ�� ������ �Ѿ��
            if (i == page * theNumberOfSlot)
                break;

            // ���۹�ȣ ~ ����ȣ�� ���� Ȱ��ȭ, �̹���, ��ư, �ؽ�Ʈ ���� Ȱ��ȭ
            image_ArchemyItems[i - pageArrayStartNumber].sprite = archemyItems[i].itemImage;
            image_ArchemyItems[i - pageArrayStartNumber].gameObject.SetActive(true);
            btn_ArchemyItems[i - pageArrayStartNumber].gameObject.SetActive(true);
            text_ArchemyItems[i - pageArrayStartNumber].text =  archemyItems[i].itemName + "\n" + archemyItems[i].itemDesc;
        }
    }

    public void ShowToolTip(int _buttonNum)     // UI ����â Ȱ��ȭ �Լ�
    {
        // �������� �ش��ϴ� ���� ��ȣ
        int _archemyItemArrayNumber = _buttonNum + ((page - 1) * theNumberOfSlot);
        // UI ����â Ȱ��ȭ(�ش� ������ ������ �̸�, �ʿ� ���� ����)
        theToolTip.ShowToolTip(archemyItems[_archemyItemArrayNumber].needItemName, archemyItems[_archemyItemArrayNumber].needItemNumber);
    }

    public void HideToolTip()            // UI ����â ��Ȱ��ȭ �Լ�
    {
        theToolTip.HideToolTip();
    }
}
