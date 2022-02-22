namespace GenshinDamageSimulator.Tests

open FsUnit
open GenshinDamageSimulator.Data
open Xunit

module DataTests =
    [<Fact>]
    let ``Elemental coefficients are loaded`` () =
        ElementalCoefficients.loaded() |> should be True

    [<Fact>]
    let ``Elemental coefficients for characters should return with an valid level`` () =
        ElementalCoefficients.getCharacterLevelMultiplier 1u

    [<Fact>]
    let ``Crystallize coefficients for characters should return with an valid level`` () =
        ElementalCoefficients.getCharacterCrystallizeLevelMultiplier 1u

    [<Fact>]
    let ``Elemental coefficients for enemies should work return an valid level`` () =
        ElementalCoefficients.getEnemyLevelMultiplier 1u

    [<Fact>]
    let ``Elemental coefficients for characters should raise a mapping exception with an invalid level`` () =
        let failingFn () = ElementalCoefficients.getCharacterLevelMultiplier 500u |> ignore
        failingFn |> should throw typeof<MappingNotFoundException>

    [<Fact>]
    let ``Crystallize coefficients for characters should raise a mapping exception with an invalid level`` () =
        let failingFn () = ElementalCoefficients.getCharacterCrystallizeLevelMultiplier 500u |> ignore
        failingFn |> should throw typeof<MappingNotFoundException>

    [<Fact>]
    let ``Elemental coefficients for enemies should raise a mapping exception with an invalid level`` () =
        let failingFn () = ElementalCoefficients.getEnemyLevelMultiplier 500u |> ignore
        failingFn |> should throw typeof<MappingNotFoundException>