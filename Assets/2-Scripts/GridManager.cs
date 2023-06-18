using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gridCellPrefab;

    [SerializeField]
    private BoxCollider groundCollider;

    private float gridPositionY;

    [SerializeField]
    private List<GameObject> gridCells = new List<GameObject>();

    [SerializeField]
    private GridSystemChannelSO gridSystemChannel;

    [SerializeField]
    private bool gridCreatingByAmount = true;



    [Header("Creating/Filling Grid by Input GridCell Length")]
    [SerializeField]
    private int perGridCellLengthX;
    [SerializeField]
    private int perGridCellLengthZ;
    [SerializeField]
    private int gridAmountY;

    private int oneLinePlaceableGridAmount;

    [Header("Creating Grid by Input Desired GridAmount X and Z")]
    [SerializeField]
    private Vector2Int desiredGridAmountXZ;
    [SerializeField]
    private int desiredGridCellLengthZ;
    private int gridCellCalculatedLengthX;

    [SerializeField]
    private int beginningPointZ;

    [Header("Common Fields")]
    [SerializeField]
    private int spaceFromExtent;


    void Start()
    {

    }

    void Update()
    {

    }

    public void BeginGridCreating()
    {
        Debug.Log("groundCollider.bounds.center : " + groundCollider.bounds.center);
        Debug.Log("groundCollider.bounds.extents : " + groundCollider.bounds.extents);
        gridPositionY = groundCollider.bounds.center.y + groundCollider.bounds.extents.y + 0.1f;

        if (gridCreatingByAmount)
        {
            CreateGrid(gridCellPrefab, CalculateGridCellBeginningXLocationByAmount(), desiredGridAmountXZ.x, desiredGridAmountXZ.y, gridCellCalculatedLengthX, desiredGridCellLengthZ);

        }
        else
        {
            CreateGrid(gridCellPrefab, CalculateGridCellBeginningXLocation(), oneLinePlaceableGridAmount, gridAmountY, perGridCellLengthX, perGridCellLengthZ);
        }
    }

    private void CreateGrid(GameObject gridPrefab, float bPointX, int gcAmountX, int gcAmountY, int gcLengthX, int gcLengthY)
    {
        float bPointZ = beginningPointZ;

        for (int j = 0; j < gcAmountY; j++)
        {
            for (int i = 0; i < gcAmountX; i++)
            {
                GameObject go = Instantiate(gridPrefab, new Vector3(bPointX + (i * gcLengthX), gridPositionY, bPointZ + (j * gcLengthY)), Quaternion.identity);
                go.transform.localScale = new Vector3(gcLengthX, 1, gcLengthY);
                go.GetComponentInChildren<GridCell>().assignedCoordinate = new Vector2Int(i, j);
                gridCells.Add(go);
            }
        }

        gridSystemChannel.RaiseOnGridCreated(gridCells);

    }

    private float CalculateGridCellBeginningXLocationByAmount()
    {
        float begginingPointX;

        gridCellCalculatedLengthX = (GetAvailableLengthXOnGround() / desiredGridAmountXZ.x);

        Debug.Log("perGridCellLengthX : " + gridCellCalculatedLengthX);

        if (desiredGridAmountXZ.x % 2 == 1)
        {
            begginingPointX = (int)groundCollider.bounds.center.x + (((desiredGridAmountXZ.x - 1) / 2) * gridCellCalculatedLengthX) * -1;
        }
        else
        {
            begginingPointX = (int)groundCollider.bounds.center.x + (((desiredGridAmountXZ.x / 2) - 1) * gridCellCalculatedLengthX + (gridCellCalculatedLengthX / 2)) * -1;
        }

        return begginingPointX;
    }

    private float CalculateGridCellBeginningXLocation()
    {
        float begginingPointX;

        oneLinePlaceableGridAmount = (GetAvailableLengthXOnGround() / perGridCellLengthX);

        Debug.Log("totalAvailableSizeX / gridCellAmountX : " + oneLinePlaceableGridAmount);

        if (oneLinePlaceableGridAmount % 2 == 1)
        {
            begginingPointX = (int)groundCollider.bounds.center.x + (((oneLinePlaceableGridAmount - 1) / 2) * perGridCellLengthX) * -1;
        }
        else
        {
            begginingPointX = (int)groundCollider.bounds.center.x + (((oneLinePlaceableGridAmount / 2) - 1) * perGridCellLengthX + (perGridCellLengthX / 2)) * -1;
        }

        return begginingPointX;
    }

    private int GetAvailableLengthXOnGround()
    {
        return ((int)groundCollider.bounds.extents.x - spaceFromExtent) * 2;
    }


    public void SetHoverIndicatorGridCell(GameObject gridCell)
    {
        for (int i = 0; i < gridCells.Count; i++)
        {
            if (gridCell == gridCells[i])
            {
                gridCells[i].GetComponentInChildren<GridCell>().SetActiveHoverIndicator();
            }
            else
            {
                gridCells[i].GetComponentInChildren<GridCell>().SetDeactiveHoverIndicator();
            }

        }

    }

    public void DeactiveAllGridCellIndicator()
    {
        for (int i = 0; i < gridCells.Count; i++)
        {
            gridCells[i].GetComponentInChildren<GridCell>().SetDeactiveHoverIndicator();
        }
    }

}