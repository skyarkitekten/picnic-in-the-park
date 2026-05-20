import type { ParkResult } from '../../types';

type Props = { data: ParkResult[] };

export function ParksCard({ data }: Props) {
  const sorted = [...data].sort((a, b) => b.score - a.score);

  return (
    <article className="agent-card" aria-label="Park recommendations">
      <header className="agent-card-header">
        <span className="agent-card-icon" aria-hidden="true">
          🏞️
        </span>
        <h3 className="agent-card-title">Parks</h3>
      </header>

      <div className="agent-card-body">
        <table className="agent-table" aria-label="Ranked parks">
          <thead>
            <tr>
              <th>Park</th>
              <th>Shelter</th>
              <th>Capacity</th>
              <th>Score</th>
            </tr>
          </thead>
          <tbody>
            {sorted.map((p, i) => (
              <tr key={`${p.parkId}-${p.shelterName}-${i}`}>
                <td>
                  {p.parkName}
                  <span className="parks-city">{p.city}</span>
                </td>
                <td>{p.shelterName}</td>
                <td>{p.capacity}</td>
                <td>{(p.score * 100).toFixed(0)}%</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </article>
  );
}
