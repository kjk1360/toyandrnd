using UnityEngine;

public abstract class State
{
    public abstract void EnterState(BaseController controller);
    public abstract void UpdateState(BaseController controller);
    public abstract void ExitState(BaseController controller);
}