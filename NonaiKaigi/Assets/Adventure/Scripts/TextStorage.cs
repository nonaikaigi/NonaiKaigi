using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceIndex
{
    Normal, Fun, Angry, Surprise, Suffer, Shout, Last
}
public enum PositionTag
{
    Right, Left, Center, None,
}

[System.Serializable]
public class TextStorage
{
    /// <summary>/// /// </summary>
    public enum CharSpriteIndex
    {
        Magia
    }
    /// <summary>キャラ名</summary>
    public string charName;
    public CharName cName;
    /// <summary>内容</summary>
    public string sentence;
    /// <summary>表情</summary>
    public string face;
    /// <summary>表情</summary>
    public int faceIndex;
    /// <summary>正体が判明しているか</summary>
    public string unknown;
    public bool isUnknown;
    /// <summary>演出内容</summary>
    public string stage;
    /// <summary>演出の種類</summary>
    public StageType stageTag;
    /// <summary>使用するファイル名</summary>
    public string sourceFileName;


    /// <summary>コンストラクタ</summary>
    public TextStorage(TextStorage storage)
    {
        charName = storage.charName;
        sentence = storage.sentence;
        stage = storage.stage;
        sourceFileName = storage.sourceFileName;
        FaceIndex tmpIndex;
        CharName tmpName;
        //StageType tmpStage;


        if (storage.unknown == "")
        {
            isUnknown = false;
        }
        else
        {
            isUnknown = true;
        }
        if (Enum.TryParse(storage.charName, out tmpName))
        {
            cName = tmpName;
        }
        else
        {
            cName = CharName.None;
        }

        if (Enum.TryParse(storage.face, out tmpIndex))
        {
            faceIndex = (int)tmpIndex;
        }
        else
        {
            faceIndex = (int)FaceIndex.Normal;
        }
    }
    public TextStorage(string s)
    {
        sentence = s;
    }
    public TextStorage()
    {

    }
    public TextStorage(int i)
    {

    }

}
