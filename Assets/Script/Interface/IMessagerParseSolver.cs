using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Interface
{
    public interface IMessagerParseSolver
    {

        void MessageSolver(byte[] data,string IPAddress);

        void SetMessageFeedback(Action<byte[], string> msgFeedback);

        void Close();
    }
}
