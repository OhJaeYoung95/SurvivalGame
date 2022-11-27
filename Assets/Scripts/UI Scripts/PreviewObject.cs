using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    public Building.Type needType;      // ��ġ�� �� �ʿ��� �ǹ� Ÿ��
    private bool needTypeFlag;          // ��ġ ���� ����

    // �浹�� ������Ʈ�� �ݶ��̴��� �����ϴ� ����Ʈ�� Ȱ��
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField]
    private int layerGround;    // ����(��) ���̾�
    private const int IGNORE_RAYCAST_LAYER = 2;         // Ignore Raycast�� ���̾�â���� 2��°�� ���Ѵ�

    [SerializeField]
    private Material green;     // �̸����� ��ġ ���ɻ�
    [SerializeField]
    private Material red;       // �̸����� ��ġ �Ұ��ɻ�

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()          // �̸����� ������ �ٲ��ִ� �Լ�
    {
        if(needType == Building.Type.Normal)    // �ǹ� Ÿ���� Normal�� ���
        {
            if (colliderList.Count > 0)     // �ݶ��̴� �����ȿ� ���� �ݶ��̴��� �������
                SetColor(red); // �̸����� ������ ���������� �����ش�
            else                            // �ݶ��� �����ȿ� ���� �ݶ��̴��� �������
                SetColor(green); // �̸����� ������ �ʷϻ����� �����ش�
        }
        else                // �ǹ� Ÿ���� Noraml���� ���
        {
            // �ݶ��̴� �����ȿ� ���� �ݶ��̴��� ������� || ��ġ �Ұ����� ���
            if (colliderList.Count > 0 || !needTypeFlag)     
                SetColor(red); // �̸����� ������ ���������� �����ش�
            else                            // �ݶ��� �����ȿ� ���� �ݶ��̴��� �������
                SetColor(green); // �̸����� ������ �ʷϻ����� �����ش�
        }
    }

    private void SetColor(Material mat)     // �̸����� ������ �����ִ� �Լ�
    {
        // �� ������Ʈ�� ���� ��ü�� Transform�� ���� ���鼭 �ݺ�
        foreach (Transform tf_Child in this.transform)
        {
            // ������ materials�� ������ �ٲٱ� ���� �ӽ� ��ü�� ���� ����
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];

            // materials�� ���̸�ŭ �ݺ�
            for (int i = 0; i < newMaterials.Length; i++)
            {
                // �Ű������� ������ �������� ���� �ٲٴ� �۾�
                newMaterials[i] = mat;
            }
            // ������ materials�� ������ ������ �Ű������� �������� �����ϴ� �۾�
            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }

    // �ݶ��̴� �����ȿ� ���� �� �ߵ�
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Structure")      // ���� ������Ʈ�� �±װ� "Structure"�϶�
        {
            // ��ġ�� �� �ʿ��� �ǹ� Ÿ���� �ƴ϶��
            if (other.GetComponent<Building>().type != needType)
                colliderList.Add(other);    // �ݶ��̴� ���������� ������ ����Ʈ�� �߰�
            else
                needTypeFlag = true;        // ��ġ ����
        }
        else                     // ���� ������Ʈ�� �±װ� "Structure" �ƴҶ�
        {
            // �浹�� ������Ʈ�� ���̾ �׶���(��)�� �ƴҶ� && Ignore Raycast���̾ �ƴҶ�
            if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
                colliderList.Add(other);                // �ݶ��̴� ���������� ������ ����Ʈ�� �߰�
        }
    }

    // �ݶ��̴� �������� ���� �� �ߵ�
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Structure")      // ���� ������Ʈ�� �±װ� "Structure"�϶�
        {
            // ��ġ�� �� �ʿ��� �ǹ� Ÿ���� �ƴ϶��
            if (other.GetComponent<Building>().type != needType)
                colliderList.Remove(other);    // �ݶ��̴� ���������� ������ ����Ʈ���� ����
            else
                needTypeFlag = false;        // ��ġ �Ұ���
        }
        else                            // ���� ������Ʈ�� �±װ� "Structure"�� �ƴҶ�
        {
            // �浹�� ������Ʈ�� ���̾ �׶���(��)�� �ƴҶ� && Ignore Raycast���̾ �ƴҶ�
            if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
                colliderList.Remove(other);                // �ݶ��̴� ���������� ������ ����Ʈ���� ����
        }
    }

    public bool IsBuildable()       // �ǹ��� ������ �ִ����� ���� ���θ� �ܺο��� ��� ���� bool�� �Լ�
    {
        if(needType == Building.Type.Normal)        // �ǹ� Ÿ���� Normal�̶��
            return colliderList.Count == 0;             // �ݶ��̴� ������ ���� �ݶ��̴��� ������ true ��ȯ
        else                         // �ǹ� Ÿ���� Normal �ƴ϶��
            return colliderList.Count == 0 && needTypeFlag;     // �ݶ��̴� ������ ���� �ݶ��̴��� ������ && ��ġ ������ �� true ��ȯ
    }
}