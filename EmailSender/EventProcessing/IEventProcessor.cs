﻿namespace EmailSender.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
