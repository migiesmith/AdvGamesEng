/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TransformDeepChildExtension
{

    // Find Child with a specific name
    public static Transform FindDeepChild(this Transform parent, string name)
    {
        Transform result = parent.Find(name);
        if (result != null)
            return result;
        foreach (Transform child in parent)
        {
            result = child.FindDeepChild(name);
            if (result != null)
                return result;
        }
        return null;
    }

    // Find Child with a specific name
    public static List<Transform> FindDeepChildren(this Transform parent, string name)
    {
        List<Transform> results = new List<Transform>();
        int childCount = parent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if (parent.GetChild(i).name.Equals(name))
            {
                results.Add(parent.GetChild(i).transform);
            }
        }
        foreach (Transform child in parent)
        {
            results.AddRange(child.FindDeepChildren(name));
        }
        return results;
    }

}