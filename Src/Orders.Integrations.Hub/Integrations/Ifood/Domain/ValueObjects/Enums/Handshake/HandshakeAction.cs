﻿namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

public enum HandshakeAction {
    CANCELLATION,
    PARTIAL_CANCELLATION,
    PROPOSED_AMOUNT_REFUND,
    PROPOSED_ADDITIONAL_TIME,
    VOID
}