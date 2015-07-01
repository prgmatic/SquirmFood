using UnityEngine;
using System.IO;
using UnityEditor;

public static class MudLayoutExporter
{

	[MenuItem("Worm Food/Export Mud Layout")]
	public static void Export()
	{
		if (Gameboard.Instance.CurrentLevel == null) return;
		Texture2D outputTexture = new Texture2D(1400, 1680, TextureFormat.ARGB32, false);
		outputTexture.SetPixels(0, 0, 1400, 1680, Color.white);

		var layout = Gameboard.Instance.CurrentLevel;
		Color[] mudTileColors = new Color[140 * 140];
		for (int i = 0; i < mudTileColors.Length; i++)
			mudTileColors[i] = Color.black;

		for (int y = 0; y < layout.Rows; y++)
		{
			for (int x = 0; x < layout.Columns; x++)
			{
				var tileAttribute = layout.BackgroundTileAttributes[x + y * layout.Columns];
				if (tileAttribute == Gameboard.BackgroundTileAttribute.FreeMove)
				{
					outputTexture.SetPixels((x + 1) * 140, (y + 1) * 140, 140, 140, mudTileColors);
				}
			}
		}
		outputTexture.Apply();

		byte[] data = outputTexture.EncodeToPNG();
		File.WriteAllBytes(Application.dataPath + @"\test.png", data);
	}

	public static void SetPixels(this Texture2D texture, int x, int y, int width, int height, Color color)
	{
		var colors = new Color[width * height];
		for (int i = 0; i < colors.Length; i++)
		{
			colors[i] = color;
		}
		texture.SetPixels(x, y, width, height, colors);
	}
}
