using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.InputOperate {

    public class BCameraRaycast : BatSingletion<BCameraRaycast>
    {
        private BCameraRaycast() { }

        private Camera RaycastCamera = Camera.main;

        private LayerMask mask ;

        public const int EVERTHINGMASK = ~(1 << 0);

        private float RayDistance = Mathf.Infinity;

        private void SetLayerMask(int value) {
            mask.value = value;
        }

        private void SetShootCamera(Camera camera) {
            RaycastCamera = camera;
        }

        public RaycastHit ShootRaycast(Vector3 postion) {
            RaycastHit hit;
            Physics.Raycast(RaycastCamera.ScreenPointToRay(Input.mousePosition), out hit, RayDistance, mask.value);
            return hit;
        }

    }

}

