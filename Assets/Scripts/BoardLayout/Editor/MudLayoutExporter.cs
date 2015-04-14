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

	[MenuItem("Worm Food/Export Pipe Layout")]
	public static void ExportPipeLayout()
	{
		var layout = Gameboard.Instance.CurrentLevel;
		if (layout == null) return;

		string data = "";
		foreach(var token in layout.Tokens)
		{
			if(!token.Token.Pushable && !token.Token.IsEdible && !token.Token.CanFall && !token.Token.IsWorm)
			{
				if (data.Length > 0) data += ",";
				data += string.Format("{0},{1}", token.Position.x, token.Position.y);
			}
		}
		File.WriteAllText(Application.dataPath + @"\test.pipeLayout", data);
	}

	[MenuItem("Worm Food/Text texture creation")]
	public static void TestTexture()
	{
		Texture2D texture = new Texture2D(16, 16, TextureFormat.ARGB32, false);
		texture.SetPixels(0, 0, 16, 16, Color.white);
		texture.SetPixels(0, 0, 8, 8, Color.black);
		texture.Apply();

		byte[] data = texture.EncodeToPNG();
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
