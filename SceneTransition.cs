using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;
    private Animator animatorController;
    [SerializeField] private Slider slider;

    private AsyncOperation loadingSceneOperation;

    [SerializeField] private bool isOpenAnimation = true;

    public void SwitchToScene(string sceneName)
    {
        animatorController.SetTrigger(name: "SceneOut");

        StartCoroutine(loadScene(sceneName));


    }


    public void SwitchToSceneWithOutLoad(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        animatorController = GetComponent<Animator>();

    }
    private void Start()
    {

        if (isOpenAnimation)
        {
            animatorController.SetTrigger(name: "SceneIn");
        }
        
    }


    IEnumerator loadScene(string sceneName)
    {
        loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingSceneOperation.allowSceneActivation = false;
        while (!loadingSceneOperation.isDone)
        {
            slider.value = Mathf.Lerp(slider.value, loadingSceneOperation.progress, Time.deltaTime);
            yield return null;
        }
      

    }

    public void OnAnimationOver()
    {
        
        loadingSceneOperation.allowSceneActivation = true;
      
    }
}
