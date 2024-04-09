public abstract class State
{
    protected Enemy enemy = null;
    public abstract void Enter(Enemy enemy);
    public abstract void Perform();
    public abstract void Exit();
}
