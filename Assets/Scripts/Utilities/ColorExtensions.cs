using UnityEngine;

namespace Utilities
{
	public static class ColorExtensions
	{
		public static Color SetAlpha(this Color color, float alpha)
		{
			color = new Color(color.r, color.g, color.b, alpha);
			return color;
		}

		public static Color SetRed(this Color color, float red)
		{
			color = new Color(red, color.g, color.b, color.a);
			return color;
		}

		public static Color SetGreen(this Color color, float green)
		{
			color = new Color(color.r, green, color.b, color.a);
			return color;
		}

		public static Color SetBlue(this Color color, float blue)
		{
			color = new Color(color.r, color.g, blue, color.a);
			return color;
		}
	}
}

