using System.Collections.Generic;
using UnityEngine;
using Managers;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    public Transform Normal;
    public Transform Apocalypse;
    public Transform NormalWhiteTwistyCard;
    public Transform ApocalypseWhiteTwistyCard;
    public Transform NormalBlackTwistyCard;
    public Transform ApocalypseBlackTwistyCard;
    public float TwistyDegrees = 15;
    public int deckSize;
    public CardScriptableObject[] possibleCards;
    private List<CardScriptableObject> deckCards;

    public void SetNormal()
    {
        Normal.gameObject.SetActive(true);
        Apocalypse.gameObject.SetActive(false);
    }

    public void SetApocalypse()
    {
        Normal.gameObject.SetActive(false);
        Apocalypse.gameObject.SetActive(true);
    }
    
    public void RenderReset()
    {
        NormalWhiteTwistyCard.gameObject.SetActive( false );
        ApocalypseWhiteTwistyCard.gameObject.SetActive( false );
        NormalBlackTwistyCard.gameObject.SetActive( false );
        ApocalypseBlackTwistyCard.gameObject.SetActive( false );

        NormalWhiteTwistyCard.transform.localEulerAngles = new Vector3(90, 0, 0);
        ApocalypseWhiteTwistyCard.transform.localEulerAngles = new Vector3(90, 0, 0);
        NormalBlackTwistyCard.transform.localEulerAngles = new Vector3( 90, 0, 0 );
        ApocalypseBlackTwistyCard.transform.localEulerAngles = new Vector3( 90, 0, 0 );
    }

    public void RenderPlayer()
    {
        RenderReset();

        if ( GameManager.Get().PlayerTurn == Players.PlayerColour.White )
        {
            NormalWhiteTwistyCard.gameObject.SetActive( true );
            ApocalypseWhiteTwistyCard.gameObject.SetActive( true );
        }
        else
        {
            NormalBlackTwistyCard.gameObject.SetActive( true );
            ApocalypseBlackTwistyCard.gameObject.SetActive( true );
        }
    }
    public void RenderHovered()
    {
        NormalWhiteTwistyCard.gameObject.SetActive( false );
        ApocalypseWhiteTwistyCard.gameObject.SetActive( false );
        NormalBlackTwistyCard.gameObject.SetActive( false );
        ApocalypseBlackTwistyCard.gameObject.SetActive( false );

        NormalWhiteTwistyCard.transform.localEulerAngles = new Vector3(90, TwistyDegrees, 0);
        ApocalypseWhiteTwistyCard.transform.localEulerAngles = new Vector3(90, TwistyDegrees, 0);
        NormalBlackTwistyCard.transform.localEulerAngles = new Vector3( 90, TwistyDegrees, 0 );
        ApocalypseBlackTwistyCard.transform.localEulerAngles = new Vector3( 90, TwistyDegrees, 0 );

        if ( GameManager.Get().PlayerTurn == Players.PlayerColour.White )
        {
            NormalWhiteTwistyCard.gameObject.SetActive( true );
            ApocalypseWhiteTwistyCard.gameObject.SetActive( true );
        }
        else
        {
            NormalBlackTwistyCard.gameObject.SetActive( true );
            ApocalypseBlackTwistyCard.gameObject.SetActive( true );
        }
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

        return Physics.Raycast(new Ray(origin, direction), 30, layerMask);
    }

    private void Awake()
    {
        instance = this;
        deckCards = GenerateCards(deckSize);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetNormal();
        RenderReset();
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


    public List<CardScriptableObject> DrawCards(int count)
    {
        List<CardScriptableObject> tempCards = new List<CardScriptableObject>();

        for (int i = 0; i < count; i++)
        {
            CardScriptableObject tempCard = DrawCard();

            if (tempCard == null)
            {
                Debug.LogError("Sorry, not enough cards in the deck");
                return null;
            }

            tempCards.Add(tempCard);
        }

        return tempCards;
    }

    public CardScriptableObject DrawCard()
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
