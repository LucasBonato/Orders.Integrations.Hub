/**
 * k6 scenarios for Orders.Integrations.Hub load tests.
 * Each scenario targets a specific endpoint with realistic payloads and signatures.
 */

import http from 'k6/http';
import { check } from 'k6';
import { Rate } from 'k6/metrics';
import { sign } from '../lib/signers.js';
import {
  BASE_URL,
  WEBHOOK_VUS,
  WEBHOOK_DURATION,
  CORE_VUS,
  CORE_DURATION,
  RAPPI_PING_VUS,
  STRESS_WEBHOOK_VUS,
  STRESS_CORE_VUS,
  STRESS_DURATION,
  MAX_ERROR_RATE,
  MAX_P95_MS,
  RAMP_UP_DURATION,
  RAMP_DOWN_DURATION,
} from '../config/config.js';

// Load payload templates
import {
  ifoodKeepalive,
  ifoodConfirmed,
  ifoodCancelled,
  rappiCreate,
  rappiCancel,
  rappiOther,
  rappiPing,
  food99OrderNew,
  food99OrderUpdate,
} from '../lib/payloads.js';

function randomItem(arr) {
  return arr[Math.floor(Math.random() * arr.length)];
}

const IFoodPayloads = [ifoodKeepalive, ifoodConfirmed, ifoodCancelled];
const RappiCreatePayloads = [rappiCreate];
const RappiCancelPayloads = [rappiCancel];
const RappiOtherPayloads = [rappiOther];
const RappiPingPayloads = [rappiPing];
const Food99Payloads = [food99OrderNew, food99OrderUpdate];

const IFoodStatuses = ['CONFIRMED', 'PREPARING', 'READY_FOR_PICKUP', 'CANCELLED'];
const RappiStatuses = ['CONFIRMED', 'READY_FOR_PICKUP', 'CANCELLED'];
const Food99Statuses = ['CONFIRMED', 'READY_FOR_PICKUP', 'DELIVERED', 'CANCELLED'];
const DisputeTypes = ['ACCEPT', 'REJECT'];

let orderCounter = 0;
const scenarioFailureRate = new Rate('scenario_failure_rate');

function recordHttpOutcome(response, scenario) {
  scenarioFailureRate.add(response.status < 200 || response.status >= 300, { scenario });
}

// ---- Webhook Scenarios ----

export function ifoodWebhook() {
  const bodyObj = randomItem(IFoodPayloads);
  const body = JSON.stringify(bodyObj);
  const sig = sign('IFOOD', body);

  const res = http.post(`${BASE_URL}/api/v1/orders-hub/ifood/webhook`, body, {
    headers: {
      'Content-Type': 'application/json',
      [sig.header]: sig.value,
    },
    tags: { name: 'ifood_webhook' },
  });
  recordHttpOutcome(res, 'ifood_webhook');

  check(res, {
    'ifood webhook is successful': (r) => r.status >= 200 && r.status < 300,
  });
}

export function food99Webhook() {
  const bodyObj = randomItem(Food99Payloads);
  const body = JSON.stringify(bodyObj);
  const sig = sign('FOOD99', body);

  const res = http.post(`${BASE_URL}/api/v1/orders-hub/food99/webhook`, body, {
    headers: {
      'Content-Type': 'application/json',
      [sig.header]: sig.value,
    },
    tags: { name: 'food99_webhook' },
  });
  recordHttpOutcome(res, 'food99_webhook');

  check(res, {
    'food99 webhook is successful': (r) => r.status >= 200 && r.status < 300,
  });
}

export function rappiWebhookCreate() {
  const bodyObj = randomItem(RappiCreatePayloads);
  const body = JSON.stringify(bodyObj);
  const sig = sign('RAPPI', body);

  const res = http.post(`${BASE_URL}/api/v1/orders-hub/rappi/webhook`, body, {
    headers: {
      'Content-Type': 'application/json',
      [sig.header]: sig.value,
    },
    tags: { name: 'rappi_webhook_create' },
  });
  recordHttpOutcome(res, 'rappi_webhook_create');

  check(res, {
    'rappi create webhook is successful': (r) => r.status >= 200 && r.status < 300,
  });
}

export function rappiWebhookCancel() {
  const bodyObj = randomItem(RappiCancelPayloads);
  const body = JSON.stringify(bodyObj);
  const sig = sign('RAPPI', body);

  const res = http.post(`${BASE_URL}/api/v1/orders-hub/rappi/webhook/cancel`, body, {
    headers: {
      'Content-Type': 'application/json',
      [sig.header]: sig.value,
    },
    tags: { name: 'rappi_webhook_cancel' },
  });
  recordHttpOutcome(res, 'rappi_webhook_cancel');

  check(res, {
    'rappi cancel webhook is successful': (r) => r.status >= 200 && r.status < 300,
  });
}

export function rappiWebhookOther() {
  const bodyObj = randomItem(RappiOtherPayloads);
  const body = JSON.stringify(bodyObj);
  const sig = sign('RAPPI', body);

  const res = http.post(`${BASE_URL}/api/v1/orders-hub/rappi/webhook/other`, body, {
    headers: {
      'Content-Type': 'application/json',
      [sig.header]: sig.value,
    },
    tags: { name: 'rappi_webhook_other' },
  });
  recordHttpOutcome(res, 'rappi_webhook_other');

  check(res, {
    'rappi other webhook is successful': (r) => r.status >= 200 && r.status < 300,
  });
}

export function rappiWebhookPing() {
  const bodyObj = randomItem(RappiPingPayloads);
  const body = JSON.stringify(bodyObj);
  const sig = sign('RAPPI', body);

  const res = http.post(`${BASE_URL}/api/v1/orders-hub/rappi/webhook/ping`, body, {
    headers: {
      'Content-Type': 'application/json',
      [sig.header]: sig.value,
    },
    tags: { name: 'rappi_webhook_ping' },
  });
  recordHttpOutcome(res, 'rappi_webhook_ping');

  check(res, {
    'rappi ping webhook is successful': (r) => r.status >= 200 && r.status < 300,
  });
}

// ---- Core Endpoint Scenarios ----

export function coreOrderStatus() {
  orderCounter++;
  const status = IFoodStatuses[orderCounter % IFoodStatuses.length];

  const body = JSON.stringify({
    orderId: `order-${orderCounter}`,
    externalId: `ext-order-${orderCounter}`,
    merchantId: 'merchant-1',
    status: status,
    integration: 'ifood',
    cancellationReason: status === 'CANCELLED' ? '503' : null,
    cancellationMetadata: null,
  });

  const res = http.patch(`${BASE_URL}/api/v1/orders-hub/orders/status`, body, {
    headers: { 'Content-Type': 'application/json' },
    tags: { name: 'core_order_status' },
  });
  recordHttpOutcome(res, 'core_order_status');

  check(res, {
    'order status PATCH 204': (r) => r.status === 204,
  });
}

export function coreOrderDispute() {
  orderCounter++;
  const type = DisputeTypes[orderCounter % DisputeTypes.length];
  const altId = type === 'COUNTER_OFFER' ? 'alt-1' : null;

  const body = JSON.stringify({
    disputeId: `dispute-${orderCounter}`,
    integration: 'ifood',
    type: type,
    alternativeId: altId,
    disputeResponse: {
      reason: type === 'ACCEPT' ? 'Accepted' : type === 'REJECT' ? 'Rejected' : 'Counter offer',
      detailsReason: null,
      type: null,
      price: null,
      additionalTimeInMinutes: null,
      additionalTimeReason: null,
    },
  });

  const res = http.post(`${BASE_URL}/api/v1/orders-hub/orders/disputes`, body, {
    headers: { 'Content-Type': 'application/json' },
    tags: { name: 'core_order_dispute' },
  });
  recordHttpOutcome(res, 'core_order_dispute');

  check(res, {
    'order dispute POST 204': (r) => r.status === 204,
  });
}

export function coreCancellationReasons(integration) {
  const res = http.get(
    `${BASE_URL}/api/v1/orders-hub/orders/cancellation-reasons?externalOrderId=order-1&integration=${integration}`,
    {
      tags: { name: `core_cancellation_reasons_${integration}` },
    }
  );
  recordHttpOutcome(res, `core_cancellation_reasons_${integration}`);

  check(res, {
    [`cancellation reasons ${integration} 200`]: (r) => r.status === 200,
  });
}

export function coreOrderStatusRappi() {
  coreOrderStatusFor('rappi', RappiStatuses, '0', 'core_order_status_rappi');
}

export function coreOrderStatusFood99() {
  coreOrderStatusFor('food99', Food99Statuses, 'CustomerCancelled', 'core_order_status_food99');
}

function coreOrderStatusFor(integration, statuses, cancellationReason, scenarioName) {
  orderCounter++;
  const status = statuses[orderCounter % statuses.length];
  const body = JSON.stringify({
    orderId: `order-${orderCounter}`,
    externalId: `ext-order-${orderCounter}`,
    merchantId: 'merchant-1',
    status,
    integration,
    cancellationReason: status === 'CANCELLED' ? cancellationReason : null,
    cancellationMetadata: null,
  });
  const res = http.patch(`${BASE_URL}/api/v1/orders-hub/orders/status`, body, {
    headers: { 'Content-Type': 'application/json' },
    tags: { name: scenarioName, scenario: scenarioName },
  });
  recordHttpOutcome(res, scenarioName);
  check(res, { [`${scenarioName} PATCH 204`]: (r) => r.status === 204 });
}

// ---- Scenario Options ----

export function sharedOptions(stress, stressDuration, webhookDuration, coreDuration) {
  const wVus = stress ? STRESS_WEBHOOK_VUS : WEBHOOK_VUS;
  const cVus = stress ? STRESS_CORE_VUS : CORE_VUS;
  const wDur = stress ? stressDuration : webhookDuration;
  const cDur = stress ? stressDuration : coreDuration;

  const scenarioNames = [
    'ifood_webhook', 'food99_webhook', 'rappi_webhook_create', 'rappi_webhook_cancel',
    'rappi_webhook_other', 'rappi_webhook_ping', 'core_order_status', 'core_order_dispute',
    'core_cancellation_reasons_ifood', 'core_cancellation_reasons_rappi',
    'core_cancellation_reasons_food99', 'core_order_status_rappi', 'core_order_status_food99',
  ];
  const thresholds = {
    http_req_failed: [`rate<=${MAX_ERROR_RATE / 100}`],
    http_req_duration: [`p(95)<=${MAX_P95_MS}`],
  };
  thresholds.scenario_failure_rate = [`rate<=${MAX_ERROR_RATE / 100}`];
  for (const name of scenarioNames) {
    thresholds[`scenario_failure_rate{scenario:${name}}`] = [`rate<=${MAX_ERROR_RATE / 100}`];
    thresholds[`http_req_failed{scenario:${name}}`] = [`rate<=${MAX_ERROR_RATE / 100}`];
    thresholds[`http_req_duration{scenario:${name}}`] = [`p(95)<=${MAX_P95_MS}`];
  }
  const rampingScenario = (vus, duration, exec, name) => ({
    executor: 'ramping-vus',
    startVUs: 0,
    stages: [
      { target: vus, duration: RAMP_UP_DURATION },
      { target: vus, duration },
      { target: 0, duration: RAMP_DOWN_DURATION },
    ],
    gracefulRampDown: '0s',
    exec,
    tags: { scenario: name },
  });

  return {
    thresholds,
    // k6 omits p(99) from Trend summary values unless requested explicitly.
    summaryTrendStats: ['avg', 'min', 'med', 'max', 'p(90)', 'p(95)', 'p(99)'],
    scenarios: {
      ifood_webhook: rampingScenario(wVus, wDur, 'ifoodWebhook', 'ifood_webhook'),
      food99_webhook: rampingScenario(wVus, wDur, 'food99Webhook', 'food99_webhook'),
      rappi_webhook_create: rampingScenario(wVus, wDur, 'rappiWebhookCreate', 'rappi_webhook_create'),
      rappi_webhook_cancel: rampingScenario(wVus, wDur, 'rappiWebhookCancel', 'rappi_webhook_cancel'),
      rappi_webhook_other: rampingScenario(wVus, wDur, 'rappiWebhookOther', 'rappi_webhook_other'),
      rappi_webhook_ping: rampingScenario(RAPPI_PING_VUS, wDur, 'rappiWebhookPing', 'rappi_webhook_ping'),
      core_order_status: rampingScenario(cVus, cDur, 'coreOrderStatus', 'core_order_status'),
      core_order_status_rappi: rampingScenario(cVus, cDur, 'coreOrderStatusRappi', 'core_order_status_rappi'),
      core_order_status_food99: rampingScenario(cVus, cDur, 'coreOrderStatusFood99', 'core_order_status_food99'),
      core_order_dispute: rampingScenario(cVus, cDur, 'coreOrderDispute', 'core_order_dispute'),
      core_cancellation_reasons_ifood: rampingScenario(cVus, cDur, 'coreCancellationReasons_ifood', 'core_cancellation_reasons_ifood'),
      core_cancellation_reasons_rappi: rampingScenario(cVus, cDur, 'coreCancellationReasons_rappi', 'core_cancellation_reasons_rappi'),
      core_cancellation_reasons_food99: rampingScenario(cVus, cDur, 'coreCancellationReasons_food99', 'core_cancellation_reasons_food99'),
    },
  };
}

// Export individual exec functions for cancellation reasons per integration
export function coreCancellationReasons_ifood() {
  coreCancellationReasons('ifood');
}
export function coreCancellationReasons_rappi() {
  coreCancellationReasons('rappi');
}
export function coreCancellationReasons_food99() {
  coreCancellationReasons('food99');
}
