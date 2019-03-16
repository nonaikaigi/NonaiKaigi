using System;
using System.Linq;
//using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum CharName
{
    Lilia, Mob, System, None
}
public class TextManager : MonoBehaviour
{
    //List<CharName> characters = new List<CharName>();

    List<TextStorage> texts = new List<TextStorage>();

    [SerializeField] Text nameText;
    [SerializeField] Progress progress;
    [SerializeField] PutSentence putSentence;
    [SerializeField] TextDirector director;

    List<TextActor> actors = new List<TextActor>();

    int textIndex = 0;

    public bool isStaging = false;

    void Start()
    {
        actors = Resources.LoadAll<TextActor>("CharacterAsset").ToList();
        SetText(progress.ThisStoryProgress);
        putSentence.CallSentence(texts[0].sentence, GetName(texts[0]));
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
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
                    putSentence.CallSentence(texts[textIndex].sentence, GetName(texts[textIndex]));
                }
                else
                {
                    director.Staging(texts[textIndex].sentence);
                }
            }
        }
    }

    /// <summary>テキストならTrue</summary>
    /// <param name="storage"></param>
    /// <returns></returns>
    bool AssortText(TextStorage storage)
    {
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
}




