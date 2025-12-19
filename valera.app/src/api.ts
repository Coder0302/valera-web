/* eslint-disable @typescript-eslint/no-explicit-any */
// src/api.ts
import type { ValeraDto, CreateValeraRequest } from "./types";

const API_BASE = (import.meta as any).env?.VITE_API_BASE || "http://localhost:5064";

const LS_TOKEN_KEY = "jwt";

let token: string | null = localStorage.getItem(LS_TOKEN_KEY) ?? null;

export function setToken(t: string | null) {
    token = t;
    if (t) localStorage.setItem(LS_TOKEN_KEY, t);
    else localStorage.removeItem(LS_TOKEN_KEY);
}

export function getToken() {
    return token;
}

export class ApiError extends Error {
    status: number;
    details?: any;

    constructor(status: number, message: string, details?: any) {
        super(message);
        this.status = status;
        this.details = details;
    }
}

async function readErrorMessage(res: Response): Promise<{ message: string; details?: any }> {
    const ct = res.headers.get("content-type")?.toLowerCase() ?? "";
    try {
        if (ct.includes("application/json")) {
            const data = await res.json();
            const msg =
                data?.message ||
                data?.error ||
                (typeof data === "string" ? data : "") ||
                `${res.status} ${res.statusText}`;
            return { message: msg, details: data };
        }

        const text = await res.text();
        return { message: text || `${res.status} ${res.statusText}` };
    } catch {
        return { message: `${res.status} ${res.statusText}` };
    }
}

async function http<T>(url: string, init: RequestInit = {}): Promise<T> {
    const headers = new Headers(init.headers);
    if (!headers.has("Content-Type")) headers.set("Content-Type", "application/json");
    if (token) headers.set("Authorization", `Bearer ${token}`);

    const res = await fetch(API_BASE + url, { ...init, headers });

    if (!res.ok) {
        const err = await readErrorMessage(res);
        if (res.status === 401) setToken(null);
        throw new ApiError(res.status, err.message, err.details);
    }

    if (res.status === 204) return undefined as unknown as T;

    const ct = res.headers.get("content-type")?.toLowerCase() ?? "";
    if (ct.includes("application/json")) {
        return (await res.json()) as T;
    }

    return (await res.text()) as unknown as T;
}

export type AuthUser = {
    id: string;
    email: string;
    role?: "User" | "Admin" | string;
};

export type AuthResponse = {
    token: string;
    user: AuthUser;
};

export type LoginRequest = { email: string; password: string };
export type RegisterRequest = { email: string; password: string; isAdmin?: boolean };

function normalizeAuthResponse(data: any, fallbackEmail?: string): AuthResponse | null {
    if (!data) return null;

    if (typeof data?.token === "string" && data?.user?.email) {
        return { token: data.token, user: data.user };
    }

    if (typeof data?.token === "string") {
        return {
            token: data.token,
            user: {
                id: data?.userId ?? data?.id ?? "",
                email: data?.email ?? fallbackEmail ?? "",
                role: data?.role,
            },
        };
    }

    return null;
}

export const AuthApi = {
    async login(req: LoginRequest): Promise<AuthResponse> {
        const data = await http<any>("/api/auth/login", {
            method: "POST",
            body: JSON.stringify(req),
        });

        const normalized = normalizeAuthResponse(data, req.email);
        if (!normalized?.token) throw new Error("Сервер не вернул JWT-токен при входе.");
        if (!normalized.user.email) normalized.user.email = req.email;

        return normalized;
    },

    async register(req: RegisterRequest): Promise<AuthResponse> {
        const data = await http<any>("/api/auth/register", {
            method: "POST",
            body: JSON.stringify(req),
        });

        const normalized = normalizeAuthResponse(data, req.email);
        if (normalized?.token) {
            if (!normalized.user.email) normalized.user.email = req.email;
            return normalized;
        }

        return await AuthApi.login({ email: req.email, password: req.password });
    },
};

export const ValeraApi = {
    list: () => http<ValeraDto[]>("/api/valera"),

    get: (id: string) => http<ValeraDto>(`/api/valera/${id}`),
    create: (req: CreateValeraRequest) =>
        http<ValeraDto>("/api/valera", { method: "POST", body: JSON.stringify(req) }),

    work: (id: string) => http<ValeraDto>(`/api/valera/${id}/work`, { method: "POST", body: "{}" }),
    contemplate: (id: string) => http<ValeraDto>(`/api/valera/${id}/contemplate`, { method: "POST", body: "{}" }),
    wineTv: (id: string) => http<ValeraDto>(`/api/valera/${id}/wine-tv`, { method: "POST", body: "{}" }),
    bar: (id: string) => http<ValeraDto>(`/api/valera/${id}/bar`, { method: "POST", body: "{}" }),
    badCompany: (id: string) => http<ValeraDto>(`/api/valera/${id}/bad-company`, { method: "POST", body: "{}" }),
    subwaySing: (id: string) => http<ValeraDto>(`/api/valera/${id}/subway-sing`, { method: "POST", body: "{}" }),
    sleep: (id: string) => http<ValeraDto>(`/api/valera/${id}/sleep`, { method: "POST", body: "{}" }),
};
