
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
