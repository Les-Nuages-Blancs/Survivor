using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using System;


public class DeckManager : NetworkBehaviour
{
    [SerializeField] private GameObject cardPrefab; 
    [SerializeField] private Transform cardContainer;
    [SerializeField] private List<Sprite> cardImages;
    [SerializeField] private int nbCards = 5 ;

    private List<Card> deck = new List<Card>();
    private int pendingLevelUps = 0;
    private bool isDrawingCards = false;

    void Start()
    {
        InitDeck();
        DrawCards();
        foreach (Player player in Player.playerList)
        {
            OnClientConnected(player.OwnerClientId);
        }
        Player.onPlayerAdded += OnClientConnected;
    }

    public override void OnDestroy()
    {
        Player.onPlayerAdded -= OnClientConnected;
    }

    private void OnClientConnected(ulong newClientId)
    {
        ulong ownerClientId = NetworkManager.Singleton.LocalClientId; // Get the owner ID

        if (newClientId == ownerClientId)
        {
            Player player = Player.GetPlayerByClientId(ownerClientId);

            GameObject playerGo = player.gameObject;

            StatistiquesLevelSystem statsLevelSystem = playerGo.GetComponent<StatistiquesLevelSystem>();

            statsLevelSystem.onCurrentLevelChange.AddListener(DrawCards);
        }
    }

    void InitDeck()
    {
        deck.Clear();

        string[] names = Enum.GetNames(typeof(Card.CardType));
        Array typeArray = Enum.GetValues(typeof(Card.CardType));
        for(int i=0; i< nbCards; i++){
            deck.AddRange(Enumerable.Repeat(new Card(), nbCards).Select(card =>
            {
                card.SetTitleAndImage(names[i], cardImages[i]);
                card.SetCardType( (Card.CardType) typeArray.GetValue(i));

                return card;
            }));
        }


        deck = deck.OrderBy(c => UnityEngine.Random.value).ToList(); 
    }

    void DrawCards()
    {
        pendingLevelUps++; // Ajoute un niveau en attente

        if (isDrawingCards) return; // Si une sélection est déjà en cours, ne rien faire

        StartCoroutine(HandleDrawCards()); // Lance la gestion des tirages
    }

    IEnumerator HandleDrawCards()
    {
        isDrawingCards = true;

        while (pendingLevelUps > 0)
        {
            pendingLevelUps--; // Consomme un niveau en attente

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

            // Attendre que le joueur choisisse une carte avant de continuer
            while (cardContainer.childCount > 0)
            {
                yield return null; // Attend le prochain frame tant qu'il y a des cartes affichées
            }
        }

        isDrawingCards = false;
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

        // Si d'autres niveaux sont en attente, continuer à afficher des cartes
        if (pendingLevelUps > 0)
        {
            StartCoroutine(HandleDrawCards());
        }
        else
        {
            isDrawingCards = false; // Sinon, libérer le flag
        }

        DebugDeck("Deck après DrawCards");
    }


    void DebugDeck(string message)
    {
        string deckContents = string.Join(", ", deck.Select(card => $"{card.title} ({card.type})"));
        //Debug.Log($"{message}: {deckContents}");
        Debug.Log($"Taille actuelle du deck: {deck.Count}");
    }
}
