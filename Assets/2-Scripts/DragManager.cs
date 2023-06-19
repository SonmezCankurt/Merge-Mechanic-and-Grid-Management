using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragManager : MonoBehaviour
{
    private Unit.Tier maxTier = Unit.Tier.Tier3;
    public static bool isDragging = false;
    private GridCell firstGridCell = null;

    public GameObject draggingObject = null;
    public Vector3 draggingObjectBaseScale;

    public GameManager gameManager;
    public GridManager gridManager;
    public LayerMask layerMask;
    public GridSystemChannelSO gridSystemChannel;

    void Start()
    {
    }

    void Update()
    {

        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, layerMask))
                {
                    Debug.Log("hitInfo : " + hitInfo.transform.gameObject.name);


                    if (hitInfo.transform.gameObject.GetComponent<GridCell>()?.attachedUnit != null)
                    {
                        isDragging = true;
                        draggingObject = hitInfo.transform.gameObject.GetComponent<GridCell>().attachedUnit;
                        Debug.Log("draggingObject SET");
                        firstGridCell = hitInfo.transform.gameObject.GetComponent<GridCell>();

                        Ray rayT = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        SetPointDraggingObject(rayT, firstGridCell.transform.position.y);

                        draggingObjectBaseScale = draggingObject.GetComponent<Unit>().scaleObjectOnGrid;
                        draggingObject.transform.DOScale(draggingObjectBaseScale * 1.25f, 0.1f);

                        gridManager.SetHoverIndicatorGridCell(hitInfo.transform.parent.gameObject);


                    }
                }


            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {

                if (isDragging && draggingObject != null)
                {
                    Ray rayT = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    SetPointDraggingObject(rayT, firstGridCell.transform.position.y);

                    if (Physics.Raycast(rayT, out RaycastHit hitInfo, 1000f, layerMask))
                    {
                        if (hitInfo.transform.gameObject.GetComponent<GridCell>() != null)
                        {
                            if (hitInfo.transform.gameObject.GetComponent<GridCell>().attachedUnit == null ||
                                hitInfo.transform.gameObject.GetComponent<GridCell>() == firstGridCell)
                            {
                                gridManager.SetHoverIndicatorGridCell(hitInfo.transform.parent.gameObject);
                            }
                            else
                            {

                                UnitData firstUnitData = firstGridCell.attachedUnit.GetComponent<Unit>().unitData;

                                GridCell hoverGridCell = hitInfo.transform.gameObject.GetComponent<GridCell>();
                                UnitData secondUnitData = hoverGridCell.attachedUnit.GetComponent<Unit>().unitData;

                                if (firstGridCell != hoverGridCell && CheckAvailableMerge(firstUnitData, secondUnitData))
                                {
                                    gridManager.SetHoverIndicatorGridCell(hitInfo.transform.parent.gameObject);
                                }
                                else
                                {
                                    gridManager.DeactiveAllGridCellIndicator();
                                }

                            }
                        }
                        else
                        {

                            gridManager.DeactiveAllGridCellIndicator();
                        }
                    }
                    else
                    {
                        gridManager.DeactiveAllGridCellIndicator();
                    }
                }

            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {


                if (isDragging)
                {
                    gridManager.DeactiveAllGridCellIndicator();

                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                    if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, layerMask))
                    {
                        Debug.Log("hitInfo : " + hitInfo.transform.gameObject.name);


                        // Not GridCell
                        if (hitInfo.transform.gameObject.GetComponent<GridCell>() == null)
                        {
                            ReturnBacktoGridCell();
                            firstGridCell = null;
                            isDragging = false;
                            Debug.Log("Draged to Not GridCell, Returned Unit to First GridCell");

                        }
                        else
                        {
                            // GridCell is Empty
                            if (hitInfo.transform.gameObject.GetComponent<GridCell>().attachedUnit == null)
                            {
                                firstGridCell.attachedUnit = null;
                                hitInfo.transform.gameObject.GetComponent<GridCell>().attachedUnit = draggingObject;
                                hitInfo.transform.gameObject.GetComponent<GridCell>().attachedUnit.transform.position = hitInfo.transform.parent.position;
                                gridSystemChannel.RaiseOnUnitChangeGrid(firstGridCell, hitInfo.transform.gameObject.GetComponent<GridCell>());
                                Debug.Log("Moved to Another Available GridCell");
                            }
                            else
                            {
                                UnitData firstUnitData = firstGridCell.attachedUnit.GetComponent<Unit>().unitData;

                                GridCell secondGridCell = hitInfo.transform.gameObject.GetComponent<GridCell>();
                                UnitData secondUnitData = secondGridCell.attachedUnit.GetComponent<Unit>().unitData;


                                if (firstGridCell != secondGridCell && CheckAvailableMerge(firstUnitData, secondUnitData))
                                {
                                    if (gameManager.MergeUnits(firstGridCell, secondUnitData, secondGridCell, hitInfo.transform.parent))
                                    {
                                        Debug.Log("Merge Successful");
                                    }
                                    else
                                    {
                                        ReturnBacktoGridCell();
                                        Debug.Log("Merge UnSuccessful, Returned Unit to First GridCell");
                                    }
                                }
                                else
                                {
                                    ReturnBacktoGridCell();
                                    Debug.Log("Draged to Not Mergeable, Returned Unit to First GridCell");
                                }


                            }
                        }
                    }
                    else
                    {
                        ReturnBacktoGridCell();
                        firstGridCell = null;
                        isDragging = false;
                        Debug.Log("Raycast hitInfo False, Draged to Not GridCell, Returned Unit to First GridCell");
                    }

                    draggingObject.transform.DOScale(draggingObjectBaseScale, 0).OnComplete(() => {
                        draggingObject = null;
                    });

                    firstGridCell = null;
                    isDragging = false;
                }

            }


        }


    }


    public bool CheckAvailableMerge(UnitData fUnitData, UnitData sUnitData)
    {

        if (fUnitData.tier.Equals(sUnitData.tier) &&
            !fUnitData.tier.Equals(maxTier))
        {
            return true;
        }
        else
            return false;
    }


    private void SetPointDraggingObject(Ray rayT, float yPosition)
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(rayT, out float distance))
        {

            //Debug.Log("rayT.origin : " + rayT.origin);
            //Debug.Log("rayT.direction : " + rayT.direction);
            //Debug.Log("rayT.distance : " + distance);
            Vector3 point = rayT.origin + rayT.direction * distance;
            draggingObject.transform.position = new Vector3(point.x, yPosition + 1.2f, point.z);

        }
    }

    private void ReturnBacktoGridCell()
    {
        draggingObject.transform.position = firstGridCell.transform.position + Vector3.up;
    }

}
