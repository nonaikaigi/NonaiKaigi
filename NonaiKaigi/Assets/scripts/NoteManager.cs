using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour
{

    private static NoteManager _noteManager = null;
    public static NoteManager GetNoteManager => _noteManager;

    [SerializeField] private GameObject _nodePrefab = null;
    [SerializeField] private Sprite[] _noteSprites = null;
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

    [SerializeField] private AudioClip _audioClip = null;
    private AudioSource _audioSource = null;

    public void InitializeNotemanager() {
        _moveSpeed = _noteSpeedObject.GetNoteSpeed(PointManager.GetPointManager.StageNum).MoveSpeed;
        _audioSource = this.gameObject.AddComponent<AudioSource>();
        _audioSource.clip = _audioClip;
    }

    private void Awake() {
        _noteManager = this;
    }

    [SerializeField] NoteSpeedObject _noteSpeedObject = null;
    private float _moveSpeed = 0;
    private float _generationTime = 0;

    public void ChangeSpeed(bool raiseSpeed) {
        if (raiseSpeed) {
            _moveSpeed += _noteSpeedObject.GetNoteSpeed(PointManager.GetPointManager.StageNum).MoveSpeed * 0.25f;
        }
        else {
            _moveSpeed -= _noteSpeedObject.GetNoteSpeed(PointManager.GetPointManager.StageNum).MoveSpeed * 0.25f;
        }

        _noteList.ForEach(note => note.ChangeSpeed(_moveSpeed));
    }

    void Update() {
        _generationTime += Time.deltaTime;
        if(_generationTime >= _noteSpeedObject.GetNoteSpeed(PointManager.GetPointManager.StageNum).GenerationSpeed) {
            _generationTime = 0;
            Random.InitState(System.DateTime.Now.TimeOfDay.Milliseconds);
            var randomLane = Random.Range(0, (int)LaneType.MAX);
            Random.InitState(System.DateTime.Now.TimeOfDay.Seconds);
            var randomType = Random.Range(0, (int)NoteType.MAX);
            var note = GameObject.Instantiate(_nodePrefab, this.transform, true).GetComponent<NoteController>();
            note.SetUpNote(
                (LaneType)randomLane,   //LaneType
                (NoteType)randomType,
                _noteSprites[randomType],
                _moveSpeed,                //Speed
                new Vector2(_lanes[randomLane].position.x + _lanes[randomLane].rect.size.x / 2, _lanes[randomLane].position.y)      //Pos
                );

            _noteList.Add(note);
        }

        var destroylist = new List<NoteController>();
        var notDestroyList = new List<NoteController>();
        _noteList.ForEach(note => {
            if(note.InCanDestroy && PlayerController.GetPlayer.CurrentLane == note.LaneType) {
                destroylist.Add(note);
            }
            else if (note.InNotDestroy) {
                notDestroyList.Add(note);
            }
        });
        if (destroylist.Count > 0) {
            foreach (var dl in destroylist) {
                _noteList.Remove(dl);
                Destroy(dl.gameObject);
            }
            _audioSource.Play();
            PlayerController.GetPlayer.Animator.Play("Cut");
        }

        if (notDestroyList.Count > 0) {
            foreach (var ndl in notDestroyList) {
                PointManager.GetPointManager.UpdateCharacterStatus(ndl.NoteType);
                _noteList.Remove(ndl);
                Destroy(ndl.gameObject);
            }
        }
    }
}
