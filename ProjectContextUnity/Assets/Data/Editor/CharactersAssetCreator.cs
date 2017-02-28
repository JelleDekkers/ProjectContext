using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Characters")]
    public static void CreateCharactersAssetFile()
    {
        Characters asset = CustomAssetUtility.CreateAsset<Characters>();
        asset.SheetName = "Spreadsheet";
        asset.WorksheetName = "Characters";
        EditorUtility.SetDirty(asset);        
    }
    
}