using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class DialogueParser : MonoBehaviour
{
    public Dictionary<int, Dialogue> Parse(string csvFileName)
    {
        Dictionary<int, Dialogue> dialogueList = new Dictionary<int, Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(csvFileName + LocaleManager.Instance.CurrentLocaleIdx.ToString());

        string[] data = csvData.text.Split('\n');
        for(int i = 1; i<data.Length;)
        {
            string[] row = data[i].Split(',');
            Dialogue newDialogue = new Dialogue();

            int key = Convert.ToInt32(row[0]);
            newDialogue.name = row[1];
            List<string> contextList = new List<string>();
            do
            {
                contextList.Add(row[2]);
                if (++i < data.Length)
                {
                    row = data[i].Split(',');
                }
                else break;
            } while (row[0].ToString() == "");
            
            newDialogue.context = contextList.ToArray();
            dialogueList.Add(key, newDialogue);
        }
        return dialogueList;
    }
}
