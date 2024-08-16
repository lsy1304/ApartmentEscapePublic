using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlidePuzzle : Puzzle
{
    [SerializeField] private string[] names = new string[9];
    [SerializeField] private GameObject tiles;
    [SerializeField] private int emptyIndex;
    private List<int> movableIdx = new List<int>();
    private int[,] array = new int[3, 3];
    private SlideTile[] slideTiles = new SlideTile[9];
    private void Awake()
    {
        for(int i =0;i<tiles.transform.childCount; i++)
        {
            slideTiles[i] = tiles.transform.GetChild(i).GetComponent<SlideTile>();
        }

        for (int i = 0; i < names.Length; i++)
        {
            names[i] = slideTiles[i].name = slideTiles[i].text.text;
            if (!slideTiles[i].img.enabled) emptyIndex = i;
            array[i / 3, i % 3] = slideTiles[i].idx = i;
        }
        UpdateMovableTileIdx();
        OnAnswerCheckEvent += CheckPuzzleAnswer;
    }

    private void CheckPuzzleAnswer()
    {
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i] != puzzleData.Answer[i].ToString())
            {
                return;
            }
        }
        UnlockGimmick();
        gameObject.SetActive(false);
    }

    public void UpdateTileValue(int idx)
    {
        bool IsOkToMove = false;

        var Tile = slideTiles[idx];
        var Tile2 = slideTiles[emptyIndex];
        for (int i = 0; i < movableIdx.Count; i++)
        {
            if (Tile.idx == movableIdx[i])
            {
                IsOkToMove = true;
                break;
            }
        }
        if (!IsOkToMove) return;

        Tile2.text.enabled = true;

        var tempTxt = Tile.text.text;
        Tile.text.text = Tile2.text.text; ;
        Tile2.text.text= tempTxt;

        Tile.text.enabled = false;

        Tile2.img.enabled = true;

        var tempSprite = Tile.img.sprite;
        Tile.img.sprite = Tile2.img.sprite;
        Tile2.img.sprite = tempSprite;

        Tile.img.enabled = false;

        var temp = names[idx];
        names[idx] = names[emptyIndex];
        names[emptyIndex] = temp;

        emptyIndex = Tile.idx;

        UpdateMovableTileIdx();
        CallAnswerCheckEvent();
    }

    private void UpdateMovableTileIdx()
    {
        movableIdx.Clear();
        if (emptyIndex % 3 - 1 > -1)
            movableIdx.Add(emptyIndex - 1);
        if (emptyIndex % 3 + 1 < 3)
            movableIdx.Add(emptyIndex + 1);
        if ((emptyIndex + 3) / 3 < 3)
            movableIdx.Add(emptyIndex + 3);
        if ((emptyIndex - 3) / 3 >= 0)
            movableIdx.Add(emptyIndex - 3);
    }
}
