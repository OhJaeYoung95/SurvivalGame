using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float secondPerRealTimeSecond;      // ���� ������ 100�� = ���� ������ 1��

    [SerializeField]
    private float fogDensityCalc;           // Fog ������ ����

    [SerializeField]
    private float nightFogDensity;          // �� ������ Fog �е�
    private float dayFogDensity;            // �� ������ Fog �е�
    private float currentFogDensity;        // ���� Fog �е� ���

    // Start is called before the first frame update
    void Start()
    {   // �������� Fog �е��� ����
        dayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {   // �ش� ������Ʈ�� ���� �¾��� ȸ���ϰ� ���ش�
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)    // ���϶�
            GameManager.isNight = true;
        else if (transform.eulerAngles.x >= 340) // ���϶�
            GameManager.isNight = false;

        if(GameManager.isNight)     // ���ϰ��
        {   // ���� Fog�е��� �� ������ Fog �е����� �������
            if(currentFogDensity <= nightFogDensity)
            {   // ���� Fog�е��� 1�ʿ� Fog������ ��ŭ ����, ������ �� ����
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else            // ���� ���
        {
            // ���� Fog�е��� �� ������ Fog �е����� Ŭ���
            if(currentFogDensity >= dayFogDensity)
            {   // ���� Fog�е��� 1�ʿ� Fog������ ��ŭ ����, ������ �� ����
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
