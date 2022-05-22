using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Players;

public enum CardType
{
    Unit = 0,
    Effect = 1
}

//this class will probably be used to create game objects later

                    //not sure this needs to be a monobevaviour? We can add it back in no prob if so
public class Card : MonoBehaviour
{
    public event Action<Card> Clicked;
    public event Action<Card> Hovered;
    public event Action<Card> Unhovered;
        
    public CardScriptableObject _cardSO;
    
    public TextMeshProUGUI Title;
    public Image _Image;
    public TextMeshProUGUI Description;
    public Image Background;

    public GameObject PlayButton;
    public GameObject BackButton;

    public Image WhiteUnit;
    public Image WhiteEffect;
    public Image BlackUnit;
    public Image BlackEffect;

    private bool hasCardBeenPlayed = false;
    private bool isHovered = false;

    public bool HasCardBeenPlayed
    {
        get { return hasCardBeenPlayed; }
        ///set { hasCardBeenPlayed = value; }
    }

   // public bool HasCardBeenPlayed { } = false;

    private string title;
    private Sprite image;
    private CardType cardType;
    private string description;
    private PlayerColour playerColour;

    private CanvasGroup cg;
    private Hand ourHand;

    private EventTrigger eventTrigger;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        eventTrigger = GetComponent<EventTrigger>();

        AddPointerEventListener(EventTriggerType.PointerClick, OnPointerClick);
        AddPointerEventListener(EventTriggerType.PointerEnter, OnPointerEnter);
        AddPointerEventListener(EventTriggerType.PointerExit, OnPointerExit);
    }

    private void AddPointerEventListener(EventTriggerType type, Action<PointerEventData> callback)
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(data => callback((PointerEventData) data));
        eventTrigger.triggers.Add(entry);
    }

    private void Start()
    {
        ourHand = GetComponentInParent<Hand>();
    }

    //public Card(CardScriptableObject cardSO)
    //{
    //    PopulateCardFields(cardSO);
    //}

    public void PopulateCardFields(CardScriptableObject cardSO, PlayerColour colour)
    {
        _cardSO = cardSO;
        title = cardSO.title;
        cardType = cardSO.cardType;
        description = cardSO.description;
        playerColour = colour;

        if ( playerColour == PlayerColour.Black )
        {
            image = cardSO.blackImage;
        }
        else
        {
            image = cardSO.whiteImage;
        }

        hasCardBeenPlayed = false;
    }

    public void DisplayCard()
    {
        Title.text = title;
        _Image.sprite = image;
        Description.text = description;

        if( playerColour == PlayerColour.Black && cardType == CardType.Effect )
        {
            Background.sprite = BlackEffect.sprite;
        }
        else if ( playerColour == PlayerColour.Black && cardType == CardType.Unit )
        {
            Background.sprite = BlackUnit.sprite;
        }
        else if ( playerColour == PlayerColour.White && cardType == CardType.Effect )
        {
            Background.sprite = WhiteEffect.sprite;
        }
        else if ( playerColour == PlayerColour.White && cardType == CardType.Unit )
        {
            Background.sprite = WhiteUnit.sprite;
        }

        if (HasCardBeenPlayed)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    public void Hide()
    {
        cg.alpha = 0.0f;
    }

    public void Show()
    {
        cg.alpha = 1.0f;
    }


    public void Play()
    {
        Debug.Log("Card: " + title + ". ACTION!");
        hasCardBeenPlayed = true;
        ourHand.DrawnCard.Hide();
        ourHand.RefreshHandCards();
    }

    //public void ReplaceWithDrawnCard()
    //{
    //    PopulateCardFields(ourHand.DrawnCard._cardSO);
    //    DisplayCard();
    //    ourHand.HideReplaceButtons();
    //    ourHand.DrawnCard.Hide();
    //}

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isHovered)
        {
            return;
        }

        isHovered = true;
        Hovered?.Invoke(this);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isHovered)
        {
            return;
        }

        isHovered = false;
        Unhovered?.Invoke(this);
    }
}
