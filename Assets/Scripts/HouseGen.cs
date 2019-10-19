using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGen : MonoBehaviour
{
    public Vector2 origin;
    public int gridSizeX, gridSizeY;
    public float tileSize = 0.1f;

    Tile[,] tiles;
    // Start is called before the first frame update
    void Awake(){
        tiles = new Tile[gridSizeX, gridSizeY];
        InitGrid();
    }

    void InitGrid(){
        for(int x = 0; x < gridSizeX; x++){
            for(int y = 0; y < gridSizeY; y++){
                tiles[x,y] = Tile.CreateTile(0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetWorldPosition(int x, int y){
        return Vector2.zero;
        // TODO 
    }

    void GenerateFullBorder(){
        for(int x = 0; x < gridSizeX; x++){
            for(int y = 0; y < gridSizeY; y++){
                if (x == 0 || y == 0 || x == gridSizeX-1 || y == gridSizeY-1){

                }
            }
        }
    }
}
