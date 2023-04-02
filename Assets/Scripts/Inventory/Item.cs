using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public sealed class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;
    public Mesh modelMesh;
    public StoryValue associatedStoryValue;
}