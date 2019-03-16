using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceChanger : MonoBehaviour
{
    public CharName charName;
    [SerializeField] FaceIndex facer;
    public Sprite[] faceSprites = new Sprite[6];
    Image myFace;
    void Awake()
    {

    }

    public void InportSprite(Sprite[] sprites)
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


    public void Init(TextActor actor)
    {
        charName = actor.id;
        faceSprites = actor.faces;
        myFace = GetComponent<Image>();
        myFace.sprite = faceSprites[(int)FaceIndex.Normal];
    }
}