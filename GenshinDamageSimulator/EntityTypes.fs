namespace GenshinDamageSimulator

open StatTypes

module EntityTypes =
    [<Struct>]
    type Element = Unknown | Pyro | Hydro | Electro | Cryo | Anemo | Geo | Dendro

    [<Struct>]
    type Artifact =
        { MainStat: StatModifier
          StatLines: StatModifier array }

    [<Struct>]
    type Weapon =
        { Attack: uint32
          MainStat: StatModifier }

    [<Struct>]
    type BattleNpc =
        { BaseHp: uint32
          BaseAttack: uint32
          BaseDefense: uint32
          MainStat: StatModifier
          Element: Element
          Weapon: Weapon
          Artifacts: Artifact array }

    [<Struct>]
    type BattleNpcState =
        { ShieldHp: uint32 }