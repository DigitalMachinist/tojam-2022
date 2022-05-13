using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public int maxHandCards = 3;

    public Card [] PlayerHand { get; private set; }


    public int MyProperty { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

        PlayerHand = new Card[maxHandCards];

        for (int i = 0; i < PlayerHand.Length; i++)
        {
            PlayerHand[i] = Deck.instance.DrawCard();
        }
            
            
    }

    public Card PlayCard(int handIndex)
    {
        Card temp = RemoveCardAtIndex(handIndex);

        temp.Play();

        return temp;
    }

    public void AddCardAt(Card card, int handIndex)
    {
        PlayerHand[handIndex] = card;
    }


    public Card RemoveCardAtIndex(int handIndex)
    {
        Card temp = PlayerHand[handIndex];

        if (handIndex >= PlayerHand.Length || temp == null)
        {
            Debug.LogError("Hand index out of range or card slot null");
            return null;
        }

        PlayerHand[handIndex] = null;

        return temp;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
