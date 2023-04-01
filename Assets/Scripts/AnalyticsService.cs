using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace CustomAnalytics
{
    public class AnalyticsService : MonoBehaviour
    {
        [SerializeField] private string serverURL;
        [SerializeField, Min(0)] private float cooldownBeforeSend;
        private readonly EventsStorage eventsStorage = new EventsStorage();
        private EventsBuffer eventsBuffer;
        private bool isWaitingSend;

        private void Awake()
        {
            eventsBuffer = eventsStorage.Load();
            if (eventsBuffer.Count > 0)
                SendEvents(eventsBuffer);
        }

        private void OnDestroy()
        {
            eventsStorage.Save(eventsBuffer);
        }

        public void TrackEvent(string type, string data)
        {
            eventsBuffer.AddEvent(new Event(type, data));
            if (isWaitingSend)
                return;

            WaitAndSend();
        }

        private async void WaitAndSend()
        {
            isWaitingSend = true;
            await UniTask.Delay(TimeSpan.FromSeconds(cooldownBeforeSend));
            SendEvents(eventsBuffer);
            isWaitingSend = false;
        }

        private async void SendEvents(EventsBuffer events)
        {
            if (string.IsNullOrEmpty(serverURL))
                throw new NullReferenceException("Server url is null");

            eventsBuffer = new EventsBuffer();
            var json = events.ToJson();
            using (var request = UnityWebRequest.Post(serverURL, json))
            {
                try
                {
                    await request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                        events.CloneEvents(eventsBuffer);
                }
                catch
                {
                    events.CloneEvents(eventsBuffer);
                }
            }
        }
    }
}