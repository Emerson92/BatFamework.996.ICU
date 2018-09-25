using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Log
{

    public class BLog : BatSingletion<BLog>
    {

        Dictionary<LogType, bool> LogState = new Dictionary<LogType, bool>(){
            {LogType.Log,true},
            {LogType.Warn,true},
            {LogType.Error,true}
        };

        public bool Switch = true;

        public enum LogType {
            Log,
            Warn,
            Error
        }

        private BLog() { }

        public void SetSwitch(bool state) {
            this.Switch = state;
        }


        public void SetLogState(LogType type,bool state) {
            LogState[type] = state;
        }

        public void Log(string msg) {
            if(Switch && LogState[LogType.Log])
                Debug.Log(msg);
        }

        public void Warn(string warnMsg) {
            if(Switch && LogState[LogType.Warn])
                Debug.Log(warnMsg);
        }

        public void Error(string errorMsg) {
            if (Switch && LogState[LogType.Error])
             Debug.Log(errorMsg);
        }
    }
}
