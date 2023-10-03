using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{

    
    public Button button;//ce que j'ai rajouté




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

        //ce que j'ai rajouté
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
        UpdateDialogSequence(dialogs[0]);
    }

   
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            OnClickNextDialog();
        }

    }


}
