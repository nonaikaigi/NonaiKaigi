using System;
using System.Linq;
//using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum CharName
{
    Lilia, Mother, Sister, System, None
}
public class TextManager : MonoBehaviour
{
    //List<CharName> characters = new List<CharName>();

    public List<TextStorage> texts = new List<TextStorage>();

    [SerializeField] Text nameText;
    [SerializeField] Progress progress;
    [SerializeField] PutSentence putSentence;
    [SerializeField] FaceChanger faceChanger;
    [SerializeField] TextDirector director;
    [SerializeField] Image back;
    [SerializeField] Sprite[] backs = new Sprite[4];

    [SerializeField] GameObject enter;

    #region Mod

    [SerializeField] GameObject textPiece;
    [SerializeField] GameObject logPanel;
    [SerializeField] Transform logParent;

    /// <summary>name,text</summary>
    List<KeyValuePair<string, string>> logs = new List<KeyValuePair<string, string>>();

    const float autoInterval = 1f;

    bool isAuto = false;

    bool isOpen = false;

    IEnumerator autoCoroutine = null;



    IEnumerator LateText()
    {
        stime = Time.time;
        yield return new WaitForSeconds(autoInterval);
        ftime = Time.time;
        Debug.Log(ftime - stime);
        TextDraw();
    }



    public void SetAuto(bool auto)
    {
        isAuto = auto;
    }


    void LogReCreate()
    {
        foreach (Transform child in logParent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < logs.Count - 1; i++)
        {
            Instantiate(textPiece, logParent).GetComponent<TextPiece>().Init(logs[i].Value, logs[i].Key);
        }
    }
    public void LogOpen()
    {
        isOpen = true;
        Time.timeScale = 0f;
        LogReCreate();
        logPanel.SetActive(true);
    }

    public void LogClose()
    {
        isOpen = false;
        Time.timeScale = 1f;
        logPanel.SetActive(false);
    }
    #endregion

    List<TextActor> actors = new List<TextActor>();

    int textIndex = -1;

    public bool isStaging = false;

    float stime = 0;
    float ftime = 0;

    private void Awake()
    {
        //progress.ThisStoryProgress = Progress.StoryProgress.TextA;
        var data = PointManager.LoadSaveData().ResultObjects;
        int index = 0;
        switch (progress.ThisStoryProgress)
        {
            case Progress.StoryProgress.TextA:
                SetText(progress.ThisStoryProgress);
                SwitchBack(0);
                break;
            case Progress.StoryProgress.ResultA:
                index = (int)data.Find(x => x.Stage == 0).Type;
                SetText(progress.ThisStoryProgress, (Progress.ChoiceTag)index);
                SwitchBack(0);
                break;
            case Progress.StoryProgress.TextB:
                SetText(progress.ThisStoryProgress);
                SwitchBack(1);
                break;
            case Progress.StoryProgress.ResultB:
                index = (int)data.Find(x => x.Stage == 1).Type;
                SetText(progress.ThisStoryProgress, (Progress.ChoiceTag)index);
                SwitchBack(1);
                break;
            case Progress.StoryProgress.TextC:
                SetText(progress.ThisStoryProgress);
                SwitchBack(2);
                break;
            case Progress.StoryProgress.ResultC:
                index = (int)data.Find(x => x.Stage == 2).Type;
                SetText(progress.ThisStoryProgress, (Progress.ChoiceTag)index);
                SwitchBack(2);
                break;
            case Progress.StoryProgress.TextEnd:
                SetText(progress.ThisStoryProgress);
                SwitchBack(3);
                break;
            case Progress.StoryProgress.ResultEnd:
                SetText(progress.ThisStoryProgress, GetResult());
                SwitchBack(3);
                break;
            default:
                break;
        }
        back.color = Color.white;

    }

    void SwitchBack(int i)
    {
        back.sprite = backs[i];
    }

    //BCA
    //AAB
    //CBC
    static NoteManager.NoteType[] high = new NoteManager.NoteType[3] { NoteManager.NoteType.B, NoteManager.NoteType.C, NoteManager.NoteType.A };
    static NoteManager.NoteType[] normal = new NoteManager.NoteType[3] { NoteManager.NoteType.A, NoteManager.NoteType.A, NoteManager.NoteType.B };
    static NoteManager.NoteType[] low = new NoteManager.NoteType[3] { NoteManager.NoteType.C, NoteManager.NoteType.B, NoteManager.NoteType.C };

    static int CheckResult(int wave, NoteManager.NoteType type)
    {
        if (high[wave] == type)
        {
            return 2;
        }
        if (normal[wave] == type)
        {
            return 1;
        }
        //if (low[wave] == type)
        //{
        return 0;
        //}

    }




    public static Progress.ChoiceTag GetResult()
    {
        var data = PointManager.LoadSaveData().ResultObjects;
        int num = 0;
        foreach (var item in data)
        {
            num += CheckResult(item.Stage, item.Type);
        }
        if (4 <= num)
        {
            return Progress.ChoiceTag.A;
        }
        else if (num >= 1)
        {
            return Progress.ChoiceTag.C;
        }
        else
        {
            return Progress.ChoiceTag.B;
        }
    }

    void Start()
    {
        logs.Clear();
        logPanel.SetActive(false);
        actors = Resources.LoadAll<TextActor>("CharacterAsset").ToList();
        //putSentence.CallSentence(texts[0].sentence, GetName(texts[0]));   
        TextDraw();

    }


    void Update()
    {
        if (putSentence.End)
        {
            enter.SetActive(true);
        }
        else
        {
            enter.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (logPanel.activeSelf == true)
            {
                LogClose();
            }
            else
            {
                LogOpen();
            }
        }
        if (isOpen)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Return)
            || (putSentence.End && isAuto))
        {
            AutoCheck();
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            SetAuto(!isAuto);
        }
    }


    public void AutoCheck()
    {
        if (putSentence.End && isAuto)
        {
            if (autoCoroutine == null)
            {
                autoCoroutine = LateText();
                Debug.Log("start");
                StartCoroutine(autoCoroutine);
            }
        }
        else
        {
            TextDraw();
        }
    }

    public void TextDraw()
    {
        if (isStaging)
        {
            return;
        }
        if (textIndex < texts.Count - 1)
        {
            if (putSentence.End)
            {
                textIndex += 1;
            }

            if (AssortText(texts[textIndex]))
            {
                if (putSentence.End)
                {
                    logs.Add(new KeyValuePair<string, string>(GetName(texts[textIndex]), texts[textIndex].sentence));
                }
                putSentence.CallSentence(texts[textIndex].sentence, GetName(texts[textIndex]));
            }
            else
            {
                director.Staging(texts[textIndex].sentence);
            }
        }
        if (autoCoroutine != null)
        {
            Debug.Log("Stop");
            StopCoroutine(autoCoroutine);
            autoCoroutine = null;
        }

    }



    /// <summary>テキストならTrue</summary>
    /// <param name="storage"></param>
    /// <returns></returns>
    bool AssortText(TextStorage storage)
    {
        if (storage.cName == CharName.System)
        {
            //director.Staging(storage.sentence);
            return false;
        }

        if (storage.cName == CharName.Lilia)
        {
            faceChanger.ChangeFace(storage.faceIndex);
        }


        return true;
    }


    void SetCharacter()
    {
        Resources.LoadAll<TextActor>("Character");
    }



    string GetName(TextStorage storage)
    {
        TextActor tmp = actors.Find(x => x.id == storage.cName);
        if (tmp == null)
        {
            return "";
        }
        return tmp.name;
    }





    #region Settings

    string filePath = "D:/SourceTree/DemonicCity/Assets/StoryScene/Texts.json";

    public string FolderPath { get { return "Scenario/"; } }

    public void SetText(Progress.StoryProgress story, Progress.ChoiceTag tag = Progress.ChoiceTag.Def)
    {
        string fileName = "";
        if (tag == Progress.ChoiceTag.Def)
        {
            fileName = story.ToString();
        }
        else
        {
            fileName = story.ToString() + "_" + tag.ToString();
        }
        filePath = FolderPath + fileName;
        Debug.Log(filePath);
        if (null == Resources.Load(filePath))
        {
            Debug.LogError("error");
            return;
        }
        Texts textAsset = Instantiate(Resources.Load<Texts>(filePath));
        foreach (TextStorage s in textAsset.texts)
        {
            texts.Add(s);
        }
    }

    #endregion

}




