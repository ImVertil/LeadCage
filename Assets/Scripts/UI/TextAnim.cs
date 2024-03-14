using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class TextAnim : MonoBehaviour
{
    private string _text;
    private TextMeshProUGUI _textComponent;

    private void Awake()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
        _text = _textComponent.text;
        _textComponent.text = "";
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        StringBuilder sb = new StringBuilder();
        yield return new WaitForSeconds(3f);

        foreach(char c in _text)
        {
            sb.Append(c);
            _textComponent.text = sb.ToString();
            yield return new WaitForSeconds(0.075f);
        }
    }
}
