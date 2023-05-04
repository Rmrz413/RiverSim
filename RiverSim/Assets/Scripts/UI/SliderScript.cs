using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] TMP_InputField tmpInput;

    void Awake()
    {
        tmpInput.text = slider.value.ToString("0.00");

        slider.onValueChanged.AddListener((v) =>
        tmpInput.text = v.ToString("0.00"));

        tmpInput.onEndEdit.AddListener(OnInputChanged);
    }

    void OnInputChanged(string v)
    {
        float.TryParse(v, out float f);
        slider.value = f;
        tmpInput.text = slider.value.ToString("0.00");
    } 
}
