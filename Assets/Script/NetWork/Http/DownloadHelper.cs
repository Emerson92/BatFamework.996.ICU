using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT;
using UnityEngine;
using UnityEngine.Networking;

namespace THEDARKKNIGHT
{
    public class DownloadHelper : BHttpCore<DownloadHandlerBuffer,UploadHandlerRaw>
    {
        protected override void HttpErrorHappen()
        {
            
        }

        protected override void HttpRequsetDone(DownloadHandlerBuffer downloadhelper,UploadHandlerRaw uploadhelper){
            

        }

        protected override void NetworkErrorHappen()
        {
            
        }

        protected override void ProgressUpdate(DownloadHandlerBuffer )
        {
           
        }

        
    }
}
