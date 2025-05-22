public interface ISkill
{
    public void Initialize();
    public void Execute();
    public void Cancel();
    public void OnSkillAnimationEffect();
    public void OnSkillAnimationEnd();
}
