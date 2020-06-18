using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public Animator settingPanel1;
    public Animator settingPanel2;
    public Animator settingPanel3;
    public Animator settingPanel4;
    public Animator settingPanel5;
    public GameObject settingbg;
    public GameObject pausebg;
    public Animator failPanel1;
    public Animator failPanel2;
    public Animator failPanel3;
    public Animator failPanel4;
    public GameObject failbg;

    public void PauseGame()
    {
        Time.timeScale = 0;
        failPanel1.SetBool("isAppear", false);
        failPanel2.SetBool("isAppear", false);
        failPanel3.SetBool("isAppear", false);
        failPanel4.SetBool("isAppear", false);
        failbg.SetActive(false);
        pausebg.SetActive(false);
        settingPanel1.SetBool("isAppear", true);
        settingPanel2.SetBool("isAppear", true);
        settingPanel3.SetBool("isAppear", true);
        settingPanel4.SetBool("isAppear", true);
        settingPanel5.SetBool("isAppear", true);
        settingbg.SetActive(true);
    }
public void ResumeGame()
    {
        Time.timeScale = 1;
        settingPanel1.SetBool("isAppear", false);
        settingPanel2.SetBool("isAppear", false);
        settingPanel3.SetBool("isAppear", false);
        settingPanel4.SetBool("isAppear", false);
        settingPanel5.SetBool("isAppear", false);
        settingbg.SetActive(false);
    }
}
