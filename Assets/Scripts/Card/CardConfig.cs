using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardMatchGame
{
    [CreateAssetMenu(fileName = "NewCardConfig", menuName = "CardMatchGame/NewCardConfig")]
    public class CardConfig : ScriptableObject
    {
        [SerializeField] private CardData[] cardConfig;

        public CardData GetCard(int index)
        {
            if (index < cardConfig.Length)
            {
                return cardConfig[index];
            }            
            return null;
        }
        public List<CardData> GetCardList() 
        {  
            return cardConfig.ToList(); 
        }
        
    }

    [Serializable]
    public class CardData
    {
        [SerializeField] private int cardId;
        [SerializeField] private Sprite frontIcon;
        [SerializeField] private AudioClip matchSFX;
        [SerializeField] private AudioClip misMatchSFX;
        [SerializeField] private AudioClip flipSFX;

        public int CardId
        {
            get 
            { 
                return cardId; 
            }
        }

        public Sprite CardFrontIcon
        {
            get 
            { 
                return frontIcon;
            }
        }
        public AudioClip MatchSFX
        {
            get
            {
                return matchSFX;
            }
        }

        public AudioClip MisMatchSFX
        {
            get
            {
                return misMatchSFX;
            }
        }
        public AudioClip FlipSFC
        {
            get
            {
                return flipSFX;
            }
        }
    }
}

