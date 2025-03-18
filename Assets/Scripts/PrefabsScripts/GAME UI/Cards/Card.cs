using UnityEngine;

[System.Serializable]
public class Card
{
    public enum CardType { Armor, Damage, Health }

    public string title;
    public Sprite image;
    public CardType type;

    public void SetTitleAndImage(string title, Sprite image)
    {
        this.title = title;
        this.image = image;
    }

    public void SetCardType(CardType type)
    {
        this.type = type;
    }
}
