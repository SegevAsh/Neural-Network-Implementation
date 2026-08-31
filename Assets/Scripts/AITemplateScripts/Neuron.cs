using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron
{
    //// holds the neurons that are connected to this one from behind
    //public Neuron[] connectedBehind;
    //public int layerNumber;
    // holds all the weights from the neurons connected behind it to it
    public float[] backwardWeights;

    //public enum NeuronType
    //{
    //    Input,
    //    Output,
    //    Hidden
    //}
    //public NeuronType neuronType;

    //public float weight;
    public float value;

    public Neuron(float[] _backwardWeights)
    {
        backwardWeights = _backwardWeights;
    }

    //// hidden layer neuron
    //public Neuron(Neuron[] _connectedBehind, float[] _backwardWeights, string This_Is_The_Constructor_For_A_Hidden_Layer_Neuron)
    //{
    //    connectedBehind = _connectedBehind;
    //    neuronType = NeuronType.Hidden;
    //    backwardWeights = _backwardWeights;
    //    value = 0f;
    //}

    //// input layer neuron
    //public Neuron()
    //{
    //    neuronType = NeuronType.Input;
    //    value = 0f;
    //}

    //// output layer neuron
    //public Neuron(Neuron[] _connectedBehind, float[] _backwardWeights)
    //{
    //    connectedBehind = _connectedBehind;
    //    backwardWeights = _backwardWeights;
    //    neuronType = NeuronType.Output;
    //    value = 0f;
    //}
}
