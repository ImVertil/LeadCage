using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle3Generator : MonoBehaviour, IInteractable
{
    [SerializeField] private int _amountOfShields = 2;
    [SerializeField] private int _amountOfPipes = 4;

    [SerializeField] private MeshRenderer _shieldLamp1;
    [SerializeField] private MeshRenderer _shieldLamp2;
    [SerializeField] private MeshRenderer _pipesLamp;

    [SerializeField] private Material _lampMatRed;
    [SerializeField] private Material _lampMatGreen;

    // REMOVE LATER AND REPLACE WHEN KEYPAD PUZZLE IS DONE
    [SerializeField] private GameObject[] _doorsToOpen;

    private HashSet<Puzzle3Lever> _part1done = new();
    private HashSet<Puzzle3Valve> _part2done = new();

    private Animator _animator;

    void Awake()
    {
        PuzzleEvents.GeneratorShieldStatusChanged += CheckPart1;
        PuzzleEvents.GeneratorPipeStatusChanged += CheckPart2;
        _animator = GetComponentInParent<Animator>();
    }

    public void OnStartLook()
    {
        InteractionManager.Instance.InteractionText.SetText($"Press [E] to turn on the generator");
    }

    public void OnEndLook()
    {
        InteractionManager.Instance.InteractionText.SetText($"");
    }

    public void OnInteract(InputAction.CallbackContext ctx) => ActivateGenerator();

    public void CheckPart1(Puzzle3Lever p)
    {
        if(_part1done.Contains(p))
        {
            _part1done.Remove(p);
        }
        else
        {
            _part1done.Add(p);
        }

        CheckForLampSwitch();
    }

    public void CheckPart2(Puzzle3Valve p)
    {
        if (_part2done.Contains(p))
        {
            _part2done.Remove(p);
        }
        else
        {
            _part2done.Add(p);
        }

        CheckForLampSwitch();
    }

    public void CheckForLampSwitch()
    {
        _animator.SetBool($"LampSwitch{_part1done.Count}", true);
        _animator.SetBool("LampSwitch3", _part2done.Count == _amountOfPipes);
    }

    public void ActivateGenerator()
    {
        if(_part1done.Count == _amountOfShields && _part2done.Count == _amountOfPipes)
        {
            gameObject.tag = Tags.UNTAGGED;
            LightManager.Instance.TurnOnLightsGlobal();
            InteractionManager.Instance.InfoText.SetText("The generator has been powered on. Find a way to escape the station.");
            StartCoroutine(TextManager.WaitAndClearInfoText());
            foreach(var door in _doorsToOpen)
            {
                door.GetComponentInChildren<DoorScript>().OpenDoor();
            }
        }
        else
        {
            InteractionManager.Instance.InfoText.SetText("Nothing happened. There's still something to do.");
            StartCoroutine(TextManager.WaitAndClearInfoText());
        }
    }

    
}
