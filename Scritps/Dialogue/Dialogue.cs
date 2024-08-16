using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [Tooltip("캐릭터 이름")]
    public string name;

    [Tooltip("대사")]
    public string[] context;
}

[Serializable]
public struct DialogueLine
{
    public string name;
    public int startLine;
    public int endLine;
}

[Serializable]
public class DialogueEvent
{
    public string name;
    public DialogueLine[] dialogueLines;
    public Dialogue[] dialogues;
}
