using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//フレーバーテキストを格納するオブジェクト
[CreateAssetMenu(fileName = "FlavorText", menuName = "TextObject/FlavorText")]
public class FlavorTextObject : ScriptableObject
{
    [SerializeField]
    private List<FlavorTextGroup> _flavorTextGroups = new List<FlavorTextGroup>();

    public string GetFlavorText(int i, FlavorText.CharacterStatus status, NoteManager.NoteType type) {
        var text = _flavorTextGroups[i]._flavorTexts.Find(t => t.Type == type);
        return text.GetText((int)status);
    }
}

[System.Serializable]
public class FlavorTextGroup
{
    public List<FlavorText> _flavorTexts = new List<FlavorText>();
}

[System.Serializable]
public class FlavorText
{
    [SerializeField]
    private NoteManager.NoteType _type = default(NoteManager.NoteType);
    public NoteManager.NoteType Type => _type;
    [SerializeField, Multiline]
    private string[] _text = null;
    public string GetText(int i) => _text[i];

    public enum CharacterStatus
    {
        Low, Normal, Super, MAX
    }
}
