using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalBus
{
    public static Signal GameOver { get; } = new Signal();
}