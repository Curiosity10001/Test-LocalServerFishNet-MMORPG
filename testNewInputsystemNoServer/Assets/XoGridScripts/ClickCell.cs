using System;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class ClickCell : MonoBehaviour
{
    [SerializeField] Button buttonX;
    [SerializeField] Button buttonO;
    [SerializeField] LayerMask grid;
    [SerializeField] Material materialX;
    [SerializeField] Material materialO;
    [SerializeField] TextMeshPro text;
    MeshRenderer cellRenderer;
    GameObject cellToChange;
   


    private void Awake()
    {
        buttonO = FindObjectOfType<IdO>().gameObject.GetComponent<Button>();
        buttonX = FindObjectOfType<IdX>().gameObject.GetComponent<Button>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        buttonO.onClick.AddListener(() => UseMaterialO());
        buttonX.onClick.AddListener(() => UseMaterialX());
        Cursor.visible = true;
    }
    void CellSelection()
    {
        //Check for mouse click 
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 300, Color.blue, 5, false);
            if (Physics.Raycast(ray, out hit, 300f, LayerMask.GetMask("Grid")))
            {
                if (hit.transform != null)
                {
                    cellToChange = hit.transform.gameObject;
                    Debug.Log(cellToChange.transform.parent.gameObject.name) ;
                    cellRenderer = cellToChange.GetComponent<MeshRenderer>();
                }
                else return;
            }
            
        }
    }
    private void UseMaterialX()
    {
        
        if (cellRenderer.materials[1] != null)
        {
            
            return;
        }
        else
        {
            cellRenderer.materials[1] = materialX;
        }
    }

    private void UseMaterialO()
    {
        if (cellRenderer.materials[1] != null)
        {
            Console.WriteLine("you already played this case");
            return ;
        }
        else
        {
            cellRenderer.materials[1] = materialO;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CellSelection();
    }
}
