/*
 * ---------------------------------------------------------------
 * Author      : Amit Pandey
 * Email       : ap3400568@gmail.com
 * Role        : Unity Developer
 * 
 * Summary     : This script handles Gameover Scene to next play or redirect to menu scene
 *               
 *
 * ---------------------------------------------------------------
 */


using CardMatchGame.Storage;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public void PlayNext()
    {
        SceneManager.LoadSceneAsync(SceneEnum.Gameplay.ToString());
    }
    public void Replay()
    {
       
    }
    public void Menu()
    {
        SceneManager.LoadSceneAsync(SceneEnum.Menu.ToString());
    }
}
