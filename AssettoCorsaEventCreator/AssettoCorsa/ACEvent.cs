using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssettoCorsaEventCreator.AssettoCorsa
{
    public enum ACEventType
    {
        Drag,
        Drift,
        Hotlap,
        TimeAttack,
        Race
    };

    public class ACEvent
    {
        ACEventType eventType;
        public ACEventType EventType
        {
            get { return eventType; }
            set
            {
                eventType = value;
                if (eventType == ACEventType.Drag) { eventStart = "PIT"; }
                else if (eventType == ACEventType.Drift) { eventStart = "PIT"; }
                else if (eventType == ACEventType.Hotlap) { eventStart = "HOTLAP_START"; }
                else if (eventType == ACEventType.TimeAttack) { eventStart = "START"; }
                else if (eventType == ACEventType.Race) { eventStart = "START"; }
            }
        }

        List<ACDriver> drivers;
        public List<ACDriver> Drivers
        {
            get { return drivers; }
        }

        List<ACCondition> conditions;
        public List<ACCondition> Conditions
        {
            get { return conditions; }
        }

        string eventStart;
        public string EventStart
        {
            get { return eventStart; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public ACEvent()
        {
            drivers = new List<ACDriver>();
            conditions = new List<ACCondition>();

            eventType = ACEventType.Hotlap;
            eventStart = "HOTLAP_START";
            name = string.Empty;
            description = string.Empty;
        }
    }
}
