using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
[System.Serializable]
public class CustomPool<T> : IDisposable where T: MonoBehaviour
{
    public int total;
    public string namePool;
    public T prefab;
    public bool canExpand = false;
    public bool isHasLifetime = false;
    public float lifeTime;
    public List<T> elements= new List<T>();
    private Dictionary<T, IDisposable> lifeTimeSequence = new Dictionary<T, IDisposable>();
    private int index=-1;
    public Transform parent;
    private bool _disposed;
    public Action<Transform> onInit;
    // Start is called before the first frame update

    public CustomPool()
    {
    }
    public CustomPool(string name, int total,T prefab,bool canExpand = false, Transform parent = null, Action<Transform> onInit = null)
    {
        this.namePool = name;
        this.total = total;
        this.prefab = prefab;
        this.canExpand = canExpand;
        this.parent = parent;
        this.onInit = onInit;
    }
    public T OnSpawned(Transform parent = null)
    {
        if (_disposed)
            return default;
        index++;

        if (index >= elements.Count )
        {
            index = 0;
        }

        T trans = elements[index];
        while(trans.gameObject.activeInHierarchy && index < elements.Count - 1)
        {
            index++;
            trans = elements[index];
        }

        if (trans.gameObject.activeInHierarchy)
        {
            index = -1;
            if (canExpand)
            {
                var transObj = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity, this.parent);
                onInit?.Invoke(transObj.transform);
                elements.Add(transObj.GetComponent<T>());
            }
            else
            {
                trans = elements[0];
            }
        }
        if (isHasLifetime)
        {
            lifeTimeSequence.Add(trans, Observable.Timer(TimeSpan.FromSeconds(lifeTime)).Subscribe(_ =>
            {
                OnDespawned(trans);
            }));
        }
        IPoolable e = trans.GetComponent<IPoolable>();
        e?.OnSpawned();
        trans.transform.SetParent(parent == null ? this.parent : parent) ;
        //trans.SetPositionAndRotation();
        //trans.localPosition = Vector3.zero;
        //trans.localRotation = Quaternion.identity;
        trans.gameObject.SetActive(true);
        //trans.localScale = Vector3.one;
        return trans;
    }
    public void OnDespawned(T trans)
    {
        if (_disposed)
            return;
        if (!trans.gameObject.activeSelf)
        {
            return;
        }
        if(elements.Contains(trans))
        {
            if(isHasLifetime)
            {
                if(lifeTimeSequence.ContainsKey(trans))
                {
                    lifeTimeSequence[trans].Dispose();
                }
            }
            IPoolable e = trans.GetComponent<IPoolable>();
            e?.OnDespawned();
            trans.gameObject.SetActive(false);
            trans.transform.SetParent(parent == null ? this.parent : parent);
        }
    }
    public void DespawnAll()
    {
        if (_disposed)
            return;
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].gameObject.activeSelf)
            {
                OnDespawned(elements[i]);
            }
        }
    }
    ~CustomPool() => Dispose(false);
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // Dispose managed state (managed objects).
            for (int i = 0; i < elements.Count; i++)
            {
                MonoBehaviour.Destroy(elements[i]);
            }
            elements.Clear();
        }
        // Dispose managed state (managed objects, not nullable).
        _disposed = true;
    }
}
