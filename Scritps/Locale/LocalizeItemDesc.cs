using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

public class LocalizeItemDesc : MonoBehaviour
{
    public void LocalizeTextString(string tableName, ItemSO item)
    {
        GetComponent<LocalizeStringEvent>().StringReference
            .SetReference(tableName, item.ItemDescription);
    }
}
