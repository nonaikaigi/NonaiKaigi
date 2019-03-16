using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "ActorData", menuName = "CreActor")]
public class TextActor : ScriptableObject
{

    public CharName id;
    public string name;

    public Sprite[] faces;
    public FaceIndex index;
}
