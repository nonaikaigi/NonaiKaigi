using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] FaceChanger lilia;
    [SerializeField] FaceChanger flame;
    [SerializeField] Image[] icons = new Image[2];
    [SerializeField] Text comment;
    // Start is called before the first frame update
    void Start()
    {
        switch (TextManager.GetResult())
        {
            case Progress.ChoiceTag.A:
                Plus();
                break;
            case Progress.ChoiceTag.B:
                Normal();
                break;
            case Progress.ChoiceTag.C:
                Zero();
                break;
            default:
                break;
        }
    }

    void Plus()
    {
        lilia.ChangeFace(FaceIndex.Fun);
        flame.ChangeFace(FaceIndex.Fun);
        icons[0].gameObject.SetActive(true);
        icons[1].gameObject.SetActive(false);
        comment.text = "すっごくよくねむれた・・・";
    }
    void Normal()
    {
        lilia.ChangeFace(FaceIndex.Normal);
        flame.ChangeFace(FaceIndex.Normal);
        icons[0].gameObject.SetActive(false);
        icons[1].gameObject.SetActive(true);
        comment.text = "よくねむれた・・・";
    }
    void Zero()
    {
        lilia.ChangeFace(FaceIndex.Wry);
        flame.ChangeFace(FaceIndex.Wry);
        icons[0].gameObject.SetActive(false);
        icons[1].gameObject.SetActive(false);
        comment.text = "ね、眠れない・・・";
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneChanger.SceneChange(SceneChanger.SceneTitle.Title);
        }
    }
}
