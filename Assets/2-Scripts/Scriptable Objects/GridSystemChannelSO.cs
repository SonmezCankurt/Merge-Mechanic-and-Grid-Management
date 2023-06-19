using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "GridSystemChannelSO/GridSystemChannelSO")]
public class GridSystemChannelSO : ScriptableObject
{
    public UnityAction<List<GameObject>> OnGridCreated;
    public UnityAction<GridCell, GridCell> OnUnitChangeGrid;

    public void RaiseOnGridCreated(List<GameObject> grids)
    {
        OnGridCreated?.Invoke(grids);
        Debug.Log("RaiseOnGridCreated");
    }
    public void RaiseOnUnitChangeGrid(GridCell gridCell, GridCell gridCell2)
    {
        OnUnitChangeGrid?.Invoke(gridCell, gridCell2);
        Debug.Log("RaiseOnUnitChangeGrid");
    }
}
