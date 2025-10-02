/*
 * ---------------------------------------------------------------
 * Author      : Amit Pandey
 * Email       : ap3400568@gmail.com
 * Role        : Unity Developer
 * 
 * Summary     : This script handles all game play scene like load card and initialize card details, check gameover and save and load game status
 *               
 *
 * ---------------------------------------------------------------
 */


using CardMatchGame;
using CardMatchGame.Handler;
using CardMatchGame.Storage;
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
    
    private List<ACard> mCards = new List<ACard>();
    private CardConfig mCardConfig;

    private ACard m1stSelectedCard;
    private ACard m2ndSelectedCard;
    
    
    private int mTotalCards;

    private GameLevelConfig mGridSizeConfig;
    private GameStatusData mGameStatusData;

    private void Start()
    {
        mGameStatusData = PersistentStorage.Instance.Load<GameStatusData>(StorageEnum.GameStatus.ToString());
        if (mGameStatusData == null || (mGameStatusData.TotalMatched == mGameStatusData.CardSetupIndexes.Count / 2))
        {
            mGameStatusData = new GameStatusData(mGameStatusData!=null?mGameStatusData.LevelId:0);           
        }
        

        mGridSizeConfig = Resources.Load<GameLevelConfig>("GameLevelConfig"); 

        mCardConfig = Resources.Load<CardConfig>("CardConfig");
        List<CardData> cardData = mCardConfig.GetCardList();        

        uIHandler.SetMatchValue(mGameStatusData.TotalMatched);
        uIHandler.SetTurnValue(mGameStatusData.TotalTurns);       

        LevelData levalData= mGridSizeConfig.GetGridSizeData(mGameStatusData.LevelId);        

        gridSystem.MaxScale = 1.2f;
        gridSystem.InitGridLayout(levalData.Row, levalData.Coloum, spacing, cardPrefab.gameObject);
        gridSystem.GetCards(ref mCards);
        mTotalCards = mCards.Count;

        

        int pairCount = mTotalCards / 2;
        int iconCount = Mathf.Min(cardData.Count, pairCount);
        
        List<CardData> sequentialCardData = new List<CardData>();
        for (int i = 0; i < iconCount; i++)
        {
            sequentialCardData.Add(cardData[i]);
            sequentialCardData.Add(cardData[i]);
        }

        if (mGameStatusData.TotalMatched>0 )
        {
            for (int i = 0; i < (mTotalCards); i++)
            {                
                ACard card = mCards[mGameStatusData.CardSetupIndexes[i]];
                if (mGameStatusData.MatchedIndexes.Contains(mGameStatusData.CardSetupIndexes[i]))
                {
                    card.gameObject.SetActive(false);
                }
                else
                {
                    card.InitCard(sequentialCardData[i % sequentialCardData.Count]);
                    AddCardClickCallback(card);
                }               
               
            }
            mCards.Clear();            
        }
        else
        {
            mGameStatusData.CardSetupIndexes.Clear();
            for (int i = 0; i < (mTotalCards); i++)
            {
                int selectedCard = Random.Range(0, mCards.Count);
                ACard card = mCards[selectedCard];

                mGameStatusData.CardSetupIndexes.Add(card.cardIndex);

                card.InitCard(sequentialCardData[i% sequentialCardData.Count]);
                mCards.RemoveAt(selectedCard);

                AddCardClickCallback(card);
            }
        }
        PersistentStorage.Instance.Save(StorageEnum.GameStatus.ToString(), mGameStatusData);
    }

    private void AddCardClickCallback(ACard card)
    {
        card.GetComponent<Button>().onClick.AddListener(() => {
            if (!card.IsFlipped && (m1stSelectedCard == null || m2ndSelectedCard == null) && m1stSelectedCard != card && m2ndSelectedCard != card)
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



    public void CheckMatch()
    {
        if (m2ndSelectedCard.CardData.CardId == m1stSelectedCard.CardData.CardId)
        {
            AudioHandler.Instance.PlaySFX(m2ndSelectedCard.CardData.MatchSFX);
            m2ndSelectedCard.ShowMatched();
            m1stSelectedCard.ShowMatched();
            mGameStatusData.TotalMatched++;
            mGameStatusData.MatchedIndexes.Add(m1stSelectedCard.cardIndex);
            mGameStatusData.MatchedIndexes.Add(m2ndSelectedCard.cardIndex);
        }
        else
        {
            AudioHandler.Instance.PlaySFX(m2ndSelectedCard.CardData.MisMatchSFX);
            m2ndSelectedCard.ShowMisMatched();
            m1stSelectedCard.ShowMisMatched();
        }
        mGameStatusData.TotalTurns++;
        m2ndSelectedCard = null;
        m1stSelectedCard = null;
        uIHandler.SetMatchValue(mGameStatusData.TotalMatched);
        uIHandler.SetTurnValue(mGameStatusData.TotalTurns);
        
        if(mGameStatusData.TotalMatched == mTotalCards / 2)
        {           
            mGameStatusData = new GameStatusData(mGridSizeConfig.GetGridSizeData(mGameStatusData.LevelId+1)!=null? mGameStatusData.LevelId+1: mGameStatusData.LevelId);           
            Invoke("ShowGameOver", 1);
        }
        PersistentStorage.Instance.Save(StorageEnum.GameStatus.ToString(), mGameStatusData);
        PersistentStorage.Instance.Save(StorageEnum.Level.ToString(), mGameStatusData.LevelId);
    }

    public void ShowGameOver()
    {
        Gameover.SetActive(true);
    }
}
