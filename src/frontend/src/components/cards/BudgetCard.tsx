import type { BudgetSummary } from '../../types';

type Props = { data: BudgetSummary; totalBudget: number };

export function BudgetCard({ data, totalBudget }: Props) {
  const overBudget = data.remaining < 0;
  const pctUsed = Math.min((data.total / totalBudget) * 100, 100);

  return (
    <article className="agent-card" aria-label="Budget summary">
      <header className="agent-card-header">
        <span className="agent-card-icon" aria-hidden="true">
          💰
        </span>
        <h3 className="agent-card-title">Budget</h3>
      </header>

      <div className="agent-card-body">
        <div className="budget-bar-container" aria-hidden="true">
          <div
            className="budget-bar-fill"
            style={{
              width: `${pctUsed}%`,
              backgroundColor: overBudget ? '#ef4444' : '#22c55e',
            }}
          />
        </div>

        <div className="weather-detail-row">
          <span className="weather-detail-label">Food</span>
          <span>${data.foodCost.toFixed(2)}</span>
        </div>
        <div className="weather-detail-row">
          <span className="weather-detail-label">Reservation</span>
          <span>${data.reservationCost.toFixed(2)}</span>
        </div>
        <div className="weather-detail-row">
          <span className="weather-detail-label">Misc</span>
          <span>${data.miscCost.toFixed(2)}</span>
        </div>

        <div className="agent-card-footer">
          <div className="budget-totals">
            <span>
              <strong>Spent:</strong> ${data.total.toFixed(2)} / ${totalBudget.toFixed(2)}
            </span>
            <span className={overBudget ? 'budget-over' : 'budget-remaining'}>
              {overBudget ? 'Over by' : 'Remaining'}: ${Math.abs(data.remaining).toFixed(2)}
            </span>
          </div>
        </div>
      </div>
    </article>
  );
}
