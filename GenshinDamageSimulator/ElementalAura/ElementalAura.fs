namespace GenshinDamageSimulator

// https://docs.google.com/document/d/e/2PACX-1vSEovpheHaeum4Ba0MlNdfyOTsJ-wZzDmBof13bVztYtKDi6OQhLqNdwEkEApo6vvtAV0L_tMal2ZTN/pub#h.6crtulvx1dlt

type ElementalAuraData =
    { Element: Element
      ApplicationSkillId: uint32 // Used for comparing ICDs
      ApplicationSkillIcdMs: float32
      GaugeUnits: GaugeUnits
      Permanent: bool }

type ElementalAura =
    | PyroAura of ElementalAuraData
    | HydroAura of ElementalAuraData
    | ElectroAura of ElementalAuraData
    | CryoAura of ElementalAuraData
    | AnemoAura of ElementalAuraData
    | GeoAura of ElementalAuraData
    | DendroAura of ElementalAuraData

module ElementalAura =
    let element aura =
        match aura with
        | PyroAura _ -> Element.Pyro
        | HydroAura _ -> Element.Hydro
        | ElectroAura _ -> Element.Electro
        | CryoAura _ -> Element.Cryo
        | AnemoAura _ -> Element.Anemo
        | GeoAura _ -> Element.Geo
        | DendroAura _ -> Element.Dendro

    let wrap aura =
        match aura.Element with
        | Element.Pyro -> PyroAura(aura)
        | Element.Hydro -> HydroAura(aura)
        | Element.Electro -> ElectroAura(aura)
        | Element.Cryo -> CryoAura(aura)
        | Element.Anemo -> AnemoAura(aura)
        | Element.Geo -> GeoAura(aura)
        | Element.Dendro -> DendroAura(aura)

    let unwrap aura =
        match aura with
        | PyroAura a -> a
        | HydroAura a -> a
        | ElectroAura a -> a
        | CryoAura a -> a
        | AnemoAura a -> a
        | GeoAura a -> a
        | DendroAura a -> a

    let oldIfPermanent aura trigger =
        if aura.Permanent then aura else trigger
        |> wrap
        |> Seq.singleton

    let reactionModifier reaction =
        match reaction with
        | StrongMelt | StrongVaporize -> 2.5f
        | WeakMelt | WeakVaporize -> 0.625f
        | Overload | Superconduct -> 1.25f
        | Crystallize | Swirl -> 0.625f
        | ElectroCharged -> 0.4f
        | _ -> 0f

    let calcUpdatedGaugeRaw aura trigger reaction =
        (GaugeUnits.unwrap aura.GaugeUnits) - reactionModifier reaction * (GaugeUnits.unwrap trigger.GaugeUnits)

    let resolveGaugeElectroCharged aura trigger = // TODO aura tax
        let newGaugeUnitsAura = calcUpdatedGaugeRaw aura trigger ElectroCharged
        if newGaugeUnitsAura <= 0f then
            Seq.singleton (wrap trigger)
        else
            [|wrap { aura with GaugeUnits = newGaugeUnitsAura |> GaugeUnits }; wrap trigger|]

    let resolveGauge aura trigger reaction =
        if aura.Permanent then
            Seq.singleton (wrap aura)
        elif trigger.Permanent then
            Seq.singleton (wrap trigger)
        else
            let newGaugeUnits = calcUpdatedGaugeRaw aura trigger reaction
            if newGaugeUnits <= 0f then Seq.empty else Seq.singleton (wrap { aura with GaugeUnits = newGaugeUnits |> GaugeUnits })

    let interact aura trigger = // TODO aura tax
        match (aura, trigger) with
        // Same-element triggers
        | (PyroAura pOld, PyroAura pNew) -> oldIfPermanent pOld pNew, None
        | (HydroAura hOld, HydroAura hNew) -> oldIfPermanent hOld hNew, None
        | (ElectroAura eOld, ElectroAura eNew) -> oldIfPermanent eOld eNew, None
        | (CryoAura cOld, CryoAura cNew) -> oldIfPermanent cOld cNew, None
        | (AnemoAura aOld, AnemoAura aNew) -> oldIfPermanent aOld aNew, None
        | (GeoAura gOld, GeoAura gNew) -> oldIfPermanent gOld gNew, None
        | (DendroAura dOld, DendroAura dNew) -> oldIfPermanent dOld dNew, None
        // Pyro aura reactions
        | (PyroAura p, HydroAura h) -> resolveGauge p h StrongVaporize, Some(StrongVaporize)
        | (PyroAura p, ElectroAura e) -> resolveGauge p e Overload, Some(Overload)
        | (PyroAura p, CryoAura c) -> resolveGauge p c WeakMelt, Some(WeakMelt)
        | (PyroAura p, AnemoAura a) -> resolveGauge p a Swirl, Some(Swirl)
        | (PyroAura p, GeoAura g) -> resolveGauge p g Crystallize, Some(Crystallize)
        | (PyroAura p, DendroAura d) -> [|PyroAura(p); DendroAura(d)|], Some(Burning) // TODO
        // Hydro aura reactions
        | (HydroAura h, PyroAura p) -> resolveGauge h p WeakVaporize, Some(WeakVaporize)
        | (HydroAura h, ElectroAura e) -> resolveGaugeElectroCharged h e, Some(ElectroCharged)
        | (HydroAura _, CryoAura c) -> Seq.singleton (CryoAura(c)), Some(Frozen) // TODO
        | (HydroAura h, AnemoAura a) -> resolveGauge h a Swirl, Some(Swirl)
        | (HydroAura h, GeoAura g) -> resolveGauge h g Crystallize, Some(Crystallize)
        // Electro aura reactions
        | (ElectroAura e, HydroAura h) -> resolveGaugeElectroCharged e h, Some(ElectroCharged)
        | (ElectroAura e, PyroAura p) -> resolveGauge e p Overload, Some(Overload)
        | (ElectroAura e, CryoAura c) -> resolveGauge e c Superconduct, Some(Superconduct)
        | (ElectroAura e, AnemoAura a) -> resolveGauge e a Swirl, Some(Swirl)
        | (ElectroAura e, GeoAura g) -> resolveGauge e g Crystallize, Some(Crystallize)
        // Cryo aura reactions
        | (CryoAura _, HydroAura h) -> Seq.singleton (HydroAura(h)), Some(Frozen) // TODO
        | (CryoAura c, PyroAura p) -> resolveGauge c p StrongMelt, Some(StrongMelt)
        | (CryoAura c, ElectroAura e) -> resolveGauge c e Superconduct, Some(Superconduct)
        | (CryoAura c, AnemoAura a) -> resolveGauge c a Swirl, Some(Swirl)
        | (CryoAura c, GeoAura g) -> resolveGauge c g Crystallize, Some(Crystallize)
        // Anemo aura reactions
        | (AnemoAura a, PyroAura p) -> resolveGauge a p Swirl, Some(Swirl)
        | (AnemoAura a, HydroAura h) -> resolveGauge a h Swirl, Some(Swirl)
        | (AnemoAura a, ElectroAura e) -> resolveGauge a e Swirl, Some(Swirl)
        | (AnemoAura a, CryoAura c) -> resolveGauge a c Swirl, Some(Swirl)
        // Geo aura reactions
        | (GeoAura g, PyroAura p) -> resolveGauge g p Crystallize, Some(Crystallize)
        | (GeoAura g, HydroAura h) -> resolveGauge g h Crystallize, Some(Crystallize)
        | (GeoAura g, ElectroAura e) -> resolveGauge g e Crystallize, Some(Crystallize)
        | (GeoAura g, CryoAura c) -> resolveGauge g c Crystallize, Some(Crystallize)
        // Dendro aura reactions
        | (DendroAura d, PyroAura p) -> [|DendroAura(d); PyroAura(p)|], Some(Burning) // TODO
        // All others (Anemo/Geo/Dendro)
        | (AnemoAura a, GeoAura g) -> oldIfPermanent a g, None
        | (AnemoAura a, DendroAura d) -> oldIfPermanent a d, None
        | (GeoAura g, AnemoAura a) -> oldIfPermanent g a, None
        | (GeoAura g, DendroAura d) -> oldIfPermanent g d, None
        | (DendroAura d, HydroAura h) -> oldIfPermanent d h, None
        | (DendroAura d, ElectroAura e) -> oldIfPermanent d e, None
        | (DendroAura d, CryoAura c) -> oldIfPermanent d c, None
        | (DendroAura d, AnemoAura a) -> oldIfPermanent d a, None
        | (DendroAura d, GeoAura g) -> oldIfPermanent d g, None
        | (HydroAura h, DendroAura d) -> oldIfPermanent h d, None
        | (ElectroAura e, DendroAura d) -> oldIfPermanent e d, None
        | (CryoAura c, DendroAura d) -> oldIfPermanent c d, None