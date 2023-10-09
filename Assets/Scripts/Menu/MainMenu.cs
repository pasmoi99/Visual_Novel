using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public Image imageFade;

    public void OnCLickPlay()
    {
        imageFade.DOFade(1, 1.5f).OnComplete(FadeComplete);
    }


   public void FadeComplete()
    {
        SceneManager.LoadScene("SceneThomas");
    }

    void Start()
    {
        
    }

 
    void Update()
    {
        
    }
}
