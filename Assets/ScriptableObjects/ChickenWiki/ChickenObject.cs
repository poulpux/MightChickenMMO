using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GRAVITYFEEL
{
    HOVERS,
    FALL
}

[CreateAssetMenu(fileName = "ChickenObject", menuName = "SO", order = 1)]
public class ChickenObject : ScriptableObject
{
    public int pv;
    public float spd;
    public float weight;
    public float radius;
    public GRAVITYFEEL gravType;

    public Sprite sprite;

    [Header("======BasicAttack======")]
    [Space(10)]
    public float basicAttackDist;
    public float basicAttackCldwn;
    public float basicAttackForce;

    [Header("======Jump======")]
    [Space(10)]
    public float jumpForce;
    //PASSIFS & ACTIFS
}
