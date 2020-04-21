using System;
using THEDARKKNIGHT.Interface;
using THEDARKKNIGHT.ThreadHelper;

namespace THEDARKKNIGHT.Network.TcpSocket.Client
{

    public class MessagerSolverClient : IMessagerParseSolver
    {

        public MessagerSolverClient() {
            ThreadCrossHelper.Instance().CreatThreadCrossHelp();
        }

        public void Close()
        {
           
        }

        public virtual void MessageSolver(byte[] data, string IPAddress)
        {

        }

        public void SetMessageFeedback(Action<byte[], string> msgFeedback)
        {
            
        }
    }

}
