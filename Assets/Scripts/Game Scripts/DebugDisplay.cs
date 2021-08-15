using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplay : MonoBehaviour
{
    private Dictionary<string, string> debugInfo = new Dictionary<string, string>();
    private string debugText;
    private Text displayText;

    // Start is called before the first frame update
    void Start()
    {   
        displayText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // UpdateDebug adds or changes values displayed by the text box
    // UpdateDebug should be called from other scripts using GameObject.Find("Debug Overlay").GetComponent<DebugDisplay>().UpdateDebug(string name, string value)
    // Try to avoid calling GameObject.Find too often if you can
    public void UpdateDebug(string name, string value)
    {
        debugInfo[name] = value;

        debugText = (string) "";
        foreach(KeyValuePair<string, string> entry in debugInfo)
        {
            debugText += entry.Key + ": " + entry.Value + "\n";
        }

        displayText.text = debugText;
    }
}
