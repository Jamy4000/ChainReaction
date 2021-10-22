using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal
{
    private event Action m_Signal;

    public void Raise()
    {
        m_Signal?.Invoke();
    }

    public void Listen(Action i_SignalListener)
    {
        m_Signal += i_SignalListener;
    }

    public void StopListening(Action i_SignalListener)
    {
        m_Signal -= i_SignalListener;
    }

    public void Reset()
    {
        m_Signal = null;
    }
}

public class Signal<T>
{
    private event Action<T> m_Signal;

    public void Raise(T i_SignalValue)
    {
        m_Signal?.Invoke(i_SignalValue);
    }

    public void Listen(Action<T> i_SignalListener)
    {
        m_Signal += i_SignalListener;
    }

    public void StopListening(Action<T> i_SignalListener)
    {
        m_Signal -= i_SignalListener;
    }

    public void Reset()
    {
        m_Signal = null;
    }
}