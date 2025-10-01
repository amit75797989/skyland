

using CardMatchGame;
using System.Collections;
using UnityEngine;

public abstract class ACard : MonoBehaviour,ICard
{
    protected CardData cardData;
    protected bool isFlipped = false;
    protected bool isMatched = false;

    public bool IsMatched => isMatched;

    public virtual void InitCard(CardData data)
    {
        this.cardData = data;
    }

    public virtual void Flip()
    {
        if (isMatched) return;

        isFlipped = !isFlipped;
        StartCoroutine(PlayFlipAnimation(isFlipped));
    }

    protected abstract IEnumerator PlayFlipAnimation(bool showFront);

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
