using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
namespace THEDARKKNIGHT
{
    public class StateObjectPool :BatSingletion<StateObjectPool>
    {

        Queue<StateObject> StateQuene = new Queue<StateObject>();

        private StateObjectPool() { }

        public void CreateStateObjectPool(int num) {
            if (StateQuene.Count < num)
            {
                for (int i = 0; i < num - StateQuene.Count; i++)
                {
                    EnQuene(new StateObject());
                }
            }
        }
 
        public void EnQuene(StateObject ob)
        {
            ClearStateObject(ob);
            StateQuene.Enqueue(ob);
        }

        public StateObject OutQuene()
        {
            if (StateQuene.Count > 0)
                return StateQuene.Dequeue();
            else
                return new StateObject();
        }

        public void ClearStateObject(StateObject ob)
        {
            if (ob != null)
                ob.workSocket = null;
        }
    }


    public class StateObject
    {

        public Socket workSocket = null;

        public const int BufferSize = 1024;

        public byte[] buffer = new byte[BufferSize];

    }
}