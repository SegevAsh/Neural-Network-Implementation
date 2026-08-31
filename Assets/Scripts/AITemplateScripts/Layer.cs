using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer
{
    public Neuron[] neurons;

    public enum LayerType
    {
        Input,
        Output,
        Hidden
    }
    public LayerType layerType;
    public int layerNumber;

    public Layer(Neuron[] _neurons, LayerType _layerType, int _layerNumber)
    {
        neurons = _neurons;
        layerType = _layerType;
        layerNumber = _layerNumber;
    }
}
