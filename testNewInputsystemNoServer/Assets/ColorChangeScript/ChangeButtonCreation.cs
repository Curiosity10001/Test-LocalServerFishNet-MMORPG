using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonCreation : MonoBehaviour
{
    GameObject button;
    public Button changeButton;
    Transform buttonParent;
    TMP_DefaultControls.Resources ressources;
    TMP_Text buttonText;
    
    private void Awake()
    {
        buttonParent = gameObject.transform.Find("ButtonParent").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public  void ButtonCreation()
    {      
        if (changeButton == null)
        {

            ressources = new TMP_DefaultControls.Resources();
         
            button = TMP_DefaultControls.CreateButton(ressources);
            button.transform.SetParent(buttonParent, false);

            changeButton = button.GetComponent<Button>();
            buttonText = button.GetComponentInChildren<TMP_Text>();

            buttonText.text = "Color me New !";
        }   

    }
}
