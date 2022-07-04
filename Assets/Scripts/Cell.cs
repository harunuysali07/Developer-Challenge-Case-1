using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject selectedSprite;

    public bool isSelected { get; set; }
    public Vector2Int position { get; set; }

    public delegate bool ConfirmSelection();
    public ConfirmSelection confirmSelection;

    public void Select()
    {
        if (confirmSelection != null)
        {
            if (confirmSelection())
            {
                isSelected = true;
                selectedSprite.SetActive(true);
            }
        }
    }

    public void Deselect()
    {
        isSelected = false;
        selectedSprite.SetActive(false);
    }

    private void OnBecameVisible()
    {
        GridController.visibleCellCount++;
    }
}
