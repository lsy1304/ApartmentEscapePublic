using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteGIF : MonoBehaviour
{
    [SerializeField] private SpriteAtlas atlas;
    [SerializeField] private float frameSpeedMS;
    [SerializeField] private string spriteName;
    [SerializeField] private bool isAtalsMoreThen100;
    private string idxformat;
    private Image image;
    private Coroutine imageShifter;

    private void Awake()
    {
        image = GetComponent<Image>();
        idxformat = isAtalsMoreThen100 ? "000" : "00";
    }

    private void OnEnable()
    {
        if(atlas == null) return;
        if(imageShifter != null) StopCoroutine(imageShifter);
        imageShifter = StartCoroutine(ImageShift());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        imageShifter = null;
    }
    private IEnumerator ImageShift()
    {
        WaitForSeconds frameSecond = new WaitForSeconds(frameSpeedMS/1000);
        int frameNumber = 0;
        while (true)
        {
            image.sprite = atlas.GetSprite(spriteName+"-"+frameNumber.ToString(idxformat));
            frameNumber++;
            if(frameNumber >= atlas.spriteCount) frameNumber = 0;
            yield return frameSecond;
        }
    }
}
