import type { MenuItem, MenuResult } from '../../types';

type Props = { data: MenuResult };

function getCost(item: MenuItem): number {
  const raw =
    item.estimatedCost ??
    (item as unknown as Record<string, unknown>).estimated_cost ??
    (item as unknown as Record<string, unknown>).cost ??
    (item as unknown as Record<string, unknown>).price ??
    0;
  const n = typeof raw === 'number' ? raw : Number(raw);
  return Number.isFinite(n) ? n : 0;
}

function groupByCategory(items: MenuItem[]): Map<string, MenuItem[]> {
  const map = new Map<string, MenuItem[]>();
  for (const item of items) {
    const category = item.category ?? 'Other';
    const existing = map.get(category);
    if (existing) {
      existing.push(item);
    } else {
      map.set(category, [item]);
    }
  }
  return map;
}

export function MenuCard({ data }: Props) {
  const items = Array.isArray(data?.items) ? data.items : [];
  const grouped = groupByCategory(items);
  const totalCost =
    typeof data?.totalCost === 'number' && Number.isFinite(data.totalCost)
      ? data.totalCost
      : items.reduce((sum, item) => sum + getCost(item), 0);

  return (
    <article className="agent-card" aria-label="Menu plan">
      <header className="agent-card-header">
        <span className="agent-card-icon" aria-hidden="true">
          🍽️
        </span>
        <h3 className="agent-card-title">Menu</h3>
      </header>

      <div className="agent-card-body">
        {[...grouped.entries()].map(([category, items]) => (
          <div key={category} className="menu-category">
            <h4 className="menu-category-label">{category}</h4>
            <ul className="menu-item-list">
              {items.map((item, index) => (
                <li key={`${item.name ?? 'item'}-${index}`} className="menu-item">
                  <span>{item.name ?? 'Unnamed item'}</span>
                  <span className="menu-item-cost">${getCost(item).toFixed(2)}</span>
                </li>
              ))}
            </ul>
          </div>
        ))}
        <div className="agent-card-footer">
          <strong>Total: ${totalCost.toFixed(2)}</strong>
        </div>
      </div>
    </article>
  );
}
