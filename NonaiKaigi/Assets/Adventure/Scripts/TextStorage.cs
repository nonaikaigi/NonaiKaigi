using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceIndex
{
    Normal, Fun, Suffer, Wry, Last
}
public enum PositionTag
{
    Right, Left, Center, None,
}

[Serializable]
public class TextStorage
{
    /// <summary>/// /// </summary>
    public enum CharSpriteIndex
    {
        Magia
    }
    /// <summary>キャラ名</summary>
    public CharName cName;
    /// <summary>内容</summary>
    public string sentence;
    /// <summary>表情</summary>
    public FaceIndex faceIndex;


    /// <summary>コンストラクタ</summary>
    public TextStorage(TextStorage storage)
    {
        sentence = storage.sentence;
        FaceIndex tmpIndex;
        CharName tmpName;
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
