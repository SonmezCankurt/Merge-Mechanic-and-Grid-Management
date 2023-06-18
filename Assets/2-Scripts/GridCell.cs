using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField]
    private GameObject hoverIndicator;

    public Vector2Int assignedCoordinate;
    public GameObject attachedUnit = null;

    void Start()
    {

    }

    void Update()
    {
        /* if (GameManager.CurrentGameState.Equals(GameManager.GameState.PREPARED) && !DragManager.isDragging && attachedUnit != null &&
             attachedUnit.GetComponent<Unit>().isAvailableScaleControlOnGrid)
         {
             attachedUnit.transform.localScale = attachedUnit.GetComponent<Unit>().scaleObjectOnGrid;
         }*/
    }

    public void SetActiveHoverIndicator()
    {
        hoverIndicator.SetActive(true);
    }

    public void SetDeactiveHoverIndicator()
    {
        hoverIndicator.SetActive(false);
    }

}
