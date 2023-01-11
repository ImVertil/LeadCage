using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Item", menuName ="Item/Create New Item")]
public class Item : ScriptableObject
{
    public int Id;
    public string ItemName;
    public Sprite Icon;
    public Type type;


    public enum Type
    {

    }
}
