
using System;
using System.Collections.Generic;

namespace CardMatchGame.Storage
{
    [Serializable]
    public class GameStatusData
    {
        public int LevelId;
        public int TotalTurns;
        public int TotalMatched;
        public List<int> MatchedIndexes;
        public List<int> CardSetupIndexes;

        public GameStatusData() 
        {
            LevelId = 0;
            
        }
        public GameStatusData(int levelid) 
        { 
            this.LevelId = levelid;
            TotalTurns = 0;
            TotalMatched = 0;
            MatchedIndexes = new List<int>();
            CardSetupIndexes = new List<int>();
        }
    }
}

