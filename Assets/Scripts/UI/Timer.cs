using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System; 

public class Timer : MonoBehaviour
{
    //The actual content of the string.
    private float curTime;
    private TimeSpan time; 
    private TextMeshProUGUI textMesh;

    //Bool for starting/stopping timer. 
    private bool isTimerActive = false;

    private void Start()
    {
        StartTimer(); 
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();

    }

    private void Update()
    {
        if (isTimerActive)
        {
            curTime += Time.deltaTime; 
        }
        time = TimeSpan.FromSeconds(curTime);
        textMesh.text = time.ToString(@"mm\:ss\:fff");

    }

    public void StartTimer()
    {
        isTimerActive = true; 
    }

    public void StopTimer()
    {
        isTimerActive = false;
    }

    public void ResetTimer()
    {
        curTime = 0; 
    }

    public int GetTimerData()
    {
        return Mathf.RoundToInt((float)time.TotalSeconds);
    }
}
