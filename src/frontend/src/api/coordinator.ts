import type { PicnicPlan } from '../types';

export async function createPlan(input: string): Promise<PicnicPlan> {
    const res = await fetch('/coordinator/responses', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ input, stream: false }),
    });

    if (!res.ok) {
        throw new Error(`Coordinator error: ${res.status}`);
    }

    const data: unknown = await res.json();
    return parsePlanFromResponse(data);
}

function parsePlanFromResponse(data: unknown): PicnicPlan {
    // The responses protocol wraps the assistant text in output[]
    if (
        typeof data === 'object' &&
        data !== null &&
        'output' in data &&
        Array.isArray((data as Record<string, unknown>).output)
    ) {
        const output = (data as Record<string, unknown[]>).output;
        for (const item of output) {
            if (
                typeof item === 'object' &&
                item !== null &&
                'type' in item &&
                (item as Record<string, unknown>).type === 'message'
            ) {
                const content = (item as Record<string, unknown[]>).content;
                if (Array.isArray(content)) {
                    for (const part of content) {
                        if (
                            typeof part === 'object' &&
                            part !== null &&
                            'text' in part &&
                            typeof (part as Record<string, unknown>).text === 'string'
                        ) {
                            const text = (part as Record<string, string>).text;
                            return JSON.parse(text) as PicnicPlan;
                        }
                    }
                }
            }
        }
    }

    // Fallback: treat the entire response as the plan
    return data as PicnicPlan;
}
