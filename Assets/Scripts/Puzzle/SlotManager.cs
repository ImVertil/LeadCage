using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private List<float> _voltages = new();
    [SerializeField] private List<GameObject> _slots = new();
    //private List<float> _voltagesScrambled = new();

    void Start()
    {
        for(int i=0; i<_slots.Count; i++) 
        {
            _slots[i].GetComponent<CableSlot>().Voltage = _voltages[i];
        }
    }

    // TODO - shuffle
    /*private List<T> ShuffleList<T>(List<T> list) 
    {
        List<T> result = new List<T>(list.Count);
        List<int> indexes = new List<int>(list.Count);
        foreach(T value in list)
        {
            int randomIndex = Random.Range(1,list.Count);
            result.Insert(randomIndex, value);
        }
        return result;
    }*/
}
