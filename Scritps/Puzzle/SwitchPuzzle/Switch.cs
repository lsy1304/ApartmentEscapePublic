using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    public int switchValue;
    [SerializeField] private SwitchPuzzle switchPuzzle;
    private Toggle toggle;
    private Image image;
    private event Action OnSwitchChange;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        image = GetComponent<Image>();
        OnSwitchChange += UpdateSwitchColor;
        UpdateSwitchColor();
        toggle.onValueChanged.AddListener(OnClick);
    }

    private void UpdateSwitchColor()
    {
        image.color = new Color(142f / 255f, 142f / 255f, 142f / 255f);
        if(toggle.isOn)
        {
            image.color = new Color(1f, 1f, 1f);
        }
        else { image.color = new Color(142f / 255f, 142f / 255f, 142f / 255f); }
    }

    void OnClick(bool boolean)
    {
        OnSwitchChange?.Invoke();
        int temp = boolean ? switchValue : -switchValue;
        switchPuzzle.UpdateInputValue(temp);
    }
}
