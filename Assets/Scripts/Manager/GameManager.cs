using CardMatchGame;
using CardMatchGame.Handler;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]private GridSystem gridSystem;
    [SerializeField] private ACard cardPrefab;


    [Header("Grid Settings")]
    [SerializeField] private float spacing = 2;
    private int mRows;
    private int mColumns;
    
    private List<ACard> mCards = new List<ACard>();
    private CardConfig mCardConfig;
    
    private void Start()
    {
        mRows = 4;
        mColumns = 4;
        gridSystem.InitGridLayout(mRows, mColumns, spacing, cardPrefab.gameObject);
        gridSystem.GetCards(ref mCards);        
        int totalCard=mCards.Count;

        mCardConfig = Resources.Load<CardConfig>("CardConfig");
        List<CardData> cardData = mCardConfig.GetCardList();
        int cardIconLenght = cardData.Count >= (totalCard / 2) ? (totalCard / 2) : (cardData.Count);
        for (int i=0; i<(totalCard); i++)
        {
            int selectedCard = Random.Range(0, mCards.Count);
            mCards[selectedCard].InitCard(cardData[(i% cardIconLenght)]);
            mCards.RemoveAt(selectedCard);
        }
    }
}
