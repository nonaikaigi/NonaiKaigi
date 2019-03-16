using System;
using System.Linq;
//using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum CharName
{
    System, None
}
public class TextManager : MonoBehaviour
{
    List<CharName> characters = new List<CharName>();

    List<TextStorage> texts = new List<TextStorage>();


    [SerializeField] Progress progress;


    public bool isStaging = false;







    string filePath = "D:/SourceTree/DemonicCity/Assets/StoryScene/Texts.json";

    public string FolderPath { get { return "Sources/"; } }

    public void SetText(string fileName)
    {
        //string folderPath += progress.ThisStoryProgress.ToString() + "/";
        filePath = FolderPath + progress.ThisStoryProgress.ToString() + "/" + fileName;
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
            characters.Add(tmpStorage.cName);
            //Debug.Log(tmpStorage);
            texts.Add(tmpStorage);

        }
        characters = characters.Distinct().Where(item => item <= CharName.Ixmagina).ToList();
        //SetCharacter(characters);
    }
}




