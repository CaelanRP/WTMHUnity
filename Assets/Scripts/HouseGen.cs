using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGen : MonoBehaviour
{
    public Vector2 origin;
    public int gridSizeX, gridSizeY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateFullBorder(){
        for(int x = 0; x < gridSizeX; x++){
            for(int y = 0; y < gridSizeY; y++){
                if (x == 0 || y == 0 || x == gridSizeX-1 || y == gridSizeY-1){

                }
            }
        }
    }

    void GenerateTile(int gridX, int gridY, int tilesetX, int tilesetY){
        
    }
}
