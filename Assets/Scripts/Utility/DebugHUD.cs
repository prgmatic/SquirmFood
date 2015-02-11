using UnityEngine;
using System.Collections.Generic;

public class DebugHUD : MonoBehaviour 
{
    private static List<string> _messages = new List<string>();
    public static event System.EventHandler MessagesCleared;

    public static void Add(string msg)
    {
        _messages.Add(msg);
    }

    void OnGUI()
    {
        foreach(var msg in _messages)
        { 
            GUILayout.Label(msg);
        }
        if (Event.current.type == EventType.Repaint)
        {
            _messages.Clear();
            if (MessagesCleared != null)
                MessagesCleared(null, System.EventArgs.Empty);
        }
    }
}
