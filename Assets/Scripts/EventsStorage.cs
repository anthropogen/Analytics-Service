using UnityEngine;

namespace CustomAnalytics
{
    public class EventsStorage
    {
        private const string SaveKey = "Events";

        public EventsBuffer Load()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                return PlayerPrefs.GetString(SaveKey).
                    ToEventsBuffer();
            }

            return new EventsBuffer();
        }

        public void Save(EventsBuffer eventsBuffer)
        {
            PlayerPrefs.SetString(SaveKey, eventsBuffer.ToJson());
            PlayerPrefs.Save();
        }
    }
}