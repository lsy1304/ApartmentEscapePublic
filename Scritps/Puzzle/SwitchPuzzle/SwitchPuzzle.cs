using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPuzzle : Puzzle
{
    public int currentValue;
    [SerializeField] private Image fillbar;
    private void Awake()
    {
        UpdateGuageDisplay();
        OnAnswerCheckEvent += CheckPuzzleAnswer;
    }

    private void CheckPuzzleAnswer()
    {
        if (currentValue >= puzzleData.Answer[0] && currentValue <= puzzleData.Answer[1])
        {
            UnlockGimmick();
            gameObject.SetActive(false);
        }
    }
    
    public void UpdateInputValue(int value)
    {
        currentValue += value;
        UpdateGuageDisplay();
        CallAnswerCheckEvent();
    }

    private void UpdateGuageDisplay()
    {
        fillbar.fillAmount = currentValue / 100f;
    }
}
