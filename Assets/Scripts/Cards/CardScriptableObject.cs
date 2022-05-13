using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Floor Whale/Card Scriptable Object")]
public class CardScriptableObject : MonoBehaviour
{
    public string title;
    public Sprite image;
    public CardType cardType;
    public string description;

    [Range(0,1)]
    public int probability;

    ///To add later
    //public Unit unit
   
}
