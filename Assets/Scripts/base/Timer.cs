using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util 
{
    public delegate void Schedu();
    public class Timer
    {
        private float firtime;
        private float waitTime;
        private static List<Timer> timerList = new List<Timer>();
        private Schedu schedu;
        private int repectTimes;
        public Timer(float time, Schedu timefunc, int repet)
        {
            waitTime = time;
            schedu = timefunc;
            repectTimes = repet;
            firtime = time;
            timerList.Add(this);
        }
        public static void Update()
        {
            foreach (var timer in timerList)
            {
                timer.update();
            }
        }
        private void update() 
        {
            firtime -= Time.deltaTime;
            if (firtime <=0)
            {
                schedu();
                repectTimes--;
                if (repectTimes != 0)
                {
                    firtime += waitTime;
                }
                else
                {
                    timerList.Remove(this);
                }
            }
        }
    }
}
