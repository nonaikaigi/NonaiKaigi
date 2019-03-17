using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//フレーバーテキストを格納するオブジェクト
[CreateAssetMenu(fileName = "FlavorText", menuName = "TextObject/FlavorText")]
public class FlavorTextObject : ScriptableObject
{
    [SerializeField]
    private List<FlavorTextGroup> _flavorTextGroups = new List<FlavorTextGroup>();

    public string GetFlavorText(int stageIdx, FlavorText.CharacterStatus status, int idx, NoteManager.NoteType type) {
        var text = _flavorTextGroups[stageIdx]._flavorTexts.Find(t => t.Type == type);
        return text.GetText(status, idx);
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
    [SerializeField, Multiline, Header("Low")]
    private string[] _lowStatustext = null;
    [SerializeField, Multiline, Header("Normal")]
    private string[] _normalStatustext = null;
    [SerializeField, Multiline, Header("Super")]
    private string[] _superStatustext = null;
    public string GetText(CharacterStatus status, int i) {
        if(status == CharacterStatus.Low) {
            return _lowStatustext[i];
        }
        else if(status == CharacterStatus.Normal) {
            return _normalStatustext[i];
        }
        else if(status == CharacterStatus.Super) {
            return _superStatustext[i];
        }
        else {
            Debug.Log("Wrong Type");
            return null;
        }
    }

    public enum CharacterStatus
    {
        Low, Normal, Super, MAX
    }
}
