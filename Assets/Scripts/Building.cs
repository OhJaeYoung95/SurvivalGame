using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Type type;       // �ǹ� Ÿ��

    public enum Type
    {
        Normal,
        Wall,
        Foundation,
        Pillar
    }
}