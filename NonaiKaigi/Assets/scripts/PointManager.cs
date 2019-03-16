using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    private static PointManager _pointManager = null;
    public static PointManager GetPointManager => _pointManager;

    //キャラクターを管理するクラス
    private class Character
    {
        public Image Image = null;
        public Text FlavorText = null;
        public float PercentageVal = 0;
        public Text PercentageText = null;

        private NoteManager.NoteType _myNoteType;
        public NoteManager.NoteType MyNoteType => _myNoteType;

        private enum CharacterStatus
        {
            Low, Normal, Super,
        }

        private CharacterStatus _status = default(CharacterStatus);

        public Character(Transform t, NoteManager.NoteType type) {
            Image = t.Find("CharacterImage").GetComponent<Image>();
            FlavorText = t.Find("FlavorText").GetComponentInChildren<Text>();
            PercentageText = t.Find("Percentage").GetComponentInChildren<Text>();
            _myNoteType = type;

            FlavorText.text = $"お寿司食べたい";
            PercentageVal = 33;
            PercentageText.text = $"{PercentageVal}%";

            _status = CharacterStatus.Low;
        }

        private const float _lowToNormal = 45;
        private const float _normalToHigh = 70;

        //ステータス値等を更新する
        public void UpdateStatus(bool add) {
            var val = PercentageVal;
            if (add) {
                PercentageVal += 2;
                if(PercentageVal >= 100) {
                    PercentageVal = 100;
                }
            }
            else {
                PercentageVal -= 1;
                if(PercentageVal <= 0) {
                    PercentageVal = 0;
                }
            }

            PercentageText.text = $"{PercentageVal}%";

            var status = _status;

            if (val < _lowToNormal && PercentageVal >= _lowToNormal) {
                if (add) {
                    _status = CharacterStatus.Normal;
                    NoteManager.GetNoteManager.ChangeSpeed(true);
                }
                else {
                    _status = CharacterStatus.Low;
                    NoteManager.GetNoteManager.ChangeSpeed(false);
                }
            }
            else if (val < _normalToHigh && PercentageVal >= _normalToHigh) {
                if (add) {
                    _status = CharacterStatus.Super;
                    NoteManager.GetNoteManager.ChangeSpeed(true);
                }
                else {
                    _status = CharacterStatus.Normal;
                    NoteManager.GetNoteManager.ChangeSpeed(false);
                }
            }

            if(status != _status) {
                if(_status == CharacterStatus.Low) {
                    FlavorText.text = $"お寿司食べたい";
                }
                else if(_status == CharacterStatus.Normal) {
                    FlavorText.text = $"焼肉食べたい";
                }
                else {
                    FlavorText.text = $"石油王と結婚したい";
                }
            }
        }
    }

    private Character[] _characters = new Character[(int)NoteManager.NoteType.MAX];

    //キャラクターのステータスを更新する
    public void UpdateCharacterStatus(NoteManager.NoteType type) {
        foreach(var c in _characters) {
            if(c.MyNoteType == type) {
                c.UpdateStatus(true);
            }
            else {
                c.UpdateStatus(false);
            }
        }
    }

    [SerializeField] private Text _timerText = null;
    float _timerVal = 0;

    private void Awake() {
        _pointManager = this;
    }

    void Start()
    {
        int i = 0;
        foreach(Transform t in transform) {
            _characters[i] = new Character(t, (NoteManager.NoteType)i);
            i++;
        }

        _timerVal = 90;
    }

    private void Update() {
        Debug.Log((_timerVal - Time.deltaTime) % 60);
    }
}
