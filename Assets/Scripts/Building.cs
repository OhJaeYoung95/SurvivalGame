using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Type type;       // 건물 타입

    public enum Type
    {
        Normal,
        Wall,
        Foundation,
        Pillar
    }
}