using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _player = null;
    public static PlayerController GetPlayer => _player;

    private NoteManager.LaneType _currentLane = default(NoteManager.LaneType);
    public NoteManager.LaneType CurrentLane => _currentLane;

    //_currentLaneの値に従って座標を決定する
    private void SetPos() {
        if(_currentLane == NoteManager.LaneType.MAX) {
            Debug.Log("Wrong Type");
            return;
        }
        var ypos = NoteManager.GetNoteManager.GetLaneYPos(_currentLane);
        transform.position = new Vector3(transform.position.x, ypos);
    }

    private void Awake() {
        _player = this;
        _currentLane = NoteManager.LaneType.MIDDLE;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            var val = (NoteManager.LaneType)((int)_currentLane - 1);
            if(val < 0) {
                _currentLane = NoteManager.LaneType.DOWN;
            }
            else {
                _currentLane = val;
            }
            SetPos();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            _currentLane = (NoteManager.LaneType)(((int)_currentLane + 1) % (int)NoteManager.LaneType.MAX);
            SetPos();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if(Time.timeScale != 0) {
                Time.timeScale = 0;
            }
            else {
                Time.timeScale = 1;
            }
        }
    }
}
