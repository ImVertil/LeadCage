using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCheck : MonoBehaviour
{
    public List<StoryValue> requiredTags;
    public List<StoryValue> blockedTags;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void IsConditionSatisfied()
    {
        if (Progression.Instance.HasAllValues(requiredTags) && (Progression.Instance.HasAnyValues(blockedTags) == false))
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
