using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ノートの挙動を制御するスクリプト
public class NoteController : MonoBehaviour
{

    private bool _inCanDestroy = false;
    public bool InCanDestroy => _inCanDestroy;

    private bool _inNotDestroy = false;
    public bool InNotDestroy => _inNotDestroy;

    private float _speed = 0;

    private NoteManager.LaneType _laneType = default(NoteManager.LaneType);
    public NoteManager.LaneType LaneType => _laneType;

    private NoteManager.NoteType _noteType = default(NoteManager.NoteType);
    public NoteManager.NoteType NoteType => _noteType;

    public void SetUpNote(NoteManager.LaneType lane, NoteManager.NoteType note, Sprite sprite, float spd, Vector2 pos) {
        _laneType = lane;
        _noteType = note;

        var image = GetComponent<Image>();
        image.sprite = sprite;

        _speed = spd;
        transform.position = pos;
    }

    public void ChangeSpeed(float sp) {
        _speed = sp;
    }

    private void Update() {
        transform.position = new Vector3(transform.position.x - _speed * Time.deltaTime, transform.position.y, transform.position.z);
    }

    //ノートとの衝突用のレイヤー
    private const int _NOTEDESTROY_LAYER = 9;
    private const int _NOTENOTDESTROY_LAYER = 10;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == _NOTEDESTROY_LAYER) {
            _inCanDestroy = true;
        }
        else if (collision.gameObject.layer == _NOTENOTDESTROY_LAYER) {
            _inNotDestroy = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        _inCanDestroy = false;
    }


}
