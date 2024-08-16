using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberPuzzle : Puzzle
{
    [SerializeField] private NumberPlate[] numberPlates;
    [SerializeField] private Button confirmBtn;

    private void Awake()
    {
        confirmBtn.onClick.AddListener(() => CallAnswerCheckEvent());
        OnAnswerCheckEvent += CheckPuzzleAnswer;
    }

    private void CheckPuzzleAnswer()
    {
        for(int i =0; i<numberPlates.Length; i++)
        {
            if (Convert.ToInt32(numberPlates[i].Number.text) != puzzleData.Answer[i])
            {
                return;
            }
        }
        UnlockGimmick();
        gameObject.SetActive(false);
    }
}
