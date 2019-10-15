using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineGroup : MonoBehaviour
{
    public void Init()
    {
        Pipeline[] pipelines = GetComponentsInChildren<Pipeline>();
        for (int i = 0; i < pipelines.Length; i++)
        {
            pipelines[i].Init(i);
        }
    }
}
