using Players;
using UnityEngine;

public class Hand : MonoBehaviour
{
    //public int maxHandCards = 3;

    public PlayerColour PlayerColour;
    public Card[] playerHand;

  
    public Card DrawnCard;

    public Card[] PlayerHand
    {
        get { return playerHand; }
    }


    // Start is called before the first frame update
    void Start()
    {
        //playerHand = new Card[maxHandCards];
        DrawnCard.PlayButton.SetActive(false);
        DrawnCard.BackButton.SetActive(false);
        DrawnCard.Hide();

        RefreshHandCards();
    }

    public void InitHand()
    {
        HideBackButtons();
        PopulateHand();
    }

    public void ShowBackButtons()
    {
        for (int i = 0; i < playerHand.Length; i++)
        {
            playerHand[i].BackButton.SetActive(true);
        }
    }

    public void HideBackButtons()
    {
        for (int i = 0; i < playerHand.Length; i++)
        {
            playerHand[i].BackButton.SetActive(false);
        }
    }

    /// <summary>
    /// Returns whether the new turn can start
    /// </summary>
    /// <returns></returns>
    public void NewTurn(PlayerColour colour)
    {
        bool fullHand = true;
        for (int i = 0; i < playerHand.Length; i++)
        {
            if (playerHand[i].HasCardBeenPlayed)
            {
                playerHand[i].PopulateCardFields(Deck.instance.DrawCard(), PlayerColour);
                fullHand = false;
            }
        }

        //bool canTurnStart = !fullHand;

        // if (fullHand)
        // {
        //     DrawnCard.PopulateCardFields(Deck.instance.DrawCard(colour));
        //     DrawnCard.Show();
        //     ShowBackButtons();
        // }

        RefreshHandCards();
        // return canTurnStart;
    }

    private void PopulateHand()
    {
        for (int i = 0; i < playerHand.Length; i++)
        {
            playerHand[i].PopulateCardFields(Deck.instance.DrawCard(), PlayerColour);
        }
    }

    public void RefreshHandCards()
    {
        for (int i = 0; i < playerHand.Length; i++)
        {
            playerHand[i].DisplayCard();
        }

        DrawnCard.DisplayCard();
        DrawnCard.Hide();
    }

    public Card PlayCard(int handIndex)
    {
        Card temp = RemoveCardAtIndex(handIndex);

        temp.Play();

        return temp;
    }

    public void AddCardAt(Card card, int handIndex)
    {
        playerHand[handIndex] = card;
    }


    public Card RemoveCardAtIndex(int handIndex)
    {
        Card temp = playerHand[handIndex];

        if (handIndex >= playerHand.Length || temp == null)
        {
            Debug.LogError("Hand index out of range or card slot null");
            return null;
        }

        playerHand[handIndex] = null;

        return temp;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
