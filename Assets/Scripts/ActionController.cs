using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;    // ���� ������ �ִ� �Ÿ�

    private bool pickupActivated = false;   // ���� ������ �� true

    private RaycastHit hitInfo;         // �浹ü ���� ����

    [SerializeField]
    private LayerMask layerMask;        // ������ ���̾�� �����ϵ��� ���̾� ����ũ ����

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Text actionText;            // �׼ǿ� ���̴� �ؽ�Ʈ

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()            // ������ ����
    {
        if(Input.GetKeyDown(KeyCode.E))     // EŰ�� �����ٸ�
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()                // ������ �����Լ�
    {
        if(pickupActivated)         // ������ ���� �����ҽ�
        {
            if(hitInfo.transform != null)       // Raycast������ �浹 ��ü�� �ִٸ�
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ���߽��ϴ�");      // ������ ȹ�濩�� �ܼ�â�� �����
                Destroy(hitInfo.transform.gameObject);          // ������ ���ӿ�����Ʈ ����
                InfoDisappear();                                // ������ ȹ��� ������ ���� ��Ȱ��ȭ
            }
        }
    }

    private void CheckItem()            // �������� ���� ���� Ȯ��
    {
        // ������ �÷��̾ ��ġ���� �÷��̾ �ٶ󺸴� �������� ��� ���̾� ����ũ�� ����� Ȯ��
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, layerMask))
        {
            if(hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else                        // �������� ������ ���� �ʾ�����
        {
            InfoDisappear();
        }
    }

    private void ItemInfoAppear()           // ������ ���� Ȱ��ȭ
    {
        pickupActivated = true;                      // ������ �ݱ� ����
        actionText.gameObject.SetActive(true);       // ������ ���� Ȱ��ȭ
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ��" + "<color=yellow>" + "(E)" + "</color>";        // �ش� ������ �̸� ȹ��ǥ��, �ؽ�Ʈ�� �� ����
    }

    private void InfoDisappear()            // ������ ���� ��Ȱ��ȭ
    {
        pickupActivated = false;                          // ������ �ݱ� �Ұ���
        actionText.gameObject.SetActive(false);           // ������ ���� ��Ȱ��ȭ
    }
}
