import type { WeatherResult } from '../../types';

const riskClasses: Record<string, string> = {
  Ideal: 'risk-ideal',
  Acceptable: 'risk-acceptable',
  'Risk:Rain': 'risk-rain',
  'Risk:Heat': 'risk-heat',
  Unsafe: 'risk-unsafe',
};

type Props = { data: WeatherResult };

export function WeatherCard({ data }: Props) {
  const riskClass = riskClasses[data.riskClassification] ?? '';

  return (
    <article className="agent-card" aria-label="Weather forecast">
      <header className="agent-card-header">
        <span className="agent-card-icon" aria-hidden="true">
          🌤️
        </span>
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
          <span className={`risk-badge ${riskClass}`}>{data.riskClassification}</span>
        </div>
      </div>
    </article>
  );
}
