// src/api.ts
import type { ValeraDto, CreateValeraRequest } from "./types";

const API_BASE = (import.meta as any).env?.VITE_API_BASE || "http://localhost:5064";

let token: string | null = localStorage.getItem("jwt") ?? null;
export function setToken(t: string | null) {
    token = t;
    if (t) localStorage.setItem("jwt", t); else localStorage.removeItem("jwt");
}
export function getToken() { return token; }

async function http<T>(url: string, init?: RequestInit): Promise<T> {
    const headers: Record<string, string> = { "Content-Type": "application/json" };
    if (token) headers["Authorization"] = `Bearer ${token}`;
    const res = await fetch(API_BASE + url, { headers, ...init });
    if (!res.ok) throw new Error(`${res.status} ${res.statusText}`);
    return await res.json();
}

export type AuthResponse = { token: string; user: { id: string; email: string } };
export type LoginRequest = { email: string; password: string };
export type RegisterRequest = { email: string; password: string };

export const AuthApi = {
    register: (req: RegisterRequest) => http<AuthResponse>("/api/auth/register", { method: "POST", body: JSON.stringify(req) }),
    login: (req: LoginRequest) => http<AuthResponse>("/api/auth/login", { method: "POST", body: JSON.stringify(req) }),
};

export const ValeraApi = {
    list: () => http<ValeraDto[]>("/api/valera"),
    get: (id: string) => http<ValeraDto>(`/api/valera/${id}`),
    create: (req: CreateValeraRequest) => http<ValeraDto>("/api/valera", { method: "POST", body: JSON.stringify(req) }),

    work: (id: string) => http<ValeraDto>(`/api/valera/${id}/work`, { method: "POST", body: "{}" }),
    contemplate: (id: string) => http<ValeraDto>(`/api/valera/${id}/contemplate`, { method: "POST", body: "{}" }),
    wineTv: (id: string) => http<ValeraDto>(`/api/valera/${id}/wine-tv`, { method: "POST", body: "{}" }),
    bar: (id: string) => http<ValeraDto>(`/api/valera/${id}/bar`, { method: "POST", body: "{}" }),
    badCompany: (id: string) => http<ValeraDto>(`/api/valera/${id}/bad-company`, { method: "POST", body: "{}" }),
    subwaySing: (id: string) => http<ValeraDto>(`/api/valera/${id}/subway-sing`, { method: "POST", body: "{}" }),
    sleep: (id: string) => http<ValeraDto>(`/api/valera/${id}/sleep`, { method: "POST", body: "{}" }),
};
