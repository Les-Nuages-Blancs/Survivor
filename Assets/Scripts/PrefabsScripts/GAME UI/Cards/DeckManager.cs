using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab; 
    [SerializeField] private Transform cardContainer;
    [SerializeField] private List<Sprite> cardImages;

    private List<Card> deck = new List<Card>();

    void Start()
    {
        InitDeck();
        DrawCards();
    }

    void InitDeck()
    {
        deck.Clear();

        deck.AddRange(Enumerable.Repeat(new Card(), 5).Select(card =>
        {
            card.SetTitleAndImage("Armor", cardImages[0]);
            card.SetCardType(Card.CardType.Armor);

            return card;
        }));

        deck.AddRange(Enumerable.Repeat(new Card(), 5).Select(card =>
        {
            card.SetTitleAndImage("Damage", cardImages[1]);
            card.SetCardType(Card.CardType.Damage);

            return card;
        }));

        deck.AddRange(Enumerable.Repeat(new Card(), 5).Select(card =>
        {
            card.SetTitleAndImage("Health", cardImages[2]);
            card.SetCardType(Card.CardType.Health);

            return card;
        }));


        deck = deck.OrderBy(c => Random.value).ToList(); 
    }

    void DrawCards()
    {
        for (int i = 0; i < 3 && deck.Count > 0; i++)
        {
            CreateCardUI(deck[i]);
        }
    }

    void CreateCardUI(Card card)
    {
        GameObject newCard = Instantiate(cardPrefab, cardContainer);
        CardUI cardUI = newCard.GetComponent<CardUI>();
        cardUI.Setup(card, this);
    }

    public void RemoveCard(Card card)
    {
        deck.Remove(card);
        DrawCards(); 
    }
}
