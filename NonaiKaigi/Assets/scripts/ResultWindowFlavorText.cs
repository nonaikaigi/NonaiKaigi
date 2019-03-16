using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ResultFlavorText", menuName = "TextObject/ResultFlavorText")]
public class ResultWindowFlavorText : ScriptableObject
{
    [SerializeField]
    private List<ResultFlavorTextGroup> _resultFlavorTextGroup = new List<ResultFlavorTextGroup>();

    public string GetProblemText(int i) => _resultFlavorTextGroup[i].ProblemText;
    public ResultFlavorText GetResult(int i, NoteManager.NoteType type) => _resultFlavorTextGroup[i].ResultFlavorTexts.Find(text => text.Type == type);
}

[System.Serializable]
public class ResultFlavorTextGroup
{
    [SerializeField] private string _problemText = null;
    public string ProblemText => _problemText;
    public List<ResultFlavorText> ResultFlavorTexts = new List<ResultFlavorText>();
}

[System.Serializable]
public class ResultFlavorText
{
    [SerializeField]
    private NoteManager.NoteType _type = default(NoteManager.NoteType);
    public NoteManager.NoteType Type => _type;
    [SerializeField, Multiline]
    private string _resultText = null;
    public string ResultText => _resultText;
    [SerializeField]
    private Sprite _clearSprite = null;
    public Sprite ClearSprite => _clearSprite;
}
