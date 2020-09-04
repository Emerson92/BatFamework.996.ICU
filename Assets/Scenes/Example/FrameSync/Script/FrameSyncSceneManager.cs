using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.BatCore;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace THEDARKKNIGHT.Example.FameSync.UI
{
    public class FrameSyncSceneManager : BatMonoSingletion<FrameSyncSceneManager>
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void EnterFrameSyncMainScene() {
            SceneManager.LoadScene("FrameSyncMain");
        }
    }
}
