using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingTextAnim : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshProUGUI;

    private void Awake()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        StartCoroutine(Anim());
    }

    private IEnumerator Anim()
    {
        while(true)
        {
            m_TextMeshProUGUI.text = "Loading";
            yield return new WaitForSeconds(1);
            m_TextMeshProUGUI.text = "Loading.";
            yield return new WaitForSeconds(1);
            m_TextMeshProUGUI.text = "Loading..";
            yield return new WaitForSeconds(1);
            m_TextMeshProUGUI.text = "Loading...";
            yield return new WaitForSeconds(1);
        }
    }
}
