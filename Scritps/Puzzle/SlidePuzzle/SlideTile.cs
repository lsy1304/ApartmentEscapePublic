using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlideTile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SlidePuzzle slidePuzzle;
    public int idx;
    public TextMeshProUGUI text;
    public Image img;
    public void OnPointerClick(PointerEventData eventData)
    {
        slidePuzzle.UpdateTileValue(idx);
    }
}
