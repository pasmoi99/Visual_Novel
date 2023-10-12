using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


using System.IO;


public class MainGame : MonoBehaviour
{
    // public static MainGame instance;


    public static Button buttonNext;

    public Button choice1; //bouton choix 1
    public Button choice2; //bouton choix 2
    public int choice; // pour id texte

   /* public TMP_Text goToSavePoint;
    public Image goToSavePointBox;
    public Button buttonGoToBeginning;
    public Button buttonGoToSavePoint; */

    private string jFile; //variable pour lire le fichier json
    private string jPath; //variable du chemin vers le fichier json
    private Dialogs jDialogs;

   /* private string jSavePath; //variable du chemin vers le fichier de sauvegarde json
    private string jSaveFile; //variable pour lire le fichier de sauvegarde json */


    public TMP_Text textCharacterName;
    public TMP_Text textDialog;
    public Image spriteCharacter;
    public Image spriteBackground; //bg
    public TMP_Text textChoice1; //text choix 1
    public TMP_Text textChoice2; // text choix 2

    public List<DialogSequence> dialogsList = new List<DialogSequence>();
    private int _sequenceNumber = 0;
    private SavePoint savePoint;

    private int chapterCount = 0;

    //cache les bouttons de choix du moment on l'on reprends
    private void Awake()
    {
        choice1.gameObject.SetActive(false);
        choice2.gameObject.SetActive(false);
       /* goToSavePointBox.gameObject.SetActive(false);
        goToSavePoint.gameObject.SetActive(false);
        buttonGoToBeginning.gameObject.SetActive(false);
        buttonGoToSavePoint.gameObject.SetActive(false); 

        jSavePath = Application.streamingAssetsPath + "/SavePoint.json"; //chemin fichier JSON des sauvegardes

        jSaveFile = File.ReadAllText(jSavePath); //lecture du fichier JSON des sauvegardes et stockage dans jSaveFile

        savePoint = JsonUtility.FromJson<SavePoint>(jSaveFile);*/




        //si dialogId dans le Json de sauvegarde n'a pas été modifié, alors on commence normalement 
        if (savePoint.dialogId == 0)
        {
            UpdateDialogSequence(dialogsList[0]);
            //charger images 
            LoadImages(dialogsList[_sequenceNumber].characterPath, dialogsList[_sequenceNumber].backgroundPath);
        }

        //sinon, on demande à l'utilisateur a partir de quel point il veut recomencer
        else
        {
            goToSavePointBox.gameObject.SetActive(true);
            goToSavePoint.gameObject.SetActive(true);
            buttonGoToBeginning.gameObject.SetActive(true);
            buttonGoToSavePoint.gameObject.SetActive(true);
            buttonGoToBeginning.onClick.AddListener(OnClickGoToBeginning);
            buttonGoToSavePoint.onClick.AddListener(OnClickGoToSavePoint);
            
        }

        choice1.onClick.AddListener(Choice1);
        choice2.onClick.AddListener(Choice2);
    }

    public void Choice1()
    {     
        chapterCount++;
        choice1.gameObject.SetActive(false);

    }

    public void Choice2()
    {
        chapterCount += 2;
        choice2.gameObject.SetActive(false);
    }

    void Start()
    {

        SelectNextFile(chapterCount);
        chapterCount++; 

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClickNextDialog();
        }

    }


    // permet de selectionner le passage du jeu qui va etre joué
    void SelectNextFile( int chapter)
    {
        jPath = Application.streamingAssetsPath + "/TextTest" + chapter + ".json"; //chemin fichier JSON des dialogues

        jFile = File.ReadAllText(jPath); //lecture du fichier JSON des dialogues et stockage dans jFile

        jDialogs = JsonUtility.FromJson<Dialogs>(jFile); //conversion de jFile en List

        setDialogs(jDialogs);
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

        if (_sequenceNumber >= dialogsList.Count)
        {
            buttonNext.gameObject.SetActive(false); //desactiver le bouton next quand plus besoin

        }


        if (_sequenceNumber < dialogsList.Count)
        {
            UpdateDialogSequence(dialogsList[_sequenceNumber]); 
        }
        
        else
        {
            choice1.gameObject.SetActive(true);
            choice2.gameObject.SetActive(true);


            chapterCount++;
        }
        CheckAndSetSavePoint(jDialogs.dialogs[_sequenceNumber]);
    }

    public void CheckAndSetSavePoint(Dialog d)
    {
        if (d.savePoint == true)
        {
            savePoint.dialogId = d.id;
        }
    }

    // met les dialogues dans la liste de type DialogSequence
    public void setDialogs(Dialogs d)
    {
        for (int i = 0; i < d.dialogs.Length; i++)
        {
            dialogsList.Add(new DialogSequence() { id = d.dialogs[i].id, textCharacterName = d.dialogs[i].name, textDialog = d.dialogs[i].dialog, characterPath = d.dialogs[i].characterPath, backgroundPath = d.dialogs[i].backgroundPath, textChoice1 = d.dialogs[i].textChoice1, textChoice2 = d.dialogs[i].textChoice2}); 
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
    public string name;
    public string dialog;
    public string characterPath;
    public string backgroundPath;
    public string? textChoice1;
    public string? textChoice2;
    public bool savePoint;
}


[System.Serializable]
public class Dialogs
{
    public Dialog[] dialogs;
}


[System.Serializable]
public class SavePoint
{
    public int dialogId;
}
