using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.EventDefine {

    public static class BatEventDefine
    {
        /// Input event 
        public const string LEFTPRESSEVENT = "LEFTPRESSEVENT";
        public const string LEFTDRAGEVENT = "LEFTDRAGEVENT";
        public const string LEFTRELEASEVENT = "LEFTRELEASEVENT";
        public const string RIGHTPRESSEVENT = "RIGHTPRESSEVENT";
        public const string RIGHTDRAGEVENT = "RIGHTDRAGEVENT";
        public const string RIGHTRELEASEVENT = "RIGHTRELEASEVENT";
        public const string ZOOMEVENT = "ZOOMEVENT";
        public const string SINGLETOUCHEVENT = "SINGLETOUCHEVENT";
        public const string SINGLEDRAGEVENT = "SINGLEDRAGEVENT";
        public const string SINGLERELEASEVENT = "SINGLERELEASEVENT";
        public const string MULTITOUCHEVENT = "MULTITOUCHEVENT";
        public const string MULTIDRAGEVENT = "MULTIDRAGEVENT";
        public const string MULTIRELEASEVENT = "MULTIRELEASEVENT";

        ////network event
        public const string UPDATEBYNETFRAME = "UPDATEBYNETFRAME";
    }
}

