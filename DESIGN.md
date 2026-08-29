---
name: Picnic In The Park
description: A multi-agent picnic planner set at golden hour, where frosted paper panels float over a single sunlit meadow photograph.
colors:
  field: "#fdf4e7"
  field-deep: "#f7e9d6"
  paper: "rgba(255, 251, 243, 0.9)"
  paper-raised: "rgba(255, 253, 248, 0.94)"
  paper-sunk: "rgba(250, 240, 226, 0.72)"
  ink: "#2b1d14"
  ink-2: "#5c4634"
  ink-3: "#77604a"
  ink-on-dark: "#fffaf1"
  accent-terracotta: "#b03a0b"
  accent-deep: "#8f2f09"
  accent-tint: "rgba(176, 58, 11, 0.09)"
  line: "#e0cfb4"
  line-strong: "#998063"
  status-ideal-bg: "#bbf7d0"
  status-ideal-ink: "#14532d"
  status-caution-bg: "#fde68a"
  status-caution-ink: "#713f12"
  status-rain-bg: "#bfdbfe"
  status-rain-ink: "#1e3a8a"
  status-heat-bg: "#fecaca"
  status-heat-ink: "#7f1d1d"
  status-unsafe-bg: "#991b1b"
  status-unsafe-ink: "#ffffff"
  status-unknown-bg: "#e7e0d5"
  status-unknown-ink: "#3f3a33"
  good-text: "#15803d"
  bad-text: "#b91c1c"
  error-bg: "#fdeee7"
  error-line: "#b91c1c"
  error-ink: "#7d1d0c"
  focus: "#7a2b06"
typography:
  display:
    fontFamily: "ui-serif, Georgia, 'Iowan Old Style', 'Palatino Linotype', Palatino, serif"
    fontSize: "clamp(2.75rem, 7vw, 5rem)"
    fontWeight: 600
    lineHeight: 1.02
    letterSpacing: "-0.03em"
  headline:
    fontFamily: "ui-serif, Georgia, 'Iowan Old Style', 'Palatino Linotype', Palatino, serif"
    fontSize: "1.5rem"
    fontWeight: 600
    lineHeight: 1.25
    letterSpacing: "-0.02em"
  title:
    fontFamily: "ui-sans-serif, system-ui, -apple-system, 'Segoe UI', Helvetica, Arial, sans-serif"
    fontSize: "0.8rem"
    fontWeight: 700
    lineHeight: 1.4
    letterSpacing: "0.08em"
  body:
    fontFamily: "ui-sans-serif, system-ui, -apple-system, 'Segoe UI', Helvetica, Arial, sans-serif"
    fontSize: "0.875rem"
    fontWeight: 400
    lineHeight: 1.5
    letterSpacing: "normal"
  label:
    fontFamily: "ui-sans-serif, system-ui, -apple-system, 'Segoe UI', Helvetica, Arial, sans-serif"
    fontSize: "0.75rem"
    fontWeight: 700
    lineHeight: 1.4
    letterSpacing: "0.08em"
rounded:
  xs: "0.35rem"
  sm: "0.7rem"
  md: "1rem"
  lg: "1.25rem"
  pill: "999px"
spacing:
  2xs: "0.25rem"
  xs: "0.5rem"
  sm: "0.75rem"
  md: "1rem"
  lg: "1.75rem"
  xl: "2rem"
components:
  button-primary:
    backgroundColor: "{colors.accent-terracotta}"
    textColor: "#ffffff"
    rounded: "{rounded.sm}"
    padding: "0.8rem 1rem"
    minHeight: "48px"
  button-primary-hover:
    backgroundColor: "{colors.accent-deep}"
  button-primary-disabled:
    backgroundColor: "#d9c6ad"
    textColor: "#6b5947"
  input-field:
    backgroundColor: "#fffdfa"
    textColor: "{colors.ink}"
    borderColor: "{colors.line-strong}"
    rounded: "{rounded.sm}"
    padding: "0.6rem 0.8rem"
    minHeight: "44px"
  wizard-panel:
    backgroundColor: "{colors.paper-raised}"
    borderColor: "{colors.line}"
    rounded: "{rounded.lg}"
    padding: "1.75rem"
  agent-card:
    backgroundColor: "#fffdf9"
    borderColor: "{colors.line}"
    textColor: "{colors.ink}"
    rounded: "{rounded.md}"
  chat-bubble-user:
    backgroundColor: "{colors.accent-terracotta}"
    textColor: "#ffffff"
    rounded: "{rounded.lg}"
    padding: "1.1rem 1.35rem"
  chat-bubble-assistant:
    backgroundColor: "{colors.paper}"
    borderColor: "{colors.line}"
    textColor: "{colors.ink}"
    rounded: "{rounded.lg}"
    padding: "1.1rem 1.35rem"
  risk-badge:
    backgroundColor: "{colors.status-ideal-bg}"
    textColor: "{colors.status-ideal-ink}"
    typography: "{typography.label}"
    rounded: "{rounded.pill}"
    padding: "0.2rem 0.6rem"
---

# Design System: Picnic In The Park

## Overview

**North Star: Golden Hour.**

This is a planning tool for something pleasant. The old system dressed that in
technical dark chrome, which made the picnic feel like a monitoring dashboard.
The current system inverts it: the product opens on a large photograph of a
meadow in late afternoon light, and every working surface is warm frosted paper
floating over it.

The use scene decides the rest. This surface is shown two ways: projected in a
lit room during a demo, and read closely by one person on a laptop. Both favour
a light, high-contrast field. Warmth comes from hue rather than from dimming,
so the interface stays legible at projector distance without going flat.

The mode is **Operate**. Once the visitor is past the hero, the job is to fill
in five fields and read what five agents came back with. Personality lives in
the header, the paper, and the terracotta; the working area stays quiet and
scannable.

## Colors

The palette runs on three families and nothing else.

**Field and paper.** `field` `#fdf4e7` is the page. Panels are near-white warm
paper at high alpha over the photograph: `paper-raised` `rgba(255,253,248,0.94)`
for the wizard, `paper` `rgba(255,251,243,0.9)` for chat and empty states, and
`paper-sunk` for recessed strips like card headers. The alpha is deliberately
high. It is enough to blur the photo into atmosphere while keeping body text
above 13:1 no matter which pixel of the image is behind it.

**Ink.** Warm browns, never neutral grey: `ink` `#2b1d14` for primary text,
`ink-2` `#5c4634` for supporting text, `ink-3` `#77604a` for labels and units.
Over the photograph the text flips to `ink-on-dark` `#fffaf1`.

**Terracotta.** `accent-terracotta` `#b03a0b` is the only saturated chrome in
the system. It appears on the primary button, the visitor's own chat bubble,
the focus ring, and the loading spinner's leading edge. Nowhere else.

Status colors are a separate, deliberately un-branded family, because they carry
agent output rather than identity. Each is a light chip with dark ink of the same
hue, and every pair clears 6.9:1.

| Token | Chip | Ink | Ratio |
|---|---|---|---|
| ideal | `#bbf7d0` | `#14532d` | 7.5:1 |
| caution | `#fde68a` | `#713f12` | 7.0:1 |
| rain | `#bfdbfe` | `#1e3a8a` | 7.3:1 |
| heat | `#fecaca` | `#7f1d1d` | 6.9:1 |
| unsafe | `#991b1b` | `#ffffff` | 8.6:1 |
| unknown | `#e7e0d5` | `#3f3a33` | 8.2:1 |

There is no dark scheme. `color-scheme` is pinned to `light` because the world
is built on a specific photograph and a specific quality of light; a mechanical
inversion would produce neither.

## Typography

Two families, split by who is speaking.

**Serif carries the human voice.** A system serif stack led by Georgia sets the
hero title at `clamp(2.75rem, 7vw, 5rem)` with `-0.03em` tracking, and the
wizard heading at `1.5rem`. These are the only places the product speaks in its
own voice, and they are the only serif in the system.

**Sans carries the machine's.** Everything an agent produced, plus every label,
value, and table cell, is set in the system sans stack. Body text is `0.875rem`
at `1.5` line height.

Micro-labels, card titles, table headers, and menu categories share one
treatment: `0.75rem` to `0.8rem`, weight 700, uppercase, `0.08em` tracking, in
`ink-3`. This is the system's connective tissue and the reason dense agent
output stays scannable.

Numbers in the budget and menu use `font-variant-numeric: tabular-nums` so
costs align down a column.

## Layout

A hero band, then a two-column working area that overlaps it.

The header is `clamp(360px, 58vh, 620px)` tall, holds the photograph, and
bottom-aligns its title inside the same `1400px` measure the content uses. The
main grid is `360px 1fr` and is pulled up by `-7rem` so the panels cut into the
hero. That overlap is what makes the photograph read as depth rather than as a
banner sitting above the app.

The wizard rail is sticky at `top: 1.5rem`. Agent results flow into
`repeat(auto-fill, minmax(280px, 1fr))`, so adding a sixth specialist agent
needs no layout change.

Two breakpoints: at `1080px` the rail narrows to `320px`; at `860px` the grid
collapses to one column, the rail stops being sticky, and the hero shortens to
`clamp(320px, 46vh, 460px)`.

## Elevation & Depth

Depth comes from three stacked ideas, in this order:

1. **The photograph** is the ground plane. It is the only image in the product.
2. **The scrim** is a five-stop vertical ramp from `rgba(48,24,9,0.62)` at the
   top of the hero to the solid field at the bottom. It guarantees the title
   clears 5:1 over the brightest pixel the image can produce, and it dissolves
   the photo into the page instead of cutting it off with an edge.
3. **Frosted paper** panels sit on top with `backdrop-filter: blur(20px)
   saturate(1.15)`, so the meadow's color bleeds faintly through the working
   surfaces.

Shadows are warm and directional, always with a real offset and a soft blur:
`--shadow-rest` for cards, `--shadow-lift` on hover, `--shadow-hero` for the
wizard, which is the one panel that overlaps the photograph most and needs the
strongest separation. Borders never do the lifting; shadows do.

## Shapes

Three radii, applied by role rather than by size. Panels and chat bubbles take
`1.25rem`, agent cards `1rem`, controls `0.7rem`, chips and the budget bar
`999px`. Chat bubbles keep one corner tightened to `0.35rem` on the side that
points at the speaker.

Borders are one pixel and come in two strengths. `line-strong` `#998063` clears
3:1 against paper and is used wherever a boundary is a control the visitor must
find: input edges, the table header rule, the empty state's dashed frame.
`line` `#e0cfb4` is decorative separation only: card edges, row dividers, the
budget track.

## Components

**Primary button.** Solid terracotta, white label at 6.1:1, `48px` minimum
height. Hover deepens the fill and lifts by 1px. Disabled goes to warm stone
`#d9c6ad` with `#6b5947` text, which stays readable rather than dropping to
half opacity.

**Input.** `44px` minimum height, near-white fill, `line-strong` border.
Focus is `:focus-visible` only, and draws a terracotta border plus a `3px`
ring at `0.28` alpha, strong enough to see rather than a decorative halo.

**Wizard panel.** The most elevated surface in the product: `paper-raised`,
`1.25rem` radius, `--shadow-hero`, overlapping the photograph.

**Agent card.** Near-opaque paper with a recessed `paper-sunk` header strip
holding an emoji glyph and an uppercase title. Lifts 3px on hover. The emoji
is always `aria-hidden`; the text label carries the meaning.

**Status chip.** Pill, uppercase micro-label, light chip with dark ink. Never
white text on a saturated fill.

**Budget meter.** A `999px` track in `field-deep` with a `1px` line, filled by
a bar that animates on `transform: scaleX()` rather than `width`, so the bar
does not force layout on every frame. Green under budget, red over.

**Chat bubbles.** The visitor's message is solid terracotta and capped at 70%
width. The agents' reply is full-width frosted paper, because it contains the
card grid.

## Do's and Don'ts

**Do**

- Keep terracotta on the primary action, the visitor's voice, and focus. Nothing else.
- Put dark ink on light chips for every status, and check the pair clears 4.5:1.
- Set anything a human wrote in the serif, and anything an agent produced in the sans.
- Let shadows carry elevation and keep borders at one pixel.
- Use `line-strong` when a boundary is a control, `line` when it is decoration.
- Give controls a 44px minimum target, 48px for the primary action.
- Keep the panels overlapping the hero; the overlap is the composition.
- Swap `--hero-image` to change the whole world's light in one line.

**Don't**

- Add a second photograph. One image, in the header, is the rule.
- Put text on the photograph outside the scrim ramp.
- Reach for gradient text, gradient buttons, or a colored left rule.
- Introduce a dark scheme by inverting; this world is built on one quality of light.
- Let status colors drift into brand colors, or brand colors into status.
- Animate `width`, `height`, or `top`; animate `transform` and `opacity`.
- Add a second serif or a third family.
- Use neutral grey for secondary text. Tint it from the warm ink ramp.
