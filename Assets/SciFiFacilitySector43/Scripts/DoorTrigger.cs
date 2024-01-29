using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private Door door;

    void Start()
    {
        door = GetComponentInParent<Door>();
    }

    void OnTriggerEnter(Collider c) {

        door.OpenDoor();

    }

    void OnTriggerExit(Collider c) {
        door.CloseDoor();
    }
}
