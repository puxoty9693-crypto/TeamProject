using UnityEngine;

public static class GoldFormatter
{
    public static string Format(int amount)
    {
        if (amount >= 1_000_000_000)
            return $"{amount / 1_000_000_000f:0.#}B";
        if (amount >= 1_000_000)
            return $"{amount / 1_000_000f:0.#}M";
        if (amount >= 1_000)
            return $"{amount / 1_000f:0.#}K";
        
        return amount.ToString();
    }
}
