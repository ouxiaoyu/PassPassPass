using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailMenu : MonoBehaviour
{
    public Animator failPanel1;
    public Animator failPanel2;
    public Animator failPanel3;
    public Animator failPanel4;
    public GameObject bg;
    public void PlayGame()
    {
        failPanel1.SetBool("isAppear", false);
        failPanel2.SetBool("isAppear", false);
        failPanel3.SetBool("isAppear", false);
        failPanel4.SetBool("isAppear", false);
        bg.SetActive(false);
        Time.timeScale = 1;
        Wall.isReset = true ;
        MovableCube.isUpdate = true;
        CanvasController.setScore = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
