using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MiniJSON;
public class LectorTxt : MonoBehaviour
{

    public void LoadLevel(int level)
    {
        try
        {
            string path;
            path = "/Assets/Levels/nivel1.json";



            var dict = Json.Deserialize(path) as Dictionary<string,object>;

            //Debug.Log((List<object>)dict["index"]);

}
        catch (System.Exception e)
        {
            Debug.LogException(e, this);
        }
    }
}


[System.Serializable]
public class PlayerInfo { 
    public int index { get; set; }
    public List<string> layout { get; set; }
    public List<List<int>> path { get; set; }
}
