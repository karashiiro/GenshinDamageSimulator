namespace GenshinDamageSimulator.Tests

open FsUnit
open GenshinDamageSimulator
open Xunit

// https://library.keqingmains.com/combat-mechanics/elemental-effects/elemental-gauge-theory

module GaugeTests =
    // Tests
    [<Fact>]
    let ``Decay rates are correctly determined from initial elemental unit values (case A)``() =
        let eu, dr = Gauge.ofUnits 1f |> Gauge.unwrap
        eu |> should be (equal 1f)
        dr |> should be (equal 11.875f)

    [<Fact>]
    let ``Decay rates are correctly determined from initial elemental unit values (case B)``() =
        let eu, dr = Gauge.ofUnits 2f |> Gauge.unwrap
        eu |> should be (equal 2f)
        dr |> should be (equal 7.5f)

    [<Fact>]
    let ``Decay rates are correctly determined from initial elemental unit values (case C)``() =
        let eu, dr = Gauge.ofUnits 4f |> Gauge.unwrap
        eu |> should be (equal 4f)
        dr |> should be (equal 5.3125f)

    [<Fact>]
    let ``Aura tax application only reduces the elemental unit value by 20%, and does not modify the decay rate``() =
        // "Kaeya's E applies 2B * 0.8 = 1.6B Cryo aura and the decay rate is 7.5s per B."
        let eu, dr = Gauge.ofUnits 2f |> Gauge.tax |> Gauge.unwrap
        eu |> should be (equal 1.6f)
        dr |> should be (equal 7.5f)

    [<Fact>]
    let ``Adding elemental units does not modify the decay rate``() =
        // "Fischl's Charged Shot applies 0.8A Electro, the use of Beidou's Q will add 3.2C
        // Electro to the gauge, resulting in a 3.2A Electro aura persisting for 38 seconds
        // from the time of Beidou Q."
        let ag = Gauge.ofUnits 1f |> Gauge.tax
        let tg = Gauge.ofUnits 4f |> Gauge.tax
        let eu, dr = ag + tg |> Gauge.unwrap
        eu |> should be (equal 3.2f)
        dr |> should be (equal 11.875f)

    [<Fact>]
    let ``Subtracting elemental units does not modify the decay rate``() =
        // "Kaeya's E applies 1.6B Cryo aura and is triggered by Fischl's charged shot,
        // which applies 1A Electro. Superconduct occurs, 0.6B Cryo aura remains."
        let ag = Gauge.ofUnits 2f |> Gauge.tax
        let tg = Gauge.ofUnits 1f
        let eu, dr = ag - tg |> Gauge.unwrap
        eu |> should be (equal 0.6f)
        dr |> should be (equal 7.5f)

    [<Fact>]
    let ``Subtracting elemental units does not bring the elemental units below 0``() =
        // "Fischl's charged shot applies 0.8A Electro aura and is triggered by Kaeya's E,
        // applying 2B Cryo. Superconduct occurs, and no aura is left behind as triggers
        // can only remove units, they can’t add aura/gauge."
        let ag = Gauge.ofUnits 1f |> Gauge.tax
        let tg = Gauge.ofUnits 2f
        let eu, _ = ag - tg |> Gauge.unwrap
        eu |> should be (equal 0f)

    [<Fact>]
    let ``Multiplying elemental units by a scalar does not modify the decay rate``() =
        let g = Gauge.ofUnits 1f
        let eu, dr = g * 2f |> Gauge.unwrap
        eu |> should be (equal 2f)
        dr |> should be (equal 11.875f)

    [<Fact>]
    let ``Elemental unit multiplication is commutative``() =
        let g = Gauge.ofUnits 1f
        let eu, dr = g * 2f |> Gauge.unwrap
        let eu', dr' = 2f * g |> Gauge.unwrap
        eu |> should be (equal eu')
        dr |> should be (equal dr')