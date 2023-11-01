using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static IEnumerator WaitAndClearInteractionText()
    {
        yield return new WaitForSeconds(3);
        InteractionManager.Instance.InteractionText.SetText("");
    }

    public static IEnumerator WaitAndClearInfoText()
    {
        yield return new WaitForSeconds(3);
        InteractionManager.Instance.InfoText.SetText("");
    }
}
