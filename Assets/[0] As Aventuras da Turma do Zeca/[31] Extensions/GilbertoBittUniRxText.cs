using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public static class GilbertoBittUniRxText
{
    public static IDisposable SubscribeToText(this IObservable<string> source, TextMeshProUGUI text)
    {
        return source.SubscribeWithState(text, (x, t) => t.text = x);
    }

    public static IDisposable SubscribeToText<T>(this IObservable<T> source, TextMeshProUGUI text)
    {
        return source.SubscribeWithState(text, (x, t) => t.text = x.ToString());
    }

    public static IDisposable SubscribeToText(this IObservable<string> source, TextMeshPro text)
    {
        return source.SubscribeWithState(text, (x, t) => t.text = x);
    }

    public static IDisposable SubscribeToText<T>(this IObservable<T> source, TextMeshPro text)
    {
        return source.SubscribeWithState(text, (x, t) => t.text = x.ToString());
    }
}
