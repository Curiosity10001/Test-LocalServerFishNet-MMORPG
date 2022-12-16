using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ClickCell : MonoBehaviour
{
    
    [SerializeField] LayerMask grid;
    [SerializeField] Material materialX;
    [SerializeField] Material materialO;
    [SerializeField] TMP_Text text;
    [SerializeField] Button buttonX;
    [SerializeField] Button buttonO;
    Renderer cellRenderer;
    Material[] cellMaterials;
    Material[] newMats ;
    bool canBeChanged;
    GameObject cellToChange;
    float timerText;



    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        buttonO = FindObjectOfType<IdO>().gameObject.GetComponent<Button>();
        buttonX = FindObjectOfType<IdX>().gameObject.GetComponent<Button>();
        text = FindObjectOfType<IdM>(true).gameObject.GetComponent<TMP_Text>();
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
                    cellRenderer = cellToChange.GetComponent<Renderer>();
                }
                else return;
            }
            
        }
    }
    private void UseMaterialX()
    {
            Debug.Log(canBeChanged);
        timerText = 0;
        cellMaterials = cellRenderer.materials;
        newMats = new Material[cellRenderer.materials.Length];
        cellMaterials.CopyTo(newMats,0);
        newMats[1] = materialX;
        
            if (canBeChanged == true)
            {
                text.gameObject.SetActive(true);
                return;
            }
            else
            {
                cellRenderer.materials = newMats;
            }
            
            
    }

    private void UseMaterialO()
    {
        Debug.Log(canBeChanged);
        timerText = 0;
        cellMaterials = cellRenderer.materials;
        newMats = new Material[cellRenderer.materials.Length];
        cellMaterials.CopyTo(newMats, 0); 
        newMats[1] = materialO;
        if (canBeChanged == false)
        {
            text.gameObject.SetActive(true);
            return;
        }
        else
        {
            cellRenderer.materials = newMats;
            canBeChanged = false;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        timerText += Time.deltaTime;
        CellSelection();
        if (timerText > 1f)
        {
            text.gameObject.SetActive(false);
        }
    }
}
