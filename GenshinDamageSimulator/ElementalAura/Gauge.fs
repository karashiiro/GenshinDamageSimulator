namespace GenshinDamageSimulator

// https://library.keqingmains.com/combat-mechanics/elemental-effects/elemental-gauge-theory
type Gauge = Gauge of float32 * float32

module Gauge =
    /// Unwraps the provided gauge into its constituent values.
    let unwrap g =
        let (Gauge (gu, dr)) = g
        gu, dr

    /// Wraps the provided values in a gauge.
    let wrap eu dr =
        (eu, dr) |> Gauge

    /// Returns the elmental units for the provided gauge.
    let eu g =
        let eu, _ = g |> unwrap
        eu

    /// Returns the decay rate for the provided gauge.
    let dr g =
        let _, dr = g |> unwrap
        dr

    /// Returns true if the provided gauge has an elemental unit value of less than or equal to 0.
    let isEmpty g =
        let eu, _ = g |> unwrap
        eu <= 0f

    /// Returns the initial duration for the provided raw elemental units, before the aura tax is applied.
    let initialDuration e =
        2.5f * e + 7f

    /// Returns the initial decay rate for the provided raw elemental units.
    let initialDecay e =
        (initialDuration e) / (e * 0.8f)

    /// Returns the gauge for the provided elemental unit value.
    let ofGauge e =
        (e, initialDecay e) ||> wrap

    /// Applies the aura tax to this gauge.
    let tax g =
        let eu, dr = g |> unwrap
        (eu * 0.8f, dr) ||> wrap

    /// Gauge addition. Adding two gauges replaces the aura elemental unit value and maintains the decay rate.
    let add ag tg =
        let _, dr1 = ag |> unwrap
        let eu2, _ = tg |> unwrap
        (eu2, dr1) ||> wrap

    /// Gauge subtraction. Subtracting two gauges subtracts the trigger elemental unit value from the
    /// aura elemental unit value and maintains the decay rate. The result of the subtraction may not
    /// go below zero.
    let sub ag tg =
        let eu1, dr1 = ag |> unwrap
        let eu2, _ = tg |> unwrap
        (max 0f (eu1 - eu2), dr1) ||> wrap

    /// Gauge multiplication. Gauges may only be multiplied by a scalar.
    let mul g m =
        let eu, dr = g |> unwrap
        (eu * m, dr) ||> wrap

type Gauge with
    /// Gauge addition. Adding two gauges replaces the aura elemental unit value and maintains the decay rate.
    static member (+) (ag, tg) = Gauge.add ag tg

    /// Gauge subtraction. Subtracting two gauges subtracts the trigger elemental unit value from the
    /// aura elemental unit value and maintains the decay rate. The result of the subtraction may not
    /// go below zero.
    static member (-) (ag, tg) = Gauge.sub ag tg

    /// Gauge multiplication. Gauges may only be multiplied by a scalar.
    static member (*) (g, m) = Gauge.mul g m

    /// Gauge multiplication. Gauges may only be multiplied by a scalar.
    static member (*) (m, g) = Gauge.mul g m