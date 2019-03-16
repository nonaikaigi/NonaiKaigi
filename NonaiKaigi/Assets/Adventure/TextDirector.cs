using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;


public enum StageType
{
    /// <summary>シーン遷移</summary>
    SceneTrans,
    /// <summary>登場</summary>
    Appear,
    /// <summary>退場</summary>
    Leave,
    /// <summary>画面の色変更</summary>
    Coloring,
    /// <summary>画面の色変更解除</summary>
    Clear,
    /// <summary>背景変更</summary>
    SwitchBack,
    /// <summary>アイテム出現</summary>
    Item,
    /// <summary>ウィンドウがポップする</summary>
    PopWindow,
}

public class TextDirector : MonoBehaviour
{
    //public List<GameObject> Characters = new List<GameObject>();

    enum StageTag
    {
        Type, Target, Content
    }


    enum BackIndex
    {
        Evening,
        Noon,
        Edge,

    }


    delegate void Stagings();
    string[] contents;
    public List<GameObject> characters = new List<GameObject>();
    [SerializeField] TextManager textManager;
    [SerializeField] GameObject blackOut;
    //[SerializeField] GameObject[] popWindows;
    //[SerializeField] Transform popCanvas;
    [SerializeField] SpriteRenderer backGround;

    SceneChanger fader;
    float moveTime = 0.5f;

    List<Vector3> targetPositions = new List<Vector3>()
        {
            new Vector2(300,0),
            new Vector2(-300,0),
            new Vector2(0,0),
        };
    List<Vector3> startPositions = new List<Vector3>()
        {
            new Vector2(700,0),
            new Vector2(-700,0),
            new Vector2(0,0),
        };
    public List<Cast> casts = new List<Cast>();


    void DivideContent(string content)
    {
        //string[] tmpTexts = content.Split(':');
        contents = content.Split(':');
        //Debug.Log(contents);
    }




    IEnumerator MoveObject(GameObject targetObj, Vector3 targetPos, float time, bool isNext = true)
    {
        //ContinueStaging();
        float start = Time.time;
        Vector3 diff = targetPos - targetObj.transform.localPosition;
        while ((0 < diff.x
            && 0 < (targetPos - targetObj.transform.localPosition).x)
            || (diff.x < 0
            && (targetPos - targetObj.transform.localPosition).x < 0)
            && Time.time < start + time)
        {
            // ContinueStaging();
            targetObj.transform.localPosition += diff * Time.deltaTime / time;
            yield return null;
        }
        if (isNext)
        {
            EndStaging();
        }
        else
        {
            textManager.isStaging = false;
        }
        //while (dif.x < 0
        //    && (targetPos - targetObj.transform.localPosition).x < 0)
        //{
        //    targetObj.transform.localPosition += dif / 100;
        //    yield return new WaitForSeconds(0.01f);
        //}
    }

    IEnumerator ChangeColor(GameObject targetObject, Color toColor, float time, bool isNext = true)
    {
        Color fromColor;
        float start = Time.time;
        //if (targetObject.GetComponent<SpriteRenderer>())
        //{
        //    fromColor = targetObject.GetComponent<SpriteRenderer>().color;
        //}
        //else 
        if (targetObject.GetComponent<Image>())
        {
            fromColor = targetObject.GetComponent<Image>().color;
        }
        else
        {
            yield break;
        }
        float diffR = toColor.r - fromColor.r;
        float diffG = toColor.g - fromColor.g;
        float diffB = toColor.b - fromColor.b;
        float diffA = toColor.a - fromColor.a;

        while (Time.time < start + time)
        {
            //ContinueStaging();
            fromColor.r += diffR * Time.deltaTime / time;
            fromColor.g += diffG * Time.deltaTime / time;
            fromColor.b += diffB * Time.deltaTime / time;
            fromColor.a += diffA * Time.deltaTime / time;
            targetObject.GetComponent<SpriteRenderer>().color = fromColor;
            Debug.Log(fromColor);
            yield return null;
        }
        Debug.Log("end");
        targetObject.GetComponent<SpriteRenderer>().color = toColor;
        if (isNext)
        {
            EndStaging();
        }
        else
        {
            textManager.isStaging = false;
        }
    }


    void EndStaging()
    {
        textManager.isStaging = false;
        textManager.TextsDraw();
    }

    void ContinueStaging()
    {
        textManager.isStaging = true;
    }

}



public class Cast
{
    public CharName name;
    public PositionTag posTag;

    public Cast(CharName charName, PositionTag positionTag)
    {
        name = charName;
        posTag = positionTag;
    }
    public Cast()
    {
        name = CharName.None;
        posTag = PositionTag.None;
    }
}
