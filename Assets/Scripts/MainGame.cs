using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


using System.IO;


public class MainGame : MonoBehaviour
{
    // public static MainGame instance;


    public Button button;



    private string jFile; //variable pour lire le fichier json
    private string jPath; //variable du chemin vers le fichier json


    public TMP_Text textCharacterName;
    public TMP_Text textDialog;
    public Image spriteCharacter;
    public Image spriteBackground; //bg
    public List<DialogSequence> dialogsList = new List<DialogSequence>();
    private int _sequenceNumber = 0;



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
            button.gameObject.SetActive(false);

        }


        if (_sequenceNumber < dialogsList.Count)
        {
            UpdateDialogSequence(dialogsList[_sequenceNumber]);
        }

    }
    // met les dialogues dans la liste DialogSequence
    public void setDialogs(Dialogs d)
    {
        for (int i = 0; i < d.dialogs.Length; i++)
        {
            dialogsList.Add(new DialogSequence() { id = d.dialogs[i].id, textCharacterName = d.dialogs[i].name, textDialog = d.dialogs[i].dialog, characterPath = d.dialogs[i].characterPath, backgroundPath = d.dialogs[i].backgroundPath}); 
        }
    }


        void Start()
        {
            jPath = Application.streamingAssetsPath + "/TextTest.json"; //chemin fichier JSON
            jFile = File.ReadAllText(jPath); //lecture fichier JSON
            Dialogs jDialogs = JsonUtility.FromJson<Dialogs>(jFile);

            setDialogs(jDialogs);

            UpdateDialogSequence(dialogsList[0]);

            //charger images 
            LoadImages(dialogsList[_sequenceNumber].characterPath, dialogsList[_sequenceNumber].backgroundPath);


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


    void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnClickNextDialog();
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

    }


    [System.Serializable]
    public class Dialogs
    {
        public Dialog[] dialogs;
    }

}
