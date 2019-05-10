using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkStructTest : MonoBehaviour {


    LinkedList<int> ProcessList = new LinkedList<int>();

    LinkedList<int> SecondProcessList = new LinkedList<int>();

    // Use this for initialization
    void Start () {
        for (int i = 0; i < 10;i++) {
            LinkedListNode<int> node = new LinkedListNode<int>(i);
            ProcessList.AddLast(node);
        }
        for (int j = 100; j < 110; j++)
        {
            LinkedListNode<int> node = new LinkedListNode<int>(j);
            SecondProcessList.AddLast(node);
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.Log("Check");
            foreach (int item in ProcessList) {
                Debug.Log(item);
            }
            foreach (int item in SecondProcessList)
            {
                Debug.Log(item);
            }

        }
        if (Input.GetKeyDown(KeyCode.I)) {
            Debug.Log("Insert the Value!");
            int num = SecondProcessList.Count;
            LinkedListNode<int> PevNode = null;
            LinkedListNode<int> LastNode = ProcessList.FindLast(9);
            for (int i = 0; i < num; i++) {
                if (PevNode != null)
                    LastNode = PevNode;
                LinkedListNode<int> node = SecondProcessList.First;
                SecondProcessList.RemoveFirst();
                Debug.Log(node.Value);
                Debug.Log("LastNode :"+ LastNode.Value);
                ProcessList.AddAfter(LastNode, node);
                PevNode = node;
            }

        }
	}
}
