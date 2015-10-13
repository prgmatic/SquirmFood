using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectButton2 : MonoBehaviour
{
	public delegate void SelectLevelEvent(int levelNumber);
	public event SelectLevelEvent LevelSelected;

    public Text ButtonText;

    [System.NonSerialized]
    public int LevelNumber;
    //public NewBoardLayout Layout;

    public void Select()
    {
		Debug.Log("Level selected");
		if(LevelSelected != null)
		{
			LevelSelected(LevelNumber);
		}
    }
}
