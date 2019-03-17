using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultWindowManager : MonoBehaviour
{
    private static ResultWindowManager _resultWindowManager = null;
    public static ResultWindowManager GetResultWindowManger => _resultWindowManager;

    [SerializeField] private Image[] _characterImages = null;
    [SerializeField] private Text _textBox = null;
    [SerializeField] private ResultWindowFlavorText _resultWindowFlavorText = null;

    public NoteManager.NoteType GetWorstCharacterType(int stageIdx) => _resultWindowFlavorText.GetWorstType(stageIdx);

    public void ShowResultWindow(bool show) {
        gameObject.SetActive(show);
    }

    [SerializeField] private Sprite _clearSprite = null;

    public void SetStageClearResultWindow(int stageIdx, NoteManager.NoteType type) {
        var result = _resultWindowFlavorText.GetResult(stageIdx, type);
        _textBox.text = result.ResultText;
        for (int j = 0; j < _characterImages.Length; j++) {
            if((int)result.Type == j) {
                _characterImages[j].GetComponentInChildren<Text>().transform.parent.gameObject.SetActive(false);
                _characterImages[j].transform.Find("Icon").GetComponent<Image>().sprite = _clearSprite;
            }
            else {
                _characterImages[j].gameObject.SetActive(false);
            }
        }

        ShowResultWindow(true);
    }

    public void InitializeResultWindow() {
        ShowResultWindow(false);
        for (int i = 0; i < _characterImages.Length; i++) {
            _characterImages[i].GetComponentInChildren<Text>().text = _resultWindowFlavorText.GetOptionText(PointManager.GetPointManager.StageNum, (NoteManager.NoteType)i);
        }
        _textBox.text = _resultWindowFlavorText.GetProblemText(PointManager.GetPointManager.StageNum);
    }

    private void Awake() {
        _resultWindowManager = this;
    }
}
