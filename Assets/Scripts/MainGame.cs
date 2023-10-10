using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


using System.IO;


public class MainGame : MonoBehaviour
{
    // public static MainGame instance;


    public Button buttonNext;

    public TMP_Text goToSavePoint;
    public Image goToSavePointBox;
    public Button buttonGoToBeginning;
    public Button buttonGoToSavePoint;
    
    private string jFile; //variable pour lire le fichier json
    private string jPath; //variable du chemin vers le fichier json

    private string jSavePath; //variable du chemin vers le fichier de sauvegarde json
    private string jSaveFile; //variable pour lire le fichier de sauvegarde json

    public TMP_Text textCharacterName;
    public TMP_Text textDialog;
    public Image spriteCharacter;
    public Image spriteBackground; //bg
    public List<DialogSequence> dialogsList = new List<DialogSequence>();
    private int _sequenceNumber = 0;
    private SavePoint savePoint;

    //cache les bouttons de choix du moment on l'on reprends
    private void Awake()
    {
        goToSavePointBox.gameObject.SetActive(false);
        goToSavePoint.gameObject.SetActive(false);
        buttonGoToBeginning.gameObject.SetActive(false);
        buttonGoToSavePoint.gameObject.SetActive(false);

        jSavePath = Application.streamingAssetsPath + "/SavePoint.json"; //chemin fichier JSON des sauvegardes

        jSaveFile = File.ReadAllText(jSavePath); //lecture du fichier JSON des sauvegardes et stockage dans jSaveFile

        savePoint = JsonUtility.FromJson<SavePoint>(jSaveFile);


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
    }


    void Start()
    {   


        jPath = Application.streamingAssetsPath + "/TextTest.json"; //chemin fichier JSON des dialogues

        jFile = File.ReadAllText(jPath); //lecture du fichier JSON des dialogues et stockage dans jFile

        Dialogs jDialogs = JsonUtility.FromJson<Dialogs>(jFile); //conversion de jFile en List

        setDialogs(jDialogs);

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClickNextDialog();
        }

    }

    void OnClickGoToSavePoint()
    {
        UpdateDialogSequence(dialogsList[savePoint.dialogId]);
        goToSavePointBox.gameObject.SetActive(false);
        goToSavePoint.gameObject.SetActive(false);
        buttonGoToBeginning.gameObject.SetActive(false);
        buttonGoToSavePoint.gameObject.SetActive(false);
    }

    void OnClickGoToBeginning()
    {
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

    public void OnClickNextDialog()
    {
        _sequenceNumber++;

        if (_sequenceNumber >= dialogsList.Count)
        {
            buttonNext.gameObject.SetActive(false);

        }


        if (_sequenceNumber < dialogsList.Count)
        {
            UpdateDialogSequence(dialogsList[_sequenceNumber]);
        }

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
            dialogsList.Add(new DialogSequence() { id = d.dialogs[i].id, textCharacterName = d.dialogs[i].name, textDialog = d.dialogs[i].dialog, characterPath = d.dialogs[i].characterPath, backgroundPath = d.dialogs[i].backgroundPath}); 
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