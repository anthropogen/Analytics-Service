using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAnalytics
{
    [Serializable]
    public class EventsBuffer
    {
        [SerializeField] private List<Event> events = new List<Event>();
        public int Count => events.Count;

        public void AddEvent(Event @event)
            => events.Add(@event);

        public void CloneEvents(EventsBuffer other)
            => other.events.InsertRange(0, events);
    }

    public static class JsonHelper
    {
        public static string ToJson(this EventsBuffer events)
              => JsonUtility.ToJson(events);

        public static EventsBuffer ToEventsBuffer(this string str)
            => JsonUtility.FromJson<EventsBuffer>(str);
    }
}