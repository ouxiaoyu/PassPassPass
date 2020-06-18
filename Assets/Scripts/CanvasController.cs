using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Image score00;
    public Image score10;
    public Image score11;
    public Image score20;
    public Image score21;
    public Image score22;

    public Image best00;
    public Image best10;
    public Image best11;
    public Image best20;
    public Image best21;
    public Image best22;

    public static Boolean setScore = false;
    public static Boolean setBest = false;
    void Start()
    {
        SetBest();
        score00.gameObject.SetActive(true);
        score10.gameObject.SetActive(false);
        score11.gameObject.SetActive(false);
        score20.gameObject.SetActive(false);
        score21.gameObject.SetActive(false);
        score22.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (setScore)
        {
            SetSprite(score00, score10, score11, score20, score21, score22, MovableCube.score);
            setScore = false;
        }
        if (setBest)
        {
            SetBest();
            setBest = false;
        }

    }

    void SetBest()
    {
        int num = -1;
#if UNITY_ANDROID || UNITY_IOS
            num = int.Parse(LoadFile(Application.persistentDataPath, "Best.txt"));
#endif
#if !UNITY_ANDROID && !UNITY_IOS
        num = int.Parse(File.ReadAllText("Assets//Resources//Best.txt"));
#endif        
        SetSprite(best00, best10, best11, best20, best21, best22, num);

    }

    void SetSprite(Image img00, Image img10, Image img11, Image img20, Image img21, Image img22, int num)
    {
        if (num < 10 && num >= 0)
        {
            img00.gameObject.SetActive(true);
            img10.gameObject.SetActive(false);
            img11.gameObject.SetActive(false);
            img20.gameObject.SetActive(false);
            img21.gameObject.SetActive(false);
            img22.gameObject.SetActive(false);

            img00.sprite = Resources.Load<Sprite>("Sprites/num/" + num.ToString() );
            if(img00.GetComponent<Animator>()!=null )
                img00.GetComponent<Animator>().Play("Score (00)", 0, 0f);
        }
        else if (num < 100)
        {
            img00.gameObject.SetActive(false);
            img10.gameObject.SetActive(true);
            img11.gameObject.SetActive(true);
            img20.gameObject.SetActive(false);
            img21.gameObject.SetActive(false);
            img22.gameObject.SetActive(false);

            int digit0 = num / 10;
            int digit1 = num % 10;
            img10.sprite = Resources.Load<Sprite>("Sprites/num/" + digit0.ToString() );
            img11.sprite = Resources.Load<Sprite>("Sprites/num/" + digit1.ToString() );
            if (img10.GetComponent<Animator>() != null && img11.GetComponent<Animator>() != null)
            {
                img10.GetComponent<Animator>().Play("Score (10)", 0, 0f);
                img11.GetComponent<Animator>().Play("Score (11)", 0, 0f);
            }

        }
        else if (num < 1000)
        {
            img00.gameObject.SetActive(false);
            img10.gameObject.SetActive(false);
            img11.gameObject.SetActive(false);
            img20.gameObject.SetActive(true);
            img21.gameObject.SetActive(true);
            img22.gameObject.SetActive(true);

            int digit0 = num / 100;
            int digit1 = (num - 100 * digit0) / 10;
            int digit2 = num % 10;
            img20.sprite = Resources.Load<Sprite>("Sprites/num/" + digit0.ToString() );
            img21.sprite = Resources.Load<Sprite>("Sprites/num/" + digit1.ToString() );
            img22.sprite = Resources.Load<Sprite>("Sprites/num/" + digit2.ToString() );
            if (img20.GetComponent<Animator>() != null && img21.GetComponent<Animator>() != null && img22.GetComponent<Animator>() != null)
            {
                img20.GetComponent<Animator>().Play("Score (20)", 0, 0f);
                img21.GetComponent<Animator>().Play("Score (21)", 0, 0f);
                img22.GetComponent<Animator>().Play("Score (22)", 0, 0f);
            }
        }
        else
        {
            img00.gameObject.SetActive(false);
            img10.gameObject.SetActive(false);
            img11.gameObject.SetActive(false);
            img20.gameObject.SetActive(true);
            img21.gameObject.SetActive(true);
            img22.gameObject.SetActive(true);

            img20.sprite = Resources.Load<Sprite>("Sprites/num/" + "0");
            img21.sprite = Resources.Load<Sprite>("Sprites/num/" + "0");
            img22.sprite = Resources.Load<Sprite>("Sprites/num/" + "0");
        }


    }
    String LoadFile(string path, string name)
    {
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            return "0";
        }
        string info;
        info = sr.ReadToEnd();
        sr.Close();
        sr.Dispose();
        return info;
    }
}
