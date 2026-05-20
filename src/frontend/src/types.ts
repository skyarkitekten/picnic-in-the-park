// ── Request ──
export type PicnicPlanRequest = {
    eventDate: string;
    partySize: number;
    budget: number;
    locationPreference: string;
    dietaryNotes?: string;
};

// ── Weather ──
export type WeatherResult = {
    date: string;
    temperatureF: number;
    condition: string;
    riskClassification: string;
};

// ── Parks ──
export type ParkResult = {
    parkId: string;
    parkName: string;
    city: string;
    shelterName: string;
    capacity: number;
    score: number;
};

// ── Menu ──
export type MenuItem = {
    name: string;
    category: string;
    estimatedCost: number;
};

export type MenuResult = {
    items: MenuItem[];
    totalCost: number;
};

// ── Grocery ──
export type GroceryItem = {
    name: string;
    quantity: string;
    price: number;
    available: boolean;
};

export type GroceryResult = {
    items: GroceryItem[];
    totalCost: number;
};

// ── Reservation ──
export type ReservationHold = {
    parkName: string;
    shelterName: string;
    date: string;
    status: string;
};

// ── Budget ──
export type BudgetSummary = {
    foodCost: number;
    reservationCost: number;
    miscCost: number;
    total: number;
    remaining: number;
};

// ── Assembled plan ──
export type PicnicPlan = {
    planId: string;
    request: PicnicPlanRequest;
    weather: WeatherResult;
    parks: ParkResult[];
    menu: MenuResult;
    grocery: GroceryResult;
    reservation: ReservationHold;
    budget: BudgetSummary;
    status: string;
};

// ── Chat message types ──
export type ChatMessage =
    | { role: 'user'; content: string }
    | { role: 'assistant'; plan: PicnicPlan };
