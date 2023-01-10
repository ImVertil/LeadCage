using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private List<float> _voltages = new();
    [SerializeField] private List<GameObject> _slots = new();

    void Start()
    {
        //turned off so it's easier to test 8)
        //ShuffleList(_voltages);
        for(int i=0; i<_slots.Count; i++) 
        {
            _slots[i].GetComponent<CableSlot>().Voltage = _voltages[i];
        }
    }

    // Fisher-Yates shuffle algorithm
    private void ShuffleList<T>(List<T> list) 
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int rand = Random.Range(0, n + 1);
            T value = list[rand];
            list[rand] = list[n];
            list[n] = value;
        }
    }
}
