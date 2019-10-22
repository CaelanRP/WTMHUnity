using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGen : MonoBehaviour
{
    public Vector2 origin;
    public int gridSizeX, gridSizeY;
    public float tileSize = 0.1f;

    public float maxPlayerPos, exitHousePos;

    private Transform m_tileContainer;
    public Transform tileContainer{
        get{
            if (m_tileContainer == null){
                m_tileContainer = new GameObject("Tile Container").transform;
                m_tileContainer.SetParent(transform);
                m_tileContainer.transform.position = origin;
            }
            return m_tileContainer;
        }
    }

    Tile[,] tiles;
    // Start is called before the first frame update

    public static HouseGen instance;
    void Awake(){
        instance = this;
        tiles = new Tile[gridSizeX, gridSizeY];
        InitGrid();

        //GenerateHouse();
    }

    void InitGrid(){
        for(int x = 0; x < gridSizeX; x++){
            for(int y = 0; y < gridSizeY; y++){
                var newTile = Tile.CreateTile(-1);
                newTile.transform.SetParent(tileContainer, true);
                newTile.transform.position = GetWorldPosition(x,y);
                tiles[x,y] = newTile;
            }
        }
    }

    public void GenerateHouse(){
        GenerateWalls();
        TextScroller.instance.Reset();
    }

    public Vector2 GetWorldPosition(int x, int y){
        return origin + new Vector2(x * tileSize, y * tileSize);
    }

    float DistFromCenterX(int x){
        float dist = (((float)gridSizeX - 1) / 2f) - (x);
        return Mathf.Abs(dist);
    }

    bool IsBorderTile(int x, int y){
        return (x == 0 || y == 0 || x == gridSizeX-1 || y == gridSizeY-1);
    }

    bool IsDoorTile(int x, int y){
        return (DistFromCenterX(x) < 1f && y == 0);
    }

    void GenerateWalls(){
        for(int x = 0; x < gridSizeX; x++){
            for(int y = 0; y < gridSizeY; y++){
                if (IsBorderTile(x, y) && !IsDoorTile(x, y)){
                    tiles[x,y].SetSprite(0);
                }
                else{
                    //tiles[x,y].SetSprite(1);
                }
            }
        }
    }
}
