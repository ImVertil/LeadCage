using System;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    #region SINGLETON
    public static Progression Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    private HashSet<StoryValue> storyValues = new();

    public event Action<StoryValue> OnStoryValueAdded;
    public event Action<StoryValue> OnStoryValueRemoved;

    public bool HasAllValues(List<StoryValue> values)
    {
        if (values is null)
            return false;

        foreach (var val in values)
        {
            if (storyValues.Contains(val) == false)
            {
                return false;
            }
        }
        return true;
    }

    public bool HasAnyValues(List<StoryValue> values)
    {
        if (values is null)
            return false;

        foreach (var val in values)
        {
            if (storyValues.Contains(val))
            {
                return true;
            }
        }
        return false;
    }

    public bool AddStoryValue(StoryValue val)
    {
        if (storyValues.Add(val))
        {
            OnStoryValueAdded?.Invoke(val);
            return true;
        }
        return false;
    }

    public bool RemoveStoryValue(StoryValue val)
    {
        if (storyValues.Remove(val))
        {
            OnStoryValueRemoved?.Invoke(val);
            return true;
        }
        return false;
    }

    public void ToggleStoryValue(string val)
    {
        StoryValue parsedVal = (StoryValue) Enum.Parse(typeof(StoryValue), val);
        if (!storyValues.Contains(parsedVal))
            storyValues.Add(parsedVal);
        else
            storyValues.Remove(parsedVal);
    }
}

public enum StoryValue
{
    HasScrewdriver,
    ReachedMagazineArea, 
    KilledX              
}