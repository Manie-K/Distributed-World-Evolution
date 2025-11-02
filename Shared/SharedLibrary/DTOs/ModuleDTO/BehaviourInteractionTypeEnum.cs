namespace SharedLibrary.DTOs.ModuleDTO
{
    [Serializable]
    public enum BehaviourInteractionTypeEnum
    {
        None, //For currently unavailable behaviours
        Move,
        Attack,
        Eat,
        Reproduce
    }
}