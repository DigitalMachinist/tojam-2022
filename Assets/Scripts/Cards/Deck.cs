using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Players;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    public Transform TwistyCard;
    public float TwistyDegrees = 15;
    public int deckSize;
    public CardScriptableObject[] possibleCards;
    private List<CardScriptableObject> deckCards;
        
    public void RenderReset()
    {
        if (TwistyCard == null)
        {
            return;
        }
        
        var temp = TwistyCard.transform.rotation;
        temp.y = 0;
        TwistyCard.transform.rotation = temp;
    }
    
    public void RenderHovered()
    {
        if (TwistyCard == null)
        {
            return;
        }
        
        var temp = TwistyCard.transform.rotation;
        temp.y = TwistyDegrees;
        TwistyCard.transform.rotation = temp;
    }
    
    public bool IsHoveringDeck()
    {
        int layerMask = LayerMask.GetMask("Cards");
        Vector3 origin = Vector3.zero;
        Vector3 direction = Vector3.zero;
        if (Camera.main.orthographic)
        {
            origin = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            direction = Camera.main.transform.forward;
        }
        else
        {
            origin = Camera.main.transform.position;
            direction = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        }
        
        return Physics
            .RaycastAll(origin, direction, layerMask)
            .Any(hit => hit.transform.tag == "Deck");
    }

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
        drawnCard.playerColour = colour;
        deckCards.RemoveAt(0);

        return drawnCard;
    }
}
