/**
 * k6 load test configuration.
 * Values can be overridden via environment variables.
 */

export const BASE_URL = __ENV.BASE_URL || 'http://127.0.0.1:5099';

function positiveInt(name, fallback) {
  const raw = __ENV[name];
  if (raw === undefined || raw === '') return fallback;
  const value = Number(raw);
  if (!Number.isInteger(value) || value <= 0) throw new Error(`${name} must be a positive integer`);
  return value;
}

function positiveNumber(name, fallback) {
  const raw = __ENV[name];
  if (raw === undefined || raw === '') return fallback;
  const value = Number(raw);
  if (!Number.isFinite(value) || value <= 0) throw new Error(`${name} must be a positive number`);
  return value;
}

function nonNegativeNumber(name, fallback) {
  const raw = __ENV[name];
  if (raw === undefined || raw === '') return fallback;
  const value = Number(raw);
  if (!Number.isFinite(value) || value < 0) throw new Error(`${name} must be a non-negative number`);
  return value;
}

// Webhook load profiles (safe defaults matching NBomber)
export const WEBHOOK_VUS = positiveInt('WEBHOOK_VUS', 3);
export const WEBHOOK_DURATION = __ENV.WEBHOOK_DURATION || '30s';
export const RAMP_UP_DURATION = __ENV.RAMP_UP_DURATION || '5s';
export const RAMP_DOWN_DURATION = __ENV.RAMP_DOWN_DURATION || '5s';

// Core endpoint load profiles (safe defaults matching NBomber)
export const CORE_VUS = positiveInt('CORE_VUS', 3);
export const CORE_DURATION = __ENV.CORE_DURATION || '30s';

// Rappi ping (lightweight trickle)
export const RAPPI_PING_VUS = positiveInt('RAPPI_PING_VUS', 2);

// Stress mode (higher load, use with caution)
export const STRESS = __ENV.STRESS === 'true';
export const STRESS_WEBHOOK_VUS = positiveInt('STRESS_WEBHOOK_VUS', 10);
export const STRESS_CORE_VUS = positiveInt('STRESS_CORE_VUS', 10);
export const STRESS_DURATION = __ENV.STRESS_DURATION || '2m';

// Summary thresholds
export const MAX_ERROR_RATE = nonNegativeNumber('MAX_ERROR_RATE', 5); // percent
if (MAX_ERROR_RATE > 100) throw new Error('MAX_ERROR_RATE must be between 0 and 100');
export const MAX_P95_MS = positiveNumber('MAX_P95_MS', 5000); // milliseconds
