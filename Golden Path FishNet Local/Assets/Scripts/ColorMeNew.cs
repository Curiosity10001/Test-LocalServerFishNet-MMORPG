using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.UI;

public class ColorMeNew : NetworkBehaviour
{
    
   //OnChange makes sure to syncronise changes on server to client--Current Comprehention 12/2022
    [SyncVar (OnChange = nameof(colorChanged))] Color color;

    Canvas canvas;
    Button button;

    void colorChanged(Color old, Color New, bool asServer)
    {
        ///Get component used here as this space acts as independent when it comes to complexe variable 
        ///(does not inherit the complexe variables defined outside only simple ones (int/bool/float ..)
        ///Need to search more about this happenning
        /// set the new color to the material!
            GetComponent<Renderer>().material.SetColor ("_Color", New);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();///Works if a client is connected

        if (!IsClient )
            return;///stop reading script if no client connected     
       
        ///finds the Canvas containing the button even if canvas is desabled in hierarchy
        canvas = FindObjectOfType<Canvas>(true);

        ///finds button in canvas
        button = canvas.GetComponentInChildren<Button>(true);

        /// adde onclick event listener to activate method on button click
        button.onClick.AddListener(() => ChangeColor());
      
    }
    /// Sends Method request to server to be used
    [ServerRpc]
      void ChangeColor()  
    {
        ///uses the free variable to 
       color = Random.ColorHSV();     
    }
  




}
