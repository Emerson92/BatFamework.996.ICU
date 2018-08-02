using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadHelper : DownloadHandlerScript
{
    /// <summary>
    /// <para>Callback, invoked as data is received from the remote server.</para>
    /// </summary>
    /// <param name="data">A buffer containing unprocessed data, received from the remote server.</param>
    /// <param name="dataLength">The number of bytes in data which are new.</param>
    /// <returns>
    /// <para>True if the download should continue, false to abort.</para>
    /// </returns>
    protected override bool ReceiveData(byte[] data, int dataLength){
        return true;
    }

    /// <summary>
    ///   <para>Callback, invoked with a Content-Length header is received.</para>
    /// </summary>
    /// <param name="contentLength">The value of the received Content-Length header.</param>
    protected override void ReceiveContentLength(int contentLength){


    }
    /// <summary>
    ///   <para>Callback, invoked when UnityWebRequest.downloadProgress is accessed.</para>
    /// </summary>
    /// <returns>
    ///   <para>The return value for UnityWebRequest.downloadProgress.</para>
    /// </returns>
    protected override float GetProgress() {
        return 0;
    }
}