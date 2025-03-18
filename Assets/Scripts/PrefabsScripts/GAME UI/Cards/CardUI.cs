using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text cardTitle;
    [SerializeField] private Button cardButton;

    private Card card;
    private DeckManager deckManager;

    public void Setup(Card card, DeckManager deckManager)
    {
        this.card = card;
        this.deckManager = deckManager;

        cardTitle.text = card.title;
        cardImage.sprite = card.image;

        cardButton.onClick.AddListener(OnCardClicked);

    }

    public void OnCardClicked()
    {
        Debug.Log($"Carte cliquée : {card.title} ({card.type})");
        deckManager.RemoveCard(card);
        Destroy(gameObject);
    }
}
