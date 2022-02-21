namespace GenshinDamageSimulator

type EntityId = EntityId of uint32

/// This is the C# interface for entity IDs.
type EntityId with
    /// Creates a new entity ID from the provided raw value. This method should be preferred
    /// over NewEntityId.
    static member Create (id: uint32) = id |> EntityId