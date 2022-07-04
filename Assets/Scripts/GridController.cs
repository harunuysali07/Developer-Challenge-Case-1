using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public static int visibleCellCount = 0;

    public int width = 10;
    
    [SerializeField] private Cell cellPrefab;

    private const float cellWidth = 1f;
    private const float cellOffset = .1f;

    private Cell[,] cells;

    public void Initialize(int _width)
    {
        width = _width;
        visibleCellCount = 0;

        CreateGrid();
        StartCoroutine(CameraViewCoroutine());
    }

    private IEnumerator CameraViewCoroutine()
    {
        yield return null;

        var camSpeed = Vector3.up * 5f * width * .1f;;

        while (camSpeed.sqrMagnitude > .1f)
        {
            if (visibleCellCount >= cells.LongLength)
            {
                camSpeed -= camSpeed * width * .1f * Time.deltaTime;
            }

            Camera.main.transform.position += camSpeed * Time.deltaTime;

            yield return null;
        }
    }

    [ContextMenu("Create Grid")]
    public void CreateGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        cells = new Cell[width, width];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var posX = i + i * cellOffset - ((width - 1) / 2f * (cellWidth + cellOffset));
                var posZ = j + j * cellOffset - ((width - 1) / 2f * (cellWidth + cellOffset));

                Cell cell = Instantiate(cellPrefab, new Vector3(posX, 0, posZ), Quaternion.identity);
                cell.transform.parent = transform;
                cell.position = new Vector2Int(i, j);
                cell.confirmSelection = () => OnCellSelected(cell);

                cells[i, j] = cell;
            }
        }
    }

    public bool OnCellSelected(Cell cell)
    {
        cell.isSelected = true;

        if (ControlEffectedCells(cell.position))
        {
            Debug.Log("False neighbors detected");

            cell.isSelected = false;
            return false;
        }

        if (SelectedNeighborsCount(cell.position) >= 2)
        {
            DeselectNeighbors(cell.position);

            Debug.Log("Too many neighbors selected");

            cell.isSelected = false;
            return false;
        }

        Debug.Log("Cell selected: " + cell.position);

        return true;
    }

    private bool ControlEffectedCells(Vector2Int position)
    {
        var selectedCells = GetNeighborCells(position).Where(c => c.isSelected).ToList();
        selectedCells.Add(cells[position.x, position.y]);
        var falseCells = selectedCells.Where(c => SelectedNeighborsCount(c.position) >= 2).ToList();
        falseCells.ForEach(c => c.Deselect());
        falseCells.ForEach(c => DeselectNeighbors(c.position));

        return falseCells.Count != 0;
    }

    private int SelectedNeighborsCount(Vector2Int position)
    {
        return GetNeighborCells(position).Where(c => c.isSelected).Count();
    }

    private void DeselectNeighbors(Vector2Int position)
    {
        GetNeighborCells(position).ForEach(cell => cell.Deselect());
    }

    private List<Cell> GetNeighborCells(Vector2Int position)
    {
        var neighbors = new List<Cell>();

        if (position.x - 1 >= 0)
        {
            neighbors.Add(cells[position.x - 1, position.y]);
        }

        if (position.x + 1 < width)
        {
            neighbors.Add(cells[position.x + 1, position.y]);
        }

        if (position.y - 1 >= 0)
        {
            neighbors.Add(cells[position.x, position.y - 1]);
        }

        if (position.y + 1 < width)
        {
            neighbors.Add(cells[position.x, position.y + 1]);
        }

        return neighbors;
    }
}
