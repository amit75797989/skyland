/*
 * ---------------------------------------------------------------
 * Author      : Amit Pandey
 * Email       : ap3400568@gmail.com
 * Role        : Unity Developer
 * 
 * Summary     : This script handles Menu Scene for showing Current Level and play game
 *               
 *
 * ---------------------------------------------------------------
 */



using CardMatchGame;
using CardMatchGame.Storage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]private Button playBtnPrefab;
    [SerializeField] private Transform content; 


    private void Start()
    {
        GameStatusData gameStatusData= PersistentStorage.Instance.Load< GameStatusData>(StorageEnum.GameStatus.ToString());
        if(gameStatusData==null) gameStatusData=new GameStatusData();

        LevelData[] levelDatas = Resources.Load<GameLevelConfig>("GameLevelConfig").GetGridSizeData();  
        for(int i = 0; i< levelDatas.Length; i++)
        {
            Button btn = Instantiate(playBtnPrefab, content);
            btn.GetComponentInChildren<Text>().text = levelDatas[i].Lebal;
            btn.transform.localScale = levelDatas[i].Id == gameStatusData.LevelId ? Vector3.one * 1.2f : Vector3.one;
            if (levelDatas[i].Id <= gameStatusData.LevelId)
            {
                int LevelId = levelDatas[i].Id;
                btn.onClick.AddListener(() => PlayGame(LevelId));
                
            }
            else
            {
                btn.GetComponent<Image>().color = Color.black;
            }
        }
        
    }    

    public void OnClickPlay()
    {
        SceneManager.LoadSceneAsync(SceneEnum.Gameplay.ToString());
    }

    public void PlayGame(int levelId)
    {
        GameStatusData gameStatusData = new GameStatusData(levelId);
        PersistentStorage.Instance.Save(StorageEnum.GameStatus.ToString(), gameStatusData);
        SceneManager.LoadSceneAsync(SceneEnum.Gameplay.ToString());
    }
}
