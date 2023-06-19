using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UnitDatabase/UnitDatabase")]
public class UnitDatabase : ScriptableObject
{
    public List<UnitData> allUnits = new List<UnitData>();
}
