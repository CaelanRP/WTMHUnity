using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static Tile _T;
    public static Tile prefab{
        get{
            if (_T == null){
                _T = Resources.Load<GameObject>("Tile").GetComponent<Tile>();
            }
            return _T;
        }
    }

    private static Sprite[] _S;
    public static Sprite[] sprites{
        get{
            if (_S == null){
                _S = Resources.LoadAll<Sprite>("Tileset_Default");
            }
            return _S;
        }
    }

    SpriteRenderer spr;

    void Awake(){
        spr = GetComponent<SpriteRenderer>();
    }

    public static Tile CreateTile(int tileIndex){
        Tile newTile = Instantiate(prefab);
        newTile.SetSprite(tileIndex);
        return newTile;
    }

    public void SetSprite(int spriteIndex){
        if (spriteIndex == -1){
            spr.sprite = null;
            return;
        }
        try{
            spr.sprite = sprites[spriteIndex];
        } catch (Exception e){
            Debug.LogError(e);
        }
    }
}
