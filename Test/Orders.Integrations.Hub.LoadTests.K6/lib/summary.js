import { MAX_ERROR_RATE, MAX_P95_MS } from '../config/config.js';

const SCENARIOS = [
  'ifood_webhook', 'food99_webhook', 'rappi_webhook_create', 'rappi_webhook_cancel',
  'rappi_webhook_other', 'rappi_webhook_ping', 'core_order_status', 'core_order_dispute',
  'core_cancellation_reasons_ifood', 'core_cancellation_reasons_rappi',
  'core_cancellation_reasons_food99', 'core_order_status_rappi', 'core_order_status_food99',
];

const number = (value) => Number.isFinite(value) ? value : 0;
const fmt = (value, digits = 2) => Number.isFinite(value) ? value.toFixed(digits) : 'N/A';
const fmtLatency = (value) => !Number.isFinite(value) ? 'N/A' : value === 0 ? '<0.001' : value.toFixed(3);
const metricValues = (metrics, name, scenario) => {
  if (!scenario) return metrics[name] ? metrics[name].values : null;
  const exact = metrics[`${name}{scenario:${scenario}}`];
  if (exact) return exact.values;
  const base = metrics[name];
  if (!base || !base.submetrics) return null;
  const entry = base.submetrics.find((metric) => metric.name === `${name}{scenario:${scenario}}`);
  return entry ? entry.values : null;
};

export function handleSummary(data) {
  const metrics = data.metrics || {};
  const lines = [
    '='.repeat(80),
    'k6 Load Test Summary',
    `Generated: ${new Date().toISOString()}`,
    `Thresholds: failed requests <= ${MAX_ERROR_RATE}% | p95 <= ${MAX_P95_MS} ms`,
    'Latency percentiles below 0.001 ms are shown as <0.001 (k6 timer resolution), not as zero.',
    '='.repeat(80),
    '',
  ];
  let passed = true;
  const rows = [];

  for (const scenario of SCENARIOS) {
    const req = metricValues(metrics, 'http_reqs', scenario);
    const failedMetric = metricValues(metrics, 'http_req_failed', scenario);
    const outcomeMetric = metricValues(metrics, 'scenario_failure_rate', scenario);
    const duration = metricValues(metrics, 'http_req_duration', scenario);
    if (!req && !failedMetric && !outcomeMetric && !duration) continue;

    const count = outcomeMetric
      ? number(outcomeMetric.passes) + number(outcomeMetric.fails)
      : failedMetric
        ? number(failedMetric.passes) + number(failedMetric.fails)
        : number(req && req.count);
    const failRate = outcomeMetric
      ? number(outcomeMetric.rate)
      : number(failedMetric && failedMetric.rate);
    const errorPercent = failRate * 100;
    const p95 = number(duration && duration['p(95)']);
    const p50 = number(duration && (duration.med ?? duration['p(50)']));
    const p99Value = duration && duration['p(99)'];
    const p99 = Number.isFinite(p99Value) ? p99Value : null;
    const avg = number(duration && duration.avg);
    const failed = outcomeMetric
      ? number(outcomeMetric.passes)
      : Math.round(count * failRate);
    const scenarioPass = count > 0 && errorPercent <= MAX_ERROR_RATE && p95 <= MAX_P95_MS;
    passed = passed && scenarioPass;
    const row = { scenario, requests: count, failed, errorRatePercent: errorPercent, avgMs: avg, p50Ms: p50, p95Ms: p95, p99Ms: p99, passed: scenarioPass };
    rows.push(row);
    lines.push('-'.repeat(80));
    lines.push(`Scenario: ${scenario}`);
    lines.push(`  Requests: ${count} | failed: ${failed} | error rate: ${fmt(errorPercent)}%`);
    lines.push(`  Latency ms: mean ${fmtLatency(avg)} | p50 ${fmtLatency(p50)} | p95 ${fmtLatency(p95)} | p99 ${fmtLatency(p99)}`);
    lines.push(`  Verdict: ${scenarioPass ? 'PASS' : 'FAIL'}`);
    lines.push('');
  }

  const total = number(metrics.http_reqs && metrics.http_reqs.values.count);
  const errorRate = number((metrics.scenario_failure_rate || metrics.http_req_failed || {}).values?.rate) * 100;
  const overallP95 = number(metrics.http_req_duration && metrics.http_req_duration.values['p(95)']);
  if (rows.length === 0 || errorRate > MAX_ERROR_RATE || overallP95 > MAX_P95_MS) passed = false;
  lines.push('='.repeat(80));
  lines.push(`Overall: ${total} requests | error rate ${fmt(errorRate)}% | p95 ${fmt(overallP95)} ms`);
  lines.push(`Overall verdict: ${passed ? 'PASS' : 'FAIL'}`);
  lines.push('='.repeat(80));

  const text = `${lines.join('\n')}\n`;
  const reportDirectory = __ENV.K6_REPORT_DIR || '.';
  const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
  const safeRows = rows.map((row) => `<tr><td>${row.scenario}</td><td>${row.requests}</td><td>${row.failed}</td><td>${fmt(row.errorRatePercent)}%</td><td>${fmtLatency(row.p50Ms)}</td><td>${fmtLatency(row.p95Ms)}</td><td>${fmtLatency(row.p99Ms)}</td><td>${row.passed ? 'PASS' : 'FAIL'}</td></tr>`).join('');
  const html = `<!doctype html><html><head><meta charset="utf-8"><title>k6 Load Test Summary</title><style>body{font:14px system-ui;margin:2rem;color:#222}table{border-collapse:collapse;width:100%}th,td{border:1px solid #ddd;padding:.5rem;text-align:right}th:first-child,td:first-child{text-align:left}th{background:#eee}.${passed ? 'pass' : 'fail'}{font-weight:bold}</style></head><body><h1>k6 Load Test Summary</h1><p>Generated ${new Date().toISOString()}</p><p>Thresholds: error rate ≤ ${MAX_ERROR_RATE}%; p95 ≤ ${MAX_P95_MS} ms</p><p>Percentiles below 0.001 ms are displayed as &lt;0.001 ms (k6 timer resolution).</p><table><thead><tr><th>Scenario</th><th>Requests</th><th>Failed</th><th>Error rate</th><th>p50 ms</th><th>p95 ms</th><th>p99 ms</th><th>Verdict</th></tr></thead><tbody>${safeRows}</tbody></table><h2 class="${passed ? 'pass' : 'fail'}">Overall verdict: ${passed ? 'PASS' : 'FAIL'}</h2></body></html>`;
  const prefix = `${reportDirectory}/k6-${timestamp}`;

  return {
    stdout: text,
    [`${prefix}-summary.txt`]: text,
    [`${prefix}-summary.json`]: JSON.stringify({ generatedAt: new Date().toISOString(), thresholds: { maxErrorRatePercent: MAX_ERROR_RATE, maxP95Ms: MAX_P95_MS }, totalRequests: total, overallErrorRatePercent: errorRate, overallP95Ms: overallP95, passed, scenarios: rows }, null, 2),
    [`${prefix}-report.html`]: html,
  };
}
