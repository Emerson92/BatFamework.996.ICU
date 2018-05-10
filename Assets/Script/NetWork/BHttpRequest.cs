using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace THEDARKKNIGHT {
    public class BHttpRequest
    {
        UnityWebRequest Request;

        DownloadHandler BDownloadHandler;

        UploadHandler BuploadHandler;

        public BHttpRequest() {
            Request = new UnityWebRequest();
        }

        public void SetConnectUrl(string url) {
            this.Request.url = url;
        }

    }
}

