using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{

    public enum SceneTitle
    {
        Title,
        Adventure,
        Conference,
        Result,
    }
    public static SceneTitle nowScene = SceneTitle.Title;
    public static SceneTitle beforeScene = SceneTitle.Title;
    public static void SceneChange(SceneTitle title)
    {
        beforeScene = nowScene;
        nowScene = title;
        SceneManager.LoadScene(title.ToString());
    }





}
