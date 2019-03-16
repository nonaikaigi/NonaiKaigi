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

    public void ShowResultWindow(bool show) {
        gameObject.SetActive(show);
    }

    public void SetStageClearResultWindow(int i, NoteManager.NoteType type) {
        var result = _resultWindowFlavorText.GetResult(i, type);
        _textBox.text = result.ResultText;
        for (int j = 0; j < _characterImages.Length; j++) {
            if((int)result.Type == j) {
                _characterImages[j].sprite = result.ClearSprite;
            }
            else {
                _characterImages[j].gameObject.SetActive(false);
            }
        }

        ShowResultWindow(true);
    }

    private void Awake() {
        _resultWindowManager = this;
        ShowResultWindow(false);
        _textBox.text = _resultWindowFlavorText.GetProblemText(0);
    }
}
