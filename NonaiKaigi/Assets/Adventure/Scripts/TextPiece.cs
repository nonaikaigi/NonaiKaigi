using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPiece : MonoBehaviour
{

    public string name;
    public string sentence;
    [SerializeField] Text nameArea;
    [SerializeField] Text sentenceArea;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(string s, string n)
    {
        sentence = s;
        name = n;
        nameArea.text = name;
        sentenceArea.text = sentence;
    }

}
