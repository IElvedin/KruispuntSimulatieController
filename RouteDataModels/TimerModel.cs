using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruispuntSimulatieController.RouteDataModels
{
    public class TimerModel
    {
        public enum TimerClass
        {
            Starter,
            GreenCycle,
            OrangeCycle,
            RedCycle,
        }

        public int time { get; private set; }
        public TimerClass timerClass { get; private set; }

        public TimerModel()
        {
            SetClass(TimerClass.Starter);
        }

        public void SetClass(TimerClass timerClass)
        {
            this.timerClass = timerClass;
            SetTime();
        }

        public void SetTime()
        {
            switch (timerClass)
            {
                case TimerClass.Starter:
                        time = 2000;
                        break;
                case TimerClass.GreenCycle:
                        time = 6000;
                        break;
                case TimerClass.OrangeCycle:
                        time = 3000;
                        break;
                case TimerClass.RedCycle:
                        time = 2000;
                        break;
            }
        }
    }
}
