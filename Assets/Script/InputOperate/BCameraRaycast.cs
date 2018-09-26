using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.BatCore;
using UnityEngine;
namespace THEDARKKNIGHT.InputOperate {

    public class BCameraRaycast : BatSingletion<BCameraRaycast>
    {
        private BCameraRaycast() { }

        private Camera RaycastCamera = Camera.main;

        private LayerMask mask = LayerMask.NameToLayer("DefaultRaycastLayers");

        private float RayDistance = Mathf.Infinity;

        private void SetLayerMask(int value) {
            mask.value = value;
        }

        private void SetShootCamera(Camera camera) {
            RaycastCamera = camera;
        }

        public Camera GetCurrentCamera() {
            return RaycastCamera;
        }

        public RaycastHit ShootRaycast(Vector3 postion) {
            RaycastHit hit;
            Ray shootRay = RaycastCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(shootRay, out hit, RayDistance, mask))
            {
                Debug.DrawLine(shootRay.origin, hit.point, Color.green);
            }
            else {
                Debug.DrawRay(shootRay.origin, shootRay.direction * 10000, Color.red);
            }
            return hit;
        }

    }

}

