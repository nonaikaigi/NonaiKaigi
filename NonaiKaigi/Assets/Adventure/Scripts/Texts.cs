using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "TextData", menuName = "CreText")]
public class Texts : ScriptableObject
{
    public List<TextStorage> texts = new List<TextStorage>();
}
