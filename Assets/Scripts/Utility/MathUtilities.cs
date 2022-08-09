using UnityEngine;

public class MathUtilities : MonoBehaviour
{
    public static float PixelsToUnits(int pixels)
    {
        // Expresses the pixel value in terms of world units (1 unit == 16 pixels).
        return 0.0625f * pixels;
    }

    public static int GetCardinal(Vector3 direction)
    {
        // Returns an integer representing degrees of rotation where north == 0 and west == 270.
        if (direction.y < 0) { return 0; }
        else if (direction.y > 0) { return 90; }
        else if (direction.x > 0) { return 180; }
        else { return 270; }
    }

    public static Color HexToColor(string hex)
    {
        // Converts a hex string to a Color object.
        hex = hex.Replace("0x", "").Replace("#", "");
        byte a = 255;
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        // Only use alpha if the string has enough characters.
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
}