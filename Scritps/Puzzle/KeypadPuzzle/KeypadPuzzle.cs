using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeypadPuzzle : Puzzle
{
    [Header("Keypad Puzzle")]
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private TextMeshProUGUI[] displayDigits;
    private int currentDisplayIdx;

    [Header("Sound")]
    [SerializeField] private AudioClip keypadWrong;
    [SerializeField] private AudioClip keypadOpen;
    private void Awake()
    {
        confirmBtn.onClick.AddListener(() => CallAnswerCheckEvent());
        cancelBtn.onClick.AddListener(() => DeleteOneDigit());
        OnAnswerCheckEvent += CheckPuzzleAnswer;
        currentDisplayIdx = 0;
    }

    private void DeleteOneDigit()
    {
        if (currentDisplayIdx <= 0) return;
        currentDisplayIdx--;
        displayDigits[currentDisplayIdx].text = "-";
    }

    void CheckPuzzleAnswer()
    {
        for (int i = 0; i < displayDigits.Length; i++)
        {
            if (displayDigits[i].text == "-" || Convert.ToInt32(displayDigits[i].text) != puzzleData.Answer[i])
            {
                SoundManager.Instance.PlayClip(keypadWrong);
                return;
            }
        }
        SoundManager.Instance.PlayClip(keypadOpen);
        UnlockGimmick();
        gameObject.SetActive(false);
    }

    public void OnNumBtnPress()
    {
        if (currentDisplayIdx >= 4) return;
        displayDigits[currentDisplayIdx].text = EventSystem.current.currentSelectedGameObject.name;
        currentDisplayIdx++;
    }

    public override void ResetValue()
    {
        base.ResetValue();
        foreach(TextMeshProUGUI digit in displayDigits)
        {
            digit.text = "-";
        }
        currentDisplayIdx = 0;
    }
}
