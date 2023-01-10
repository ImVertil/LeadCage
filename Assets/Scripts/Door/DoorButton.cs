using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] private DoorController _doorController;
    [SerializeField] private Cable _buttonCable;

    void Start()
    {
        
    }

    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.F))
        {
            _doorController.ToggleState();
        }*/
    }
}
