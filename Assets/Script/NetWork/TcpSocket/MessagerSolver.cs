using THEDARKKNIGHT.Interface;
using THEDARKKNIGHT.ThreadHelper;

namespace THEDARKKNIGHT.Network.TcpSocket
{

    public class MessagerSolver : IMessagerParseSolver
    {

        public MessagerSolver() {
            ThreadCrossHelper.Instance().CreatThreadCrossHelp();
        }

        public void MessageSolver(object data)
        {
            ThreadCrossHelper.Instance().ExcutionFunc(() => {

            });
        }
    }

}
