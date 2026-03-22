using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using SimpleSpritePacker;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using EditorAttributes;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Themes/MazeTheme", order = 1)]
[System.Serializable]
public class MazeTheme : MazeThemeBase
{
    public string themeName;
    public string themeUUID;
    public bool supportsRecolors = false;
    public int tileResolution = 24;
    [SerializedDictionary("Tile Group", "Sprites")] public SerializedDictionary<string, List<Sprite>> tileList;
    public override SerializedDictionary<string, List<Sprite>> GetTiles() { return tileList; }



    #region Migration / Setup Stuff
    [FoldoutGroup("Migratables", nameof(migrateItems), nameof(migrateHolder))]
    [SerializeField] private EditorAttributes.Void groupHolder;
    [SerializeField, HideProperty] private List<Sprite> migrateItems = new List<Sprite>();
    [SerializeField, ButtonField(nameof(Migrate)), HideProperty] private EditorAttributes.Void migrateHolder;
    private void Migrate()
    {
        void AddTo(string group, string sprite_id)
        {
            var result = migrateItems.Find(x => x.name.EndsWith("_" + sprite_id));
            if (result) tileList[group].Add(result);
            else tileList[group].Add(null);
        }

        

        if (tileList.Count != 0)
        {
            Debug.LogError("Can't Migate a Existing List, Please Clear it First");
            return;
        }

        tileList.Add("Home", new List<Sprite>());
        tileList.Add("BorderV", new List<Sprite>());
        tileList.Add("BorderH", new List<Sprite>());
        tileList.Add("BorderTV", new List<Sprite>());
        tileList.Add("BorderTH", new List<Sprite>());
        tileList.Add("EnclosedSolid", new List<Sprite>());
        tileList.Add("EnclosedThin", new List<Sprite>());
        tileList.Add("EnclosedTT", new List<Sprite>());
        tileList.Add("EnclosedThick", new List<Sprite>());
        tileList.Add("Floors", new List<Sprite>());
        tileList.Add("Pillars", new List<Sprite>());
        tileList.Add("Dev", new List<Sprite>());

        AddTo("Home", "48");
        AddTo("Home", "01");
        AddTo("Home", "47");

        AddTo("BorderV", "28");
        AddTo("BorderV", "25");
        AddTo("BorderV", "30");

        AddTo("BorderH", "29");
        AddTo("BorderH", "26");
        AddTo("BorderH", "27");

        AddTo("BorderTV", "23");
        AddTo("BorderTV", "12");
        AddTo("BorderTV", "21");

        AddTo("BorderTH", "22");
        AddTo("BorderTH", "11");
        AddTo("BorderTH", "24");

        AddTo("EnclosedSolid", "04");
        AddTo("EnclosedSolid", "09");
        AddTo("EnclosedSolid", "05");
        AddTo("EnclosedSolid", "08");
        AddTo("EnclosedSolid", "02");
        AddTo("EnclosedSolid", "10");
        AddTo("EnclosedSolid", "03");
        AddTo("EnclosedSolid", "07");
        AddTo("EnclosedSolid", "06");

        AddTo("EnclosedThin", "31");
        AddTo("EnclosedThin", "36");
        AddTo("EnclosedThin", "33");
        AddTo("EnclosedThin", "37");
        AddTo("EnclosedThin", "51");
        AddTo("EnclosedThin", "35");
        AddTo("EnclosedThin", "32");
        AddTo("EnclosedThin", "38");
        AddTo("EnclosedThin", "34");

        AddTo("EnclosedTT", "45");
        AddTo("EnclosedTT", "39");
        AddTo("EnclosedTT", "44");
        AddTo("EnclosedTT", "41");
        AddTo("EnclosedTT", "50");
        AddTo("EnclosedTT", "42");
        AddTo("EnclosedTT", "46");
        AddTo("EnclosedTT", "40");
        AddTo("EnclosedTT", "43");

        AddTo("EnclosedThick", "16");
        AddTo("EnclosedThick", "19");
        AddTo("EnclosedThick", "14");
        AddTo("EnclosedThick", "18");
        AddTo("EnclosedThick", "49");
        AddTo("EnclosedThick", "20");
        AddTo("EnclosedThick", "15");
        AddTo("EnclosedThick", "17");
        AddTo("EnclosedThick", "13");

        AddTo("Floors", "53");
        AddTo("Floors", "54");

        AddTo("Pillars", "52");
        
        AddTo("Dev", "00");

    }
    #endregion

}
