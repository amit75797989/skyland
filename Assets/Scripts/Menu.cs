using CardMatchGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]private Dropdown menuDropdown;

    private GameLevelConfig mGameLevelConfig;

    private void Start()
    {
        
        mGameLevelConfig = Resources.Load<GameLevelConfig>("GameLevelConfig");
        menuDropdown.ClearOptions();

        for(int i = 0; i < mGameLevelConfig.GetGridSizeData().Length; i++)
        {
            menuDropdown.options.Add(new Dropdown.OptionData(mGameLevelConfig.GetGridSizeData()[i].Lebal));
            if (PlayerPrefs.GetInt("level", 0) == mGameLevelConfig.GetGridSizeData()[i].Id)
            {
                menuDropdown.value = i;
            }
        }

       
        menuDropdown.RefreshShownValue();
        menuDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int index)
    {        ;
        PlayerPrefs.SetInt("level", mGameLevelConfig.GetGridSizeData()[index].Id);
    }

    public void OnClickPlay()
    {
        SceneManager.LoadSceneAsync("Gameplay");
    }
}
