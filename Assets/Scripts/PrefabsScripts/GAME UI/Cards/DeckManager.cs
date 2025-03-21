using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab; 
    [SerializeField] private Transform cardContainer;
    [SerializeField] private List<Sprite> cardImages;
    [SerializeField] private int nbCards = 5 ;
    private StatistiquesLevelSystem statLevelSystem;


    private List<Card> deck = new List<Card>();

    void Start()
    {
        InitDeck();
        DrawCards();
        //statLevelSystem = FindObjectOfType<StatistiquesLevelSystem>();
        //if (statLevelSystem != null)
        //{
        //    statLevelSystem.onLevelUp.AddListener(DrawCards);
        //}
    }

    void InitDeck()
    {
        deck.Clear();

        deck.AddRange(Enumerable.Repeat(new Card(), nbCards).Select(card =>
        {
            card.SetTitleAndImage("Armor", cardImages[0]);
            card.SetCardType(Card.CardType.Armor);

            return card;
        }));

        deck.AddRange(Enumerable.Repeat(new Card(), nbCards).Select(card =>
        {
            card.SetTitleAndImage("Damage", cardImages[1]);
            card.SetCardType(Card.CardType.Damage);

            return card;
        }));

        deck.AddRange(Enumerable.Repeat(new Card(), nbCards).Select(card =>
        {
            card.SetTitleAndImage("Health", cardImages[2]);
            card.SetCardType(Card.CardType.Health);

            return card;
        }));

        deck.AddRange(Enumerable.Repeat(new Card(), nbCards).Select(card =>
        {
            card.SetTitleAndImage("HealthRegen", cardImages[3]);
            card.SetCardType(Card.CardType.HealthRegen);

            return card;
        }));

        deck.AddRange(Enumerable.Repeat(new Card(), nbCards).Select(card =>
        {
            card.SetTitleAndImage("AttackSpeed", cardImages[4]);
            card.SetCardType(Card.CardType.AttackSpeed);

            return card;
        }));


        deck = deck.OrderBy(c => Random.value).ToList(); 
    }

    void DrawCards()
    {
        List<Card.CardType> displayedCardTypes = new List<Card.CardType>();

        for (int i = 0; i < 3 && deck.Count > 0; i++)
        {
            Card cardToDraw = null;

            foreach (Card card in deck)
            {
                if (!displayedCardTypes.Contains(card.type))
                {
                    cardToDraw = card;
                    break; 
                }
            }

            if (cardToDraw != null)
            {
                displayedCardTypes.Add(cardToDraw.type);

                CreateCardUI(cardToDraw);
            }
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

    public void RemoveAndRefresh(Card clickedCard)
    {
        deck.Remove(clickedCard);

        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        DrawCards();

        DebugDeck("Deck après DrawCards");

    }

    void DebugDeck(string message)
    {
        string deckContents = string.Join(", ", deck.Select(card => $"{card.title} ({card.type})"));
        //Debug.Log($"{message}: {deckContents}");
        Debug.Log($"Taille actuelle du deck: {deck.Count}");
    }
}
