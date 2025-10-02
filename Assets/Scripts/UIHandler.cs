/*
 * ---------------------------------------------------------------
 * Author      : Amit Pandey
 * Email       : ap3400568@gmail.com
 * Role        : Unity Developer
 * 
 * Summary     : This script handles Gameplay Scene for showing playing turn and total found matches
 *               
 *
 * ---------------------------------------------------------------
 */



using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Text TurnValueTxt;
    [SerializeField] private Text MatchValueTxt;

    public void SetTurnValue(int value)
    {
        TurnValueTxt.text = value.ToString();
    }

    public void SetMatchValue(int value)
    {
        MatchValueTxt.text = value.ToString();
    }
}
