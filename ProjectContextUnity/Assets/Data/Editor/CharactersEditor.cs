using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using GDataDB;
using GDataDB.Linq;

using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
[CustomEditor(typeof(Characters))]
public class CharactersEditor : BaseGoogleEditor<Characters>
{	    
    public override bool Load()
    {        
        Characters targetData = target as Characters;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<CharactersData>(targetData.WorksheetName) ?? db.CreateTable<CharactersData>(targetData.WorksheetName);
        
        List<CharactersData> myDataList = new List<CharactersData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            CharactersData data = new CharactersData();
            
            data = Cloner.DeepCopy<CharactersData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
