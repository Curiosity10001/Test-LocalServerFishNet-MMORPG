using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridXO : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab;
    [SerializeField] int horizontalRow;
    [SerializeField] int depthRow;
    [SerializeField] int verticalRow;
    [SerializeField] float cellSpacing;
    GameObject[,,] meGrid;

    
    // Start is called before the first frame update
    void Start()
    {
        GridCreator();
    }

    private void GridCreator()
    {
        meGrid = new GameObject[horizontalRow,verticalRow , depthRow];
        if(cellPrefab == null) 
        { 
            Debug.Log("No cell prefab assigned");
            return;
        }
        //Grid creation
        for (int x = 0; x < horizontalRow; x++)
        {
            for (int z = 0; z < depthRow; z++)
            {
                for(int y = 0;y < verticalRow; y++)
                {
                    //create a GridSpace for each Cell
                    meGrid[x,y,z] = Instantiate(cellPrefab, new Vector3(x * cellSpacing, y*cellSpacing,z * cellSpacing), Quaternion.identity) ;
                    meGrid[x,y,z].transform.parent = transform;
                    meGrid[x,y,z].gameObject.name = "(X: " + x.ToString()+ " , Y: " + y.ToString() + ")" + " , Z: " + z.ToString() + ")";
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
