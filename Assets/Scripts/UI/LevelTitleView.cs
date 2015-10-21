using UnityEngine;
using UnityEngine.UI;

public class LevelTitleView : MonoBehaviour
{
    public Text LevelName;
    public Text Diffuculty;
	
    public void SetAlpha(float alpha)
    {
        LevelName.color = new Color(LevelName.color.a, LevelName.color.g, LevelName.color.b, alpha);
        Diffuculty.color = new Color(Diffuculty.color.a, Diffuculty.color.g, Diffuculty.color.b, alpha);
    }
}
