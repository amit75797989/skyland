using CardMatchGame;
using CardMatchGame.Handler;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]private GameObject Gameover;
    [SerializeField] private UIHandler uIHandler;
    [SerializeField]private GridSystem gridSystem;
    [SerializeField] private ACard cardPrefab;

    [Header("Grid Settings")]
    [SerializeField] private float spacing = 2;
    private int mRows;
    private int mColumns;
    
    private List<ACard> mCards = new List<ACard>();
    private CardConfig mCardConfig;

    private ACard m1stSelectedCard;
    private ACard m2ndSelectedCard;
    private int mTotalTurn;
    private int mTotalMatch;
    private int mTotalCards;

    private GameLevelConfig mGridSizeConfig;

    private void Start()
    {
        mGridSizeConfig= Resources.Load<GameLevelConfig>("GameLevelConfig");
        uIHandler.SetMatchValue(mTotalMatch);
        uIHandler.SetTurnValue(mTotalTurn);
        mRows = mGridSizeConfig.GetGridSizeData(PlayerPrefs.GetInt("level")).Row;
        mColumns = mGridSizeConfig.GetGridSizeData(PlayerPrefs.GetInt("level")).Coloum;
        gridSystem.MaxScale = 1.2f;
        gridSystem.InitGridLayout(mRows, mColumns, spacing, cardPrefab.gameObject);
        gridSystem.GetCards(ref mCards);        
        mTotalCards=mCards.Count;

        mCardConfig = Resources.Load<CardConfig>("CardConfig");
        List<CardData> cardData = mCardConfig.GetCardList();
        int cardIconLenght = cardData.Count >= (mTotalCards / 2) ? (mTotalCards / 2) : (cardData.Count);
        for (int i=0; i<(mTotalCards); i++)
        {
            int selectedCard = Random.Range(0, mCards.Count);
            ACard card = mCards[selectedCard];

            card.InitCard(cardData[(i% cardIconLenght)]);
            mCards.RemoveAt(selectedCard);
            card.GetComponent<Button>().onClick.AddListener(() => {
                if(!card.IsFlipped && ( m1stSelectedCard == null || m2ndSelectedCard == null) && m1stSelectedCard!= card && m2ndSelectedCard != card)
                {
                    AudioHandler.Instance.PlaySFX(card.CardData.FlipSFC);
                    card.Flip();
                    if (m1stSelectedCard != null && card != m1stSelectedCard)
                    {
                        m2ndSelectedCard = card;
                        Invoke("CheckMatch", card.FlipTime);
                    }
                    else
                    {
                        m1stSelectedCard = card;
                    }
                }                 
            });
        }
    }



    public void CheckMatch()
    {
        if (m2ndSelectedCard.CardId == m1stSelectedCard.CardId)
        {
            AudioHandler.Instance.PlaySFX(m2ndSelectedCard.CardData.MatchSFX);
            m2ndSelectedCard.ShowMatched();
            m1stSelectedCard.ShowMatched();
            mTotalMatch++;
        }
        else
        {
            AudioHandler.Instance.PlaySFX(m2ndSelectedCard.CardData.MisMatchSFX);
            m2ndSelectedCard.ShowMisMatched();
            m1stSelectedCard.ShowMisMatched();
        }
        mTotalTurn++;
        m2ndSelectedCard = null;
        m1stSelectedCard = null;
        uIHandler.SetMatchValue(mTotalMatch);
        uIHandler.SetTurnValue(mTotalTurn);

        if(mTotalMatch== mTotalCards / 2)
        {
            Invoke("ShowGameOver", 1);
        }
    }

    public void ShowGameOver()
    {
        Gameover.SetActive(true);
    }
}
