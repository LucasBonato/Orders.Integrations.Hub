/**
 * Webhook signature computation for k6 load tests.
 * Matches the TestCommon C# signers: IFoodSigner (HMACSHA256), RappiSigner (HMACSHA256 + timestamp), Food99Signer (MD5).
 */

import * as crypto from 'k6/crypto';

const SECRETS = {
  IFOOD: 'test-ifood-secret',
  RAPPI: 'test-rappi-secret',
  FOOD99: 'test-food99-secret',
};

/**
 * Compute HMAC-SHA256 signature (used by iFood).
 * @param {string} body - Raw JSON body
 * @param {string} secret - Signing secret
 * @returns {string} Hex-encoded lowercase signature
 */
export function signIFood(body, secret) {
  return crypto.hmac('sha256', secret, body, 'hex');
}

/**
 * Compute Rappi signature: HMAC-SHA256 over "timestamp.reserialized_body".
 * Returns "t=TIMESTAMP,sign=HEX_SIGNATURE".
 * @param {string} body - Raw JSON body
 * @param {string} secret - Signing secret
 * @returns {{header: string, value: string}} Header name and computed value
 */
export function signRappi(body, secret) {
  const timestamp = Math.floor(Date.now() / 1000).toString();
  // Match System.Text.Json's default escaping in RappiJsonSerializer. JSON.stringify
  // leaves Unicode (and HTML-sensitive characters) literal, while .NET escapes it.
  const reserialized = JSON.stringify(JSON.parse(body)).replace(/[\u007f-\uffff<>&'`]/g,
    (character) => `\\u${character.charCodeAt(0).toString(16).padStart(4, '0').toUpperCase()}`);
  const payloadToSign = `${timestamp}.${reserialized}`;

  const signature = crypto.hmac('sha256', secret, payloadToSign, 'hex');

  return {
    header: 'Rappi-Signature',
    value: `t=${timestamp},sign=${signature}`,
  };
}

/**
 * Compute Food99 signature: MD5(body + secret).
 * @param {string} body - Raw JSON body
 * @param {string} secret - Signing secret
 * @returns {{header: string, value: string}} Header name and computed value
 */
export function signFood99(body, secret) {
  return {
    header: 'didi-header-sign',
    value: crypto.md5(`${body}${secret}`, 'hex'),
  };
}

/**
 * Compute signature headers for a given integration.
 * @param {string} integration - Integration key: "IFOOD", "RAPPI", or "FOOD99"
 * @param {string} body - Raw JSON body
 * @returns {{header: string, value: string}} Header name and computed value
 */
export function sign(integration, body) {
  switch (integration.toUpperCase()) {
    case 'IFOOD':
      return {
        header: 'X-IFood-Signature',
        value: signIFood(body, SECRETS.IFOOD),
      };
    case 'RAPPI':
      return signRappi(body, SECRETS.RAPPI);
    case 'FOOD99':
      return signFood99(body, SECRETS.FOOD99);
    default:
      throw new Error(`Unknown integration: ${integration}`);
  }
}
