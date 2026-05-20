import type { ReservationHold } from '../../types';

type Props = { data: ReservationHold };

export function ReservationCard({ data }: Props) {
  return (
    <article className="agent-card" aria-label="Reservation hold">
      <header className="agent-card-header">
        <span className="agent-card-icon" aria-hidden="true">📋</span>
        <h3 className="agent-card-title">Reservation</h3>
      </header>

      <div className="agent-card-body">
        <div className="weather-detail-row">
          <span className="weather-detail-label">Park</span>
          <span>{data.parkName}</span>
        </div>
        <div className="weather-detail-row">
          <span className="weather-detail-label">Shelter</span>
          <span>{data.shelterName}</span>
        </div>
        <div className="weather-detail-row">
          <span className="weather-detail-label">Date</span>
          <span>{data.date}</span>
        </div>
        <div className="weather-detail-row">
          <span className="weather-detail-label">Status</span>
          <span className="reservation-status">{data.status}</span>
        </div>
      </div>
    </article>
  );
}
