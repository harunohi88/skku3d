using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Behaviour
{
    private Queue<T> _poolQueue = new Queue<T>();
    private List<T> _prefabList;
    private Transform _parent;
    
    /// <summary>
    /// 생성자에서 initialSize만큼 만들어서 pool에 넣어놓기
    /// </summary>
    /// <param name="prefabList"></param>
    /// <param name="initialSize"></param>
    /// <param name="parent"></param>
    public ObjectPool(List<T> prefabList, int initialSize, Transform parent = null)
    {
        this._prefabList = prefabList;
        this._parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            /// 랜덤 프리팹으로 만든다.
            T randomPrefab = _prefabList[Random.Range(0, _prefabList.Count)];
            T obj = Object.Instantiate(randomPrefab, parent);
            obj.gameObject.SetActive(false);
            _poolQueue.Enqueue(obj);
        }
    }

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this._prefabList = new List<T>();
        this._prefabList.Add(prefab);
        this._parent = parent;

        for(int i = 0; i < initialSize; i++)
        {
            T obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            _poolQueue.Enqueue(obj);
        }
    }
    
    /// <summary>
    /// pool에 남아있다면 빼서 주고, 없다면 새로 만들어서 줌
    /// </summary>
    /// <returns></returns>
    public T Get()
    {
        if (_poolQueue.Count > 0)
        {
            T obj = _poolQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            T randomPrefab = _prefabList[Random.Range(0, _prefabList.Count)];
            T newObj = Object.Instantiate(randomPrefab, _parent);
            return newObj;
        }
    }

    /// <summary>
    /// 리턴된 오브젝트는 비활성화 시킨 뒤 pool에 넣어놓는다.
    /// </summary>
    /// <param name="obj"></param>
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        _poolQueue.Enqueue(obj);
    }
}
