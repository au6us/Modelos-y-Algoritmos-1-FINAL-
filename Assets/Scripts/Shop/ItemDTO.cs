using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "New Item", order = 0)]
public class ItemDTO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int itemCost;
}
