using System.Collections;
using System.Collections.Generic;
using Players;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    public int deckSize;
    public CardScriptableObject[] possibleCards;
    private List<CardScriptableObject> deckCards;

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

    private List<CardScriptableObject> GenerateCards(int numberOfCardsToGenerate)
    {
        List<CardScriptableObject> tempNewCards = new List<CardScriptableObject>();

        for (int i = 0; i < numberOfCardsToGenerate; i++)
        {
            CardScriptableObject tempCard;

            do
            {
                tempCard = possibleCards[Random.Range(0, possibleCards.Length)];
            } while (tempCard.probability < Random.Range(0f, 1f - Mathf.Epsilon));

            tempNewCards.Add(tempCard);
        }

        return tempNewCards;
    }


    public List<CardScriptableObject> DrawCards(int count, PlayerColour colour)
    {
        List<CardScriptableObject> tempCards = new List<CardScriptableObject>();

        for (int i = 0; i < count; i++)
        {
            CardScriptableObject tempCard = DrawCard(colour);

            if (tempCard == null)
            {
                Debug.LogError("Sorry, not enough cards in the deck");
                return null;
            }

            tempCards.Add(tempCard);
        }

        return tempCards;
    }

    public CardScriptableObject DrawCard(PlayerColour colour)
    {
        CardScriptableObject drawnCard = null;

        if (deckCards.Count == 0)
        {
            deckCards = GenerateCards(deckSize);
        }

        drawnCard = deckCards[0];
        deckCards.RemoveAt(0);

        return drawnCard;
    }
}
