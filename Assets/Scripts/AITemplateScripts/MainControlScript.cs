using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainControlScript : MonoBehaviour
{
    [SerializeField] SaveLoad saveLoadScript;
    [SerializeField] PixelGrid pixelGridScript;
    [SerializeField] AIScript AI;
    [SerializeField] GameObject resultView;

    private void Start()
    {
        List<int> ints = new List<int> { 10000, 500, 1 };
        AI.InitialiseAI(saveLoadScript.InitialiseFreshNetwork(ints));
    }

    public void LoadNetwork()
    {
        saveLoadScript.LoadNetwork();
        AI.InitialiseAI(saveLoadScript.network);
    }

    public void SaveNetwork()
    {
        saveLoadScript.SaveNetwork();
    }

    public void NextShape()
    {
        pixelGridScript.RefreshGrid();

        List<float> results = AI.UseTheAI();
        if (results[0] <= 0f)
        {
            resultView.GetComponent<TextMeshProUGUI>().text = "Circle!";
        }
        else
        {
            resultView.GetComponent<TextMeshProUGUI>().text = "Rectangle!";
        }
    }

    public void BeginTraining()
    {
        AI.Train();
    }

    public void FinishTraining()
    {
        AI.StopTraining();
    }

    public void CalculatePercentageCorrect()
    {
        float corrects = 0;
        List<float> results;
        for (int i = 0; i < 1000; i++)
        {
            results = AI.UseAIToGenerateCorrectPercentage();
            if ((AI.cirOrRec == 0 && results[0] <= 0f) || (AI.cirOrRec == 1 && results[0] >= 0f))
            {
                corrects++;
            }
        }
        float correctPercentage = (corrects / 1000f) * 100f;
        Debug.Log(correctPercentage);
    }
}
