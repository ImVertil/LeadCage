using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoTextTrigger : MonoBehaviour
{
    [SerializeField] private string _textToShow;
    private bool _shownAlready = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!_shownAlready)
        {
            InteractionManager.Instance.SetInfoText(_textToShow);
            _shownAlready = true;
        }
    }
}
