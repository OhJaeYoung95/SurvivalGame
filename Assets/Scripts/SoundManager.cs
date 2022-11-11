using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// MonoBehaviour�� ������ ������ �ٸ� ��ü�� Sound�� ������Ʈ�� Ȱ���� �� ����.
[System.Serializable]           // Ŭ���� ��ü ������ ����ȭ, [SerializeField]���� �� ���� ȿ��
public class Sound              // ȿ���� ���� Ŭ����
{
    public string name;         // ������ �̸�
    public AudioClip clip;      // ����(Ŭ��)
}

public class SoundManager : MonoBehaviour
{
    //�̱��� singleton 1��. ���̵��� �̷����� �Ѱ��� �����ϵ��� ����
    static public SoundManager instance;

    #region singleton
    void Awake()    // ��ü ������ ���� ����
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    #endregion singleton

    void OnEnable() // �Ź� Ȱ��ȭ�� ����. �ڷ�ƾ ���� X
    {
        
    }

    // �Ź� Ȱ��ȭ�� ���� ex) SetActive(false) -> SetActive(true) �� ����, �ڷ�ƾ ���� o
    void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }


    public AudioSource[] audioSourceEffects;        // ȿ���� ����
    public AudioSource audioSourceBgm;              // ����� ����

    public string[] playSoundName;

    public Sound[] effectSounds;                    // ȿ���� ����
    public Sound[] bgmSounds;                       // ����� ����

    public void PlaySE(string _name)        // ȿ���� ��� �Լ�
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name  == effectSounds[i].name)              // ȿ���� ������ �ִ� �̸��� ��ġ�ϴ°� �ִ��� Ȯ��
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if(!audioSourceEffects[j].isPlaying)                // ȿ���� �������� ��������� ���� ȿ������ Ȯ��
                    {
                        playSoundName[j] = effectSounds[i].name;                // ȿ���� �̸� ����
                        audioSourceEffects[j].clip = effectSounds[i].clip;      // �Ķ���� _name�� ��ġ�ϴ� ȿ������ �־��ش�
                        audioSourceEffects[j].Play();                           // ȿ���� ���
                        return;                                                 // return ���� �Լ��� ������ for���� ��������
                    }
                }
                Debug.Log("��� ���� AudioSource�� ������Դϴ�");
                return;
            }
        }
        Debug.Log(_name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�");
    }

    public void StopAllSE()               // ��� ȿ���� ��� ��� �Լ�
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();                       // Ž���ϴ� ȿ���� ��� ���
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)                  // �Ķ���� _name�� ���� ȿ�������� Ȯ��
            {
                audioSourceEffects[i].Stop();               // ȿ���� ��� ���       
                return;
            }
        }
        Debug.Log("��� ����" + _name + "���尡 �����ϴ�");
    }
}
