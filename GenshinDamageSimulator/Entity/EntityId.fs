namespace GenshinDamageSimulator

exception InvalidEntityIdException of string * int32

type InvalidEntityIdException with
    static member format =
        sprintf "Invalid ID. Valid IDs must be greater than or equal to 0. If you would like to get an invalid ID, use %s."

type EntityId = EntityId of int32

module EntityId =
    let create id =
        if id >= 0 then
            id |> EntityId
        else
            raise (InvalidEntityIdException(InvalidEntityIdException.format "EntityId.none", id))

    let none = -1 |> EntityId

/// This is the C# interface for entity IDs.
type EntityId with
    /// Creates a new entity ID from the provided raw value. This method should be preferred
    /// over NewEntityId.
    static member Create (id: int32) =
        try
            EntityId.create id
        with :? InvalidEntityIdException ->
            // Create a new exception with the C# member name
            raise (InvalidEntityIdException(InvalidEntityIdException.format "EntityId.None", id))

    /// The placeholder entity ID (-1).
    static member None = EntityId.none