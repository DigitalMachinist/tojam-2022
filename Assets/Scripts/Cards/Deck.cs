using System;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Players;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    public event Action Hovered; 
    public event Action Unhovered;
    
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

    public bool IsHovering { get; private set; }
    
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
        IsHovering = false;
        
        NormalWhiteTwistyCard.gameObject.SetActive( false );
        ApocalypseWhiteTwistyCard.gameObject.SetActive( false );
        NormalBlackTwistyCard.gameObject.SetActive( false );
        ApocalypseBlackTwistyCard.gameObject.SetActive( false );

        NormalWhiteTwistyCard.transform.localEulerAngles = new Vector3(90, 0, 0);
        ApocalypseWhiteTwistyCard.transform.localEulerAngles = new Vector3(90, 0, 0);
        NormalBlackTwistyCard.transform.localEulerAngles = new Vector3( 90, 0, 0 );
        ApocalypseBlackTwistyCard.transform.localEulerAngles = new Vector3( 90, 0, 0 );
    }
    
    public void AnimateDeckNonTwisty(float duration)
    {
        if (GameManager.Get().PlayerTurn == PlayerColour.Black)
        {
            NormalBlackTwistyCard.gameObject.SetActive(true);
            LeanTween.cancel(NormalBlackTwistyCard.gameObject);
            LeanTween
                .rotateY(NormalBlackTwistyCard.gameObject, 0f, duration)
                .setEaseOutCubic();

            ApocalypseBlackTwistyCard.gameObject.SetActive(true);
            LeanTween.cancel(ApocalypseBlackTwistyCard.gameObject);
            LeanTween
                .rotateY(ApocalypseBlackTwistyCard.gameObject, 0f, duration)
                .setEaseOutCubic();
        }
        else
        {
            NormalWhiteTwistyCard.gameObject.SetActive(true);
            LeanTween.cancel(NormalWhiteTwistyCard.gameObject);
            LeanTween
                .rotateY(NormalWhiteTwistyCard.gameObject, 0f, duration)
                .setEaseOutCubic();

            ApocalypseWhiteTwistyCard.gameObject.SetActive(true);
            LeanTween.cancel(ApocalypseWhiteTwistyCard.gameObject);
            LeanTween
                .rotateY(ApocalypseWhiteTwistyCard.gameObject, 0f, duration)
                .setEaseOutCubic();
        }
    }

    public void AnimateDeckTwisty(float duration)
    {
        if (GameManager.Get().PlayerTurn == PlayerColour.Black)
        {
            NormalBlackTwistyCard.gameObject.SetActive(true);
            LeanTween
                .rotateY(NormalBlackTwistyCard.gameObject, TwistyDegrees, duration)
                .setEaseOutCubic();
            
            ApocalypseBlackTwistyCard.gameObject.SetActive(true);
            LeanTween
                .rotateY(ApocalypseBlackTwistyCard.gameObject, TwistyDegrees, duration)
                .setEaseOutCubic();
        }
        else
        {
            NormalWhiteTwistyCard.gameObject.SetActive(true);
            LeanTween
                .rotateY(NormalWhiteTwistyCard.gameObject, TwistyDegrees, duration)
                .setEaseOutCubic();
            
            ApocalypseWhiteTwistyCard.gameObject.SetActive(true);
            LeanTween
                .rotateY(ApocalypseWhiteTwistyCard.gameObject, TwistyDegrees, duration)
                .setEaseOutCubic();
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
        var isHovering = IsHoveringDeck();
        if (!IsHovering && isHovering)
        {
            IsHovering = true;
            Hovered?.Invoke();
        }
        else if (IsHovering && !isHovering)
        {
            IsHovering = false;
            Unhovered?.Invoke();
        }
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
