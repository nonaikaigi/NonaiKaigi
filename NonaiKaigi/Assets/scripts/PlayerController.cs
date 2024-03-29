﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _player = null;
    public static PlayerController GetPlayer => _player;

    private NoteManager.LaneType _currentLane = default(NoteManager.LaneType);
    public NoteManager.LaneType CurrentLane => _currentLane;

    private Animator _animator = null;
    public Animator Animator => _animator;

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
        _animator = GetComponent<Animator>();
    }

    private bool _isPaused = false;
    private bool _clearedStage = false;

    public void PauseGame() {
        _isPaused = true;
        Time.timeScale = 0;
    }

    public void SetClearStageWindow() {
        _clearedStage = true;
    }

    void Update()
    {
        if (_clearedStage) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                //Load Scene
                Time.timeScale = 1;
                SceneChanger.SceneChange(SceneChanger.SceneTitle.Adventure);
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if(!_isPaused) {
                Time.timeScale = 0;
                _isPaused = true;
                ResultWindowManager.GetResultWindowManger.ShowResultWindow(true);
            }
            else {
                Time.timeScale = 1;
                _isPaused = false;
                ResultWindowManager.GetResultWindowManger.ShowResultWindow(false);
                PointManager.GetPointManager.ClearHowToCanvas();
            }
        }

        if (_isPaused) return;

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            var val = (NoteManager.LaneType)((int)_currentLane - 1);
            if (val < 0) {
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
    }
}
