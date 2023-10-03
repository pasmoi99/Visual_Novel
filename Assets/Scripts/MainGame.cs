using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class MainGame : MonoBehaviour
{

    
    public Button button;//ce que j'ai rajoute



    private string file,path,text;//Donnees pour lire un fichier json
    public TMP_Text textCharacterName;
    public TMP_Text textDialog;
    public Image spriteCharacter;
    public Image spriteBackground; //bg
    public DialogSequence[] dialogs;
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

        //ce que j'ai rajoute
        if (_sequenceNumber >= dialogs.Length)
        {
            button.gameObject.SetActive(false);

        }




        if (_sequenceNumber < dialogs.Length)
        {
            UpdateDialogSequence(dialogs[_sequenceNumber]);
        }

    }

  
    void Start()
    {
        path = Application.streamingAssetsPath + "/TextTest.json";
        file = File.ReadAllText(path);
        UpdateDialogSequence(dialogs[0]);

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

