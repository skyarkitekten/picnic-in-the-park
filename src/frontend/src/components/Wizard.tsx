import { useState } from 'react';
import type { PicnicPlanRequest } from '../types';

type Props = {
  onSubmit: (request: PicnicPlanRequest) => void;
  disabled: boolean;
};

export function Wizard({ onSubmit, disabled }: Props) {
  const [eventDate, setEventDate] = useState('');
  const [partySize, setPartySize] = useState(4);
  const [budget, setBudget] = useState(80);
  const [locationPreference, setLocationPreference] = useState('');
  const [dietaryNotes, setDietaryNotes] = useState('');

  const tomorrow = new Date();
  tomorrow.setDate(tomorrow.getDate() + 1);
  const minDate = tomorrow.toISOString().split('T')[0];

  const canSubmit = !disabled && eventDate !== '' && partySize > 0 && budget > 0;

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!canSubmit) return;

    onSubmit({
      eventDate,
      partySize,
      budget,
      locationPreference: locationPreference || 'any',
      dietaryNotes: dietaryNotes || undefined,
    });
  }

  return (
    <form className="wizard" onSubmit={handleSubmit} aria-label="Plan your picnic">
      <h2 className="wizard-title">Plan a Picnic</h2>

      <div className="wizard-field">
        <label htmlFor="eventDate">Event Date</label>
        <input
          id="eventDate"
          type="date"
          min={minDate}
          value={eventDate}
          onChange={(e) => setEventDate(e.target.value)}
          required
          disabled={disabled}
        />
      </div>

      <div className="wizard-field">
        <label htmlFor="partySize">Party Size</label>
        <input
          id="partySize"
          type="number"
          min={1}
          max={100}
          value={partySize}
          onChange={(e) => setPartySize(Number(e.target.value))}
          required
          disabled={disabled}
        />
      </div>

      <div className="wizard-field">
        <label htmlFor="budget">Budget ($)</label>
        <input
          id="budget"
          type="number"
          min={1}
          max={2000}
          step="any"
          value={budget}
          onChange={(e) => setBudget(Number(e.target.value))}
          required
          disabled={disabled}
        />
      </div>

      <div className="wizard-field">
        <label htmlFor="locationPreference">Location Preference</label>
        <input
          id="locationPreference"
          type="text"
          placeholder="e.g. near the lake"
          value={locationPreference}
          onChange={(e) => setLocationPreference(e.target.value)}
          disabled={disabled}
        />
      </div>

      <div className="wizard-field">
        <label htmlFor="dietaryNotes">Dietary Notes</label>
        <input
          id="dietaryNotes"
          type="text"
          placeholder="e.g. vegetarian, nut allergy"
          value={dietaryNotes}
          onChange={(e) => setDietaryNotes(e.target.value)}
          disabled={disabled}
        />
      </div>

      <button type="submit" className="wizard-submit" disabled={!canSubmit}>
        {disabled ? 'Planning…' : 'Create Plan'}
      </button>
    </form>
  );
}
