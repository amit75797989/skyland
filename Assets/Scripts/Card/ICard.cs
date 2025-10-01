namespace CardMatchGame
{
    public interface ICard
    {
        void Flip();
        void FlipBack();
        void InitCard(CardData cardData);
        void ShowMatched();
        void ShowMisMatched();
    }
}

