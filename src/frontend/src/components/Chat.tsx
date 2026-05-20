import type { ChatMessage } from '../types';
import { CardRenderer } from './cards/CardRenderer';

type Props = {
  messages: ChatMessage[];
};

export function Chat({ messages }: Props) {
  if (messages.length === 0) return null;

  return (
    <section className="chat" aria-label="Plan conversation">
      {messages.map((msg, i) => (
        <div key={i} className={`chat-message chat-message-${msg.role}`}>
          {msg.role === 'user' ? (
            <div className="chat-bubble chat-bubble-user">
              <p>{msg.content}</p>
            </div>
          ) : (
            <div className="chat-bubble chat-bubble-assistant">
              <p className="chat-plan-status">
                Plan <strong>{msg.plan.status}</strong>
              </p>
              <CardRenderer plan={msg.plan} />
            </div>
          )}
        </div>
      ))}
    </section>
  );
}
