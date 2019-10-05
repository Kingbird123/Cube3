using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCode : MonoBehaviour 
{

    [SerializeField] private Text timeText = null;

    private float timer;

	// Update is called once per frame
	void Update () 
	{
        CountTime();
	}

    void CountTime()
    {
        timer += Time.deltaTime;
        timer = Mathf.Round(timer * 100) / 100;
        timeText.text = timer.ToString();
    }
}
