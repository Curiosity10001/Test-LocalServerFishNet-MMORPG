using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class ChangeColor : MonoBehaviour
{
    Renderer playerColor;
    Color color;
    ChangeButtonCreation buttonCreation;
    public Canvas canvas;


    private void Awake()
    {
        playerColor = GetComponent<Renderer>();  
        buttonCreation= canvas.GetComponent<ChangeButtonCreation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonCreation.ButtonCreation();
        buttonCreation.changeButton.onClick.AddListener(()=>ColorChange());
    }

    // Update is called once per frame

    void Update()
    {
       
    }

    public  void ColorChange()
    {
        Color color = Random.ColorHSV();
        playerColor.material.SetColor("_Color",color) ; 
    }

}
