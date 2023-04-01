using UnityEngine;

namespace CustomAnalytics
{
    [System.Serializable]
    public class Event
    {
        [SerializeField] private string type;
        [SerializeField] private string data;

        public Event(string type, string data)
        {
            this.type = type;
            this.data = data;
        }
    }
}