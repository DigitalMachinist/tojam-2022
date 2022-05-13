using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    public int deckSize;
    public CardScriptableObject[] possibleCards;
    private List<Card> deckCards;

    private void Awake()
    {
        instance = this;
        deckCards = GenerateCards(deckSize);
    }

    // Start is called before the first frame update
    void Start()
    {
        //deckCards = GenerateCards(deckSize);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<Card> GenerateCards(int numberOfCardsToGenerate)
    {
        List<Card> tempNewCards = new List<Card>();

        for (int i = 0; i < numberOfCardsToGenerate; i++)
        {
            CardScriptableObject tempCard;

            do
            {
                tempCard = possibleCards[Random.Range(0, possibleCards.Length)];
            } while (tempCard.probability < Random.Range(0f, 1f - Mathf.Epsilon));

            tempNewCards.Add(new Card(tempCard));
        }

        return tempNewCards;
    }


    public List<Card> DrawCards(int count)
    {
        List<Card> tempCards = new List<Card>();

        for (int i = 0; i < count; i++)
        {
            Card tempCard = DrawCard();

            if (tempCard == null)
            {
                Debug.LogError("Sorry, not enough cards in the deck");
                return null;
            }

            tempCards.Add(tempCard);
        }

        return tempCards;
    }

    public Card DrawCard()
    {
        Card drawnCard = null;

        if (deckCards.Count > 0)
        {
            drawnCard = deckCards[0];
            deckCards.RemoveAt(0);
        }
        else
        {
            Debug.LogError("No cards to remove, deck empty!");
        }

        return drawnCard;
    }
}
