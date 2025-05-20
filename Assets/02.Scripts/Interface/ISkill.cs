public interface ISkill
{
    void Execute();
    void Cancel();
    string SkillName { get; }
}
