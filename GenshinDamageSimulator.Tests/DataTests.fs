namespace GenshinDamageSimulator.Tests

open FsUnit
open GenshinDamageSimulator.Data
open Xunit

module DataTests =
    [<Fact>]
    let ``Elemental coefficients are loaded`` () =
        ElementalCoefficients.loaded() |> should be True