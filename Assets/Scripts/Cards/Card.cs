using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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


    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        ourHand = GetComponentInParent<Hand>();
    }


    //public Card(CardScriptableObject cardSO)
    //{
    //    PopulateCardFields(cardSO);
    //}

    public void PopulateCardFields(CardScriptableObject cardSO)
    {
        _cardSO = cardSO;
        title = cardSO.title;
        image = cardSO.image;
        cardType = cardSO.cardType;
        description = cardSO.description;
        playerColour = cardSO.playerColour;

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

    //// Start is called before the first frame update
    //void Start()
    //{
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
