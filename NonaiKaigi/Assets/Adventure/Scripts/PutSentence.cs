using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PutSentence : MonoBehaviour
{
    /// <summary>/// 文字送り速度/// </summary>
    const float charFeedSpeed = 0.1f;
    /// <summary>/// 表示用のTextComponent/// </summary>
    [SerializeField] Text text;
    /// <summary>/// /// </summary>
    //TextStorage textContena = new TextStorage();
    /// <summary>/// 現在表示している文字列/// </summary>
    string sentence;
    /// <summary>/// /// </summary>
    int charCount = 0;
    /// <summary>コルーチンが終了しているか </summary>
    bool end = true;
    ///// <summary>全文表示しているか</summary>
    //public bool onoff;
    /// <summary>コルーチンを保存する</summary>
    IEnumerator feedCoroutine;
    [SerializeField] Text nameArea;
    public bool End { get { return end; } set { end = value; } }

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        text.text = "";
        nameArea.text = "";
    }


    /// <summary>引数のStringを一文字ずつ表示する。endがtrueならコルーチンが終了</summary>
    public IEnumerator SentenceFeed(string s)
    {
        sentence = s;
        text.text = "";
        for (charCount = 0; charCount < sentence.Length; charCount++)
        {
            text.text += sentence[charCount];

            yield return new WaitForSeconds(charFeedSpeed);
        }
        End = true;
    }

    /// <summary>テキストをすべて表示する。</summary>
    public void FullTexts()
    {
        if (feedCoroutine != null) { StopCoroutine(feedCoroutine); }
        text.text = sentence;
        End = true;
    }
    /// <summary>コルーチンを開始</summary>
    public void CallSentence(string s, string n)
    {
        nameArea.text = n;
        if (End)
        {
            feedCoroutine = SentenceFeed(s);
            StartCoroutine(feedCoroutine);
            End = false;
        }
        else
        {
            FullTexts();
        }
    }


}


