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
    /// <summary>移動</summary>
    Move,
    /// <summary>キャラ揺れる</summary>
    Shake,
    /// <summary>画面の色変更</summary>
    Coloring,
    /// <summary>画面の色変更</summary>
    SwitchColor,
    /// <summary>画面の色変更解除</summary>
    Clear,
    /// <summary>背景変更</summary>
    SwitchBack,
    /// <summary>ウィンドウがポップする</summary>
    PopWindow,
    NextScene,
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
        Morning,
        Noon,
        Evening,
        Night,
    }


    //delegate void Stagings();
    string[] contents;
    public List<GameObject> characters = new List<GameObject>();
    [SerializeField] TextManager textManager;
    [SerializeField] GameObject blackOut;
    //[SerializeField] GameObject[] popWindows;
    //[SerializeField] Transform popCanvas;
    [SerializeField] Image backGround;

    [SerializeField] Sprite[] BackSprites = new Sprite[4];


    //SceneChanger fader;
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



    void Awake()
    {
        SwitchColor(Color.black);
    }

    void Start()
    {
        if (textManager.texts[0].cName != CharName.System)
        {
            StartCoroutine(ChangeColor(blackOut, Color.clear, 0.5f, false));
        }
    }



    void DivideContent(string content)
    {
        //string[] tmpTexts = content.Split(':');
        contents = content.Split(':');
        //Debug.Log(contents);
    }

    public void Staging(string content)
    {
        Debug.Log(content);
        textManager.isStaging = true;
        DivideContent(content);
        //StageType type;

        if (Enum.TryParse(contents[(int)StageTag.Type], out StageType type))
        {
            switch (type)
            {
                case StageType.SceneTrans:
                    SceneTrans(contents[(int)StageTag.Target]);
                    break;
                case StageType.Move:
                    Move(contents[(int)StageTag.Target]);
                    break;
                case StageType.Shake:
                    Shake();
                    break;
                case StageType.Coloring:
                    Coloring(contents[(int)StageTag.Target]);
                    break;
                case StageType.SwitchColor:
                    SwitchColor(contents[(int)StageTag.Target]);
                    break;
                case StageType.Clear:
                    Clear();
                    break;
                case StageType.SwitchBack:
                    SwitchBack(contents[(int)StageTag.Target]);
                    break;
                case StageType.PopWindow:
                    PopWindow(contents[(int)StageTag.Target]);
                    break;
                case StageType.NextScene:
                    NextScene();
                    break;
                default:
                    break;
            }
        }

    }

    void SceneTrans(string content)
    {
        StartCoroutine(ChangeColor(blackOut, Color.black, 0.5f));
        if (Enum.TryParse(content, out SceneChanger.SceneTitle title))
        {

            SceneChanger.SceneChange(title);

        }

        Debug.Log(content);
        EndStaging();
    }
    void Move(string content)
    {

        Debug.Log(content);
        EndStaging();
    }
    void Shake()
    {

        Debug.Log("shake");
        EndStaging();
    }
    /// <summary>カラーコード</summary>
    /// <param name="content"></param>
    void Coloring(string content)
    {
        Debug.Log(content);
        Color color;
        if (ColorUtility.TryParseHtmlString(content, out color))
        {
            Debug.Log(color);
            blackOut.SetActive(true);
            StartCoroutine(ChangeColor(blackOut, color, 0.5f));

        }
    }
    void Coloring(Color color)
    {
        StartCoroutine(ChangeColor(blackOut, color, 0.5f));
    }
    void SwitchColor(string content)
    {
        Debug.Log(content);
        Color color;
        if (ColorUtility.TryParseHtmlString(content, out color))
        {
            Debug.Log(color);
            blackOut.SetActive(true);
            blackOut.GetComponent<Image>().color = color;
        }
        EndStaging();
    }
    void SwitchColor(Color color)
    {
        blackOut.SetActive(true);
        blackOut.GetComponent<Image>().color = color;
    }
    void Clear()
    {
        StartCoroutine(ChangeColor(blackOut, Color.clear, 0.5f));
    }
    void SwitchBack(string content)
    {
        Debug.Log(content);
        BackIndex index;
        if (Enum.TryParse(content, out index))
        {
            backGround.sprite = BackSprites[(int)index];
        }
        EndStaging();
    }
    void PopWindow(string content)
    {

        Debug.Log(content);
        EndStaging();
    }

    void NextScene()
    {

        EndStaging();
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
            targetObject.SetActive(true);
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
            targetObject.GetComponent<Image>().color = fromColor;
            //Debug.Log(fromColor);
            yield return null;
        }
        Debug.Log("end");
        targetObject.GetComponent<Image>().color = toColor;
        if (toColor == Color.clear)
        {
            targetObject.SetActive(false);
        }
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
        Debug.Log("EndStaging");
        textManager.isStaging = false;
        textManager.TextDraw();
    }

    //void ContinueStaging()
    //{
    //    textManager.isStaging = true;
    //}

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
