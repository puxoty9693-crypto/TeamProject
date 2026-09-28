using UnityEngine;

public static class ObjectIds
{
    public const string Table = "Table";
    public const string TableIdPrefix = "table_";
}

public static class ColorExtensions
{
    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}