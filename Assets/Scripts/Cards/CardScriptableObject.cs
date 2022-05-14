using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Floor Whale/Card Scriptable Object", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public string title;
    public Sprite image;
    public CardType cardType;
    public string description;

    [Range(0.0f, 1.0f)]
    public float probability = .5f;

    ///To add later
    //public Unit unit
   
}
