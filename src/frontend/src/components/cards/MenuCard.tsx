import type { MenuItem, MenuResult } from '../../types';

type Props = { data: MenuResult };

function groupByCategory(items: MenuItem[]): Map<string, MenuItem[]> {
  const map = new Map<string, MenuItem[]>();
  for (const item of items) {
    const existing = map.get(item.category);
    if (existing) {
      existing.push(item);
    } else {
      map.set(item.category, [item]);
    }
  }
  return map;
}

export function MenuCard({ data }: Props) {
  const grouped = groupByCategory(data.items);

  return (
    <article className="agent-card" aria-label="Menu plan">
      <header className="agent-card-header">
        <span className="agent-card-icon" aria-hidden="true">🍽️</span>
        <h3 className="agent-card-title">Menu</h3>
      </header>

      <div className="agent-card-body">
        {[...grouped.entries()].map(([category, items]) => (
          <div key={category} className="menu-category">
            <h4 className="menu-category-label">{category}</h4>
            <ul className="menu-item-list">
              {items.map((item) => (
                <li key={item.name} className="menu-item">
                  <span>{item.name}</span>
                  <span className="menu-item-cost">
                    ${item.estimatedCost.toFixed(2)}
                  </span>
                </li>
              ))}
            </ul>
          </div>
        ))}
        <div className="agent-card-footer">
          <strong>Total: ${data.totalCost.toFixed(2)}</strong>
        </div>
      </div>
    </article>
  );
}
