using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
   
    public TMP_Text textCharacterName;
    public TMP_Text textDialog;
    public Image spriteCharacter;
    public DialogSequence[] dialogs;
    private int _sequenceNumber = 0;

    void UpdateDialogSequence(DialogSequence s)
    {
        textDialog.text = s.textDialog;
        textCharacterName.text = s.textCharacterName;
        spriteCharacter.sprite = s.spriteCharacter;
    }

    public void OnClickNextDialog()
    {
        _sequenceNumber++;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateDialogSequence(dialogs[0]);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDialogSequence(dialogs[_sequenceNumber]);
    }
}
