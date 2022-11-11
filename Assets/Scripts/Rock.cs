using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;                 // ������ ü��

    [SerializeField]
    private float destroyTime;      // ���� �ı��� ���󰡴� ����� ���� �ð�

    [SerializeField]
    private SphereCollider col;     // ������ ��ü �ݶ��̴�


    // �ʿ��� ���� ������Ʈ
    [SerializeField]
    private GameObject go_rock;     // �Ϲ� ����
    [SerializeField]
    private GameObject go_debris;   // ���� ���� ����
    [SerializeField]
    private GameObject go_effect_prefabs;    // ���� ä���� ������ ����Ʈ

    //// ���� ���� ������Ʈ
    //[SerializeField]
    //private AudioSource audioSource;        // ���� ä�� ����
    //[SerializeField]
    //private AudioClip effect_sound;         // ���� �μ����� �� ä�� ����
    //[SerializeField]
    //private AudioClip effect_sound2;        // ���� �μ��� �� ä�� ���� 

    // �ʿ��� ���� �̸�
    [SerializeField]
    private string strike_Sound;        // ���� �μ����� �� ä�� ���� �̸�
    [SerializeField]
    private string destroy_Sound;       // ���� �μ��� �� ä�� ���� �̸�

    public void Mining()    // ���� ä�� �Լ�
    {
        //audioSource.clip = effect_sound;
        //audioSource.Play();

        SoundManager.instance.PlaySE(strike_Sound);             // ���� �μ����� �� ä�� ���� ���
        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);         // ���� ä���� ����Ʈ ������ ����
        Destroy(clone, destroyTime);            // ������ �������� �־������ν�, ������ ���Ŀ� �������� �ٷ�� �ְ� ���ش�. ������ destroyTime�ڿ� ����

        hp--;
        if (hp <= 0)
            Destruction();
    }

    private void Destruction()  // ���� �ı� �Լ�
    {
        //audioSource.clip = effect_sound2;
        //audioSource.Play();

        SoundManager.instance.PlaySE(destroy_Sound);            // ���� �μ��� �� ä�� ���� ���
        col.enabled = false;                    // �ݶ��̴� ����
        Destroy(go_rock);                       // ���� ��ü ����

        go_debris.SetActive(true);              // ���� ���� ����
        Destroy(go_debris, destroyTime);        // ���� ���� destroyTime ���Ŀ� ����
    }
}