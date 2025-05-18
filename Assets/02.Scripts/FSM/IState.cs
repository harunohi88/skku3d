using UnityEngine;

public interface IState<T>
{
    public void Enter(T entity);
    public void Update(T entity);
    public void Exit(T entity);
}
