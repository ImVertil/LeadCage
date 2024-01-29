using System.Text;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    [SerializeField] private string _code = "0000";
    private StringBuilder _currentCode = new();

    public void AddToCode(int number)
    {
        _currentCode.Append(number);
        PlayPressSound();
    }

    public void RemoveFromCode()
    {
        PlayPressSound();
        if (_currentCode.Length > 0 )
        {
            _currentCode.Remove(_currentCode.Length - 1, 1);
        }
    }


    public void Validate()
    {
        PlayPressSound();
        if (_currentCode.ToString() == _code)
        {
            Debug.Log("OPEN");
        }
        else
        {
            Debug.Log("Nope");
        }
        ClearCode();
    }
    public void ClearCode() => _currentCode.Clear();
    private void PlayPressSound() => SoundManager.Instance.PlaySound(Sound.KeypadPress, transform, false);
}
