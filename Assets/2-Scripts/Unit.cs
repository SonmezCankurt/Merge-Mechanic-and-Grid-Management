using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour
{
    [HideInInspector] public UnitData unitData;

    [Header("Unit Data Fields")]
    [HideInInspector] public float hitPoints;
    [HideInInspector] public float attackRange;
    [HideInInspector] public float attackRatio;
    [HideInInspector] public float damage;
    [HideInInspector] public float movementSpeed;



    public Vector3 scaleObjectOnGrid;
    [HideInInspector] public bool isAvailableScaleControlOnGrid = false;


    private void Awake()
    {
    }

    private void Start()
    {
    }

    private void Update()
    {
    }



    public virtual void Activate(UnitData uData)
    {
        unitData = uData;
        hitPoints = uData.hitPoints;
        attackRange = uData.attackRange;
        attackRatio = uData.attackRatio;
        movementSpeed = uData.movementSpeed;
        damage = uData.damagePerAttack;

        scaleObjectOnGrid = transform.localScale;
    }


    public enum Tier
    {
        None,
        Tier1,
        Tier2,
        Tier3,
    }

}
