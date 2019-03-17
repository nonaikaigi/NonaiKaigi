using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Keys
{
    public enum KeyTag
    {
        Progress,
        ChoiceA,
        ChoiceB,
        ChoiceC,
        ChoiceD,
        End,
        StageType,
        Miss,
    }

    public static Dictionary<KeyTag, string> KeyList = new Dictionary<KeyTag, string>()
    {
        {KeyTag.Progress,"Progress" },
        {KeyTag.ChoiceA,"ChoiceA" },
        {KeyTag.ChoiceB,"ChoiceB" },
        {KeyTag.ChoiceC,"ChoiceC" },
        {KeyTag.ChoiceD,"ChoiceD" },
        { KeyTag.StageType,"StageTag"},
        {KeyTag.End,"End" },
    };




}
