---
description: "Use when writing, modifying, or generating TypeScript or TSX code. Covers idiomatic TypeScript 6 conventions, type definitions, and strict type safety."
applyTo: "**/*.{ts,tsx}"
---

# TypeScript Conventions

## Language Version

Target TypeScript 6. Use modern TypeScript features and syntax.

## Types Over Interfaces

Use `type` aliases for all type definitions. Only use `interface` when you need declaration merging or `extends` on a class.

```typescript
// Good
type WeatherForecast = {
  date: string;
  temperatureC: number;
  summary: string;
};

type Props = {
  title: string;
  children: React.ReactNode;
};

// Bad — don't use interface for simple object shapes
interface WeatherForecast {
  date: string;
  temperatureC: number;
  summary: string;
}
```

## No `any`

Never use `any`. Use `unknown` for truly unknown types, narrow with type guards, or define a proper type.

```typescript
// Good
function parse(input: unknown): string {
  if (typeof input === "string") return input;
  throw new Error("Expected string");
}

// Bad
function parse(input: any): string {
  return input;
}
```

## Package Management

Always use `npm install <PackageName>` to add packages. Never edit `.package.json` files directly to add or modify package references.
