namespace Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;

public enum Food99DeliveryStatus {
    /// <summary>
    /// Assigned a courier.
    /// </summary>
    Assigned = 120,

    /// <summary>
    /// Courier has reached the store.
    /// </summary>
    ArrivedAtB = 130,

    /// <summary>
    /// The courier has picked up food.
    /// </summary>
    Taken = 140,

    /// <summary>
    /// The courier has reached the destination.
    /// </summary>
    ArrivedAtC = 150,

    /// <summary>
    /// Delivered. Order completed.
    /// </summary>
    Finish = 160,

    /// <summary>
    /// The delivery has been canceled.
    /// </summary>
    Cancel = 170,

    /// <summary>
    /// A riders has been reassigned.
    /// </summary>
    Reassigned = 180,

    /// <summary>
    /// The delivery has been aborted.
    /// </summary>
    Aborted = 190
}