/*
 * ---------------------------------------------------------------
 * Author      : Amit Pandey
 * Email       : ap3400568@gmail.com
 * Role        : Unity Developer
 * 
 * Summary     : This script is scriptable object that is used to setup level data of gameplay. Here we can add and remove game level
 *               
 *
 * ---------------------------------------------------------------
 */


using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMatchGame
{
    [CreateAssetMenu(fileName = "GameLevelConfig", menuName = "CardMatchGame/New GameLevelConfig")]
    public class GameLevelConfig : ScriptableObject
    {
        [SerializeField]private LevelData[] gameLevelDataList;

        private Dictionary<int, LevelData> gameLevelDataDic;

        private void OnEnable()
        {
            gameLevelDataDic=new Dictionary<int, LevelData>();
            for(int i = 0; gameLevelDataList!=null && i < gameLevelDataList.Length; i++)
            {
                gameLevelDataDic.Add(gameLevelDataList[i].Id, gameLevelDataList[i]);
            }
        }

        public LevelData[] GetGridSizeData()
        {
            return gameLevelDataList;
        }

        public LevelData GetGridSizeData(int id)
        {
            if (gameLevelDataDic.ContainsKey(id))
            {
                return gameLevelDataDic[id];
            }
            return null;
        }
    }

    [Serializable]
    public class LevelData
    {
        [SerializeField] private int id;
        [SerializeField] private int row;
        [SerializeField] private int coloum;
        [SerializeField] private string lebal;

        public int Id
        {
            get 
            { 
                return id; 
            } 
        }
        public int Row
        {
            get 
            { 
                return row; 
            }
        }
        public int Coloum
        {
            get
            {
                return coloum;
            }
        }

        public string Lebal
        {
            get
            {
                return lebal;
            }
        }
    }
}

