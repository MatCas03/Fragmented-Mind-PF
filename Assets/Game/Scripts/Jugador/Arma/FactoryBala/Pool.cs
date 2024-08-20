using System;
using System.Collections.Generic;

public class Pool<T> 
{
    private Func<T> _factoryMethod;

    private Action<T> _turnOnCallback;
    private Action<T> _turnOffCallback;

    private List<T> _stockActual;

    public Pool(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, int initialCount)
    {
        _stockActual = new List<T>();

        _factoryMethod = factoryMethod;

        _turnOnCallback = turnOnCallback;

        _turnOffCallback = turnOffCallback;

        for (int i = 0; i < initialCount; i++)
        {
            T obj = _factoryMethod();

            _turnOffCallback(obj);

            _stockActual.Add(obj);
        }
    }

    public T GetObject()
    {
        T result;

        if (_stockActual.Count == 0)
        {
            result = _factoryMethod();
        }
        else
        {
            result = _stockActual[0];
            _stockActual.RemoveAt(0);
        }

        _turnOnCallback(result);

        return result;
    }

    public void ReturnObjectToPool(T obj)
    {
        _turnOffCallback(obj);
        _stockActual.Add(obj);
    }
}