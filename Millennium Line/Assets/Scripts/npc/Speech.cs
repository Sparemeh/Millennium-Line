using UnityEngine;

[System.Serializable]
public class Speech
{
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;


    
}