using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HighlightEntry
{
    public string key;
    public RectTransform target;
    public float padding = 10f;
}

public class TutorialHighlightMap : MonoBehaviour
{
    [SerializeField] List<HighlightEntry> entries;

    public bool TryGetTarget(string key, out RectTransform target, out float padding)
    {
        var entry = entries.Find(e => e.key == key);
        if (entry == null)
        {
            target = null;
            padding = 0f;
            return false;
        }

        target = entry.target;
        padding = entry.padding;
        return true;
    }
}
