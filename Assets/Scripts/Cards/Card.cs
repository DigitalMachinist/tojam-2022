using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Unit = 0,
    Effect = 1
}

//this class will probably be used to create game objects later

                    //not sure this needs to be a monobevaviour? We can add it back in no prob if so
public class Card //: MonoBehaviour
{
    private CardScriptableObject _cardSO;

    private string title;
    private Sprite image;
    private CardType cardType;
    private string description;
   


    public Card(CardScriptableObject cardSO)
    {
        PopulateCardFields(cardSO);
    }

    public void PopulateCardFields(CardScriptableObject cardSO)
    {
        title = cardSO.title;
        image = cardSO.image;
        cardType = cardSO.cardType;
        description = cardSO.description;
    }

    public void Play()
    {
        Debug.Log("Card: " + title + ". ACTION!");
    }


    //// Start is called before the first frame update
    //void Start()
    //{
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
