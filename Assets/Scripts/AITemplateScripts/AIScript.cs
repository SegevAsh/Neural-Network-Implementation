using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class AIScript : MonoBehaviour
{
    public Network currentBestNetwork;
    public List<Network> networkOffspring;
    bool training = false;
    CancellationTokenSource tokenSource;
    [SerializeField] PixelGrid pixelGrid;


    public int cirOrRec;

    private void Start()
    {
        tokenSource = new CancellationTokenSource();
        networkOffspring = new List<Network>();
    }

    public void InitialiseAI(Network _network)
    {
        currentBestNetwork = _network;
    }

    public List<float> UseTheAI()
    {
        List<float> results = new List<float>();
        for (int x = 1; x < currentBestNetwork.layers.Length; x++)
        {
            foreach (Neuron neuron in currentBestNetwork.layers[x].neurons)
            {
                for (int y = 0; y < neuron.backwardWeights.Length; y++)
                {
                    neuron.value += neuron.backwardWeights[y] * currentBestNetwork.layers[x - 1].neurons[y].value;
                }
                if (currentBestNetwork.layers[x].layerType == Layer.LayerType.Output)
                {
                    results.Add(neuron.value);
                }
            }
        }
        for (int x = 0; x < currentBestNetwork.layers.Length; x++)
        {
            foreach (Neuron neuron in currentBestNetwork.layers[x].neurons)
            {
                for (int y = 0; y < neuron.backwardWeights.Length; y++)
                {
                    neuron.value = 0;
                }
            }
        }
        return results;
    }

    public List<float> UseAIToGenerateCorrectPercentage()
    {
        List<float> results = new List<float>();

        cirOrRec = pixelGrid.MakeInvisibleGrid(currentBestNetwork);

        for (int x = 1; x < currentBestNetwork.layers.Length; x++)
        {
            foreach (Neuron neuron in currentBestNetwork.layers[x].neurons)
            {
                for (int y = 0; y < neuron.backwardWeights.Length; y++)
                {
                    neuron.value += neuron.backwardWeights[y] * currentBestNetwork.layers[x - 1].neurons[y].value;
                }
                if (currentBestNetwork.layers[x].layerType == Layer.LayerType.Output)
                {
                    results.Add(neuron.value);
                }
            }
        }
        for (int x = 0; x < currentBestNetwork.layers.Length; x++)
        {
            foreach (Neuron neuron in currentBestNetwork.layers[x].neurons)
            {
                for (int y = 0; y < neuron.backwardWeights.Length; y++)
                {
                    neuron.value = 0;
                }
            }
        }
        return results;
    }

    float GetFloatInRange(System.Random random, double min, double max)
    {
        return (float)(min + (random.NextDouble() * (max - min)));
    }

    public async void Train()
    {
        training = true;
        int circleOrRectangle = 0;
        var rnd = new System.Random();
        //tokenSource.Dispose();
        //tokenSource = new CancellationTokenSource();
        currentBestNetwork = await Task.Run(() =>
        {
            while (training)
            {
                // generate offspring of best previous network with random mutations in synapse weight
                for (int i = 0; i < 100; i++)
                {
                    networkOffspring.Add(new Network(currentBestNetwork.layers));
                    for (int x = 1; x < networkOffspring[i].layers.Length; x++)
                    {
                        foreach (Neuron neuron in networkOffspring[i].layers[x].neurons)
                        {
                            for (int y = 0; y < neuron.backwardWeights.Length; y++)
                            {
                                //neuron.backwardWeights[y] += Random.Range(-0.3f, 0.3f);
                                neuron.backwardWeights[y] += GetFloatInRange(rnd, -1, 1);
                            }
                        }
                    }
                }

                // for each of them, give them the shape test 100 times and count how many they get right
                List<float> results = new List<float>();
                List<int> scorePerNetwork = new List<int>();
                int num = 0;
                foreach (Network network in networkOffspring)
                {
                    scorePerNetwork.Add(0);
                    for (int i = 0; i < 100; i++)
                    {
                        circleOrRectangle = pixelGrid.MakeInvisibleGrid(network);
                        for (int x = 1; x < network.layers.Length; x++)
                        {
                            foreach (Neuron neuron in network.layers[x].neurons)
                            {
                                for (int y = 0; y < neuron.backwardWeights.Length; y++)
                                {
                                    neuron.value += neuron.backwardWeights[y] * network.layers[x - 1].neurons[y].value;
                                    network.layers[x - 1].neurons[y].value = 0f;
                                }
                                if (network.layers[x].layerType == Layer.LayerType.Output)
                                {
                                    results.Add(neuron.value);
                                    neuron.value = 0f;
                                }
                            }
                        }
                        if ((circleOrRectangle == 0 && results[0] <= 0f) || (circleOrRectangle == 1 && results[0] >= 0f))
                        {
                            scorePerNetwork[num]++;
                        }
                    }
                    num++;
                }

                // picks best one
                int bestScore = -1;
                foreach (int score in scorePerNetwork)
                {
                    if (score > bestScore)
                    {
                        bestScore = score;
                    }
                }
                currentBestNetwork = networkOffspring[scorePerNetwork.IndexOf(bestScore)];

                // stops training if stop training button is pressed
                if (tokenSource.IsCancellationRequested)
                {
                    training = false;
                }
            }

            return currentBestNetwork;
        }, tokenSource.Token);
    }

    public void StopTraining()
    {
        tokenSource.Cancel();
    }
}
