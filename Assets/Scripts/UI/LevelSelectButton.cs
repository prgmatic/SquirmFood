using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
	public delegate void SelectLevelEvent(NewBoardLayout level);
	public event SelectLevelEvent LevelSelected;

    public Text ButtonText;

    [System.NonSerialized]
    public NewBoardLayout Layout;

    public void Select()
    {
		Debug.Log("Level selected");
		if(LevelSelected != null)
		{
			LevelSelected(Layout);
		}
    }
}
