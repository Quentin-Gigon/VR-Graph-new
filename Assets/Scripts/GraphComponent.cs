using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]

public class GraphComponent : MonoBehaviour
{

    public DrawableGraph<DrawableNode, float> Graph { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Graph = new DrawableGraph<DrawableNode, float>();
        //var zero = new Vector3(0, 0, 0);
        float ew = 0.02f; // set edge width

        //declare and initialize 5 nodes
        var node1 = Graph.createDrawableNode(1, 0.5f, 1.5f, 1.5f, Color.red, "Node1", "Description 1 Description 1 ");
        var node2 = Graph.createDrawableNode(2, 0f, 0f, 1.5f, Color.red, "Node2", "Description 2");
        var node3 = Graph.createDrawableNode(3, 0.7f, 0.3f, 1.7f, Color.red, "Node3", "Description 3");
        var node4 = Graph.createDrawableNode(4, 0.2f, 1.7f, 0.3f, Color.red, "Node4", "Description 4");
        //var node5 = graph.createDrawableNode(5, 0.7f, 1.6f, 2f, Color.red);

        //declare and initialize 5x2 edges (edge must be initialized both ways)
        var edge1 = Graph.createDrawableNodeEdge(1.0f, node1, node2, Color.yellow, ew);
        var edge2 = Graph.createDrawableNodeEdge(1.0f, node2, node1, Color.yellow, ew);

        var edge3 = Graph.createDrawableNodeEdge(1.0f, node2, node3, Color.yellow, ew);
        var edge4 = Graph.createDrawableNodeEdge(1.0f, node3, node2, Color.yellow, ew);

        var edge5 = Graph.createDrawableNodeEdge(1.0f, node3, node4, Color.yellow, ew);
        var edge6 = Graph.createDrawableNodeEdge(1.0f, node4, node3, Color.yellow, ew);

        var edge7 = Graph.createDrawableNodeEdge(1.0f, node1, node4, Color.yellow, ew);
        var edge8 = Graph.createDrawableNodeEdge(1.0f, node4, node1, Color.yellow, ew);

        var edge9 = Graph.createDrawableNodeEdge(1.0f, node2, node4, Color.yellow, ew);
        var edge0 = Graph.createDrawableNodeEdge(1.0f, node4, node2, Color.yellow, ew);

        
        //TODO better?
        Graph.Nodes.Add(node1);
        Graph.Nodes.Add(node2);
        Graph.Nodes.Add(node3);
        Graph.Nodes.Add(node4);
       // graph.Nodes.Add(node5);

        //TODO better?
        Graph.Edges.Add(edge1);
        Graph.Edges.Add(edge2);
        Graph.Edges.Add(edge3);
        Graph.Edges.Add(edge4);
        Graph.Edges.Add(edge5);
        Graph.Edges.Add(edge6);
        Graph.Edges.Add(edge7);
        Graph.Edges.Add(edge8);
        Graph.Edges.Add(edge9);
        Graph.Edges.Add(edge0);
    }

    private void Update()
    {
        //call step in every update to update the node positions and forces
        Graph.step();

        foreach (var node in Graph.Nodes)
        {
            //only update the position of the node game object (representing the sphere) if the value is not a nan
            if (float.IsNaN(node.Value.coord.magnitude))
            {
                Debug.Break();
            } else
            {
                node.Value.obj.transform.position = node.Value.coord;
            }

            //keep text aligned with main camera
            node.Value.objlabel.transform.LookAt(node.Value.objlabel.transform.position - Camera.main.transform.position);
            node.Value.objdesc.transform.LookAt(node.Value.objdesc.transform.position - Camera.main.transform.position);
        }

        //update the start and endpoint of the edges
        foreach (var edge in Graph.Edges)
        {
            edge.LRend.SetPosition(0, edge.From.Value.coord);
            edge.LRend.SetPosition(1, edge.To.Value.coord);

        }


    }

}