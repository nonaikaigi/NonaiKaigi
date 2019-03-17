using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NoteSpeed", menuName = "Note/NoteSpeed")]
public class NoteSpeedObject : ScriptableObject
{

    public NoteSpeed[] _noteSpeed = null;

    public NoteSpeed GetNoteSpeed(int stageIdx) => _noteSpeed[stageIdx];

    [System.Serializable]
    public class NoteSpeed
    {
        public float MoveSpeed = 0;
        [Range(0, 1f)]
        public float GenerationSpeed = 0;
    }
}
