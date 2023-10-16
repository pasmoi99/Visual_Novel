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
    private int chapterProgress = 0;

    private bool isFinalFile;
    private string[] perso = { "Tatsuo"};
    private int currentChara=0;
    private int multiplier=1;

    //cache les bouttons de choix du moment on l'on reprends
    private void Awake()
    {
        buttonGoToBeginning.onClick.AddListener(OnClickGoToBeginning);
        buttonGoToSavePoint.onClick.AddListener(OnClickGoToSavePoint);
        choice1.gameObject.SetActive(false);
        choice2.gameObject.SetActive(false);
        goToSavePointBox.gameObject.SetActive(false);
        goToSavePoint.gameObject.SetActive(false);
        buttonGoToBeginning.gameObject.SetActive(false);
        buttonGoToSavePoint.gameObject.SetActive(false);



        jSavePath = Application.streamingAssetsPath + "/SavePoint.json"; //chemin fichier JSON des sauvegardes

        jSaveFile = File.ReadAllText(jSavePath); //lecture du fichier JSON des sauvegardes et stockage dans jSaveFile

        savePoint = JsonUtility.FromJson<SavePoint>(jSaveFile);

        SelectNextFile(chapterCount, perso[currentChara]);


        //si dialogId dans le Json de sauvegarde n'a pas �t� modifi�, alors on commence normalement 
        if (savePoint.dialogId == 0 || savePoint.chapterId == 0)

        //si dialogId dans le Json de sauvegarde n'a pas �t� modifi�, alors on commence normalement 

        {
            // SelectNextFile(chapterCount);
            UpdateDialogSequence(dialogsList[0]);
            //charger images 
            LoadImages(dialogsList[_sequenceNumber].characterPath, dialogsList[_sequenceNumber].backgroundPath);
        }

        //sinon, on demande � l'utilisateur a partir de quel point il veut recomencer
        else
        {

            SetSaveChoice(true);
        }



        //_sequenceNumber = savePoint.dialogId;//test
        //chapterCount = savePoint.chapterId; //test 


        choice1.onClick.AddListener(() => { Choice(1); });
        choice2.onClick.AddListener(() => { Choice(2); });

    }


    private void Choice(int value)
    {
        if (value == 1)
        {
            chapterCount= (chapterCount+1) * multiplier;
        }

        if (value == 2)
        {
            chapterCount = (chapterCount+2)*multiplier;
        }
        multiplier++;
        SelectNextFile(chapterCount, perso[currentChara]);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClickNextDialog();
        }

    }


    // permet de selectionner le passage du jeu qui va etre jou�
    void SelectNextFile(int chapter, string perso)
    {
        chapterProgress = chapter;
        jPath = Application.streamingAssetsPath + "/" + perso + chapter + ".json"; //chemin fichier JSON des dialogues

        jFile = File.ReadAllText(jPath); //lecture du fichier JSON des dialogues et stockage dans jFile

        jDialogs = JsonUtility.FromJson<Dialogs>(jFile); //conversion de jFile en List
        //print(jDialogs.ToString());
        setDialogs(jDialogs);

       


        // finalFile = JsonUtility.FromJson<bool>(jFile); 
    }
    void OnClickGoToSavePoint()
    {
        chapterCount = savePoint.chapterId;
        _sequenceNumber = savePoint.dialogId;
        SelectNextFile(chapterCount, perso[currentChara]);

        UpdateDialogSequence(dialogsList[_sequenceNumber]);
        SetSaveChoice(false);

    }

    void OnClickGoToBeginning()
    {
        chapterCount = 0;
        _sequenceNumber = 0;
        UpdateDialogSequence(dialogsList[0]);
        SetSaveChoice(false);
    }

    // remplace les dialogues actuel par les suivants
    void UpdateDialogSequence(DialogSequence sequence)
    {
        textDialog.text = sequence.textDialog;
        textCharacterName.text = sequence.textCharacterName;
        textChoice1.text = sequence.textChoice1;
        textChoice2.text = sequence.textChoice2;


        Sprite characterSprite = Resources.Load<Sprite>(sequence.characterPath);
        if (characterSprite != null)
        {
            spriteCharacter.sprite = characterSprite;
        }
        else
        {
            Debug.LogError("Impossible de charger l'image du chara");
        }

        Sprite backgroundSprite = Resources.Load<Sprite>(sequence.backgroundPath);
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

        if (_sequenceNumber == dialogsList.Count)
        {
            buttonNext.gameObject.SetActive(false);


            if (chapterProgress == chapterCount)
            {

                SetChoiceButtons(true);//test

                if (isFinalFile == true)
                {
                    SetChoiceButtons(false);
                    SetSaveChoice(true);


                }
                if (jDialogs.nextChara == -1 && _sequenceNumber == dialogsList.Count)
                {
                    SetSaveChoice(false);
                    SceneManager.LoadScene("MainMenu");
                }

                else if (currentChara != jDialogs.nextChara)
                {
                    currentChara = jDialogs.nextChara;
                    SetSaveChoice(false);
                    _sequenceNumber = 0;
                    chapterCount = 1;
                    SelectNextFile(chapterCount, perso[currentChara]);
                    UpdateDialogSequence(dialogsList[0]);

                   
                }
              
            }
        
        }




        if (_sequenceNumber < dialogsList.Count)
        {
            UpdateDialogSequence(dialogsList[_sequenceNumber]);
        }


    }


    public void SetSaveChoice(bool active)
    {
        goToSavePointBox.gameObject.SetActive(active); //test
        goToSavePoint.gameObject.SetActive(active);//test
        buttonGoToBeginning.gameObject.SetActive(active);//test
        buttonGoToSavePoint.gameObject.SetActive(active);//test 
        buttonNext.gameObject.SetActive(!active);
        textBox.gameObject.SetActive(!active);
        textDialog.gameObject.SetActive(!active);
        textCharacterName.gameObject.SetActive(!active);
        spriteCharacter.gameObject.SetActive(!active);
        spriteBackground.gameObject.SetActive(!active);
        nameBox.gameObject.SetActive(!active);
    }




    public void SetChoiceButtons(bool active)
    {
        choice1.gameObject.SetActive(active);
        choice2.gameObject.SetActive(active);
    }

    public void CheckAndSetSavePoint(DialogSequence dial)
    {
        if (dial.savePoint == true)
        {
            savePoint.dialogId = dial.id;
            savePoint.chapterId = chapterCount;
            File.WriteAllText(jSavePath, JsonUtility.ToJson(savePoint));//test
                                                                        // JsonUtility.ToJson(savePoint);


        }
    }

    // met les dialogues dans la liste de type DialogSequence
    public void setDialogs(Dialogs dial)
    {
        dialogsList = new List<DialogSequence>();
        for (int i = 0; i < dial.dialogs.Length; i++)
        {
            dialogsList.Add(new DialogSequence()
            {
                id = dial.dialogs[i].id,
                textCharacterName = dial.dialogs[i].name,
                textDialog = dial.dialogs[i].dialog,
                characterPath = dial.dialogs[i].characterPath,
                backgroundPath = dial.dialogs[i].backgroundPath,
                textChoice1 = dial.dialogs[i].choice1,
                textChoice2 = dial.dialogs[i].choice2,
                chapterId = dial.dialogs[i].chapterId,
            });

        }
        if (!dial.finalFile)
        {
            //print(d);
            isFinalFile = false;
        }
        else
        {

            isFinalFile = (bool)dial.finalFile;
            //print(finalFile);
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
    public string? characterPath;
    public string? backgroundPath;
    public string? choice1;
    public string? choice2;
    public bool savePoint;
    public int chapterId;
}




[System.Serializable]
public class SavePoint
{
    public int dialogId;
    public int chapterId;
}

