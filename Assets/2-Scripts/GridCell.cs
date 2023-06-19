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
