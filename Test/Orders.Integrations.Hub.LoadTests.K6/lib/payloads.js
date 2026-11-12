/**
 * Payload templates loaded from JSON files.
 * open() resolves paths relative to the script file's directory.
 * Since this file is in lib/, paths use ../payloads/.
 */

export const ifoodKeepalive = JSON.parse(open('../payloads/IFood/keepalive.json'));
export const ifoodConfirmed = JSON.parse(open('../payloads/IFood/confirmed.json'));
export const ifoodCancelled = JSON.parse(open('../payloads/IFood/cancelled.json'));

export const rappiCreate = JSON.parse(open('../payloads/Rappi/create.json'));
export const rappiCancel = JSON.parse(open('../payloads/Rappi/cancel.json'));
export const rappiOther = JSON.parse(open('../payloads/Rappi/other.json'));
export const rappiPing = JSON.parse(open('../payloads/Rappi/ping.json'));

export const food99OrderNew = JSON.parse(open('../payloads/Food99/order-new.json'));
export const food99OrderUpdate = JSON.parse(open('../payloads/Food99/order-update.json'));
