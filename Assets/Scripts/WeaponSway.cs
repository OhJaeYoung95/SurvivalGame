using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public static bool isActivated = true;      // ���� ��鸲 Ȱ��ȭ��

    // ���� ��ġ
    private Vector3 originPos;

    // ���� ��ġ
    private Vector3 currentPos;

    // sway(���� ��鸲) �Ѱ�
    [SerializeField]
    private Vector3 LimitPos;

    // ������ sway(���� ��鸲) �Ѱ�
    [SerializeField]
    private Vector3 fineSightLimitPos;

    // �ε巯�� ������ ����
    [SerializeField]
    private Vector3 smoothSway;


    // �ʿ��� ������Ʈ
    [SerializeField]
    private GunController theGunController;     // ������ ������ ���� �޾ƿ��� ���� GunController
    // Start is called before the first frame update
    void Start()
    {
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Inventory.inventoryActivated && isActivated)       // �κ��丮 ��Ȱ��ȭ�ÿ��� && ���� ��鸲 Ȱ��ȭ��
            TrySway();
    }

    private void TrySway()
    {
        if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }

    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if(!theGunController.isFineSightMode)       // ������ ���� �ƴҶ��� �ѱ� ��鸲
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -LimitPos.x, LimitPos.x),
               Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -LimitPos.y, LimitPos.y),
               originPos.z);
        }
        else                                // ������ �����ϋ��� �ѱ� ��鸲                            
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                           Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                           originPos.z);

        }
        transform.localPosition = currentPos;
    }

    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.y);
        transform.localPosition = currentPos;
    }
}
