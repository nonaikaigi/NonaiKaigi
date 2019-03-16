using System;
using System.Linq;
//using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum CharName
{
    Lilia, Mother, Mob, System, None
}
public class TextManager : MonoBehaviour
{
    //List<CharName> characters = new List<CharName>();

    List<TextStorage> texts = new List<TextStorage>();

    [SerializeField] Text nameText;
    [SerializeField] Progress progress;
    [SerializeField] PutSentence putSentence;
    [SerializeField] FaceChanger faceChanger;
    [SerializeField] TextDirector director;



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

    void Start()
    {
        logs.Clear();
        logPanel.SetActive(false);
        actors = Resources.LoadAll<TextActor>("CharacterAsset").ToList();
        SetText(progress.ThisStoryProgress);
        //putSentence.CallSentence(texts[0].sentence, GetName(texts[0]));   
        TextDraw();
    }


    void Update()
    {
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
        TextAsset textAsset = Instantiate((TextAsset)Resources.Load(filePath));
        string textsJson = textAsset.text;
        Debug.Log(textsJson);
        string[] spritKey = { "><" };

        string[] tmpTexts = textsJson.Split(spritKey, StringSplitOptions.None);
        //Debug.Log(tmpTexts);
        foreach (string s in tmpTexts)
        {

            //Debug.Log(s);
            var sss = JsonUtility.FromJson<TextStorage>(s);
            //Debug.Log(sss);   
            if (sss.face == null || sss.face == "")
            {
                sss.face = FaceIndex.Last.ToString();
            }
            var tmpStorage = new TextStorage(JsonUtility.FromJson<TextStorage>(s));
            //characters.Add(tmpStorage.cName);
            //Debug.Log(tmpStorage);
            texts.Add(tmpStorage);

        }
        //characters = characters.Distinct().Where(item => item <= CharName.Ixmagina).ToList();
        //SetCharacter(characters);
    }

    #endregion

}




