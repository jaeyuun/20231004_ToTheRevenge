using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class ScriptLoad : MonoBehaviour
{   
    public ScriptData Load(string fileName)
    {
        if (!fileName.Contains(".json"))
        {
            fileName += ".json";
        }
        fileName = Path.Combine(Application.streamingAssetsPath, fileName);
        string readData = File.ReadAllText(fileName);
        ScriptData scriptData = new ScriptData();

        scriptData = JsonConvert.DeserializeObject<ScriptData>(readData);
        return scriptData;
    }
}
