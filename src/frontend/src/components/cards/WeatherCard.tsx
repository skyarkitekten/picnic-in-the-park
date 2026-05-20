import type { WeatherResult } from '../../types';

const riskColors: Record<string, string> = {
  Ideal: '#22c55e',
  Acceptable: '#eab308',
  'Risk:Rain': '#3b82f6',
  'Risk:Heat': '#ef4444',
  Unsafe: '#dc2626',
};

type Props = { data: WeatherResult };

export function WeatherCard({ data }: Props) {
  const color = riskColors[data.riskClassification] ?? '#94a3b8';

  return (
    <article className="agent-card" aria-label="Weather forecast">
      <header className="agent-card-header">
        <span className="agent-card-icon" aria-hidden="true">🌤️</span>
        <h3 className="agent-card-title">Weather</h3>
      </header>

      <div className="agent-card-body">
        <div className="weather-detail-row">
          <span className="weather-detail-label">Date</span>
          <span>{data.date}</span>
        </div>
        <div className="weather-detail-row">
          <span className="weather-detail-label">Condition</span>
          <span>{data.condition}</span>
        </div>
        <div className="weather-detail-row">
          <span className="weather-detail-label">Temperature</span>
          <span>{data.temperatureF}°F</span>
        </div>
        <div className="weather-detail-row">
          <span className="weather-detail-label">Risk</span>
          <span className="risk-badge" style={{ backgroundColor: color }}>
            {data.riskClassification}
          </span>
        </div>
      </div>
    </article>
  );
}
