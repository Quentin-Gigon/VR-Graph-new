using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Graph<TNodeType,TEdgeType>
{
    public Graph()
    {
        Nodes = new List<Node<TNodeType>>();
        Edges = new List<Edge<TEdgeType, TNodeType>>(); 
    }

    public List<Node<TNodeType>> Nodes { get; private set; }
    public List<Edge<TEdgeType,TNodeType>> Edges { get; private set; }
}

//class which extends the Graph class to build a drawable force directed graph 
public class DrawableGraph<N, E> : Graph<N, E> where N : Coordinate
{

    public static float C = 0.5f; // how wide the graph is
    public static float K = 1.0f; // how elastic the graph is
    public static float T = 0.001f; // how fast does it converge to a stable state

    //function which when called executes one iteration in the building of the force directed graph
    public void step()
    {
        //dictionary to keep track of the forces of each node wrt other nodes
        var forces = new SortedDictionary<int, Vector3>();

        //compute forces of every node wrt other nodes
        foreach (var node in Nodes)
        {
            //temp vector 3 to store the repelling force of "node" wrt "other" nodes
            Vector3 repel = new Vector3(0, 0, 0);
            foreach (var other in Nodes)
            {
                if (node == other) continue; //node does not have any repelling force with itself 
                var direction = node.Value.coord - other.Value.coord; //compute the direction vector (node - other) 
                if (direction.magnitude < 0.01) continue; //ignore computations if magnitude of direction vector is too small, else leads to nans in the computations
                direction = direction / direction.magnitude; // divide direction vector by its magnitude to obtain a unit vector 
                direction *= C; //multiply by C (???) 
                repel += direction; // add repelling force for this "other" node to repel vector
            }
            forces.Add(node.Id, repel); // add repelling force of node with other nodes to the forces dictionary
        }

        //for each edge in the graph, add elasticity  (???) in 
        foreach (var edge in Edges)
        {
            var direction = edge.From.Value.coord - edge.To.Value.coord; //compute the direction vector (node - other) 
            var force = forces.GetValueOrDefault(edge.To.Id, new Vector3(0, 0, 0)); //get the force of the "to" node connected to this edge
            force += K * direction; //add the direction mutiplied by K (???) to the force of the to node

            //update the force for the "to" of this edge node
            forces[edge.To.Id] = force;

            /*
            forces.Remove(edge.To.Id);
            forces.Add(edge.To.Id, force);
            */
        }

        //add the force of every node computed above multiplied by speed of convergence to it's coordinates
        //(the smaller, K, the more iterations are necessary to converge to a steady state)
        foreach (var kv in forces)
        {
            foreach (var n in Nodes) {
                if (n.Id == kv.Key)
                    n.Value.coord += T * kv.Value;
            }
        }
    }

    //function to build a node
    public Node<DrawableNode> createDrawableNode(int id, float x, float y, float z, Color color, String s, String t)
    {
        var node = new Node<DrawableNode>() { Id = id, Value = new DrawableNode(new Vector3(x, y, z)), NodeColor = color };
        //Nodes.Add(node);

        //add label with text s
        node.Value.golab = new GameObject();
        node.Value.objlabel = node.Value.golab.AddComponent<TextMesh>();
        node.Value.objlabel.text = s;
        node.Value.objlabel.characterSize = 0.1f;
        node.Value.objlabel.alignment = TextAlignment.Center;
        node.Value.objlabel.anchor = TextAnchor.MiddleCenter;
        node.Value.objlabel.color = Color.red;
        node.Value.objlabel.transform.parent = node.Value.obj.transform;
        node.Value.objlabel.transform.position = node.Value.obj.transform.position; //+ new Vector3(0, 0.2f, 0);


        //add pane with text t
        node.Value.godesc = new GameObject();
        node.Value.objdesc = node.Value.godesc.AddComponent<TextMesh>();
        node.Value.objdesc.text = t;
        node.Value.objdesc.characterSize = 0.1f;
        node.Value.objdesc.alignment = TextAlignment.Center;
        node.Value.objdesc.anchor = TextAnchor.MiddleCenter;
        node.Value.objdesc.color = Color.red;
        node.Value.objdesc.transform.parent = node.Value.obj.transform;
        node.Value.objdesc.transform.position = node.Value.obj.transform.position + new Vector3(0f, 0.2f, 0);

       

        return node;
    }

    //TODO function to build an edge
    public Edge<float, DrawableNode> createDrawableNodeEdge(float v, Node<DrawableNode> from, Node<DrawableNode> to, Color color, float width)
    {
        //game object used to generate the line renderer component (must be unique to each node)
        GameObject g1 = new();
        var edge1 = new Edge<float, DrawableNode>()
        {
            Value = v,
                From = from,
                To = to,
                EdgeColor = color,
                LRend = g1.AddComponent<LineRenderer>()
        };

        edge1.LRend.startWidth = width;
        edge1.LRend.endWidth = width;
        //Edges.Add(edge1);
        return edge1;
    }

    //TODO simple function to connect 2 nodes



}

public class Coordinate {
    public Vector3 coord { get; set; }
}

public class Node<TNodeType>
{
    public Color NodeColor { get; set; }
    public TNodeType Value { get; set; }
    public int Id { get; set; }

}

public class Edge<TEdgeType, TNodeType>
{
    public Color EdgeColor { get; set; }
    public TEdgeType Value { get; set; }
    public Node<TNodeType> From { get; set; }
    public Node<TNodeType> To { get; set; }
    public LineRenderer LRend { get; set; }
}

public class DrawableNode : Coordinate
{
    public DrawableNode(Vector3 x0)
    {
        this.obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        this.obj.transform.position = x0;
        this.obj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        this.coord = x0;
    }

    public GameObject obj { get; set; }
    public TextMesh objlabel;
    public TextMesh objdesc;
    public GameObject golab;
    public GameObject godesc;
}



