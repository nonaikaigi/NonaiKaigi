using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    public enum StoryProgress
    {
        TextA,
        ResultA,
        TextB,
        ResultB,
        TextC,
        ResultC,
        TextEnd,
        ResultEnd,
    }
    public enum ChoiceTag
    {
        A,
        B,
        C,
        Def,
    }
    public enum ChoicePhase
    {
        A,
        B,
        C,
        End,
    }

    StoryProgress storyProgress = StoryProgress.TextA;
    ChoiceTag[] choiceTags = new ChoiceTag[4];
    /// <summary>進捗のプロパティ</summary>
    public StoryProgress ThisStoryProgress
    {
        get
        {
            int tmp = PlayerPrefs.GetInt(Keys.KeyList[Keys.KeyTag.Progress], -1);
            if (tmp == -1)
            {
                return StoryProgress.TextA;
            }
            return (StoryProgress)tmp;
        }
        set
        {
            PlayerPrefs.SetInt(Keys.KeyList[Keys.KeyTag.Progress], (int)value);
            PlayerPrefs.Save();
        }
    }
    Dictionary<ChoicePhase, Keys.KeyTag> Ckeys = new Dictionary<ChoicePhase, Keys.KeyTag>()
    {
        {ChoicePhase.A,Keys.KeyTag.ChoiceA },
        {ChoicePhase.B,Keys.KeyTag.ChoiceB },
        {ChoicePhase.C,Keys.KeyTag.ChoiceC },
        //{ChoicePhase.D,Keys.KeyTag.ChoiceD },
        {ChoicePhase.End,Keys.KeyTag.End },
    };
    /// <summary>特定の会議の結果を登録する</summary>
    /// <param name="phase"></param>
    /// <param name="tag"></param>
    public void SetChoice(ChoicePhase phase, ChoiceTag tag)
    {
        Keys.KeyTag key;

        key = Ckeys[phase];

        PlayerPrefs.SetInt(Keys.KeyList[key], (int)tag);
        PlayerPrefs.Save();
        choiceTags[(int)phase] = tag;
    }
    /// <summary>特定の会議の結果を取得する</summary>
    /// <param name="phase"></param>
    /// <returns></returns>
    public ChoiceTag GetChoice(ChoicePhase phase)
    {
        int tmp = PlayerPrefs.GetInt(Keys.KeyList[Ckeys[phase]], -1);
        if (tmp == -1)
        {
            return ChoiceTag.A;
        }
        return (ChoiceTag)tmp;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
