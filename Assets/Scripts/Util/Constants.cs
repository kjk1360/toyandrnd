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
    Volatility, //한번만 선택 가능
    Two, //두번만 선택가능
    Three, //세번만 선택가능
    Four, //네번만 선택가능
    Five, //다섯번만 선택가능
    GroupVolatility, //해당 그룹에서 한번만 선택 가능
}
public enum SkillShapeType
{
    ProjectileSector,   //부채꼴모양 투사체 방사 360이면 원형방사
    ProjectileOpenFire, //투사체 연사
    TargetPosition,       //타겟 좌표에 소환
}

/// <summary>
/// 스킬 타겟 위치
/// </summary>
public enum SkillTargetPos
{
    User,   //사용자
    CloseEnemy, //근접한 적
    TriggerPos,   //트리거 촉발 스킬 위치
}
//스킬 움직임 타입
public enum SkillObjectMoveType
{
    None,
    Direction,  //방향 지정
    Target
}

/// <summary>
/// FSM이 붙은 오브젝트들의 스킬 발동 조건
/// </summary>
public enum ControllerTrigger
{
    //상태 = 1000부터
    Damaged = 1000,
    Healed,
    Death,
    Attack, //공격 시도임 First어쩌구를 대체함
    CastComplete,

    //상태이상여부 = 1100부터
    Stun = 1100,
    Bleeding,
    Poison,
    Burn,
    Freeze,
    Blind,
    Fear,

    //특수상태 = 1200부터
    OverKill = 1200, //사망에 이르는 데미지가 전체HP의 50%를 초과하여 받았을때

    //판별
}

/// <summary>
/// 스킬에 할당된 발동 조건
/// </summary>
public enum SkillObjTrigger
{
    //속성 = 2000 부터
    Light = 2000,
    Dark,
    Fire,
    Ice,
    Wind,
    Earth,
    Physic,
    Projectile,
    Aoe,
    Origin, //최초 생성된 스킬인지(복제로 스킬을 부른게 아닌지)

    Remove = 2100,

}

public enum TriggerKeyword
{
    //속성 = 0 부터
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

    //팀 = 100부터
    Team = 100,
    Enemy,
    //상태 = 200부터
    Hit = 200,
    ProjectileEnd,
    Death,
    FirstTrigger,
    //스킬 사용자 상태이상여부 = 300부터
    Stun = 300,
    Bleeding,
    Poison,
    Burn,
    Freeze,
    Blind,
    Fear,
    //스킬 피격자 상태이상 여부 = 400부터

    //유발 스킬 = 1000부터 아이디로 취급
}