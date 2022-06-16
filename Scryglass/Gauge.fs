namespace Scryglass

// https://library.keqingmains.com/combat-mechanics/elemental-effects/elemental-gauge-theory

[<AutoOpen>]
module GaugeTypes =
    type ElementalUnits = float
    type DecayRate = float
    type Gauge = Gauge of ElementalUnits * DecayRate

module Gauge =
    let create = (<|) Gauge

    /// Gauge addition. Adding two gauges replaces the aura elemental unit value and maintains the decay rate.
    let add aura trigger =
        let (Gauge (_, dr)) = aura
        let (Gauge (eu, _)) = trigger
        (eu, dr) |> Gauge

    /// Gauge subtraction. Subtracting two gauges subtracts the trigger elemental unit value from the
    /// aura elemental unit value and maintains the decay rate. The result of the subtraction may not
    /// go below zero.
    let sub aura trigger =
        let (Gauge (eu1, dr1)) = aura
        let (Gauge (eu2, _)) = trigger
        (max 0.0 (eu1 - eu2), dr1) |> Gauge

    /// Scalar gauge subtraction. This operation does not modify the decay rate.
    let subs aura gu =
        let (Gauge (eu, dr)) = aura
        (max 0.0 (eu - gu), dr) |> Gauge