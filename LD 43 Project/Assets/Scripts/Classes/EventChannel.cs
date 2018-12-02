using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IEventChannel {
    void AddCallback(System.Action voidCallback);
}

public class EventChannel : IEventChannel {
    private event System.Action VoidEventHandler;

    public void AddCallback(System.Action voidCallback){
        if(VoidEventHandler == null){
            VoidEventHandler += voidCallback;
        }
    }

    public void RemoveCallback(System.Action oldCallback){
        if(VoidEventHandler != null){
            VoidEventHandler -= oldCallback;
        }
    }

    public void Invoke(){
        if(VoidEventHandler != null){
            VoidEventHandler.Invoke();
        }
    }
}

public class EventChannel<T> : IEventChannel {
    private event System.Action<T> EventHandler;
    private EventChannel VoidChannel;

    public EventChannel() {
        this.VoidChannel = new EventChannel();
    }

    public void AddCallback(System.Action<T> callback){
        if(EventHandler == null){
            EventHandler += callback;
        }
    }

    public void RemoveCallback(System.Action<T> oldCallback){
        if(EventHandler != null){
            EventHandler -= oldCallback;
        }
    }

    public void Invoke(T args){
        if(EventHandler != null) {
            EventHandler.Invoke(args);
        }
        VoidChannel.Invoke();
    }

    public void AddCallback(System.Action voidCallback){
        VoidChannel.AddCallback(voidCallback);
    }

    public void RemoveCallback(System.Action oldCallback){
        VoidChannel.RemoveCallback(oldCallback);
    }
}