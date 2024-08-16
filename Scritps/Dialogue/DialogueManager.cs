using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] string csvFileName;

    [HideInInspector] public DialogueUI UI;

    Dictionary<int,  Dialogue> dialogueDic = new Dictionary<int, Dialogue>();

    public static bool IsFinish = false;

    DialogueParser theParser;

    protected void Awake()
    {
        UI = GetComponent<DialogueUI>();
        theParser = GetComponent<DialogueParser>();
        dialogueDic = theParser.Parse(csvFileName);
        
        IsFinish = true;
        LocalizationSettings.SelectedLocaleChanged += ReParse;
    }

    private void ReParse(Locale locale)
    {
        dialogueDic.Clear();
        dialogueDic = theParser.Parse(csvFileName);
    }

    public Dialogue[] GetDialogues(int startNum, int endNum)
    {
        List<Dialogue> dialogues = new List<Dialogue>();
        for (int i = startNum; i <= endNum; i++)
        {
            if(dialogueDic.ContainsKey(i))
            dialogues.Add(dialogueDic[i]);
        }
        return dialogues.ToArray();
    }
}
