using UnityEngine;
public class PhantomBehaviour : MonoBehaviour
{
    [SerializeField] Renderer rend;
    private CombatUtils.Team Team;
    private float timeCounter;
    private int PassStage;
    private float Duration;
    private Animator animator;
    private float StepSize;
    public void Init(CombatUtils.Team team, float duration, float stepSize)
    {
        this.Team = team;
        this.Duration = duration;
        this.StepSize = stepSize;
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("moveZ", -1);
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        rend.material = Team == CombatUtils.Team.Player
            ? MaterialLibrary.Instance.transparentPlayerMaterial
            : MaterialLibrary.Instance.transparentEnemyMaterial;
        Color color = rend.material.color;
        color.a = 0.5f;
        rend.material.color = color;
        timeCounter = 0;
        PassStage = 0;
    }
    void Update()
    {
        BehaviourWhilePassing();
    }
    private void BehaviourWhilePassing()
    {
        transform.Translate(Time.deltaTime * new Vector3(0, 0, -StepSize));
        timeCounter += Time.deltaTime;
        if (timeCounter > (Duration / 5) * (PassStage + 1) - 0.15f)
        {
            NextStage();
        }
    }
    private void NextStage()
    {
        PassStage++;
        if (PassStage % 2 != 0)
        {
            animator.SetTrigger("Shoot");
        }
        if (PassStage == 5)
            Destroy(this.gameObject, 0.25f);
    }
}