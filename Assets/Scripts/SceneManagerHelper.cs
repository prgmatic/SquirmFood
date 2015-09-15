using UnityEngine;
using System.Collections.Generic;

public class SceneManagerHelper : MonoBehaviour 
{
    public bool GoToMenuOnEscape = true;

    private void Update()
    {
        if (!GoToMenuOnEscape) return;
        if (Input.GetKeyDown(KeyCode.Escape))
            OpenMainMenu();
    }

	public void OpenLevelSelection()
	{
        Application.LoadLevel("LevelSelection");
	}

    public void OpenMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }
}
