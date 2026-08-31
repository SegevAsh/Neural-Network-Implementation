using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad : MonoBehaviour
{
    public Network network;

    public Network InitialiseFreshNetwork(List<int> numOfNeuronsInEachLayer)
    {
        List<Layer> layers = new List<Layer>();
        List<float> weightsTempVar = new List<float>();
        List<Neuron> neuronsTempVar = new List<Neuron>();

        //List<Neuron> layer1Neurons;
        //List<Neuron> layer2Neurons;
        //List<Neuron> layer3Neurons;

        //int numOfL1Neurons = 10000;
        //int numOfL2Neurons = 5000;
        //int numOfL3Neurons = 1;

        //layer1Neurons = new List<Neuron>();
        //layer2Neurons = new List<Neuron>();
        //layer3Neurons = new List<Neuron>();
        //weightsTempVar = new List<float>();
        //layers = new List<Layer>();

        //for (int i = 0; i < numOfL3Neurons; i++)
        //{
        //    for (int x = 0; x < numOfL2Neurons; x++)
        //    {
        //        weightsTempVar.Add(Random.Range(-1f, 1f));
        //    }
        //    layer3Neurons.Add(new Neuron(weightsTempVar.ToArray()));
        //    weightsTempVar.Clear();
        //}

        //for (int i = 0; i < numOfL2Neurons; i++)
        //{
        //    for (int x = 0; x < numOfL1Neurons; x++)
        //    {
        //        weightsTempVar.Add(Random.Range(-1f, 1f));
        //    }
        //    layer2Neurons.Add(new Neuron(weightsTempVar.ToArray()));
        //    weightsTempVar.Clear();
        //}

        //float[] temp = new float[0];
        //for (int i = 0; i < numOfL1Neurons; i++)
        //{
        //    layer1Neurons.Add(new Neuron(temp));
        //}

        //layers.Add(new Layer(layer1Neurons.ToArray(), Layer.LayerType.Input, 0));
        //layers.Add(new Layer(layer2Neurons.ToArray(), Layer.LayerType.Hidden, 1));
        //layers.Add(new Layer(layer3Neurons.ToArray(), Layer.LayerType.Output, 2));

        for (int i = 0; i < numOfNeuronsInEachLayer.Count; i++)
        {
            neuronsTempVar.Clear();
            if (i == 0)
            {
                float[] temp = new float[0];
                for (int x = 0; x < numOfNeuronsInEachLayer[i]; x++)
                {
                    neuronsTempVar.Add(new Neuron(temp));
                }
                layers.Add(new Layer(neuronsTempVar.ToArray(), Layer.LayerType.Input, i));
            }
            else if (i == numOfNeuronsInEachLayer.Count - 1)
            {
                for (int x = 0; x < numOfNeuronsInEachLayer[i]; x++)
                {
                    for (int y = 0; y < numOfNeuronsInEachLayer[i - 1]; y++)
                    {
                        weightsTempVar.Add(Random.Range(-1f, 1f));
                    }
                    neuronsTempVar.Add(new Neuron(weightsTempVar.ToArray()));
                    weightsTempVar.Clear();
                }
                layers.Add(new Layer(neuronsTempVar.ToArray(), Layer.LayerType.Output, i));
            }
            else
            {
                for (int x = 0; x < numOfNeuronsInEachLayer[i]; x++)
                {
                    for (int y = 0; y < numOfNeuronsInEachLayer[i - 1]; y++)
                    {
                        weightsTempVar.Add(Random.Range(-1f, 1f));
                    }
                    neuronsTempVar.Add(new Neuron(weightsTempVar.ToArray()));
                    weightsTempVar.Clear();
                }
                layers.Add(new Layer(neuronsTempVar.ToArray(), Layer.LayerType.Hidden, i));
            }
        }

        return new Network(layers.ToArray());
    }

    public void SaveNetwork()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination))
        {
            file = File.OpenWrite(destination);
        }
        else
        {
            file = File.Create(destination);
        }

        Network data = network;
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadNetwork()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination))
        {
            file = File.OpenRead(destination);
            BinaryFormatter bf = new BinaryFormatter();
            Network data = (Network)bf.Deserialize(file);
            file.Close();
            network = data;
        }
        else
        {
            List<int> ints = new List<int> { 10000, 500, 1 };
            network = InitialiseFreshNetwork(ints);
        }
    }
}
