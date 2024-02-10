﻿using ChatAppServer.Util;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.CodeAnalysis;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ChatAppServer.Model
{
    public class CallInfoModel
    {
        private readonly Timer _timer;
      
      

        public string CallerConnectionId { get; }
        public IHubCallerClients Clients { get; }
        public ElapsedEventHandler TimeElapsed { get; }
       //public string[] ReseiversId { get; }



        public CallInfoModel(ElapsedEventHandler timeElapsed,string callerConnectionId,
          IHubCallerClients clients)
        {

            if (Utils.CheckForNull(timeElapsed))
            {
                throw new ArgumentNullException("timeElapsed is null");
            }
            if (Utils.CheckStrForNull(callerConnectionId))
            {
                throw new ArgumentNullException("callerConnectionId is null");
            }
            if (Utils.CheckForNull(clients))
            {
                throw new ArgumentNullException("clients is null");
            }
            CallerConnectionId = callerConnectionId;
            Clients = clients;
            TimeElapsed = timeElapsed;
            _timer = new Timer
            {
                Interval = 5000, // интервал в миллисекундах (здесь каждые 5 секунд)
                AutoReset = false // устанавливаем автоматическое повторение
            };
            _timer.Elapsed += timeElapsed; // подписываемся на событие "Elapsed"
            _timer.Start(); // запускаем таймер
        }

        public void StopTimer()
        {
            _timer.Enabled = false;
            _timer.Dispose();
        }
     

    }
}
