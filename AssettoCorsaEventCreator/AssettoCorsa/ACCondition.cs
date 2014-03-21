using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssettoCorsaEventCreator.AssettoCorsa
{
    public enum ACConditionType
    {
        Time,
        Points,
        Wins,
        Position,
        AI
    };

    public class ACCondition
    {
        ACConditionType type;
        public ACConditionType Type
        {
            get { return type; }
            set { type = value; }
        }

        ACEventType eventType;
        public ACEventType EventType
        {
            get { return eventType; }
            set { eventType = value; }
        }

        int time;
        public int Value
        {
            get { return time; }
            set { time = value; }
        }

        public ACCondition()
        {
            type = ACConditionType.Time;
            time = 0;
        }

        public override string ToString()
        {
            string text = string.Empty;

            if (type == ACConditionType.Time)
            {
                TimeSpan ts = new TimeSpan(0, 0, 0, 0, time);
                text = string.Format("Beat a lap of {0}:{1:00}", ts.Minutes, ts.Seconds);
            }
            else if (type == ACConditionType.AI)
            {
                text = string.Format("AI Level : {0}", time);
            }
            else if (type == ACConditionType.Position)
            {
                text = string.Format("Finish Position {0} or Better", time);
            }
            else
            {
                text = string.Format("{0} - Points", time);
            }

            return text;
        }
    }
}
