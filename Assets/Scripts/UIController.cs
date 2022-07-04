using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider widthSlider;
    [SerializeField] private TextMeshProUGUI widthText;

    private void Start()
    {
        widthSlider.value = GameController.Instance.gridController.width;
        OnWidthSliderValueChanged(widthSlider.value);
    }

    public void OnWidthSliderValueChanged(float value)
    {
        widthText.text = value.ToString();
    }

    public void OnStartButtonClicked()
    {
        gameObject.SetActive(false);
        GameController.Instance.gridController.Initialize((int)widthSlider.value);
    }
}
