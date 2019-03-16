using UnityEngine;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour
{

    private static NoteManager _noteManager = null;
    public static NoteManager GetNoteManager => _noteManager;

    [SerializeField] private GameObject _nodePrefab = null;
    //ノートを管理するためのリスト
    private List<NoteController> _noteList = new List<NoteController>();

    //ノートが走っているノートの場所を判定するためのenum
    public enum LaneType
    {
        UP, MIDDLE, DOWN, MAX
    }

    public enum NoteType
    {
        A, B, C, MAX
    }

    [SerializeField] private RectTransform[] _lanes = null;
    public float GetLaneYPos(LaneType type) => _lanes[(int)type].position.y;

    private void Awake() {
        _noteManager = this;
    }

    private float _time = 0;

    [SerializeField] private float _speed = 500;

    public void ChangeSpeed(bool raiseSpeed) {
        if (raiseSpeed) {
            _speed += 150;
        }
        else {
            _speed -= 150;
        }

        _noteList.ForEach(note => note.ChangeSpeed(_speed));
    }

    void Update() {
        _time += Time.deltaTime;
        if(_time >= 1) {
            _time = 0;
            Random.InitState(System.DateTime.Now.TimeOfDay.Milliseconds);
            var randomLane = Random.Range(0, (int)LaneType.MAX);
            Random.InitState(System.DateTime.Now.TimeOfDay.Seconds);
            var randomType = Random.Range(0, (int)NoteType.MAX);
            var note = GameObject.Instantiate(_nodePrefab, this.transform, true).GetComponent<NoteController>();
            note.SetUpNote(
                (LaneType)randomLane,   //LaneType
                (NoteType)randomType,
                _speed,                //Speed
                new Vector2(_lanes[randomLane].position.x + _lanes[randomLane].rect.size.x / 2, _lanes[randomLane].position.y)      //Pos
                );

            _noteList.Add(note);
        }

        var Destroylist = new List<NoteController>();
        var NotDestroyList = new List<NoteController>();
        _noteList.ForEach(note => {
            if(note.InCanDestroy && PlayerController.GetPlayer.CurrentLane == note.LaneType) {
                Destroylist.Add(note);
            }
            else if (note.InNotDestroy) {
                NotDestroyList.Add(note);
            }
        });

        foreach(var dl in Destroylist) {
            _noteList.Remove(dl);
            Destroy(dl.gameObject);
        }

        foreach(var ndl in NotDestroyList) {
            PointManager.GetPointManager.UpdateCharacterStatus(ndl.NoteType);
            _noteList.Remove(ndl);
            Destroy(ndl.gameObject);
        }
    }
}
