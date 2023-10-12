using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    // public static MainGame instance;


    public Button buttonNext;

    public Button choice1; //bouton choix 1
    public Button choice2; //bouton choix 2
    public int choice; // pour id texte

    public TMP_Text goToSavePoint;
    public Image goToSavePointBox;
    public Button buttonGoToBeginning;
    public Button buttonGoToSavePoint;

    private string jFile; //variable pour lire le fichier json
    private string jPath; //variable du chemin vers le fichier json
    private Dialogs jDialogs;

    private string jSavePath; //variable du chemin vers le fichier de sauvegarde json
    private string jSaveFile; //variable pour lire le fichier de sauvegarde json 


    public TMP_Text textCharacterName;
    public TMP_Text textDialog;
    public Image spriteCharacter;
    public Image spriteBackground; //bg
    public TMP_Text textChoice1; //text choix 1
    public TMP_Text textChoice2; // text choix 2
    public Image textBox;
    public Image nameBox;

    public List<DialogSequence> dialogsList = new List<DialogSequence>();
    private int _sequenceNumber = 0;
    private SavePoint savePoint;

    private int chapterCount = 1;

    private bool finalFile;

    //cache les bouttons de choix du moment on l'on reprends
    private void Awake()
    {   
        choice1.gameObject.SetActive(false);
        choice2.gameObject.SetActive(false);
        goToSavePointBox.gameObject.SetActive(false);
        goToSavePoint.gameObject.SetActive(false);
        buttonGoToBeginning.gameObject.SetActive(false);
        buttonGoToSavePoint.gameObject.SetActive(false);

        jSavePath = Application.streamingAssetsPath + "/SavePoint.json"; //chemin fichier JSON des sauvegardes

        jSaveFile = File.ReadAllText(jSavePath); //lecture du fichier JSON des sauvegardes et stockage dans jSaveFile

        savePoint = JsonUtility.FromJson<SavePoint>(jSaveFile);

        SelectNextFile(chapterCount);

        //si dialogId dans le Json de sauvegarde n'a pas été modifié, alors on commence normalement 
        if (savePoint.dialogId == 0)
        {
         //   SelectNextFile(chapterCount);
            UpdateDialogSequence(dialogsList[0]);
            //charger images 
            LoadImages(dialogsList[_sequenceNumber].characterPath, dialogsList[_sequenceNumber].backgroundPath);
        }

        //sinon, on demande à l'utilisateur a partir de quel point il veut recomencer
        else
        {
            buttonNext.gameObject.SetActive(false);
            textBox.gameObject.SetActive(false);
            textDialog.gameObject.SetActive(false);
            textCharacterName.gameObject.SetActive(false);
            spriteCharacter.gameObject.SetActive(false);
            spriteBackground.gameObject.SetActive(false);
            nameBox.gameObject.SetActive(false);

            goToSavePointBox.gameObject.SetActive(true);
            goToSavePoint.gameObject.SetActive(true);
            buttonGoToBeginning.gameObject.SetActive(true);
            buttonGoToSavePoint.gameObject.SetActive(true);

            buttonGoToBeginning.onClick.AddListener(OnClickGoToBeginning);
            buttonGoToSavePoint.onClick.AddListener(OnClickGoToSavePoint);
            

        }

       choice1.onClick.AddListener(() => { Choice(1); });
       choice2.onClick.AddListener(() => { Choice(2); });
       
    }


    private void Choice(int v)
    {
        if(v==1)
        {
            chapterCount++;
        }

        if(v==2)
        {
            chapterCount += 2;
        }

        SelectNextFile(chapterCount);
        SetChoiceButtons(false);
        buttonNext.gameObject.SetActive(true);
        _sequenceNumber = 0;
        UpdateDialogSequence(dialogsList[_sequenceNumber]);

    }

    void Start()
    {



    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            OnClickNextDialog();
        }

    }


    // permet de selectionner le passage du jeu qui va etre joué
    void SelectNextFile(int chapter)
    {

        jPath = Application.streamingAssetsPath + "/TextTest" + chapter + ".json"; //chemin fichier JSON des dialogues

        jFile = File.ReadAllText(jPath); //lecture du fichier JSON des dialogues et stockage dans jFile

        jDialogs = JsonUtility.FromJson<Dialogs>(jFile); //conversion de jFile en List

        setDialogs(jDialogs);

        //finalFile = JsonUtility.FromJson<bool>(jFile);
    }
    void OnClickGoToSavePoint()
    {
        _sequenceNumber = savePoint.dialogId;
        UpdateDialogSequence(dialogsList[_sequenceNumber]);
        goToSavePointBox.gameObject.SetActive(false);
        goToSavePoint.gameObject.SetActive(false);
        buttonGoToBeginning.gameObject.SetActive(false);
        buttonGoToSavePoint.gameObject.SetActive(false);
    }

    void OnClickGoToBeginning()
    {
        chapterCount = 1;
        _sequenceNumber = 0;
        UpdateDialogSequence(dialogsList[0]);
        goToSavePointBox.gameObject.SetActive(false);
        goToSavePoint.gameObject.SetActive(false);
        buttonGoToBeginning.gameObject.SetActive(false);
        buttonGoToSavePoint.gameObject.SetActive(false);
    }

    // remplace les dialogues actuel par les suivants
    void UpdateDialogSequence(DialogSequence s)
    {
        textDialog.text = s.textDialog;
        textCharacterName.text = s.textCharacterName;
        textChoice1.text = s.textChoice1;
        textChoice2.text = s.textChoice2;

        Sprite characterSprite = Resources.Load<Sprite>(s.characterPath);
        if (characterSprite != null)
        {
            spriteCharacter.sprite = characterSprite;
        }
        else
        {
            Debug.LogError("Impossible de charger l'image du chara");
        }

        Sprite backgroundSprite = Resources.Load<Sprite>(s.backgroundPath);
        if (backgroundSprite != null)
        {
            spriteBackground.sprite = backgroundSprite;
        }
        else
        {
            Debug.LogError("Impossible de charger l'image du background");
        }


    }


    //permet de passer au dialogue suivant
    public void OnClickNextDialog()
    {
        _sequenceNumber++;

        if (_sequenceNumber == dialogsList.Count && finalFile==false)
        {
            buttonNext.gameObject.SetActive(false); //desactiver le bouton next quand plus besoin
            SetChoiceButtons(true);
        }

        if (_sequenceNumber == dialogsList.Count && finalFile==true)
        {
            buttonNext.gameObject.SetActive(false); //desactiver le bouton next quand plus besoin

            CheckAndSetSavePoint(jDialogs.dialogs[_sequenceNumber]);
        }


        if (_sequenceNumber < dialogsList.Count)
        {
            UpdateDialogSequence(dialogsList[_sequenceNumber]);
        }

       if (choice1.IsActive() == false)
        {
            buttonNext.gameObject.SetActive(true);
        } 


    }

    public void SetChoiceButtons(bool b)
    {
        choice1.gameObject.SetActive(b);
        choice2.gameObject.SetActive(b);
    }
    
    public void CheckAndSetSavePoint(Dialog d)
    {
        if (d.savePoint == true)
        {
            savePoint.dialogId = d.id;
            JsonUtility.ToJson(savePoint);
        }
    }

    // met les dialogues dans la liste de type DialogSequence
    public void setDialogs(Dialogs d)
    {
        dialogsList = new List<DialogSequence>();
        for (int i = 0; i < d.dialogs.Length; i++)
        {
            dialogsList.Add(new DialogSequence()
            {
                id = d.dialogs[i].id,
                textCharacterName = d.dialogs[i].name,
                textDialog = d.dialogs[i].dialog,
                characterPath = d.dialogs[i].characterPath,
                backgroundPath = d.dialogs[i].backgroundPath,
                textChoice1 = d.dialogs[i].choice1,
                textChoice2 = d.dialogs[i].choice2
            });
        }
    }





    void LoadImages(string characterPath, string backgroundPath)
    {

        Sprite characterSprite = Resources.Load<Sprite>(characterPath);
        if (characterSprite != null)
        {
            spriteCharacter.sprite = characterSprite;
        }
        else
        {
            Debug.LogError("Impossible de charger l'image du chara");
        }


        Sprite backgroundSprite = Resources.Load<Sprite>(backgroundPath);
        if (backgroundSprite != null)
        {
            spriteBackground.sprite = backgroundSprite;
        }
        else
        {
            Debug.LogError("Impossible de charger l'image du background");
        }


    }




}


[System.Serializable]
public class Dialog
{
    public int id;
    public string? name; //cacher le nom et les textes
    public string dialog;
    public string characterPath;
    public string backgroundPath;
    public string? choice1;
    public string? choice2;
    public bool savePoint;
    public bool finalFile;
}


[System.Serializable]
public class SavePoint
{
    public int dialogId;
}
