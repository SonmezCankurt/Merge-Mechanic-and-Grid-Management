using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class GameManager : MonoBehaviour
{

    [SerializeField] private GridManager gridManager;
    [SerializeField] private GridSystemChannelSO gridSystemChannel;
    [SerializeField] public UnitDatabase unitsDatabase;

    public List<GameObject> gridCells = new List<GameObject>();


    private void OnEnable()
    {
        gridSystemChannel.OnGridCreated += OnGridCreated;

    }

    private void OnDisable()
    {
        gridSystemChannel.OnGridCreated -= OnGridCreated;

    }


    void Start()
    {
        gridManager.BeginGridCreating();
        SpawnUnit(Unit.Tier.Tier1);
        SpawnUnit(Unit.Tier.Tier1);
        SpawnUnit(Unit.Tier.Tier1);
        SpawnUnit(Unit.Tier.Tier1);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnUnit(Unit.Tier.Tier1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnUnit(Unit.Tier.Tier2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnUnit(Unit.Tier.Tier3);
        }


    }

    private void OnGridCreated(List<GameObject> grids)
    {
        Debug.Log("GAME MANAGER OnGridCreated");
        gridCells = grids;
    }

    public bool SpawnUnit(Unit.Tier tier)
    {
        bool retVal = false;

        GameObject gridCellObject = GetRandomEmptyGridCellLocaiton(gridCells);

        if (gridCellObject != null && gridCells.Contains(gridCellObject))
        {
            UnitData uData = FindUnitDataFromDatabase(unitsDatabase.allUnits, tier);

            if (uData != null)
            {
                GameObject go = Instantiate(uData.unitPrefab, gridCellObject.transform.position + uData.unitPrefab.transform.position, Quaternion.identity);

                Vector3 tmpScale = go.transform.localScale;

                go.transform.DOScale(tmpScale / 2, 0).OnComplete(() => {
                    go.transform.DOScale(tmpScale, 0.15f).OnComplete(() => {
                        go.GetComponent<Unit>().scaleObjectOnGrid = tmpScale;
                        go.GetComponent<Unit>().isAvailableScaleControlOnGrid = true;
                    });
                });


                SetupUnit(go.GetComponent<Unit>(), uData);
                gridCellObject.GetComponentInChildren<GridCell>().attachedUnit = go;

                retVal = true;
            }
            else
            {
                Debug.LogWarning("uData is null at Spawn Unit, didnt find in in unit database");
            }

        }

        return retVal;
    }

    public GameObject GetRandomEmptyGridCellLocaiton(List<GameObject> gridCells)
    {
        GameObject gridCell = null;
        List<GameObject> tmp = new List<GameObject>();

        if (IsAnyAvailableGridCell(gridCells))
        {
            for (int i = 0; i < gridCells.Count; i++)
            {
                if (gridCells[i].GetComponentInChildren<GridCell>().attachedUnit == null)
                    tmp.Add(gridCells[i]);
            }

            if (tmp.Count > 0)
            {
                gridCell = tmp[UnityEngine.Random.Range(0, tmp.Count)];
            }

        }

        return gridCell;
    }

    private bool IsAnyAvailableGridCell(List<GameObject> gridCells)
    {
        bool returnVal = false;

        for (int i = 0; i < gridCells.Count; i++)
        {
            if (gridCells[i].GetComponentInChildren<GridCell>().attachedUnit == null)
            {
                returnVal = true;
                break;
            }
        }

        return returnVal;
    }

    public UnitData FindUnitDataFromDatabase(List<UnitData> uDataList, Unit.Tier tier)
    {
        UnitData uData = null;

        for (int i = 0; i < uDataList.Count; i++)
        {
            if (uDataList[i].tier.Equals(tier))
            {
                uData = uDataList[i];
                break;
            }
        }

        return uData;
    }

    public void SetupUnit(Unit unitScript, UnitData uDataRef)
    {
        unitScript.Activate(uDataRef);
    }

    public bool MergeUnits(GridCell firstGridCell, UnitData secondUnitData, GridCell secondGridCell, Transform gridTransform)
    {
        bool retVal = false;

        UnitData uData = FindUnitDataFromDatabase(unitsDatabase.allUnits, (Unit.Tier)(secondUnitData.tier + 1));

        if (uData != null)
        {

            Unit first = firstGridCell.attachedUnit.GetComponent<Unit>();
            Unit second = secondGridCell.attachedUnit.GetComponent<Unit>();

            if (first != null && second != null)
            {

                Destroy(firstGridCell.attachedUnit);
                Destroy(secondGridCell.attachedUnit);

                firstGridCell.attachedUnit = null;
                secondGridCell.attachedUnit = null;

                GameObject go = Instantiate(uData.unitPrefab, gridTransform.position + Vector3.up * 0.1f, Quaternion.identity);

                Vector3 tmpScale = go.transform.localScale;
                Debug.Log("tmpScale spawn Unit : " + tmpScale);

                go.transform.DOScale(tmpScale / 2, 0).OnComplete(() => {
                    go.transform.DOScale(tmpScale * 1.1f, 0.15f).OnComplete(() => {
                        go.transform.DOScale(tmpScale, 0.15f).OnComplete(() => {
                            go.GetComponent<Unit>().scaleObjectOnGrid = tmpScale;
                            go.GetComponent<Unit>().isAvailableScaleControlOnGrid = true;
                        });
                    });
                });

                SetupUnit(go.GetComponent<Unit>(), uData);
                secondGridCell.attachedUnit = go;

                retVal = true;

            }
            else
            {
                retVal = false;
            }



        }
        else
        {
            retVal = false;
        }

        return retVal;

    }
}
