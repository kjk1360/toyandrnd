using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PerkGrade
{
    Normal = 0,
    Rare,
    Epic,
    Legendary
}
enum ItemGrade
{
    Normal = 0,
    Rare,
    Epic,
    Legary
}
enum CharacterGrade
{
    Normal = 0,
    Special
}

enum PerkType
{
    Volatility, //한번만 선택 가능
    Two, //두번만 선택가능
    Three, //세번만 선택가능
    Four, //네번만 선택가능
    Five, //다섯번만 선택가능
    GroupVolatility, //해당 그룹에서 한번만 선택 가능
}