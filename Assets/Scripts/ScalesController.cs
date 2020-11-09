using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ScalesController : MonoBehaviour
{
    public Slider scaleSlider;

    private ARSessionOrigin _arSessionOrigin;

    private void Awake()
    {
        _arSessionOrigin = GetComponent<ARSessionOrigin>();
    }

    // Start is called before the first frame update
    void Start()
    {
        scaleSlider.onValueChanged.AddListener(OnScaleSliderValueChange);
    }

    public void OnScaleSliderValueChange(float value)
    {
        if (scaleSlider != null)
        {
            // decreasing the scale makes AR objects looks BIGGER
            // while increasing the scale makes AR objects looks SMALLER
            _arSessionOrigin.transform.localScale = Vector3.one / value;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
