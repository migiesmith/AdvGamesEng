﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class Billboard : MonoBehaviour
    {
        private NVRHead head;

        private void Start()
        {
            head = FindObjectOfType<NVRHead>();
        }
        void Update()
        {
            transform.LookAt(head.transform.position);
        }
    }
}
