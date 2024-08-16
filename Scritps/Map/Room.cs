using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [SerializeField] private bool isCorridor;
    [SerializeField] private int roomNumber;
    public Transform mainRoom { get; private set; }
    public Transform bedRoom { get; private set; }
    public Transform bathRoom { get; private set; }
    public GameObject doorBedtoMain {  get; private set; }
    public GameObject doorBathtoMain {  get; private set; }
    public SpriteRenderer[] mainSprites {  get; private set; }
    public SpriteRenderer[] bedSprites {  get; private set; }
    public SpriteRenderer[] bathSprites {  get; private set; }
    public Puzzle[] puzzlesInRoom{ get; private set; }
    public Gimmick[] gimmicksInRoom { get; private set; }
    public Room SetRoom()
    {
        puzzlesInRoom = transform.GetComponentsInChildren<Puzzle>(true);
        gimmicksInRoom = transform.GetComponentsInChildren<Gimmick>(true);
        if (isCorridor) return this;
        doorBedtoMain = transform.GetChild(0).GetChild(0).gameObject;
        doorBathtoMain = transform.GetChild(0).GetChild(1).gameObject;
        mainRoom = transform.GetChild(1);
        bedRoom = transform.GetChild(2);
        bathRoom = transform.GetChild(3);
        mainSprites = mainRoom.GetComponentsInChildren<SpriteRenderer>(true);
        bedSprites = bedRoom.GetComponentsInChildren<SpriteRenderer>(true);
        bathSprites = bathRoom.GetComponentsInChildren<SpriteRenderer>(true);
        return this;
    }
}
