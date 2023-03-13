using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDescriptionActive : MonoBehaviour
{

    public int id;
    public GraphComponent graph;

    public void ToggleDescription()
    {
        //Debug.Log("My node id is " + id);
        foreach (var node in graph.Graph.Nodes)
        {
            if (node.Id == id)
            {
                node.Value.godesc.SetActive(!node.Value.godesc.activeSelf);
                break;
            }
        }
    }
}
