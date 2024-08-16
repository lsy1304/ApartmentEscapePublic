using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberPlate : MonoBehaviour
{
    public TextMeshProUGUI Number;
    [SerializeField] private Button LeftBtn;
    [SerializeField] private Button RightBtn;
    private void Awake()
    {
        Number.text = "0";
        LeftBtn.onClick.AddListener(() => RotateLeft());
        RightBtn.onClick.AddListener(() => RotateRight());
    }

    private void RotateRight()
    {
        int temp = Convert.ToInt32(Number.text);
        if (++temp > 9) temp = 0;
        Number.text = temp.ToString();
    }

    private void RotateLeft()
    {
        int temp = Convert.ToInt32(Number.text);
        if (--temp < 0) temp = 9;
        Number.text = temp.ToString();
    }
}
