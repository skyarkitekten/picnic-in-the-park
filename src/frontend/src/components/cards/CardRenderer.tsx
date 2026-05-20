import type { PicnicPlan } from '../../types';
import { WeatherCard } from './WeatherCard';
import { ParksCard } from './ParksCard';
import { MenuCard } from './MenuCard';
import { GroceryCard } from './GroceryCard';
import { ReservationCard } from './ReservationCard';
import { BudgetCard } from './BudgetCard';

type Props = { plan: PicnicPlan };

export function CardRenderer({ plan }: Props) {
  return (
    <div className="card-grid" role="region" aria-label="Picnic plan details">
      <WeatherCard data={plan.weather} />
      <ParksCard data={plan.parks} />
      <MenuCard data={plan.menu} />
      <GroceryCard data={plan.grocery} />
      <ReservationCard data={plan.reservation} />
      <BudgetCard data={plan.budget} totalBudget={plan.request.budget} />
    </div>
  );
}
