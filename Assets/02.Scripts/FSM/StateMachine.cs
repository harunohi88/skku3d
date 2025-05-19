using UnityEngine;

public class StateMachine<T>
{
    private T _owner;
    private IState<T> _currentState;

    public IState<T> CurrentState => _currentState;

    public StateMachine(T owner) { this._owner = owner; }

    public void ChangeState(IState<T> newState)
    {
        if(_currentState != null)
        {
            _currentState.Exit(_owner);
        }

        _currentState = newState;
        _currentState.Enter(_owner);
    }

    public void Update()
    {
        if (_currentState != null) _currentState.Update(_owner);
    }
}
