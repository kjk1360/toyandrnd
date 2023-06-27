public enum PerkGrade
{
    Normal = 0,
    Rare,
    Epic,
    Legendary
}
public enum ItemGrade
{
    Normal = 0,
    Rare,
    Epic,
    Legary
}
public enum CharacterGrade
{
    Normal = 0,
    Special
}

public enum PerkType
{
    Volatility, //�ѹ��� ���� ����
    Two, //�ι��� ���ð���
    Three, //������ ���ð���
    Four, //�׹��� ���ð���
    Five, //�ټ����� ���ð���
    GroupVolatility, //�ش� �׷쿡�� �ѹ��� ���� ����
}
public enum SkillShapeType
{
    ProjectileSector,   //��ä�ø�� ����ü ��� 360�̸� �������
    ProjectileOpenFire, //����ü ����
    TargetPosition,       //Ÿ�� ��ǥ�� ��ȯ
}

/// <summary>
/// ��ų Ÿ�� ��ġ
/// </summary>
public enum SkillTargetPos
{
    User,   //�����
    CloseEnemy, //������ ��
    TriggerPos,   //Ʈ���� �˹� ��ų ��ġ
}
//��ų ������ Ÿ��
public enum SkillObjectMoveType
{
    None,
    Direction,  //���� ����
    Target
}

/// <summary>
/// FSM�� ���� ������Ʈ���� ��ų �ߵ� ����
/// </summary>
public enum ControllerTrigger
{
    //���� = 1000����
    Damaged = 1000,
    Healed,
    Death,
    Attack, //���� �õ��� First��¼���� ��ü��
    CastComplete,

    //�����̻󿩺� = 1100����
    Stun = 1100,
    Bleeding,
    Poison,
    Burn,
    Freeze,
    Blind,
    Fear,

    //Ư������ = 1200����
    OverKill = 1200, //����� �̸��� �������� ��üHP�� 50%�� �ʰ��Ͽ� �޾�����

    //�Ǻ�
}

/// <summary>
/// ��ų�� �Ҵ�� �ߵ� ����
/// </summary>
public enum SkillObjTrigger
{
    //�Ӽ� = 2000 ����
    Light = 2000,
    Dark,
    Fire,
    Ice,
    Wind,
    Earth,
    Physic,
    Projectile,
    Aoe,
    Origin, //���� ������ ��ų����(������ ��ų�� �θ��� �ƴ���)

    Remove = 2100,

}

public enum TriggerKeyword
{
    //�Ӽ� = 0 ����
    Light = 0,
    Dark,
    Fire,
    Ice,
    Wind,
    Earth,
    Physic,
    Magic,
    Projectile,
    Aoe,

    //�� = 100����
    Team = 100,
    Enemy,
    //���� = 200����
    Hit = 200,
    ProjectileEnd,
    Death,
    FirstTrigger,
    //��ų ����� �����̻󿩺� = 300����
    Stun = 300,
    Bleeding,
    Poison,
    Burn,
    Freeze,
    Blind,
    Fear,
    //��ų �ǰ��� �����̻� ���� = 400����

    //���� ��ų = 1000���� ���̵�� ���
}