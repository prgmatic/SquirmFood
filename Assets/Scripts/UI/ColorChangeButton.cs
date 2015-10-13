using UnityEngine;
using System.Collections;

public class ColorChangeButton : Button {

    public override void Click()
    {
        var newColor = new Color32(GetRandomColorChannel(),
                                   GetRandomColorChannel(),
                                   GetRandomColorChannel(),
                                   255);
        GetComponent<Renderer>().sharedMaterial.color = newColor;
    }

    private byte GetRandomColorChannel()
    {
        return (byte)Random.Range(0, 255);
    }
}
