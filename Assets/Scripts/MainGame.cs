using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class MainGame : MonoBehaviour
{

    
    public Button button;



    private string jFile; //variable pour lire le fichier json
    private string jPath; //variable du chemin vers le fichier json


    public TMP_Text textCharacterName;
    public TMP_Text textDialog;
    public Image spriteCharacter;
    public Image spriteBackground; //bg
    public List<DialogSequence> dialogsList=new List<DialogSequence>();
    private int _sequenceNumber = 0;

    void UpdateDialogSequence(DialogSequence s)
    {
        textDialog.text = s.textDialog;
        textCharacterName.text = s.textCharacterName;
        spriteCharacter.sprite = s.spriteCharacter;
        spriteBackground.sprite = s.spriteBackground; //bg
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
        for (int i=0; i<d.dialogs.Length; i++)
        {
            dialogsList.Add(new DialogSequence() { id = d.dialogs[i].id, textCharacterName = d.dialogs[i].name, textDialog= d.dialogs[i].dialog });
        }
    }

  
    void Start()
    {
        jPath = Application.streamingAssetsPath + "/TextTest.json"; //chemin fichier JSON
        jFile = File.ReadAllText(jPath); //lecture fichier JSON
        Dialogs jDialogs = JsonUtility.FromJson<Dialogs>(jFile);

        setDialogs(jDialogs);

        UpdateDialogSequence(dialogsList[0]);

        //text
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClickNextDialog();
        }

    }


}
[System.Serializable]
public class Dialog
{
    public int id;
    public string name;
    public string dialog;
}
[System.Serializable]
public class Dialogs
{
    public Dialog[] dialogs;
}

