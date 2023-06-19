using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "GridSystemChannelSO/GridSystemChannelSO")]
public class GridSystemChannelSO : ScriptableObject
{
    public UnityAction<List<GameObject>> OnGridCreated;

    public void RaiseOnGridCreated(List<GameObject> grids)
    {
        OnGridCreated?.Invoke(grids);
        Debug.Log("RaiseOnGridCreated");
    }
    
}
