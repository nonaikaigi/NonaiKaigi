using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ResultFlavorText", menuName = "TextObject/ResultFlavorText")]
public class ResultWindowFlavorText : ScriptableObject
{
    [SerializeField]
    private List<ResultFlavorTextGroup> _resultFlavorTextGroup = new List<ResultFlavorTextGroup>();

    public string GetProblemText(int stageIdx) => _resultFlavorTextGroup[stageIdx].ProblemText;
    public string GetOptionText(int stageIdx, NoteManager.NoteType type) => _resultFlavorTextGroup[stageIdx].ResultFlavorTexts.Find(text => text.Type == type).OptionText;
    public ResultFlavorText GetResult(int stageIdx, NoteManager.NoteType type) => _resultFlavorTextGroup[stageIdx].ResultFlavorTexts.Find(text => text.Type == type);
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
    private string _optionText = null;
    public string OptionText => _optionText;
    [SerializeField, Multiline]
    private string _resultText = null;
    public string ResultText => _resultText;
}
