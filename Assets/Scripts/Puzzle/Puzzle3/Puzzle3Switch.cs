using System.Collections;
using UnityEngine;

public class Puzzle3Switch : MonoBehaviour
{
    private float _startPosX;
    private float _endPosX = -0.11f;
    private float _time = 0.33f;
    private bool _isMoving = false;

    public bool isOn = false;

    void Start()
    {
        _startPosX = transform.localPosition.x;
    }

    private void OnMouseDown()
    {
        if(!_isMoving)
        {
            if (transform.localPosition.x == _startPosX)
            {
                StartCoroutine(Move(_endPosX));
            }
            else
            {
                StartCoroutine(Move(_startPosX));
            }
        }
    }

    public void DisableSwitch() => GetComponent<BoxCollider>().enabled = false;
    
    private IEnumerator Move(float pos)
    {
        _isMoving = true;
        LeanTween.moveLocalX(gameObject, pos, _time)
            .setEase(LeanTweenType.easeInOutQuad);

        gameObject.tag = "Untagged";
        yield return new WaitForSeconds(_time);
        gameObject.tag = Tags.INTERACTABLE;
        _isMoving = false;
        isOn = !isOn;
    }
}
