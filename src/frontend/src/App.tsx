import { useState } from 'react';
import { Wizard } from './components/Wizard';
import { Chat } from './components/Chat';
import { createPlan } from './api/coordinator';
import type { ChatMessage, PicnicPlanRequest } from './types';
import './App.css';

function App() {
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handlePlanRequest(request: PicnicPlanRequest) {
    setLoading(true);
    setError(null);

    const userMsg = `Plan a picnic on ${request.eventDate} for ${request.partySize} people, $${request.budget} budget, ${request.locationPreference}${request.dietaryNotes ? `, dietary: ${request.dietaryNotes}` : ''}`;

    setMessages((prev) => [...prev, { role: 'user', content: userMsg }]);

    try {
      const plan = await createPlan(userMsg);
      setMessages((prev) => [...prev, { role: 'assistant', plan }]);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create plan');
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="app-container">
      <header className="app-header">
        <h1 className="app-title">Picnic Planner</h1>
        <p className="app-subtitle">AI-powered picnic planning with multi-agent orchestration</p>
      </header>

      <main className="main-content">
        <aside className="wizard-panel">
          <Wizard onSubmit={handlePlanRequest} disabled={loading} />
        </aside>

        <section className="chat-panel">
          {error && (
            <div className="error-message" role="alert" aria-live="polite">
              <svg
                width="20"
                height="20"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
                aria-hidden="true"
              >
                <circle cx="12" cy="12" r="10" />
                <line x1="12" y1="8" x2="12" y2="12" />
                <line x1="12" y1="16" x2="12.01" y2="16" />
              </svg>
              <span>{error}</span>
            </div>
          )}

          {loading && messages.length > 0 && (
            <div className="loading-indicator" role="status" aria-live="polite">
              <div className="loading-spinner" aria-hidden="true" />
              <span>Agents are planning your picnic…</span>
            </div>
          )}

          <Chat messages={messages} />

          {messages.length === 0 && !loading && (
            <div className="empty-state">
              <span className="empty-state-icon" aria-hidden="true">
                🧺
              </span>
              <p>
                Fill in the form and click <strong>Create Plan</strong> to get started.
              </p>
            </div>
          )}
        </section>
      </main>
    </div>
  );
}

export default App;
