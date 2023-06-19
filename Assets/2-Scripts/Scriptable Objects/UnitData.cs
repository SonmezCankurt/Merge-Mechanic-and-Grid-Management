using UnityEngine;

[CreateAssetMenu(menuName = "UnitData/UnitData")]
public class UnitData : ScriptableObject
{
    [Header("Common")]
    public GameObject unitPrefab;

    public Unit.Tier tier = Unit.Tier.None;
    public float attackRatio = 1f;
    [Tooltip("Damage each attack deals")]
    public float damagePerAttack = 2f;
    public float attackRange = 1f;
    [Tooltip("Health of the Unit")]
    public float hitPoints = 10f;
    public float movementSpeed = 5f;


}
