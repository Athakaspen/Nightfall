using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // place object to initiate intro animation
    public GameObject plane;
    public GameObject mainMenu;
    public GameObject difficultyMenu;


    public void StartGame(){
        plane.GetComponent<Animator>().SetTrigger("doCrash");
        GetComponent<AudioSource>().Play();
        GetComponentInChildren<FadeToBlack>().DoFade(true, 3f);
        StartCoroutine(executeAfterTime(gotoGame, 3.3f));
    }
    void gotoGame(){
        SceneManager.LoadScene("Island", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void ShowDifficultySelect(){
        mainMenu.SetActive(false);
        difficultyMenu.SetActive(true);
    }
    public void Easy(){
        PersistentData.difficulty = "Easy";
        PersistentData.dayLength = 90;
        PersistentData.maxWood = 30;
        PersistentData.repairTime = 1.2f;
        PersistentData.repairCost = 10;
        PersistentData.difficultyCurve = PersistentData.easyCurve;
        StartGame();
    }
    public void Normal(){
        PersistentData.difficulty = "Normal";
        PersistentData.dayLength = 70;
        PersistentData.maxWood = 25;
        PersistentData.repairTime = 2f;
        PersistentData.repairCost = 10;
        PersistentData.difficultyCurve = PersistentData.normalCurve;
        StartGame();
    }
    public void Lunatic(){
        PersistentData.difficulty = "Lunatic";
        PersistentData.dayLength = 50;
        PersistentData.maxWood = 15;
        PersistentData.repairTime = 2.5f;
        PersistentData.repairCost = 15;
        PersistentData.difficultyCurve = PersistentData.lunaticCurve;
        StartGame();
    }

    public void ShowCredits(){
        GetComponentInChildren<FadeToBlack>().DoFade(true, 2f);
        StartCoroutine(executeAfterTime(gotoCredits, 2.5f));
    }
    void gotoCredits(){
        SceneManager.LoadScene("Credits", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void BackToTitle(){
        GetComponentInChildren<FadeToBlack>().DoFade(true, 2f);
        StartCoroutine(executeAfterTime(gotoTitle, 2.5f));
    }
    void gotoTitle(){
        SceneManager.LoadScene("Title", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void QuitGame(){
        GetComponentInChildren<FadeToBlack>().DoFade(true, 2f);
        StartCoroutine(executeAfterTime(Application.Quit, 2.5f));
    }

    IEnumerator executeAfterTime(Action func, float time)
    {
        yield return new WaitForSeconds(time);
        func();
    }

    
}
