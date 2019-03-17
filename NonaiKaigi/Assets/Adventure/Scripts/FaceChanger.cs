using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceChanger : MonoBehaviour
{
    public CharName charName;
    [SerializeField] FaceIndex facer;
    public Sprite[] faceSprites = new Sprite[6];
    //[SerializeField] Image[] faceImages = new Image[6];
    [SerializeField] Image myFace;
    void Awake()
    {
        ChangeFace(FaceIndex.Normal);
    }

    public void ImportSprite(Sprite[] sprites)
    {
        faceSprites = sprites;
        myFace.sprite = faceSprites[(int)FaceIndex.Normal];
    }

    public void ChangeFace(FaceIndex faceIndex)
    {
        myFace.sprite = faceSprites[(int)faceIndex];
    }
    public void ChangeFace(int faceIndex)
    {
        myFace.sprite = faceSprites[faceIndex];
    }

    //public void ChangeFace(FaceIndex index)
    //{
    //    foreach (Image item in faceImages)
    //    {
    //        item.gameObject.SetActive(false);
    //    }
    //    faceImages[(int)index].gameObject.SetActive(true);
    //}
    //public void ChangeFace(int index)
    //{
    //    foreach (Image item in faceImages)
    //    {
    //        item.gameObject.SetActive(false);
    //    }
    //    faceImages[index].gameObject.SetActive(true);
    //}


    public void Init(TextActor actor)
    {
        charName = actor.id;
        faceSprites = actor.faces;
        //myFace = GetComponent<Image>();
        myFace.sprite = faceSprites[(int)FaceIndex.Normal];
    }
}