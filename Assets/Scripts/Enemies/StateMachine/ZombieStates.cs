public class ZombieStates : State
{
    protected ZombieComponent OwnerZombie;
    public ZombieStates(ZombieComponent zombie, StateMachine stateMachine) : base(stateMachine)
    {
        OwnerZombie = zombie;
    }

}


public enum ZombieStateType
{
    Idle,
    Attack,
    Follow,
    Dead
}