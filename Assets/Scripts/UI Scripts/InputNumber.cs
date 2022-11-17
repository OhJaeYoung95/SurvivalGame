using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    private bool activated = false;

    [SerializeField]
    private Text text_Preview;      // ������ �ִ� ������ �ִ� ����
    [SerializeField]
    private Text text_Input;        // ������ ���� ����
    [SerializeField]
    private InputField if_text;     // Input�ϴ� Text�� �ʱ�ȭ �ϱ� ���� ����

    [SerializeField]
    private GameObject go_Base;     // ������ ������ ���� â(��ǲ�ʵ�)

    [SerializeField]
    private ActionController thePlayer;       // ������ ���� ��ǥ�� ��� ���� �÷��̾� ī�޶� ��ġ

    void Update()
    {
        if(activated)
        {
            if (Input.GetKeyDown(KeyCode.Return))        // ���� ������ OK�Լ� ����
                OK();
            else if (Input.GetKeyDown(KeyCode.Escape))     // ESCŰ�� ������ Cancel�Լ� ����  
                Cancel();
        }
    }

    public void Call()              // ������ ������ ���� â(��ǲ�ʵ�) Ȱ��ȭ
    {
        go_Base.SetActive(true);
        activated = true;            // ��ǲâ�� Ȱ��ȭ �Ǿ��ٴ� Bool ��
        if_text.text = "";           // ������ ���� ���� �ؽ�Ʈ ���� �ʱ�ȭ
        text_Preview.text = DragSlot.instance.dragSlot.itemCount.ToString();        // ������ �ִ밳�� �� ����(���ڿ��� ��ȯ),
    }

    public void Cancel()            // ������ ������ ���� â(��ǲ�ʵ�) Ȱ��ȭ
    {
        activated = false;            // ��ǲâ�� ��Ȱ��ȭ �Ǿ��ٴ� Bool ��
        go_Base.SetActive(false);
        DragSlot.instance.SetColor(0);          // ���� ��Ȱ��ȭ
        DragSlot.instance.dragSlot = null;        // ���� �ʱ�ȭ
    }

    public void OK()                // OK��ư ������ ��Ÿ���� �Լ�(������ ���� ���� ������)
    {
        DragSlot.instance.SetColor(0);          // ���� ��Ȱ��ȭ

        int num;
        if (text_Input.text != "")               // �ؽ�Ʈ�� ��ĭ�� �ƴ϶��
        {
            if (CheckNumber(text_Input.text))        // �ؽ�Ʈ�� ���ڶ��
            {
                num = int.Parse(text_Input.text);    // text ������ -> int �� ��ȯ
                if (num > DragSlot.instance.dragSlot.itemCount)     // �Է��� ���� ���ڰ� �巡���� �������� �������� Ŭ ���
                    num = DragSlot.instance.dragSlot.itemCount;     // �巡���� �������� �ִ� ������ ����
            }
            else                        // �ؽ�Ʈ�� ���ڰ� �ƴ϶��
            {
                num = 1;            // 1���� �������� ����
            }
        }
        else                    // �ؽ�Ʈ�� ��ĭ�̶��
            num = int.Parse(text_Preview.text);     // ������ �ִ밹�� ����

        StartCoroutine(DropItemCoroutine(num));
    }

    IEnumerator DropItemCoroutine(int _num)
    {
        for (int i = 0; i < _num; i++)      // ������ ���� ��ŭ ������ ���
        {
            if(DragSlot.instance.dragSlot.item.itemPrefab != null)      // ������ �������� �����ϸ�
                Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, thePlayer.transform.position + thePlayer.transform.forward, Quaternion.identity);
            DragSlot.instance.dragSlot.SetSlotCount(-1);            // ���Ծ����� ���� ����
            yield return new WaitForSeconds(0.05f);
        }

        DragSlot.instance.dragSlot = null;      // ���� �ʱ�ȭ
        go_Base.SetActive(false);               // ������ ������ ���� â(��ǲ�ʵ�)��Ȱ��ȭ
        activated = false;            // ��ǲâ�� ��Ȱ��ȭ �Ǿ��ٴ� Bool ��

    }

    private bool CheckNumber(string _argString)         // ���ڿ��� �������� Ȯ�����ִ� �Լ�
    {
        char[] _tempCharArray = _argString.ToCharArray();       // ���ڿ��� ���ڹ迭�� ����ִ´�
        bool isNumber = true;
        for (int i = 0; i < _tempCharArray.Length; i++)
        {
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57)     // �ƽ�Ű�ڵ� 0����9������ ���ڿ� ��ٸ�
                continue;   // ���� for���� ���ƶ�
            isNumber = false;       // ���ڰ� ��� ���ڶ�� ���⿡ �������� ����
        }

        return isNumber;
    }
}
