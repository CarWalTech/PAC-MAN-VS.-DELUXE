using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapExtensions
{
    public static void Overwrite(this Tilemap tilemap, Tilemap copyFrom)
    {
        var origin = new Vector2(copyFrom.origin.x, copyFrom.origin.y);
        var size = new Vector2(copyFrom.size.x, copyFrom.size.y);
        tilemap.Overwrite(copyFrom, origin, size);
    }

    public static void Overwrite(this Tilemap tilemap, Tilemap copyFrom, Vector2 pos, Vector2 size)
    {
        tilemap.ClearAllTiles();
        
        var origin_x = (int)pos.x;
        var origin_y = (int)pos.y;
        var width = (int)size.x;
        var height = (int)size.y;

        for (int y = origin_y; y < (origin_y + height); y++)
        {
            for (int x = origin_x; x < (origin_x + width); x++)
            {
                var position = new Vector3Int(x, y, 0);
                TileBase tile = copyFrom.GetTile(position);
                if (tile != null)
                {
                    tilemap.SetTile(position, tile);
                }
            }
        }
    }

    public static T[] GetTiles<T>(this Tilemap tilemap) where T : TileBase
    {
        List<T> tiles = new List<T>();
        
        for (int y = tilemap.origin.y; y < (tilemap.origin.y + tilemap.size.y); y++)
        {
            for (int x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
            {
                T tile = tilemap.GetTile<T>(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    tiles.Add(tile);
                }
            }
        }
        return tiles.ToArray();
    }
}