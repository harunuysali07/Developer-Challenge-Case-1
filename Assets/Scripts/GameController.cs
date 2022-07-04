using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public GridController gridController;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Cell cell = hit.transform.GetComponent<Cell>();

                if (cell != null)
                {
                    if (cell.isSelected)
                    {
                        cell.Deselect();
                    }
                    else
                    {
                        cell.Select();
                    }
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }
}
