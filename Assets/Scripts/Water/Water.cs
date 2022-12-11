using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    [SerializeField]
    private float waterDrag;        // ���� �߷�
    private float originDrag;       // �ʱ� �߷�

    [SerializeField] 
    private Color waterColor;          // ���� ����
    [SerializeField] 
    private float waterFogDensity;     // �� Ź�� ����

    [SerializeField]
    private Color waterNightColor;          // �� ������ ���� ����
    [SerializeField]
    private float waterNightFogDensity;     // �� ������ �� Ź�� ����


    // �ʱⰪ
    private Color originColor;          // �ʱ� ����
    private float originFogDensity;     // �ʱ� Ź������
    [SerializeField]
    private Color originNightColor;     // �ʱ� ������� ����
    [SerializeField]
    private float originNightFogDensity;    // �ʱ� ������� Ź�� ����

    [SerializeField]
    private string sound_WaterOut;      // ������ ���ö� ȿ����
    [SerializeField]    
    private string sound_WaterIn;       // ���� ���� ȿ����
    [SerializeField]
    private string sound_Breathe;       // ȣ���Ҷ� ȿ����

    [SerializeField]
    private float breatheTIme;          // ȣ�� �ð�
    private float currentBreatheTime;   // ���� ȣ�� ��� �ð�

    [SerializeField]
    private float totalOxygen;      // ��ҷ�
    private float currentOxygen;    // ��ҷ� ��갪
    private float temp;         // ��Ұ� ������ HP���Ҹ� ���� �ð�����

    [SerializeField]
    private GameObject go_BaseUI;       // ���UIâ
    [SerializeField]
    private Text text_totalOxygen;      // ��ҷ� �ؽ�Ʈ
    [SerializeField]
    private Text text_currentOxygen;    // �����ҷ� �ؽ�Ʈ
    [SerializeField]
    private Image image_gauge;          // ��ҷ� ������

    private StatusController thePlayerStat;

    // Start is called before the first frame update
    void Start()
    {   // �ʱⰪ ����
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;
        thePlayerStat = FindObjectOfType<StatusController>();
        currentOxygen = totalOxygen;
        text_totalOxygen.text = totalOxygen.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.isWater)     // ���ӿ� ������
        {
            currentBreatheTime += Time.deltaTime;       // ���� ȣ�� ��� �ð��� 1�ʿ� 1�� ����
            if(currentBreatheTime >= breatheTIme)       // ���� ȣ�� ��� �ð��� ȣ��ð��� �����ϸ�
            {
                SoundManager.instance.PlaySE(sound_Breathe);    // ȣ���Ҷ� ȿ���� ���
                currentBreatheTime = 0;     // ���� ȣ�� ��� �ð� �ʱ�ȭ
            }
        }

        DecreaseOxygen();   // ��� ����
    }

    private void DecreaseOxygen()   // ��� ���� �Լ�
    {
        if(GameManager.isWater)     // ���ӿ� ������
        {
            if(currentOxygen >= 0)  // ���� ��ҷ��� 0���� ũ�ٸ�
            {
                currentOxygen -= Time.deltaTime;        // ���� ��ҷ� 1�ʿ� 1�� ����
            }
            text_currentOxygen.text = Mathf.RoundToInt(currentOxygen).ToString();     // ���� ��ҷ� �ؽ�Ʈȭ, �ݿø����� Int�� ����ȯ�� ���ڿ�ȭ
            image_gauge.fillAmount = currentOxygen / totalOxygen;   // ���� ��ҷ� ������ȭ

            if(currentOxygen <= 0)      // ���� ��ҷ��� 0���� �۾�����
            {
                temp += Time.deltaTime;     // temp 1�ʿ� 1�� ����
                if(temp >= 1)   // 1�ʸ��� HP�� ��� �ϱ����� ��ġ
                {
                    thePlayerStat.DecreaseHP(1);    // HP����
                    temp = 0;       // temp �ʱ�ȭ
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    private void GetWater(Collider _player)     // ���� ���� �߻��ϴ� �Լ�
    {
        SoundManager.instance.PlaySE(sound_WaterIn);    // ���� ���� ȿ���� ���

        go_BaseUI.SetActive(true);      // ��� UIâ Ȱ��ȭ
        GameManager.isWater = true;     // ���ӿ� ��
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;   // ���� �߷� ����

        if(!GameManager.isNight)        // ���� ���
        {
            RenderSettings.fogColor = waterColor;                   // ���� �� ����
            RenderSettings.fogDensity = waterFogDensity;            // �� Ź�� ���� ����
        }
        else                // ���� ���
        {
            RenderSettings.fogColor = waterNightColor;                   // �� ������ ���� �� ����
            RenderSettings.fogDensity = waterNightFogDensity;            // �� ������ �� Ź�� ���� ����
        }
    }
    private void GetOutWater(Collider _player)  // ������ ���ö� �߻��ϴ� �Լ�
    {
        if(GameManager.isWater)     // ���ӿ� ������
        {
            go_BaseUI.SetActive(false);     // ��� UIâ ��Ȱ��ȭ
            currentOxygen = totalOxygen;        // �����ҷ� �ʱ�ȭ, �� ��
            SoundManager.instance.PlaySE(sound_WaterOut);   // ������ ���ö� ȿ���� ���

            GameManager.isWater = false;        // ������ ����
            _player.transform.GetComponent<Rigidbody>().drag = originDrag;   // �ʱ� �߷� ����

            if(!GameManager.isNight)        // ���� ���
            {
                RenderSettings.fogColor = originColor;                   // �ʱ� �� ����
                RenderSettings.fogDensity = originFogDensity;            // �ʱ� Ź�� ���� ����
            }
            else                            // ���� ���
            {
                RenderSettings.fogColor = originNightColor;                   // �ʱ� �㶧�� �� ����
                RenderSettings.fogDensity = originNightFogDensity;            // �ʱ� �㶧�� Ź�� ���� ����
            }
        }
    }
}
