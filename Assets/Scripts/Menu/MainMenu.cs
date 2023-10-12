using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

public class MainMenu : MonoBehaviour
{

    public Image imageFade;
    public List<Button> buttonsList;
    public Image imageButton;
    public List<TMPro.TextMeshProUGUI> textList;
    public AudioSource music;
    public GameObject CanvasGroup;


    // transition entre les scenes
    public void OnCLickPlay()
    {
        imageFade.DOFade(1, 1.5f).OnComplete(FadeComplete);

        for (int i = 1; i < buttonsList.Count; i++)
        {
            buttonsList[i].gameObject.SetActive(false);

            //cacher image et texte des boutons
            imageButton.DOKill();
            imageButton.DOFade(0, 0.5f);

            textList[i].DOKill();
            textList[i].DOFade(0, 0.5f);
        }
        
    }

    // button play -> changement de scene vers gameplay
   public void FadeComplete()
    {
        SceneManager.LoadScene("SceneThomas");
    }


    public void OnMusicValueChanged(float newValue)
    {
        music.volume = newValue;
    }


     /* public void OnCLickOptions()
    {
        CanvasGroup.gameObject.SetActive(true);
        CanvasGroup.Alpha = 0;


    } */

}
