using UnityEngine;
using System.Collections;

public class TextureQualityChanger : MonoBehaviour {

	public void ChangeTextureQuality(int mipmap)
    {
        QualitySettings.masterTextureLimit = mipmap;
    }
}
