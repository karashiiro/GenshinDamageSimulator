namespace GenshinDamageSimulator.Tests

open FsUnit
open GenshinDamageSimulator
open Xunit

module FormulaTests =
    [<Fact>]
    let ``Test that enemy defense multipliers are calculated correctly``() =
        let levelChar = 90u
        let levelEnemy = 81u
        let defReduction = 0f
        let defIgnore = 0f
        let actual = Formulas.calcDefenseMultiplier levelChar levelEnemy defReduction defIgnore
        abs (actual - 0.51212938f) |> should be (lessThan 0.01f)