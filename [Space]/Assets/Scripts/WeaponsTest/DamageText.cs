using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space {
    public class DamageText : MonoBehaviour {
        public ParticleSystem dt0;
        public ParticleSystem dt1;
        public ParticleSystem dt2;
        public ParticleSystem dt3;
        public ParticleSystem dt4;
        public ParticleSystem dt5;
        public ParticleSystem dt6;
        public ParticleSystem dt7;
        public ParticleSystem dt8;
        public ParticleSystem dt9;

        public NVRPlayer player;
        private ParticleSystem.EmitParams emitParams;
        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
        }

        public void displayDamage(Vector3 position, float damage)
        {
            transform.position = position + Random.insideUnitSphere;
            int damageInt = (int)damage;
            
            while (damageInt > 0)
            {
                int damageDigit = damageInt % 10;
                switch (damageDigit)
                {
                    case 0:
                        dt0.Emit(1);
                        break;

                    case 1:
                        dt1.Emit(1);
                        break;

                    case 2:
                        dt2.Emit(1);
                        break;

                    case 3:
                        dt3.Emit(1);
                        break;

                    case 4:
                        dt4.Emit(1);
                        break;

                    case 5:
                        dt5.Emit(1);
                        break;

                    case 6:
                        dt6.Emit(1);
                        break;

                    case 7:
                        dt7.Emit(1);
                        break;

                    case 8:
                        dt8.Emit(1);
                        break;

                    case 9:
                        dt9.Emit(1);
                        break;

                    default:
                        break;
                }
                damageInt /= 10;
                transform.position -= 0.08f*Vector3.Normalize(player.Head.transform.right);
            }
        }
    }
}
