using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using THEDARKKNIGHT.Interface;
using THEDARKKNIGHT.ThreadHelper;
using UnityEngine;

namespace THEDARKKNIGHT.TcpSocket
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
