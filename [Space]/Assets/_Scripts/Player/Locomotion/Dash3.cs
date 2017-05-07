using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(LineRenderer))]
    public class Dash3 : MonoBehaviour
    {
        public LineRenderer line;
        public NVRPlayer player;
        public PlayerState state;
        private NVRHand hand;
        private NVRButtonInputs touchpad;
        private RaycastHit hitInfo;

        private Vector2 dashDirection;
        private Vector3 headStart;
        private Vector3 headEnd;
        private Vector3 playerStart;
        private Vector3 playerEnd;

        public float yOffset;
        public float borderWidth;
        public float dashSpeed;
        public float maxDistance;
        private float dashDistance;

        public int combatLimit;
        private int dashCount;
        public float combatCooldown;
        private float limitTimer;

        public bool gazeControl;
        public bool leftHanded;
        private bool isDashing;
        private bool inCombat;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
