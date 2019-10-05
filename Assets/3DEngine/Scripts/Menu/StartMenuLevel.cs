using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuLevel : MonoBehaviour 
{
   
    [Range(0, 1)]
    [SerializeField] private float fadeAlphaAmount = 0.5f;
    [SerializeField] private int levelNumber = 0;
    [SerializeField] private string levelToPlay = "Level";

    [SerializeField] private Image image = null;
    [SerializeField] private Button button = null;

    private SceneTransitionData lm;

    void Start()
    {
        //get components
        lm = GameManager.instance.GetSceneTransitionData();
    }

    public void SetLevelPlayable(bool _playable)
    {
        if (_playable)
        {
            SetImageAlpha(1);
            button.interactable = true;
        }
        else
        {
            SetImageAlpha(fadeAlphaAmount);
            button.interactable = false;
        }
    }

    void SetImageAlpha(float _amount)
    {
        Color imageColor = image.color;
        imageColor.a = _amount;
        image.color = imageColor;
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public void LoadLevel()
    {
        lm.LoadLevelWithLoadingScreen(levelToPlay);
    }
	

}
