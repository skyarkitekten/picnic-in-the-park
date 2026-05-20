import type { GroceryResult } from '../../types';

type Props = { data: GroceryResult };

export function GroceryCard({ data }: Props) {
  return (
    <article className="agent-card" aria-label="Shopping list">
      <header className="agent-card-header">
        <span className="agent-card-icon" aria-hidden="true">
          🛒
        </span>
        <h3 className="agent-card-title">Grocery List</h3>
      </header>

      <div className="agent-card-body">
        <table className="agent-table" aria-label="Grocery items">
          <thead>
            <tr>
              <th>Item</th>
              <th>Qty</th>
              <th>Price</th>
              <th>Available</th>
            </tr>
          </thead>
          <tbody>
            {data.items.map((g, i) => (
              <tr key={`${g.name}-${i}`}>
                <td>{g.name}</td>
                <td>{g.quantity}</td>
                <td>${g.price.toFixed(2)}</td>
                <td>{g.available ? '✓' : '✗'}</td>
              </tr>
            ))}
          </tbody>
        </table>

        <div className="agent-card-footer">
          <strong>Total: ${data.totalCost.toFixed(2)}</strong>
        </div>
      </div>
    </article>
  );
}
