/**
 * k6 load test for Orders.Integrations.Hub.
 * Simulates webhook bombardment + Core endpoint concurrency.
 *
 * Usage:
 *   k6 run loadtest.js
 *   k6 run --env STRESS=true loadtest.js
 *
 * Prerequisites:
 *   - Kestrel host running on port 5099 (see run.ps1 / run.sh)
 *   - WireMock container for outbound boundaries
 */

import { check } from 'k6';
import {
  sharedOptions,
  ifoodWebhook,
  food99Webhook,
  rappiWebhookCreate,
  rappiWebhookCancel,
  rappiWebhookOther,
  rappiWebhookPing,
  coreOrderStatus,
  coreOrderDispute,
  coreCancellationReasons_ifood,
  coreCancellationReasons_rappi,
  coreCancellationReasons_food99,
  coreOrderStatusRappi,
  coreOrderStatusFood99,
} from './scenarios/scenarios.js';
import { handleSummary } from './lib/summary.js';
import { BASE_URL, STRESS, STRESS_DURATION, WEBHOOK_DURATION, CORE_DURATION } from './config/config.js';

export const options = sharedOptions(STRESS, STRESS_DURATION, WEBHOOK_DURATION, CORE_DURATION);

export { handleSummary };

// Re-export scenario functions so k6 can resolve them by name
export {
  ifoodWebhook,
  food99Webhook,
  rappiWebhookCreate,
  rappiWebhookCancel,
  rappiWebhookOther,
  rappiWebhookPing,
  coreOrderStatus,
  coreOrderDispute,
  coreCancellationReasons_ifood,
  coreCancellationReasons_rappi,
  coreCancellationReasons_food99,
  coreOrderStatusRappi,
  coreOrderStatusFood99,
};

export default function () {
  // All scenarios are defined in scenarios.js and run via options.scenarios
  // This default function is a no-op; scenarios are executed independently.
}
