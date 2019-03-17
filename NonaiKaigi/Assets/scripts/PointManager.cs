using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    private static PointManager _pointManager = null;
    public static PointManager GetPointManager => _pointManager;

    [SerializeField] private FlavorTextObject _flavorTextObject = null;

    [SerializeField] private Sprite[] _icons = null;

    private int _stageNum = 0;
    public int StageNum => _stageNum;

    //キャラクターを管理するクラス
    private class Character
    {
        public Image Image = null;
        public Image Icon = null;
        public Text FText = null;
        public float PercentageVal = 0;
        public Text PercentageText = null;

        private int _flavorIdx = 0;
        private float _flavorTime = 0;

        private NoteManager.NoteType _myNoteType;
        public NoteManager.NoteType MyNoteType => _myNoteType;

        private FlavorText.CharacterStatus _status = default(FlavorText.CharacterStatus);

        public Character(Transform t, NoteManager.NoteType type) {
            Image = t.Find("CharacterImage").GetComponent<Image>();
            Icon = t.Find("Icon").GetComponent<Image>();
            FText = t.Find("FlavorText").GetComponentInChildren<Text>();
            PercentageText = t.Find("Percentage").GetComponentInChildren<Text>();
            _myNoteType = type;
            PercentageVal = 33;
            PercentageText.text = $"{PercentageVal}%";

            _status = FlavorText.CharacterStatus.Low;
            FText.text = $"{GetPointManager._flavorTextObject.GetFlavorText(GetPointManager._stageNum, this._status, 0, this._myNoteType)}";
        }

        private const float _lowToNormal = 45;
        private const float _normalToHigh = 70;

        //ステータス値等を更新する
        public void UpdateStatus(bool add) {
            var val = PercentageVal;
            if (add) {
                PercentageVal += 2;
                if(PercentageVal >= 99) {
                    PercentageVal = 99;
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
                    _status = FlavorText.CharacterStatus.Normal;
                    NoteManager.GetNoteManager.ChangeSpeed(true);
                }
                else {
                    _status = FlavorText.CharacterStatus.Low;
                    NoteManager.GetNoteManager.ChangeSpeed(false);
                }
            }
            else if (val < _normalToHigh && PercentageVal >= _normalToHigh) {
                if (add) {
                    _status = FlavorText.CharacterStatus.Super;
                    NoteManager.GetNoteManager.ChangeSpeed(true);
                }
                else {
                    _status = FlavorText.CharacterStatus.Normal;
                    NoteManager.GetNoteManager.ChangeSpeed(false);
                }
            }

            if(status != _status) {
                FText.text = GetPointManager._flavorTextObject.GetFlavorText(GetPointManager._stageNum, this._status, _flavorIdx, this._myNoteType);
            }
        }

        public void UpdateFlavorText() {
            _flavorTime += Time.deltaTime;
            if(_flavorTime >= 6.0f) {
                _flavorTime = 0;
                _flavorIdx = (_flavorIdx + 1) % 3;
                FText.text = GetPointManager._flavorTextObject.GetFlavorText(GetPointManager._stageNum, this._status, _flavorIdx, this._myNoteType);
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
        var highestCharacter = _characters.Aggregate((x, y) => x.PercentageVal >= y.PercentageVal ? x : y);
        foreach(var chara in _characters) {
            if(chara == highestCharacter) {
                chara.Icon.sprite = _icons[0];
            }
            else {
                chara.Icon.sprite = _icons[1];
            }
        }

    }

    [SerializeField] private Text _timerText = null;
    [SerializeField]float _timerVal = 0;

    private void Awake() {
        _pointManager = this;
    }

    void Start()
    {
        _stageNum = PlayerPrefs.GetInt(Keys.KeyList[Keys.KeyTag.StageType], -1);
        if (_stageNum == -1) {
            _stageNum = 0;
        }

        int i = 0;
        foreach(Transform t in transform) {
            _characters[i] = new Character(t, (NoteManager.NoteType)i);
            i++;
        }

        NoteManager.GetNoteManager.InitializeNotemanager();
        ResultWindowManager.GetResultWindowManger.InitializeResultWindow();
    }

    private bool _clear = false;

    //セーブデータ用のオブジェクト
    [System.Serializable]
    public class ResultObject
    {
        public int Stage = 0;
        public NoteManager.NoteType Type = default(NoteManager.NoteType);
        public float Percentage = 0;

        public ResultObject(int stage, NoteManager.NoteType type, float percentage) {
            Stage = stage;
            Type = type;
            Percentage = percentage;
        }
    }

    [System.Serializable]
    public class ResultObjectGroup
    {
        public List<ResultObject> ResultObjects = new List<ResultObject>();
    }

        //JSON形式のセーブデータをロードする
    public static ResultObjectGroup LoadSaveData() {
        string filePath = $"{Application.streamingAssetsPath}/{"SaveData.json"}";

        if (File.Exists(filePath)) {
            string dataAsJson = File.ReadAllText(filePath);
            ResultObjectGroup loadedData = JsonUtility.FromJson<ResultObjectGroup>(dataAsJson);
            
            Debug.Log("Succesfully loaded data");
            return loadedData;
        }
        else {
            Debug.LogError("Cannot Load PlayerInfo");
            return null;
        }

    }

    //JSON形式のファイルへデータをセーブし、そのパスを返す
    private void SaveData(Character chara) {

        var data = LoadSaveData();
        if(data == null) {
            string filePath =  $"{Application.streamingAssetsPath}/SaveData.json";
            var group = new ResultObjectGroup();
            group.ResultObjects.Add(new ResultObject(_stageNum, chara.MyNoteType, chara.PercentageVal));
            var dataAsJson = JsonUtility.ToJson(group);
            File.WriteAllText(filePath, dataAsJson);
            Debug.Log("Created new file");
            return;
        }
        else {
            ResultObject res;
            if (data.ResultObjects.Any(result => result.Stage == _stageNum)) {
                res = data.ResultObjects.FirstOrDefault(r => r.Stage == _stageNum);
                res.Type = chara.MyNoteType;
                res.Percentage = chara.PercentageVal;
            }
            else {
                res = new ResultObject(_stageNum, chara.MyNoteType, chara.PercentageVal);
                data.ResultObjects.Add(res);
            }
            var dataAsJson = JsonUtility.ToJson(data);
            File.WriteAllText($"{Application.streamingAssetsPath}/SaveData.json", dataAsJson);
            return;
        }
    }

    private void Update() {
        if (_clear) return;

        var seconds = (_timerVal - Time.deltaTime) % 60;
        if (seconds < 0) seconds = 0;
        var minutes = Mathf.Floor((_timerVal - seconds) / 60);

        _timerText.text = $"{minutes:00}:{Mathf.Floor(seconds):00}";
        _timerVal = (minutes * 60) + seconds;
        if(_timerVal <= 0) {
            Time.timeScale = 0;
            _clear = true;
            var highestCharacter = _characters.Aggregate((x, y) => x.PercentageVal >= y.PercentageVal ? x : y);
            SaveData(highestCharacter);
            ResultWindowManager.GetResultWindowManger.SetStageClearResultWindow(_stageNum, highestCharacter.MyNoteType);
            PlayerController.GetPlayer.SetClearStageWindow();
        }

        foreach(var chara in _characters) {
            chara.UpdateFlavorText();
        }
    }
}
