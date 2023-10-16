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


    //GO.gameObject.interactable = false;

    public Button buttonNext;

    public Button choice1; //bouton choix 1
    public Button choice2; //bouton choix 2

    public Button choiceChara1;
    public Button choiceChara2;
    public Button choiceChara3;

    public int choice; // pour id texte

    public TMP_Text goToSavePoint;
    public Image goToSavePointBox;
    public Button buttonGoToBeginning;
    public Button buttonGoToSavePoint;
    public Image bgBlack;

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
    private int sequenceNumber = 0;
    private SavePoint savePoint;


    private bool isFinalFile;
    private string[] perso = { "debut1", "aneko1", "Tatsuo1", "Kumo1" };
    private string nextFileName;
    private int currentChara = 0;
    //private int multiplier=1;

    //cache les bouttons de choix du moment on l'on reprends
    private void Awake()
    {

        savePoint = new SavePoint();

        buttonGoToBeginning.onClick.AddListener(OnClickGoToBeginning);
        buttonGoToSavePoint.onClick.AddListener(OnClickGoToSavePoint);
        choice1.gameObject.SetActive(false);
        choice2.gameObject.SetActive(false);
        choiceChara1.gameObject.SetActive(false);
        choiceChara2.gameObject.SetActive(false);
        choiceChara3.gameObject.SetActive(false);
        goToSavePointBox.gameObject.SetActive(false);
        goToSavePoint.gameObject.SetActive(false);
        buttonGoToBeginning.gameObject.SetActive(false);
        buttonGoToSavePoint.gameObject.SetActive(false);


        jSavePath = Application.streamingAssetsPath + "/SavePoint.json"; //chemin fichier JSON des sauvegardes
        savePoint.chapterId = "Tatsuo1";
        savePoint.dialogId = 0;
        File.WriteAllText(jSavePath, JsonUtility.ToJson(savePoint));

        jSaveFile = File.ReadAllText(jSavePath); //lecture du fichier JSON des sauvegardes et stockage dans jSaveFile

        savePoint = JsonUtility.FromJson<SavePoint>(jSaveFile);

        SelectNextFile(perso[currentChara]);


        //si dialogId dans le Json de sauvegarde n'a pas �t� modifi�, alors on commence normalement 

        //si dialogId dans le Json de sauvegarde n'a pas �t� modifi�, alors on commence normalement 


        // SelectNextFile(chapterCount);
        UpdateDialogSequence(dialogsList[0]);
        //charger images 
        LoadImages(dialogsList[sequenceNumber].characterPath, dialogsList[sequenceNumber].backgroundPath);


        //sinon, on demande � l'utilisateur a partir de quel point il veut recomencer



        //_sequenceNumber = savePoint.dialogId;//test
        //chapterCount = savePoint.chapterId; //test 

        choice1.onClick.AddListener(() => { Choice(1); });
        choice2.onClick.AddListener(() => { Choice(2); });
        //choiceChara1.onClick.AddListener(() => { Choice(3); });
        //choiceChara2.onClick.AddListener(() => { Choice(4); });
        //choiceChara3.onClick.AddListener(() => { Choice(5); });
    }


    private void Choice(int value)
    {
        if (value == 1)
        {
            nextFileName = jDialogs.nextFile1;

        }

        if (value == 2)
        {
            nextFileName = jDialogs.nextFile2;

        }

        //if (value == 3)
        //{
        //    nextFileName = jDialogs.nextFileChara1;
        //    SelectNextFile(perso[1]);
        //}

        //if (value == 4)
        //{
        //    nextFileName = jDialogs.nextFileChara2;
        //    SelectNextFile(perso[2]);
        //}

        //if (value == 5)
        //{
        //    nextFileName = jDialogs.nextFileChara3;
        //    SelectNextFile(perso[3]);
        //}

        SelectNextFile(nextFileName);
        SetChoiceButtons(false);
        buttonNext.gameObject.SetActive(true);
        sequenceNumber = 0;
        UpdateDialogSequence(dialogsList[sequenceNumber]);

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
    void SelectNextFile(string name)
    {
        jPath = Application.streamingAssetsPath + "/" + name + ".json"; //chemin fichier JSON des dialogues

        jFile = File.ReadAllText(jPath); //lecture du fichier JSON des dialogues et stockage dans jFile

        jDialogs = JsonUtility.FromJson<Dialogs>(jFile); //conversion de jFile en List
        //print(jDialogs.ToString());
        setDialogs(jDialogs);




        // finalFile = JsonUtility.FromJson<bool>(jFile); 
    }
    void OnClickGoToSavePoint()
    {
        //chapterCount = savePoint.chapterId;
        sequenceNumber = savePoint.dialogId;
        SelectNextFile(savePoint.chapterId);
        savePoint.chapterId = "";
        savePoint.dialogId = 0;
        UpdateDialogSequence(dialogsList[sequenceNumber - 1]);
        SetSaveChoice(false);

    }

    void OnClickGoToBeginning()
    {

        //chapterCount = 0;
        sequenceNumber = 0;
        SelectNextFile(perso[0]);
        savePoint.chapterId = "";
        savePoint.dialogId = 0;
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
        //else
        //{
        //    Debug.LogError("Impossible de charger l'image du chara");
        //}

        Sprite backgroundSprite = Resources.Load<Sprite>(sequence.backgroundPath);
        if (backgroundSprite != null)
        {
            spriteBackground.sprite = backgroundSprite;
        }
        //else
        //{
        //    Debug.LogError("Impossible de charger l'image du background");
        //}


    }


    //permet de passer au dialogue suivant
    public void OnClickNextDialog()
    {
        if (sequenceNumber < dialogsList.Count)
        {
            CheckAndSetSavePoint(dialogsList[sequenceNumber], jDialogs);
            sequenceNumber++;
        }

        if (sequenceNumber == dialogsList.Count)
        {
            buttonNext.gameObject.SetActive(false);

            SetChoiceButtons(true);//test

            if (isFinalFile == true)
            {
                SetChoiceButtons(false);
                SetSaveChoice(true);
               //setCharaButtons(false);
                //fileCount = savePoint.chapterId;
            }

            if (isFinalFile == true && jDialogs.nextChara != -1)
            {
                currentChara = jDialogs.nextChara;
                SetChoiceButtons(false);
                SetSaveChoice(false);
                sequenceNumber = 0;
                SelectNextFile(perso[currentChara]);
               // SelectNextFile(nextFileName);
                UpdateDialogSequence(dialogsList[0]);
            }
            //if (jDialogs.isStart == true)
            //{
            //    SetChoiceButtons(false);
            //    SetSaveChoice(false);
            //   // setCharaButtons(true);

            ////    //fileCount = savePoint.chapterId;


            //}

            if (jDialogs.nextChara == 3 && sequenceNumber == dialogsList.Count && isFinalFile ==true)
            {
                SetSaveChoice(false);
               // setCharaButtons(false);
                SceneManager.LoadScene("MainMenu");
            }

            //else if (jDialogs.nextChara != -1 && )
            //{
            //    currentChara = jDialogs.nextChara;
            //    SetSaveChoice(false);
            //    sequenceNumber = 0;
            //    // chapterCount = 1;
            //    SelectNextFile(nextFileName);
            //    UpdateDialogSequence(dialogsList[0]);


            //}



        }




        if (sequenceNumber < dialogsList.Count)
        {
            UpdateDialogSequence(dialogsList[sequenceNumber]);
        }


    }

    //public void setCharaButtons(bool active)
    //{
    //   choiceChara1.gameObject.SetActive(active);
    //   choiceChara2.gameObject.SetActive(active);
    //   choiceChara3.gameObject.SetActive(active);
    //   buttonNext.gameObject.SetActive(!active);
    //   textBox.gameObject.SetActive(!active);
    //   textDialog.gameObject.SetActive(!active);
    //   textCharacterName.gameObject.SetActive(!active);
    //  spriteCharacter.gameObject.SetActive(!active);
    //   spriteBackground.gameObject.SetActive(!active);
    //   nameBox.gameObject.SetActive(!active);
    //}

    public void SetSaveChoice(bool active)
    {
        bgBlack.gameObject.SetActive(active);
        goToSavePointBox.gameObject.SetActive(active);
        goToSavePoint.gameObject.SetActive(active);
        buttonGoToBeginning.gameObject.SetActive(active);
        buttonGoToSavePoint.gameObject.SetActive(active);
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
        //if (dialogsList[sequenceNumber]!=null)
        //{
        //    choiceChara1.gameObject.SetActive(true);
        //    choiceChara2.gameObject.SetActive(true);
        //    choiceChara3.gameObject.SetActive(true);
        //}
    }

    public void CheckAndSetSavePoint(DialogSequence dialSeq, Dialogs dials)
    {
        if (dialSeq.savePoint == true)
        {
            savePoint.dialogId = dialSeq.id;
            savePoint.chapterId = dials.chapterId;
            File.WriteAllText(jSavePath, JsonUtility.ToJson(savePoint));//test



        }

    }

    // met les dialogues dans la liste de type DialogSequence
    public void setDialogs(Dialogs dials)
    {
        dialogsList = new List<DialogSequence>();
        for (int i = 0; i < dials.dialogs.Length; i++)
        {
            dialogsList.Add(new DialogSequence()
            {
                id = dials.dialogs[i].id,
                textCharacterName = dials.dialogs[i].name,
                textDialog = dials.dialogs[i].dialog,
                characterPath = dials.dialogs[i].characterPath,
                backgroundPath = dials.dialogs[i].backgroundPath,
                textChoice1 = dials.dialogs[i].choice1,
                textChoice2 = dials.dialogs[i].choice2,
                //textChoiceChara1 = dials.dialogs[i].choiceChara1,
                //textChoiceChara2 = dials.dialogs[i].choiceChara2,
                //textChoiceChara3 = dials.dialogs[i].choiceChara3,
                savePoint = dials.dialogs[i].savePoint
            });

        }
        if (!dials.finalFile)
        {
            //print(d);
            isFinalFile = false;
        }
        else
        {

            isFinalFile = (bool)dials.finalFile;
            //print(finalFile);
        }

        //if (!dials.isStart)
        //{
        //    //print(d);
        //    //print(d);
        //    jDialogs.isStart = false;
        //}
        //else
        //{

        //    jDialogs.isStart = (bool)dials.isStart;
        //    //print(isStart);
        //}


    }





    void LoadImages(string characterPath, string backgroundPath)
    {

        Sprite characterSprite = Resources.Load<Sprite>(characterPath);
        if (characterSprite != null)
        {
            spriteCharacter.gameObject.SetActive(true);
            spriteCharacter.sprite = characterSprite;
        }
        //else
        //{
        //    spriteCharacter.gameObject.SetActive(false);
        //    Debug.LogError("Impossible de charger l'image du chara");
        //}


        Sprite backgroundSprite = Resources.Load<Sprite>(backgroundPath);
        if (backgroundSprite != null)
        {
            spriteBackground.sprite = backgroundSprite;
        }
        //else
        //{
        //    Debug.LogError("Impossible de charger l'image du background");
        //}


    }




}


[System.Serializable]
public class Dialog
{
    public int id;
    public string name; //cacher le nom et les textes
    public string dialog;
    public string characterPath;
    public string backgroundPath;
    public string choice1;
    public string choice2;
    //public string choiceChara1;
    //public string choiceChara2;
    //public string choiceChara3;
    public bool savePoint;
}




[System.Serializable]
public class SavePoint
{
    public int dialogId;
    public string chapterId;
}

