/* eslint-disable @typescript-eslint/no-explicit-any */
// src/components/AuthForm.tsx
import { useMemo, useState } from "react";
import { AuthApi, setToken } from "../api";

export function AuthForm({ onAuth }: { onAuth: (email: string) => void }) {
    const [mode, setMode] = useState<"login" | "register">("login");
    const [email, setEmail] = useState("");
    const [pass, setPass] = useState("");
    const [isAdmin, setIsAdmin] = useState(false);
    const [err, setErr] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);

    const canSubmit = useMemo(() => !!email && !!pass, [email, pass]);

    const submit = async () => {
        setBusy(true);
        setErr(null);

        try {
            const res =
                mode === "login"
                    ? await AuthApi.login({ email, password: pass })
                    : await AuthApi.register({ email, password: pass, isAdmin });

            setToken(res.token);
            onAuth(res.user.email || email);
        } catch (e: any) {
            setErr(e?.message ?? "Auth error");
        } finally {
            setBusy(false);
        }
    };

    return (
        <div className="card vstack" style={{ maxWidth: 420, margin: "40px auto" }}>
            <h3 className="title" style={{ textAlign: "center" }}>
                {mode === "login" ? "Вход" : "Регистрация"}
            </h3>

            <input
                className="input"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                autoComplete="email"
            />

            <input
                className="input"
                placeholder="Пароль"
                type="password"
                value={pass}
                onChange={(e) => setPass(e.target.value)}
                autoComplete={mode === "login" ? "current-password" : "new-password"}
            />

            {mode === "register" && (
                <label className="hstack" style={{ gap: 10, alignItems: "center", marginTop: 6 }}>
                    <input type="checkbox" checked={isAdmin} onChange={(e) => setIsAdmin(e.target.checked)} />
                    <span>Создать аккаунт администратора</span>
                </label>
            )}

            {err && (
                <span className="badge" style={{ borderColor: "#ff9b9b", background: "#fff5f5" }}>
                    ⚠ {err}
                </span>
            )}

            <div className="hstack" style={{ justifyContent: "space-between" }}>
                <button
                    className="btn"
                    onClick={() => {
                        setMode((m) => (m === "login" ? "register" : "login"));
                        setErr(null);
                    }}
                    disabled={busy}
                >
                    {mode === "login" ? "Создать аккаунт" : "У меня уже есть аккаунт"}
                </button>

                <button className="btn primary" disabled={busy || !canSubmit} onClick={submit}>
                    {mode === "login" ? "Войти" : "Зарегистрироваться"}
                </button>
            </div>
        </div>
    );
}
