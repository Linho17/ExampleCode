using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{


    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI fishCollectText;
    [SerializeField] private GameObject statisticsUI;
    [SerializeField] private GameObject gameUI;

    [SerializeField] private GameObject buttonStart;
    [SerializeField] private GameObject buttonContinue;




    private void OnEnable()
    {
        statisticsUI.SetActive(false);
        gameUI.SetActive(false);
        fishCollectText.text = SettingsKey.GetFishCollect().ToString();
    }

    private void Start()
    {
        buttonStart.SetActive(true);
    }



    public void LoadScene()
    {
        SceneTransition.instance.SwitchToScene("Prototype");
        SettingsKey.SetDistanceAndFish(0, 0);
        
    }

    public void StartGame()
    {
        animator.CrossFade("ManuAnimationOut", 0f);
        gameUI.SetActive(true);
        buttonStart.SetActive(false);
        Game.GAME.StartGame();
    }

    public void StatisticsBtn()
    {
        statisticsUI.SetActive(!statisticsUI.activeSelf);
    }

   public void SetActive()
   {

        gameObject.SetActive(true);
        animator.CrossFade("ManuAnimationIn", 0f);

    }

    public void SetUnActive()
    {

        gameObject.SetActive(false);

    }


    public void PauseBtn()
    {
        
        buttonContinue.SetActive(true);

        Game.GAME.PauseGame();
        gameUI.SetActive(false);
        SetActive();
        
        
    }

    public void ContinueBtn()
    {

        Game.GAME.ContinueGame();
        gameUI.SetActive(true);
        
        
        animator.CrossFade("ManuAnimationOut", 0f);

    }
}
