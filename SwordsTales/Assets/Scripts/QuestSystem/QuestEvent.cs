using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public  class QuestEvent
    {
        public enum EventStatus
        {
            WAITING,
            CURRENT,
            DONE
        };
    
        public string name;
        public string description;
        public string id;
    
        public int order = -1;
    
        public EventStatus status;
        public QuestButton button;
        public GameObject location;
        public List<QuestPath> pathList = new List<QuestPath>();

        public QuestEvent(string n, string d, GameObject loc)
        {
            id = Guid.NewGuid().ToString();
            name = n;
            description = d;
            status = EventStatus.WAITING;
            location = loc;
        }

        public void UpdateQuestEvent(EventStatus es)
        {
            status = es;
            button.UpdateButton(es);
        }

        public string GetId()
        {
            return id;
        }
    }
}
