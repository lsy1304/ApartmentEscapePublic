using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [Tooltip("ĳ���� �̸�")]
    public string name;

    [Tooltip("���")]
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
