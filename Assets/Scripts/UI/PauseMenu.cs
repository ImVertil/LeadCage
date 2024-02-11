using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _backgroundDarken;
    [SerializeField] private GameObject _settingsPanel;
    private bool _isPaused = false;

    private void Start()
    {
        InputManager.current.PauseToggleAction.performed += TogglePauseMenu;
    }

    public void TogglePauseMenu(InputAction.CallbackContext context) => TogglePauseMenu();

    public void TogglePauseMenu()
    {
        SoundEvents.GamePaused(!_isPaused);

        if (_isPaused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _settingsPanel.SetActive(false);
            _pausePanel.SetActive(false);
            _backgroundDarken.SetActive(false);
            Time.timeScale = 1;
            _isPaused = false;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _pausePanel.SetActive(true);
            _backgroundDarken.SetActive(true);
            Time.timeScale = 0;
            _isPaused = true;
        }
    }
}
