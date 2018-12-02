using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WillDid {
    public readonly EventChannel Will;
    public readonly EventChannel Did;

    public WillDid(){
        this.Will = new EventChannel();
        this.Did = new EventChannel();
    }
}

public class WillDid<T> {
    public readonly EventChannel<T> Will;
    public readonly EventChannel<T> Did;

    public WillDid(){
        this.Will = new EventChannel<T>();
        this.Did = new EventChannel<T>();
    }
}