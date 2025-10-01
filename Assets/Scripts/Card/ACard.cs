using UnityEngine;

namespace CardMatchGame
{
    public abstract class ACard : MonoBehaviour, ICard
    {
        [SerializeField] protected float flipTime;
        protected CardData cardData;
        protected bool isFlipped = false;
        protected bool isMatched = false;

        public bool IsMatched => isMatched;
        public bool IsFlipped => isFlipped;

        public float FlipTime
        {
            get
            {
                return flipTime;
            }
        }
        public int CardId
        {
            get
            {
                return cardData.CardId;
            }
        }

        public CardData CardData
        {
            get 
            { 
                return cardData; 
            }
        }

        public virtual void InitCard(CardData data)
        {
            this.cardData = data;
        }

        public virtual void Flip()
        {
            if (isMatched) return;

            isFlipped = !isFlipped;            
        }        

        public virtual void FlipBack()
        {

        }

        public virtual void ShowMatched()
        {

        }

        public virtual void ShowMisMatched()
        {

        }
    }
}

