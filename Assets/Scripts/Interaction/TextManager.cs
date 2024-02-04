using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static void SetInteractionText(string text)
    {
        InteractionManager.Instance.InteractionText.SetText(text);

    }

    public static void SetInfoText(string text)
    {
        InteractionManager.Instance.InfoText.SetText(text);
        LeanTween.LeanTMPAlpha(InteractionManager.Instance.InfoText, 255f, 1f)
            .setEase(LeanTweenType.easeInCirc);
        //
        LeanTween.LeanTMPAlpha(InteractionManager.Instance.InfoText, 0f, 2f)
            .setEase(LeanTweenType.easeOutCirc)
            .setDelay(3f);
    }

    /*public static IEnumerator WaitAndClearInteractionText()
    {
        yield return new WaitForSeconds(3);
        InteractionManager.Instance.InteractionText.SetText("");
    }*/

    public static IEnumerator WaitAndClearInfoText()
    {
        yield return new WaitForSeconds(3);
        LeanTween.LeanTMPAlpha(InteractionManager.Instance.InfoText, 0, 2f)
            .setEase(LeanTweenType.easeOutCirc);
        //InteractionManager.Instance.InfoText.SetText("");
    }

}
